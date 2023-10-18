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
    public class BonoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BonoController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtiene una lista de bonos.
        /// </summary>
        /// <response code="200" type="string">La solicitud se procesó correctamente.</response>
        /// <response code="400" type="string">La solicitud contiene datos no válidos.</response>
        /// <response code="401" type="string">No autorizado.</response>
        /// <response code="404" type="string">El recurso solicitado no fue encontrado.</response>
        /// <response code="500" type="string">Error interno del servidor.</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Bono>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bono>>> GetBonos()
        {
            try
            {
                var bonos = await _context.Bonos.ToListAsync();
                if (bonos == null || !bonos.Any()) 
                    return NotFound("El recurso solicitado no fue encontrado.");

                return Ok(bonos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }

        /// <summary>
        /// Obtiene un bono.
        /// </summary>
        /// <param name="id">ID del bono que quiere obtener</param>
        /// <response code="200" type="string">La solicitud se procesó correctamente.</response>
        /// <response code="400" type="string">La solicitud contiene datos no válidos.</response>
        /// <response code="401" type="string">No autorizado.</response>
        /// <response code="404" type="string">El recurso solicitado no fue encontrado.</response>
        /// <response code="500" type="string">Error interno del servidor.</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Bono))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpGet("{id}")]
        public async Task<ActionResult<Bono>> GetBono(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("El parámetro 'id' es inválido.");

                var bono = await _context.Bonos.FindAsync(id);
                if (bono == null)
                    return NotFound("El recurso solicitado no fue encontrado");

                return Ok(bono);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }

        /// <summary>
        /// Crea un nuevo Bono.
        /// </summary>
        /// <param name="newBonoDTO">
        /// Revisar el esquema de la clase
        /// </param>
        /// <response code="201" type="string">La acción se creo correctamente.</response>
        /// <response code="400" type="string">La solicitud contiene datos no válidos.</response>
        /// <response code="401" type="string">No autorizado.</response>
        /// <response code="500" type="string">Error interno del servidor.</response>
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Bono))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpPost]
        public async Task<ActionResult<Bono>> PostBono([FromBody] NewBonoDTO newBonoDTO)
        {
            if (!newBonoDTO.IsValid(ModelState))
                return BadRequest(ModelState);

            try
            {
                var nuevoBono = new Bono
                {
                    Ticker = newBonoDTO.Ticker.ToUpper(),
                    Nombre = newBonoDTO.Nombre,
                };

                _context.Bonos.Add(nuevoBono);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetBono", new { id = nuevoBono.IdBono }, nuevoBono);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }

        /// <summary>
        /// Modifica un Bono.
        /// </summary>
        /// <param name="id">ID del bono que quiere modificar</param>
        /// <param name="updateBonoDTO">Revisar el esquema de la clase</param>
        /// <response code="200" type="string">La solicitud se procesó correctamente.</response>
        /// <response code="400" type="string">La solicitud contiene datos no válidos.</response>
        /// <response code="401" type="string">No autorizado.</response>
        /// <response code="404" type="string">El recurso solicitado no fue encontrado.</response>
        /// <response code="500" type="string">Error interno del servidor.</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Bono))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpPut("{id}")]
        public async Task<ActionResult<Bono>> PutBono(int id, [FromBody] UpdateBonoDTO updateBonoDTO)
        {
            if (!updateBonoDTO.IsValid(ModelState))
                return BadRequest(ModelState);

            if (id <= 0)
                return BadRequest("El parámetro 'id' es inválido.");

            try
            {
                var bono = await _context.Bonos.FindAsync(id);
                if (bono == null)
                    return NotFound("El recurso solicitado no fue encontrado.");

                bono.Ticker = updateBonoDTO.Ticker ?? bono.Ticker;
                bono.Nombre = updateBonoDTO.Nombre ?? bono.Nombre;

                _context.Entry(bono).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(bono);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Bonos.Any(e => e.IdBono == id))
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
        /// Elimina un bono.
        /// </summary>
        /// <param name="id">ID del bono que quiere eliminar</param>
        /// <response code="200" type="string">La solicitud se procesó correctamente.</response>
        /// <response code="400" type="string">La solicitud contiene datos no válidos.</response>
        /// <response code="401" type="string">No autorizado.</response>
        /// <response code="404" type="string">El recurso solicitado no fue encontrado.</response>
        /// <response code="500" type="string">Error interno del servidor.</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Bono))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Bono>> DeleteBono(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("El parámetro 'id' es inválido.");

                var bono = await _context.Bonos.FindAsync(id);
                if (bono == null)
                    return NotFound("El recurso solicitado no fue encontrado.");

                _context.Bonos.Remove(bono);
                await _context.SaveChangesAsync();

                return Ok(bono);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error interno del servidor: " + ex.Message);
            }
        }
    }
}
