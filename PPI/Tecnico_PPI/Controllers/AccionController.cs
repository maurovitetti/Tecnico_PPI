using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PPI.Models;
using PPI.Data;
using Microsoft.AspNetCore.Authorization;
using PPI.DTOs;

namespace PPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class AccionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AccionController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene una lista de acciones.
        /// </summary>
        /// <response code="200" type="string">La solicitud se procesó correctamente.</response>
        /// <response code="400" type="string">La solicitud contiene datos no válidos.</response>
        /// <response code="401" type="string">No autorizado.</response>
        /// <response code="404" type="string">El recurso solicitado no fue encontrado.</response>
        /// <response code="500" type="string">Error interno del servidor.</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Accion>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Accion>>> GetAcciones()
        {
            try
            {
                var acciones = await _context.Acciones.ToListAsync();
                if (acciones == null || !acciones.Any())
                    return NotFound("El recurso solicitado no fue encontrado.");

                return Ok(acciones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }

        /// <summary>
        /// Obtiene una acción.
        /// </summary>
        /// <param name="id">ID de la acción que quiere obtener</param>
        /// <response code="200" type="string">La solicitud se procesó correctamente.</response>
        /// <response code="400" type="string">La solicitud contiene datos no válidos.</response>
        /// <response code="401" type="string">No autorizado.</response>
        /// <response code="404" type="string">El recurso solicitado no fue encontrado.</response>
        /// <response code="500" type="string">Error interno del servidor.</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Accion))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpGet("{id}")]
        public async Task<ActionResult<Accion>> GetAccion(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("El parámetro 'id' es inválido.");

                var accion = await _context.Acciones.FindAsync(id);
                if (accion == null)
                    return NotFound("El recurso solicitado no fue encontrado");

                return Ok(accion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }

        /// <summary>
        /// Crea una nueva acción.
        /// </summary>
        /// <param name="newAccionDTO">
        /// Revisar el esquema de la clase
        /// </param>
        /// <response code="201" type="string">La acción se creo correctamente.</response>
        /// <response code="400" type="string">La solicitud contiene datos no válidos.</response>
        /// <response code="401" type="string">No autorizado.</response>
        /// <response code="500" type="string">Error interno del servidor.</response>
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Accion))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpPost]
        public async Task<ActionResult<Accion>> PostAccion([FromBody] NewAccionDTO newAccionDTO)
        {
            if (!newAccionDTO.IsValid(ModelState))
                return BadRequest(ModelState);

            try
            {
                var nuevaAccion = new Accion
                {
                    Ticker = newAccionDTO.Ticker.ToUpper(),
                    Nombre = newAccionDTO.Nombre,
                    PrecioUnitario = newAccionDTO.PrecioUnitario
                };

                _context.Acciones.Add(nuevaAccion);
                await _context.SaveChangesAsync();

                // Retorna un 201 cuando se cree correctamente
                return CreatedAtAction("GetAccion", new { id = nuevaAccion.IdAccion }, nuevaAccion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }

        /// <summary>
        /// Modifica una acción.
        /// </summary>
        /// <param name="id">ID de la acción que quiere modificar</param>
        /// <param name="updateAccionDTO">Revisar el esquema de la clase</param>
        /// <response code="200" type="string">La solicitud se procesó correctamente.</response>
        /// <response code="400" type="string">La solicitud contiene datos no válidos.</response>
        /// <response code="401" type="string">No autorizado.</response>
        /// <response code="404" type="string">El recurso solicitado no fue encontrado.</response>
        /// <response code="500" type="string">Error interno del servidor.</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Accion))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpPut("{id}")]
        public async Task<ActionResult<Accion>> PutAccion(int id, [FromBody] UpdateAccionDTO updateAccionDTO)
        {
            if (!updateAccionDTO.IsValid(ModelState))
                return BadRequest(ModelState);

            if (id <= 0)
                return BadRequest("El parámetro 'id' es inválido.");

            try
            {
                var accion = await _context.Acciones.FindAsync(id);
                if (accion == null)
                    return NotFound("El recurso solicitado no fue encontrado.");

                accion.Ticker = updateAccionDTO.Ticker ?? accion.Ticker;
                accion.Nombre = updateAccionDTO.Nombre ?? accion.Nombre;
                accion.PrecioUnitario = updateAccionDTO.PrecioUnitario ?? accion.PrecioUnitario;

                _context.Entry(accion).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(accion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }

        /// <summary>
        /// Elimina una acción.
        /// </summary>
        /// <param name="id">ID de la acción que quiere eliminar</param>
        /// <response code="200" type="string">La solicitud se procesó correctamente.</response>
        /// <response code="400" type="string">La solicitud contiene datos no válidos.</response>
        /// <response code="401" type="string">No autorizado.</response>
        /// <response code="404" type="string">El recurso solicitado no fue encontrado.</response>
        /// <response code="500" type="string">Error interno del servidor.</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Accion))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Accion>> DeleteAccion(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("El parámetro 'id' es inválido.");

                var accion = await _context.Acciones.FindAsync(id);
                if (accion == null)
                    return NotFound("El recurso solicitado no fue encontrado.");

                _context.Acciones.Remove(accion);
                await _context.SaveChangesAsync();

                return Ok(accion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }
    }
}
