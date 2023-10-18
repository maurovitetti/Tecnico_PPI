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
    public class FCIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FCIController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene una lista de FCI.
        /// </summary>
        /// <response code="200" type="string">La solicitud se procesó correctamente.</response>
        /// <response code="400" type="string">La solicitud contiene datos no válidos.</response>
        /// <response code="401" type="string">No autorizado.</response>
        /// <response code="404" type="string">El recurso solicitado no fue encontrado.</response>
        /// <response code="500" type="string">Error interno del servidor.</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<FCI>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FCI>>> GetFCIs()
        {
            try
            {
                var fcis = await _context.FCIs.ToListAsync();
                if (fcis == null || !fcis.Any())
                    return NotFound("El recurso solicitado no fue encontrado.");

                return Ok(fcis);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }

        /// <summary>
        /// Obtiene un FCI.
        /// </summary>
        /// <param name="id">ID del FCI que quiere obtener</param>
        /// <response code="200" type="string">La solicitud se procesó correctamente.</response>
        /// <response code="400" type="string">La solicitud contiene datos no válidos.</response>
        /// <response code="401" type="string">No autorizado.</response>
        /// <response code="404" type="string">El recurso solicitado no fue encontrado.</response>
        /// <response code="500" type="string">Error interno del servidor.</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FCI))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpGet("{id}")]
        public async Task<ActionResult<FCI>> GetFCI(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("El parámetro 'id' es inválido.");

                var fci = await _context.FCIs.FindAsync(id);
                if (fci == null)
                    return NotFound("El recurso solicitado no fue encontrado");

                return Ok(fci);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }

        /// <summary>
        /// Crea un nuevo fci.
        /// </summary>
        /// <param name="newFCIDTO">
        /// Revisar el esquema de la clase
        /// </param>
        /// <response code="201" type="string">La acción se creo correctamente.</response>
        /// <response code="400" type="string">La solicitud contiene datos no válidos.</response>
        /// <response code="401" type="string">No autorizado.</response>
        /// <response code="500" type="string">Error interno del servidor.</response>
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(FCI))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpPost]
        public async Task<ActionResult<FCI>> PostFCI([FromBody] NewFCIDTO newFCIDTO)
        {
            if (!newFCIDTO.IsValid(ModelState))
                return BadRequest(ModelState);

            try
            {
                var nuevoFCI = new FCI
                {
                    Ticker = newFCIDTO.Ticker.ToUpper(),
                    Nombre = newFCIDTO.Nombre,
                };

                _context.FCIs.Add(nuevoFCI);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetFCI", new { id = nuevoFCI.IdFCI }, newFCIDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }

        /// <summary>
        /// Modifica un FCI.
        /// </summary>
        /// <param name="id">ID del FCI que quiere modificar</param>
        /// <param name="updateFCIDTO">Revisar el esquema de la clase</param>
        /// <response code="200" type="string">La solicitud se procesó correctamente.</response>
        /// <response code="400" type="string">La solicitud contiene datos no válidos.</response>
        /// <response code="401" type="string">No autorizado.</response>
        /// <response code="404" type="string">El recurso solicitado no fue encontrado.</response>
        /// <response code="500" type="string">Error interno del servidor.</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FCI))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpPut("{id}")]
        public async Task<ActionResult<FCI>> PutFCI(int id, [FromBody] UpdateFCIDTO updateFCIDTO)
        {
            if (!updateFCIDTO.IsValid(ModelState))
                return BadRequest(ModelState);

            if (id <= 0)
                return BadRequest("El parámetro 'id' es inválido.");

            try
            {
                var fci = await _context.FCIs.FindAsync(id);
                if (fci == null)
                    return NotFound("El recurso solicitado no fue encontrado.");

                fci.Ticker = updateFCIDTO.Ticker ?? fci.Ticker;
                fci.Nombre = updateFCIDTO.Nombre ?? fci.Nombre;

                _context.Entry(fci).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(fci);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_context.FCIs.Any(e => e.IdFCI == id))
                    return NotFound("El recurso solicitado no fue encontrado.");
                else
                    throw;
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }

        /// <summary>
        /// Elimina un FCI.
        /// </summary>
        /// <param name="id">ID del FCI que quiere eliminar</param>
        /// <response code="200" type="string">La solicitud se procesó correctamente.</response>
        /// <response code="400" type="string">La solicitud contiene datos no válidos.</response>
        /// <response code="401" type="string">No autorizado.</response>
        /// <response code="404" type="string">El recurso solicitado no fue encontrado.</response>
        /// <response code="500" type="string">Error interno del servidor.</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FCI))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpDelete("{id}")]
        public async Task<ActionResult<FCI>> DeleteFCI(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("El parámetro 'id' es inválido.");

                var fci = await _context.FCIs.FindAsync(id);
                if (fci == null)
                    return NotFound("El recurso solicitado no fue encontrado.");

                _context.FCIs.Remove(fci);
                await _context.SaveChangesAsync();

                return Ok(fci);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }
    }
}
