using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using PPI.Controllers;
using PPI.Data;
using PPI.Models;
using PPI.DTOs;
using Xunit;

namespace PPI.Test
{
    public class FCIControllerTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly FCIController _controller;

        public FCIControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabaseFCIs")
                .Options;

            _context = new ApplicationDbContext(options);
            _controller = new FCIController(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        //-------------------------------------------------------------------------------------------------------------------- INICIO GET -------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Crea FCIs en la base de datos y las lista
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetFCIs_ResultadoOk_RetornaListaFCI()
        {
            // Arrange
            var fcis = new List<FCI>
            {
                new FCI { IdFCI = 1, Ticker = "FCI1", Nombre = "FCI 1" },
                new FCI { IdFCI = 2, Ticker = "FCI2", Nombre = "FCI 2" }
            };

            _context.FCIs.AddRange(fcis);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetFCIs();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var fcisResult = Assert.IsType<List<FCI>>(okResult.Value);
            Assert.Equal(fcis.Count, fcisResult.Count);

            // Valida las propiedades de los FCI en la respuesta
            foreach (var fci in fcis)
            {
                var fciResult = fcisResult.FirstOrDefault(f => f.IdFCI == fci.IdFCI);
                Assert.NotNull(fciResult); // Asegura que se encontró un FCI correspondiente
                Assert.Equal(fci.Ticker, fciResult.Ticker);
                Assert.Equal(fci.Nombre, fciResult.Nombre);
            }
        }

        /// <summary>
        /// Intenta obtener un listado de FCIs cuando la tabla esta vacia
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetFCIs_SinInformacionEnBaseDatos_RetornaNotFound()
        {
            // Act
            var result = await _controller.GetFCIs();

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        //--------------------------------------------------------------------------------------------------------------------- FIN GET ---------------------------------------------------------------------------------------------------------//


        //--------------------------------------------------------------------------------------------------------------------- INICIO POST -----------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Crea un FCI correcto
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PostFCI_ConDatosValidos_RetornaCreatedAtActionResult()
        {
            // Arrange
            var newFCIDTO = new NewFCIDTO { Ticker = "NuevoFCI", Nombre = "Nuevo FCI" };

            // Act
            var result = await _controller.PostFCI(newFCIDTO);

            // Assert
            var actionResult = Assert.IsType<ActionResult<FCI>>(result);
            var fciObjectResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var fciResult = Assert.IsType<NewFCIDTO>(fciObjectResult.Value);

            Assert.Equal(newFCIDTO.Ticker, fciResult.Ticker);
            Assert.Equal(newFCIDTO.Nombre, fciResult.Nombre);
        }

        /// <summary>
        /// Se intenta crear un FCI enviando información incompleta
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PostFCI_ConDatosIncompletos_RetornaBadRequest()
        {
            // Act
            var result = await _controller.PostFCI(new NewFCIDTO());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        //--------------------------------------------------------------------------------------------------------------------- FIN POST --------------------------------------------------------------------------------------------------------//


        //-------------------------------------------------------------------------------------------------------------------- INICIO GET CON PARÁMETRO -----------------------------------------------------------------------------------------//

        /// <summary>
        /// Crea un FCI en la base de datos y la obtiene
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetFCI_ConIdValido_RetornaResultadoOk()
        {
            // Arrange
            var fci = new FCI { IdFCI = 1, Ticker = "FCI1", Nombre = "FCI 1" };
            _context.FCIs.Add(fci);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetFCI(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var fciResult = Assert.IsType<FCI>(okResult.Value);
            Assert.Equal(fci.IdFCI, fciResult.IdFCI);
        }

        /// <summary>
        /// Intenta obtener un FCI enviando un ID igual a cero 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetFCI_ConIdInvalido_RetornaBadRequest()
        {
            // Act
            var result = await _controller.GetFCI(0);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        /// <summary>
        /// Intenta obtener un FCI que no existe en la DB
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetFCI_ConIdNoExistente_RetornaNotFound()
        {
            // Act
            var result = await _controller.GetFCI(999);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        //-------------------------------------------------------------------------------------------------------------------- FIN GET CON PARÁMETRO --------------------------------------------------------------------------------------------//


        //-------------------------------------------------------------------------------------------------------------------- INICIO PUT -------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Modifica un FCI con información correcta
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PutFCI_ConDatosValidos_RetornaResultadoOk()
        {
            // Arrange
            var fci = new FCI { IdFCI = 1, Ticker = "FCI1", Nombre = "FCI 1" };
            _context.FCIs.Add(fci);
            await _context.SaveChangesAsync();

            var updateFCIDTO = new UpdateFCIDTO { Ticker = "FCI1-Updated", Nombre = "FCI 1 (Actualizado)" };

            // Act
            var result = await _controller.PutFCI(1, updateFCIDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var updatedFCI = Assert.IsType<FCI>(okResult.Value);
            Assert.Equal(updateFCIDTO.Ticker, updatedFCI.Ticker);
            Assert.Equal(updateFCIDTO.Nombre, updatedFCI.Nombre);
        }

        /// <summary>
        /// Intenta modificar un FCI sin enviar ninguno de los campos optativos, solo enviando el id
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PutFCI_ConDatosIncompletos_RetornaBadRequest()
        {
            // Arrange
            var fci = new FCI { IdFCI = 1, Ticker = "FCI1", Nombre = "FCI 1" };
            _context.FCIs.Add(fci);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.PutFCI(1, new UpdateFCIDTO());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        /// <summary>
        /// Se intenta cambiar la información de un FCI, pero se envia un ID cero
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PutFCI_ConIdCero_RetornaBadRequest()
        {
            // Act
            var result = await _controller.PutFCI(0, new UpdateFCIDTO { Ticker = "FCI1-Updated" });

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        //-------------------------------------------------------------------------------------------------------------------- FIN PUT ----------------------------------------------------------------------------------------------------------//


        //-------------------------------------------------------------------------------------------------------------------- INICIO DELETE ----------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Se crea un FCI correcto y se la elimina 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DeleteFCI_ConIdValido_RetornaResultadoOk()
        {
            // Arrange
            var fci = new FCI { IdFCI = 1, Ticker = "FCI1", Nombre = "FCI 1" };
            _context.FCIs.Add(fci);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.DeleteFCI(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var deletedFCI = Assert.IsType<FCI>(okResult.Value);
            Assert.Equal(fci.IdFCI, deletedFCI.IdFCI);
        }

        /// <summary>
        /// Se eliminarun FCI enviando un ID igual a cero
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DeleteFCI_ConIdCero_RetornaBadRequest()
        {
            // Act
            var result = await _controller.DeleteFCI(0);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        /// <summary>
        /// Se intenta eliminar un FCI enviando un ID que no existe
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DeleteFCI_ConIdNoExistente_RetornaNotFound()
        {
            // Act
            var result = await _controller.DeleteFCI(999);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        //-------------------------------------------------------------------------------------------------------------------- FIN DELETE ------------------------------------------------------------------------------------------------------//
    }
}
