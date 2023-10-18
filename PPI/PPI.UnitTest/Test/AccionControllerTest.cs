using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PPI.Controllers;
using PPI.Data;
using PPI.DTOs;
using PPI.Models;
using Xunit;

namespace PPI.Test
{
    public class AccionControllerTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly AccionController _controller;

        public AccionControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabaseAcciones")
                .Options;

            _context = new ApplicationDbContext(options);
            _controller = new AccionController(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }


        //-------------------------------------------------------------------------------------------------------------------- INICIO GET -------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Crea acciones en la base de datos y las lista
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAcciones_ResultadoOk_RetornaListaAccion()
        {
            // Arrange
            var acciones = new List<Accion>
            {
                new Accion { IdAccion = 1, Ticker = "AAPL", Nombre = "Apple Inc.", PrecioUnitario = 150.0m },
                new Accion { IdAccion = 2, Ticker = "GOOGL", Nombre = "Alphabet Inc.", PrecioUnitario = 2500.0m }
            };

            _context.Acciones.AddRange(acciones);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetAcciones();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var accionesResult = Assert.IsType<List<Accion>>(okResult.Value);
            Assert.Equal(acciones.Count, accionesResult.Count);

            // Valida las propiedades de las acciones en la respuesta
            foreach (var accion in acciones)
            {
                var accionResult = accionesResult.FirstOrDefault(a => a.IdAccion == accion.IdAccion);
                Assert.NotNull(accionResult); // Asegura que se encontró una acción correspondiente
                Assert.Equal(accion.Ticker, accionResult.Ticker);
                Assert.Equal(accion.Nombre, accionResult.Nombre);
                Assert.Equal(accion.PrecioUnitario, accionResult.PrecioUnitario);
            }
        }


