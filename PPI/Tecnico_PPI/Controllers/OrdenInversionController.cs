using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PPI.Models;
using PPI.Data;
using PPI.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace PPI.Controllers
{
    [Route("api/[controller]")]
     [Authorize]
    [ApiController]
    public class OrdenInversionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrdenInversionController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene una lista ordenes de inversión.
        /// </summary>
        /// <response code="200" type="string">La solicitud se procesó correctamente.</response>
        /// <response code="400" type="string">La solicitud contiene datos no válidos.</response>
        /// <response code="401" type="string">No autorizado.</response>
        /// <response code="404" type="string">El recurso solicitado no fue encontrado.</response>
        /// <response code="500" type="string">Error interno del servidor.</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OrdenInversion>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrdenInversion>>> GetOrdenesInversion()
        {
            try
            {
                var ordenesInversionSinActivos = await _context.OrdenInversiones
                    .Include(oi => oi.EstadoOrden)
                    .Include(oi => oi.TipoActivo)
                    .ToListAsync();

                if (ordenesInversionSinActivos == null || !ordenesInversionSinActivos.Any())
                    return NotFound("El recurso solicitado no fue encontrado.");

                //foreach (var orden in ordenesInversionSinActivos)
                //{
                //    await CargarActivoRelacionado(orden);
                //}

                //TODO: ver por que me duplica los valores con el método CargarActivoRelacionado(), si es uno solo funciona. Es por la referencia? Intentar mejorarlo, mientras este for funciona Ok, pero tengo que crear de nuevo las ordenes, no es óptimo

                var ordenesInversion = new List<OrdenInversion>();
                foreach (var ordenRecorrida in ordenesInversionSinActivos)
                {
                    var nuevaOrden = OrdenInversion.ClonarOrden(ordenRecorrida);

                    if (ordenRecorrida.TipoActivo.Nombre == "FCI")
                    {
                        var fci = await _context.Entry(ordenRecorrida).Reference(oi => oi.FCI).Query().FirstOrDefaultAsync();
                        nuevaOrden.FCI = fci;
                    }
                    else if (ordenRecorrida.TipoActivo.Nombre == "Bono")
                    {
                        var bono = await _context.Entry(ordenRecorrida).Reference(oi => oi.Bono).Query().FirstOrDefaultAsync();
                        nuevaOrden.Bono = bono;
                    }
                    else if (ordenRecorrida.TipoActivo.Nombre == "Accion")
                    {
                        var accion = await _context.Entry(ordenRecorrida).Reference(oi => oi.Accion).Query().FirstOrDefaultAsync();
                        nuevaOrden.Accion = accion;
                    }
                    ordenesInversion.Add(nuevaOrden);
                }

                return Ok(ordenesInversion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }

        /// <summary>
        /// Obtiene una orden de inversión.
        /// </summary>
        /// <param name="id">ID de la orden inversion que quiere obtener</param>
        /// <response code="200" type="string">La solicitud se procesó correctamente.</response>
        /// <response code="400" type="string">La solicitud contiene datos no válidos.</response>
        /// <response code="401" type="string">No autorizado.</response>
        /// <response code="404" type="string">El recurso solicitado no fue encontrado.</response>
        /// <response code="500" type="string">Error interno del servidor.</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrdenInversion))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpGet("{id}")]
        public async Task<ActionResult<OrdenInversion>> GetOrdenInversion(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("El parámetro 'id' es inválido.");

                var ordenInversion = await _context.OrdenInversiones
                   .Include(oi => oi.EstadoOrden)
                   .Include(oi => oi.TipoActivo)
                   .FirstOrDefaultAsync(oi => oi.IdOrdenInversion == id);

                if (ordenInversion == null)
                    return NotFound("El recurso solicitado no fue encontrado");

                await CargarActivoRelacionado(ordenInversion);

                return Ok(ordenInversion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }

        /// <summary>
        /// Crea una nueva orden de inversión de tipo Accion.
        /// </summary>
        /// <param name="newOrdenInversionDTO">
        /// Revisar el esquema de la clase
        /// </param>
        /// <response code="201" type="string">La acción se creo correctamente.</response>
        /// <response code="400" type="string">La solicitud contiene datos no válidos.</response>
        /// <response code="401" type="string">No autorizado.</response>
        /// <response code="500" type="string">Error interno del servidor.</response>
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(OrdenInversion))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpPost]
        public async Task<ActionResult<OrdenInversion>> PostOrdenInversion([FromBody] NewOrdenInversionDTO newOrdenInversionDTO)
        {
            if (!newOrdenInversionDTO.IsValid(ModelState))
                return BadRequest(ModelState);

            try
            {
                //--- Validaciones varias
                var tipoActivo = await _context.TipoActivos.FindAsync(newOrdenInversionDTO.IdTipoActivo);
                if (tipoActivo == null)
                    return BadRequest($"El tipo de activo con Id '{newOrdenInversionDTO.IdTipoActivo}' no existe.");

                if (tipoActivo.Nombre != "Accion")
                {
                    if (newOrdenInversionDTO.PrecioUnitario == null || newOrdenInversionDTO.PrecioUnitario <= 0)
                        return BadRequest("El precio unitario debe ser mayor a cero");
                }

                var activo = await GetActivo(newOrdenInversionDTO.IdActivo, tipoActivo.Nombre);
                if (activo == null)
                    return BadRequest($"El activo con Id '{newOrdenInversionDTO.IdActivo}' no existe.");

                var estadoOrden = _context.EstadoOrdenes.FirstOrDefault(e => e.DescripcionEstado == "En proceso");
                if (estadoOrden == null)
                    return BadRequest("No se pudo encontrar el estado 'En Proceso'.");
                //--- Fin validaciones

                var nuevaOrdenInversion = CreateOrdenInversion(newOrdenInversionDTO, tipoActivo, estadoOrden, activo);

                _context.OrdenInversiones.Add(nuevaOrdenInversion);
                await _context.SaveChangesAsync();

                await CargarActivoRelacionado(nuevaOrdenInversion);

                return CreatedAtAction("GetOrdenInversion", new { id = nuevaOrdenInversion.IdOrdenInversion }, nuevaOrdenInversion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }

        /// <summary>
        /// Modifica una orden de inversión.
        /// </summary>
        /// <param name="id">ID de la orden inversion que quiere modificar</param>
        /// <param name="idEstado"></param>
        /// <response code="200" type="string">La solicitud se procesó correctamente.</response>
        /// <response code="400" type="string">La solicitud contiene datos no válidos.</response>
        /// <response code="401" type="string">No autorizado.</response>
        /// <response code="404" type="string">El recurso solicitado no fue encontrado.</response>
        /// <response code="500" type="string">Error interno del servidor.</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrdenInversion))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpPut("{id}")]
        public async Task<ActionResult<OrdenInversion>> PutOrdenInversion(int id, [FromBody] int idEstado)
        {
            if (id <= 0)
                return BadRequest("El parámetro 'id' es inválido.");
            else if (idEstado <= 0)
                return BadRequest("El parámetro 'idEstado' es inválido.");

            try
            {
                var ordenInversionExistente = await _context.OrdenInversiones
                    .FirstOrDefaultAsync(o => o.IdOrdenInversion == id);

                if (ordenInversionExistente == null)
                    return NotFound("El recurso solicitado no fue encontrado.");

                var nuevoEstadoOrden = await _context.EstadoOrdenes.FindAsync(idEstado);
                if (nuevoEstadoOrden == null)
                    return BadRequest("El nuevo idEstado es incorrecto. No se encontró coincidencia en la base de datos.");

                ordenInversionExistente.IdEstado = nuevoEstadoOrden.IdEstadoOrden;

                _context.OrdenInversiones.Update(ordenInversionExistente);
                await _context.SaveChangesAsync();

                return Ok(ordenInversionExistente); // Solo devuelve la relacion con el estado de la orden. En caso de ser necesario se puede devolver todas sus relacion
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }

        /// <summary>
        /// Elimina una orde de inversión.
        /// </summary>
        /// <param name="id">ID de la orden inversion que quiere eliminar</param>
        /// <response code="200" type="string">La solicitud se procesó correctamente.</response>
        /// <response code="400" type="string">La solicitud contiene datos no válidos.</response>
        /// <response code="401" type="string">No autorizado.</response>
        /// <response code="404" type="string">El recurso solicitado no fue encontrado.</response>
        /// <response code="500" type="string">Error interno del servidor.</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrdenInversion))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpDelete("{id}")]
        public async Task<ActionResult<OrdenInversion>> DeleteOrdenInversion(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("El parámetro 'id' es inválido.");

                var ordenInversion = await _context.OrdenInversiones.FindAsync(id);

                if (ordenInversion == null)
                    return NotFound("El recurso solicitado no fue encontrado.");

                _context.OrdenInversiones.Remove(ordenInversion);
                await _context.SaveChangesAsync();

                return Ok(ordenInversion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }

        /// <summary>
        /// Obtiene el precio de una acción
        /// </summary>
        /// <param name="IdAccion"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private decimal ObtenerPrecioAccion(int IdAccion)
        {
            var accion = _context.Acciones.Find(IdAccion);

            if (accion == null)
                throw new ArgumentException($"La acción con ID '{IdAccion}' no existe.");

            return accion.PrecioUnitario;
        }

        /// <summary>
        /// Calcula el monto total de un activo
        /// </summary>
        /// <param name="tipoActivo"></param>
        /// <param name="newOrdenInversionDTO"></param>
        /// <returns></returns>
        private decimal CalcularMontoTotal(TipoActivo tipoActivo, NewOrdenInversionDTO newOrdenInversionDTO)
        {
            decimal precioUnitario = tipoActivo.Nombre == "Accion" ? ObtenerPrecioAccion(newOrdenInversionDTO.IdActivo) : newOrdenInversionDTO.PrecioUnitario ?? 0M;
            decimal comisionRate = tipoActivo.Comision;
            decimal impuestoRate = tipoActivo.Impuesto;
            int cantidad = newOrdenInversionDTO.CantidadActivos;
            decimal montoTotal = precioUnitario * cantidad;

            if (comisionRate > 0)
            {
                decimal comisiones = montoTotal * (comisionRate / 100);
                montoTotal += comisiones;

                if (impuestoRate > 0)
                {
                    decimal impuestos = comisiones * (impuestoRate / 100);
                    montoTotal += impuestos;
                }
            }
            return Math.Round(montoTotal, 2);
        }

        /// <summary>
        /// Obtiene el tipo de activo
        /// </summary>
        /// <param name="idActivo"></param>
        /// <param name="tipoActivo">FCI-Bono-Accion</param>
        /// <returns></returns>
        private async Task<object?> GetActivo(int idActivo, string tipoActivo)
        {
            switch (tipoActivo)
            {
                case "FCI":
                    return await _context.FCIs.FindAsync(idActivo);
                case "Bono":
                    return await _context.Bonos.FindAsync(idActivo);
                case "Accion":
                    return await _context.Acciones.FindAsync(idActivo);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Crea un objeto de tipo OrdenInversion
        /// </summary>
        /// <param name="newOrdenInversionDTO"></param>
        /// <param name="tipoActivo"></param>
        /// <param name="estadoOrden"></param>
        /// <param name="activo"></param>
        /// <returns></returns>
        private OrdenInversion CreateOrdenInversion(NewOrdenInversionDTO newOrdenInversionDTO, TipoActivo tipoActivo, EstadoOrden estadoOrden, object activo)
        {
            return new OrdenInversion
            {
                IdCuenta = newOrdenInversionDTO.IdCuenta,
                IdEstado = estadoOrden.IdEstadoOrden,
                Cantidad = newOrdenInversionDTO.CantidadActivos,
                Operacion = newOrdenInversionDTO.Operacion,
                MontoTotal = CalcularMontoTotal(tipoActivo, newOrdenInversionDTO),
                TipoActivo = tipoActivo,
                EstadoOrden = estadoOrden,
                PrecioUnitario = newOrdenInversionDTO.PrecioUnitario,
                IdActivo = newOrdenInversionDTO.IdActivo,
            };
        }

        /// <summary>
        /// Cargar el activo correspondiente a la orden de inversion
        /// </summary>
        /// <param name="ordenInversion"></param>
        /// <returns></returns>
        private async Task CargarActivoRelacionado(OrdenInversion ordenInversion)
        {
            if (ordenInversion.TipoActivo.Nombre == "FCI")
                await _context.Entry(ordenInversion).Reference(oi => oi.FCI).LoadAsync();
            else if (ordenInversion.TipoActivo.Nombre == "Bono")
                await _context.Entry(ordenInversion).Reference(oi => oi.Bono).LoadAsync();
            else if (ordenInversion.TipoActivo.Nombre == "Accion")
                await _context.Entry(ordenInversion).Reference(oi => oi.Accion).LoadAsync();
        }
    }
}
