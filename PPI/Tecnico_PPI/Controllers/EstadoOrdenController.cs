using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PPI.Models;
using PPI.Data;
using Microsoft.AspNetCore.Authorization;

namespace PPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class EstadoOrdenController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EstadoOrdenController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene una lista de estados de orden.
        /// </summary>
        /// <response code="200" type="string">La solicitud se procesó correctamente.</response>
        /// <response code="400" type="string">La solicitud contiene datos no válidos.</response>
        /// <response code="401" type="string">No autorizado.</response>
        /// <response code="404" type="string">El recurso solicitado no fue encontrado.</response>
        /// <response code="500" type="string">Error interno del servidor.</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<EstadoOrden>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpGet]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstadoOrden>>> GetEstadosOrden()
        {
            try
            {
                var estadosOrden = await _context.EstadoOrdenes.ToListAsync();
                if (estadosOrden == null || !estadosOrden.Any())
                    return NotFound("El recurso solicitado no fue encontrado.");

                return Ok(estadosOrden);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }

        /// <summary>
        /// Obtiene un estado de orden.
        /// </summary>
        /// <param name="id">ID del estado orden que quiere obtener</param>
        /// <response code="200" type="string">La solicitud se procesó correctamente.</response>
        /// <response code="400" type="string">La solicitud contiene datos no válidos.</response>
        /// <response code="401" type="string">No autorizado.</response>
        /// <response code="404" type="string">El recurso solicitado no fue encontrado.</response>
        /// <response code="500" type="string">Error interno del servidor.</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EstadoOrden))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpGet("{id}")]
        public async Task<ActionResult<EstadoOrden>> GetEstadoOrden(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("El parámetro 'id' es inválido.");

                var estadoOrden = await _context.EstadoOrdenes.FirstOrDefaultAsync(eo => eo.IdEstadoOrden == id);

                if (estadoOrden == null)
                    return NotFound("El recurso solicitado no fue encontrado");

                return Ok(estadoOrden);

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);

            }
        }


        /// <summary>
        /// Crea un nuevo estado de orden.
        /// </summary>
        /// <param name="descripcionEstado">Descripción del estado que desea agregar</param>
        /// <response code="201" type="string">La acción se creo correctamente.</response>
        /// <response code="400" type="string">La solicitud contiene datos no válidos.</response>
        /// <response code="401" type="string">No autorizado.</response>
        /// <response code="500" type="string">Error interno del servidor.</response>
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(EstadoOrden))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpPost]
        public async Task<ActionResult<EstadoOrden>> PostEstadoOrden([FromBody] string descripcionEstado)
        {
            if (string.IsNullOrWhiteSpace(descripcionEstado))
                ModelState.AddModelError("descripcionEstado", "La descripción del estado es obligatoria.");
            else if (descripcionEstado.Length > 50)
                ModelState.AddModelError("descripcionEstado", "La descripción del estado no puede tener más de 50 caracteres.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var nuevoEstado = new EstadoOrden
                {
                    DescripcionEstado = descripcionEstado
                };

                _context.EstadoOrdenes.Add(nuevoEstado);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetEstadoOrden", new { id = nuevoEstado.IdEstadoOrden }, nuevoEstado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }


        /// <summary>
        /// Modifica una orden de estado.
        /// </summary>
        /// <param name="id">ID del estado orden que quiere modificar</param>
        /// <param name="descripcionEstado">Revisar el esquema de la clase</param>
        /// <response code="200" type="string">La solicitud se procesó correctamente.</response>
        /// <response code="400" type="string">La solicitud contiene datos no válidos.</response>
        /// <response code="401" type="string">No autorizado.</response>
        /// <response code="404" type="string">El recurso solicitado no fue encontrado.</response>
        /// <response code="500" type="string">Error interno del servidor.</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EstadoOrden))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpPut("{id}")]
        public async Task<ActionResult<EstadoOrden>> PutEstadoOrden(int id, [FromBody] string descripcionEstado)
        {
            if (id <= 0)
                return BadRequest("El parámetro 'id' es inválido.");

            if (string.IsNullOrWhiteSpace(descripcionEstado))
                ModelState.AddModelError("descripcionEstado", "La descripción del estado es obligatoria.");
            else if (descripcionEstado.Length > 50)
                ModelState.AddModelError("descripcionEstado", "La descripción del estado no puede tener más de 50 caracteres.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var estadoOrdenExistente = await _context.EstadoOrdenes.FindAsync(id);           

                if (estadoOrdenExistente == null)
                    return NotFound("El recurso solicitado no fue encontrado.");

                estadoOrdenExistente.DescripcionEstado = descripcionEstado;

                _context.EstadoOrdenes.Update(estadoOrdenExistente);
                await _context.SaveChangesAsync();

                return Ok(estadoOrdenExistente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }

        /// <summary>
        /// Elimina una orden de estado.
        /// </summary>
        /// <param name="id">ID del estado orden que quiere eliminar</param>
        /// <response code="200" type="string">La solicitud se procesó correctamente.</response>
        /// <response code="400" type="string">La solicitud contiene datos no válidos.</response>
        /// <response code="401" type="string">No autorizado.</response>
        /// <response code="404" type="string">El recurso solicitado no fue encontrado.</response>
        /// <response code="500" type="string">Error interno del servidor.</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EstadoOrden))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpDelete("{id}")]
        public async Task<ActionResult<EstadoOrden>> DeleteEstadoOrden(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("El parámetro 'id' es inválido.");

                var estadoOrden = await _context.EstadoOrdenes.FindAsync(id);
                if (estadoOrden == null)
                    return NotFound("El recurso solicitado no fue encontrado.");

                _context.EstadoOrdenes.Remove(estadoOrden);
                await _context.SaveChangesAsync();

                return Ok(estadoOrden);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }        
        }
    }
}
