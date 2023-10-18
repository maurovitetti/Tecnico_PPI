using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PPI.Data;
using PPI.DTOs;
using PPI.Models;

namespace PPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TipoActivoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TipoActivoController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene una lista de tipos de activos.
        /// </summary>
        /// <response code="200" type="string">La solicitud se procesó correctamente.</response>
        /// <response code="400" type="string">La solicitud contiene datos no válidos.</response>
        /// <response code="401" type="string">No autorizado.</response>
        /// <response code="404" type="string">El recurso solicitado no fue encontrado.</response>
        /// <response code="500" type="string">Error interno del servidor.</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TipoActivo>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoActivo>>> GetTiposDeActivos()
        {
            try
            {
                var tipoActivos = await _context.TipoActivos.ToListAsync();
                if (tipoActivos == null || !tipoActivos.Any())
                    return NotFound("El recurso solicitado no fue encontrado.");

                return Ok(tipoActivos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }

        /// <summary>
        /// Obtiene un tipo de activo.
        /// </summary>
        /// <param name="id">ID del tipo activo que quiere obtener</param>
        /// <response code="200" type="string">La solicitud se procesó correctamente.</response>
        /// <response code="400" type="string">La solicitud contiene datos no válidos.</response>
        /// <response code="401" type="string">No autorizado.</response>
        /// <response code="404" type="string">El recurso solicitado no fue encontrado.</response>
        /// <response code="500" type="string">Error interno del servidor.</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TipoActivo))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoActivo>> GetTipoActivo(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("El parámetro 'id' es inválido.");

                var tipoActivo = await _context.TipoActivos.FindAsync(id);
                if (tipoActivo == null)
                    return NotFound("El recurso solicitado no fue encontrado");

                return Ok(tipoActivo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }

        }

        /// <summary>
        /// Crea un nuevo tipo de activo.
        /// </summary>
        /// <param name="newTipoActivoDTO">
        /// Revisar el esquema de la clase
        /// </param>
        /// <response code="201" type="string">La acción se creo correctamente.</response>
        /// <response code="400" type="string">La solicitud contiene datos no válidos.</response>
        /// <response code="401" type="string">No autorizado.</response>
        /// <response code="500" type="string">Error interno del servidor.</response>
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TipoActivo))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpPost]
        public async Task<ActionResult<TipoActivo>> PostTipoActivo([FromBody] NewTipoActivoDTO newTipoActivoDTO)
        {
            if (!newTipoActivoDTO.IsValid(ModelState))
                return BadRequest(ModelState);

            try
            {
                var nuevoTipoActivo = new TipoActivo
                {
                    Nombre = newTipoActivoDTO.Nombre ?? "",
                    Comision = newTipoActivoDTO.Comision ?? 0.00m,
                    Impuesto = newTipoActivoDTO.Impuesto ?? 0.00m
                };

                _context.TipoActivos.Add(nuevoTipoActivo);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetTipoActivo", new { id = nuevoTipoActivo.IdTipoActivo }, nuevoTipoActivo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }

        /// <summary>
        /// Modifica un tipo de activo.
        /// </summary>
        /// <param name="id">ID del tipo activo que quiere modificar</param>
        /// <param name="updateTipoActivoDTO">Revisar el esquema de la clase</param>
        /// <response code="200" type="string">La solicitud se procesó correctamente.</response>
        /// <response code="400" type="string">La solicitud contiene datos no válidos.</response>
        /// <response code="401" type="string">No autorizado.</response>
        /// <response code="404" type="string">El recurso solicitado no fue encontrado.</response>
        /// <response code="500" type="string">Error interno del servidor.</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TipoActivo))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpPut("{id}")]
        public async Task<ActionResult<TipoActivo>> PutTipoActivo(int id, [FromBody] UpdateTipoActivoDTO updateTipoActivoDTO)
        {
            if (!updateTipoActivoDTO.IsValid(ModelState))
                return BadRequest(ModelState);

            try
            {
                var tipoActivo = await _context.TipoActivos.FindAsync(id);
                if (tipoActivo == null)
                    return NotFound("El recurso solicitado no fue encontrado.");

                tipoActivo.Nombre = updateTipoActivoDTO.Nombre ?? tipoActivo.Nombre;
                tipoActivo.Comision = updateTipoActivoDTO.Comision ?? tipoActivo.Comision;
                tipoActivo.Impuesto = updateTipoActivoDTO.Impuesto ?? tipoActivo.Impuesto;

                _context.Entry(tipoActivo).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(tipoActivo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }

        /// <summary>
        /// Elimina un tipo de activo.
        /// </summary>
        /// <param name="id">ID del tipo activo que quiere eliminar</param>
        /// <response code="200" type="string">La solicitud se procesó correctamente.</response>
        /// <response code="400" type="string">La solicitud contiene datos no válidos.</response>
        /// <response code="401" type="string">No autorizado.</response>
        /// <response code="404" type="string">El recurso solicitado no fue encontrado.</response>
        /// <response code="500" type="string">Error interno del servidor.</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TipoActivo))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpDelete("{id}")]
        public async Task<ActionResult<TipoActivo>> DeleteTipoActivo(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("El parámetro 'id' es inválido.");

                var tipoActivo = await _context.TipoActivos.FindAsync(id);
                if (tipoActivo == null)
                    return NotFound("El recurso solicitado no fue encontrado.");

                _context.TipoActivos.Remove(tipoActivo);
                await _context.SaveChangesAsync();

                return Ok(tipoActivo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }

        }
    }
}