        /// <summary>
        /// Intenta obtener un listado de accciones cuando la tabla esta vacia
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAcciones_SinInformacionEnBaseDatos_RetornaNoEncontrado()
        {
            // Act
            var result = await _controller.GetAcciones();

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        //--------------------------------------------------------------------------------------------------------------------- FIN GET ---------------------------------------------------------------------------------------------------------//


        //--------------------------------------------------------------------------------------------------------------------- INICIO POST -----------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Crea una acción correcta
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PostAccion_ConDatosValidos_RetornaCreatedAtActionResult()
        {
            // Arrange
            var newAccionDTO = new NewAccionDTO
            {
                Ticker = "AAPL",
                Nombre = "Apple Inc.",
                PrecioUnitario = 150.0m
            };

            // Act
            var result = await _controller.PostAccion(newAccionDTO);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Accion>>(result);
            var accionObjectResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var accionResult = Assert.IsType<Accion>(accionObjectResult.Value);

            Assert.Equal(newAccionDTO.Ticker.ToUpper(), accionResult.Ticker);
            Assert.Equal(newAccionDTO.Nombre, accionResult.Nombre);
            Assert.Equal(newAccionDTO.PrecioUnitario, accionResult.PrecioUnitario);
        }

        /// <summary>
        /// Se intenta crear una acción enviando información incompleta
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PostAccion_ConDatosIncompletos_RetornaBadRequest()
        {
            // Arrange
            var newAccionDTO = new NewAccionDTO
            {
                Ticker = "AAPL",
                // Faltan otros campos requeridos, por ejemplo, Nombre y PrecioUnitario
            };

            // Act
            var result = await _controller.PostAccion(newAccionDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
        //--------------------------------------------------------------------------------------------------------------------- FIN POST --------------------------------------------------------------------------------------------------------//


        //-------------------------------------------------------------------------------------------------------------------- INICIO GET CON PARÁMETRO -----------------------------------------------------------------------------------------//

        /// <summary>
        /// Crea una acción en la base de datos y la obtiene
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAccion_ConIdValido_RetornaResultadoOk()
        {
            // Arrange
            var accion = new Accion { IdAccion = 1, Ticker = "AAPL", Nombre = "Apple Inc.", PrecioUnitario = 150.0m };
            _context.Acciones.Add(accion);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetAccion(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var accionResult = Assert.IsType<Accion>(okResult.Value);
            Assert.Equal(accion.IdAccion, accionResult.IdAccion);
        }

        /// <summary>
        /// Intenta obtener una acción enviando un ID igual a cero 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAccion_ConIdInvalido_RetornaBadRequest()
        {
            // Act
            var result = await _controller.GetAccion(0);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        /// <summary>
        /// Intenta obtener una acción que no existe en la DB
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAccion_ConIdNoExistente_RetornaNoEncontrado()
        {
            // Act
            var result = await _controller.GetAccion(999);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        //-------------------------------------------------------------------------------------------------------------------- FIN GET CON PARÁMETRO --------------------------------------------------------------------------------------------//


        //-------------------------------------------------------------------------------------------------------------------- INICIO PUT -------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Modifica una acción con información correcta
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PutAccion_ConDatosValidos_RetornaResultadoOk()
        {
            // Arrange
            var accion = new Accion { IdAccion = 1, Ticker = "AAPL", Nombre = "Apple Inc.", PrecioUnitario = 150.0m };
            _context.Acciones.Add(accion);
            await _context.SaveChangesAsync();

            var updateAccionDTO = new UpdateAccionDTO
            {
                Nombre = "Apple Inc. (Updated)",
                PrecioUnitario = 160.0m,
            };

            // Act
            var result = await _controller.PutAccion(1, updateAccionDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var updatedAccion = Assert.IsType<Accion>(okResult.Value);
            Assert.Equal("Apple Inc. (Updated)", updatedAccion.Nombre);
            Assert.Equal(160.0m, updatedAccion.PrecioUnitario);
        }

        /// <summary>
        /// Intenta modificar una acción sin enviar ninguno de los campos optativos, solo enviando el id
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PutAccion_ConDatosIncompletos_RetornaBadRequest()
        {
            // Arrange
            var accion = new Accion { IdAccion = 1, Ticker = "AAPL", Nombre = "Apple Inc.", PrecioUnitario = 150.0m };
            _context.Acciones.Add(accion);
            await _context.SaveChangesAsync();

            var updateAccionDTO = new UpdateAccionDTO();

            // Act
            var result = await _controller.PutAccion(1, updateAccionDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        /// <summary>
        /// Se intenta cambiar la información de una acción, pero se envia un ID cero
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PutAccion_ConIdCero_RetornaBadRequest()
        {
            // Arrange
            var accion = new Accion { IdAccion = 1, Ticker = "AAPL", Nombre = "Apple Inc.", PrecioUnitario = 150.0m };
            _context.Acciones.Add(accion);
            await _context.SaveChangesAsync();

            var updateAccionDTO = new UpdateAccionDTO
            {
                Nombre = "Apple Inc. (Updated)",
                PrecioUnitario = 160.0m,
            };

            // Act
            var result = await _controller.PutAccion(0, updateAccionDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        //-------------------------------------------------------------------------------------------------------------------- FIN PUT ----------------------------------------------------------------------------------------------------------//


        //-------------------------------------------------------------------------------------------------------------------- INICIO DELETE ----------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Se crea una acción correcta y se la elimina 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DeleteAccion_ConIdValido_RetornaResultadoOk()
        {
            // Arrange
            var accion = new Accion { IdAccion = 1, Ticker = "AAPL", Nombre = "Apple Inc.", PrecioUnitario = 150.0m };
            _context.Acciones.Add(accion);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.DeleteAccion(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            // Verificar que la acción fue eliminada
            var deletedAccion = await _context.Acciones.FindAsync(1);
            Assert.Null(deletedAccion); // Debería ser nulo ya que se eliminó
        }


        /// <summary>
        /// Intenta eliminar una acción enviando un ID igual a cero
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DeleteAccion_ConIdCero_RetornaBadRequest()
        {
            // Act
            var result = await _controller.DeleteAccion(0);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        /// <summary>
        /// Intenta eliminar una acción que no existe en la DB
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DeleteAccion_ConIdNoExistente_RetornaNotFound()
        {
            // Act
            var result = await _controller.DeleteAccion(999);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        //-------------------------------------------------------------------------------------------------------------------- FIN DELETE ------------------------------------------------------------------------------------------------------//
    }
}



