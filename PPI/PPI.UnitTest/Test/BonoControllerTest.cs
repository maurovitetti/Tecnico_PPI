using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using PPI.Controllers;
using PPI.Data;
using PPI.DTOs;
using PPI.Models;
using Xunit;

namespace PPI.Test
{
    public class BonoControllerTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly BonoController _controller;

        public BonoControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabaseBonos")
                .Options;

            _context = new ApplicationDbContext(options);
            _controller = new BonoController(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        //-------------------------------------------------------------------------------------------------------------------- INICIO GET -------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Crea bonos en la base de datos y las lista
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetBonos_ResultadoOk_RetornaListaBono()
        {
            // Arrange
            var bonos = new List<Bono>
            {
                new Bono { IdBono = 1, Ticker = "BONO1", Nombre = "Bono 1" },
                new Bono { IdBono = 2, Ticker = "BONO2", Nombre = "Bono 2" }
            };

            _context.Bonos.AddRange(bonos);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetBonos();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var bonosResult = Assert.IsType<List<Bono>>(okResult.Value);
            Assert.Equal(bonos.Count, bonosResult.Count);

            // Valida las propiedades de los bonos en la respuesta
            foreach (var bono in bonos)
            {
                var bonoResult = bonosResult.FirstOrDefault(b => b.IdBono == bono.IdBono);
                Assert.NotNull(bonoResult); // Asegura que se encontró un bono correspondiente
                Assert.Equal(bono.Ticker, bonoResult.Ticker);
                Assert.Equal(bono.Nombre, bonoResult.Nombre);
            }
        }

        /// <summary>
        /// Intenta obtener un listado de bonos cuando la tabla esta vacia
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetBonos_SinInformacionEnBaseDatos_RetornaNotFound()
        {
            // Act
            var result = await _controller.GetBonos();

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }



        //--------------------------------------------------------------------------------------------------------------------- FIN GET ---------------------------------------------------------------------------------------------------------//


        //--------------------------------------------------------------------------------------------------------------------- INICIO POST -----------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Crea un bono correcto
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PostBono_ConDatosValidos_RetornaCreatedAtActionResult()
        {
            // Arrange
            var newBonoDTO = new NewBonoDTO
            {
                Ticker = "BONO3",
                Nombre = "Bono 3"
            };

            // Act
            var result = await _controller.PostBono(newBonoDTO);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Bono>>(result);
            var bonoObjectResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var bonoResult = Assert.IsType<Bono>(bonoObjectResult.Value);

            Assert.Equal(newBonoDTO.Ticker, bonoResult.Ticker);
            Assert.Equal(newBonoDTO.Nombre, bonoResult.Nombre);
        }

        /// <summary>
        /// Se intenta crear un bono enviando información incompleta
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PostBono_ConDatosIncompletos_RetornaBadRequest()
        {
            // Arrange
            var newBonoDTO = new NewBonoDTO
            {
                Ticker = "BONO4"
                // Faltan otros campos requeridos, por ejemplo, Nombre
            };

            // Act
            var result = await _controller.PostBono(newBonoDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        //--------------------------------------------------------------------------------------------------------------------- FIN POST --------------------------------------------------------------------------------------------------------//


        //-------------------------------------------------------------------------------------------------------------------- INICIO GET CON PARÁMETRO -----------------------------------------------------------------------------------------//

        /// <summary>
        /// Crea un bono en la base de datos y la obtiene
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetBono_ConIdValido_RetornaResultadoOk()
        {
            // Arrange
            var bono = new Bono { IdBono = 1, Ticker = "BONO1", Nombre = "Bono 1" };
            _context.Bonos.Add(bono);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetBono(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var bonoResult = Assert.IsType<Bono>(okResult.Value);
            Assert.Equal(bono.IdBono, bonoResult.IdBono);
        }

        /// <summary>
        /// Intenta obtener un bono enviando un ID igual a cero 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetBono_ConIdInvalido_RetornaBadRequest()
        {
            // Act
            var result = await _controller.GetBono(0);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        /// <summary>
        /// Intenta obtener un bono que no existe en la DB
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetBono_ConIdNoExistente_RetornaNotFound()
        {
            // Act
            var result = await _controller.GetBono(999);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        //-------------------------------------------------------------------------------------------------------------------- FIN GET CON PARÁMETRO --------------------------------------------------------------------------------------------//


        //-------------------------------------------------------------------------------------------------------------------- INICIO PUT -------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Modifica un bono con información correcta
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PutBono_ConDatosValidos_RetornaResultadoOk()
        {
            // Arrange
            var bono = new Bono { IdBono = 1, Ticker = "BONO1", Nombre = "Bono 1" };
            _context.Bonos.Add(bono);
            await _context.SaveChangesAsync();

            var updateBonoDTO = new UpdateBonoDTO
            {
                Nombre = "Bono 1 (Updated)",
                Ticker = "BONO1-UPDATED"
            };

            // Act
            var result = await _controller.PutBono(1, updateBonoDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var updatedBono = Assert.IsType<Bono>(okResult.Value);
            Assert.Equal("Bono 1 (Updated)", updatedBono.Nombre);
            Assert.Equal("BONO1-UPDATED", updatedBono.Ticker);
        }

        /// <summary>
        /// Intenta modificar un bono enviando informacion incompleta
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PutBono_ConDatosIncompletos_RetornaBadRequest()
        {
            // Arrange
            var bono = new Bono { IdBono = 1, Ticker = "BONO1", Nombre = "Bono 1" };
            _context.Bonos.Add(bono);
            await _context.SaveChangesAsync();

            var updateBonoDTO = new UpdateBonoDTO();

            // Act
            var result = await _controller.PutBono(1, updateBonoDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        /// <summary>
        /// Intenta eliminar un bono enviando un ID igual a cero
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PutBono_ConIdCero_RetornaBadRequest()
        {
            // Arrange
            var updateBonoDTO = new UpdateBonoDTO
            {
                Nombre = "Bono 1 (Updated)",
                Ticker = "BONO1-UPDATED"
            };

            // Act
            var result = await _controller.PutBono(0, updateBonoDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        //-------------------------------------------------------------------------------------------------------------------- FIN PUT ----------------------------------------------------------------------------------------------------------//


        //-------------------------------------------------------------------------------------------------------------------- INICIO DELETE ----------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Crea un bono y lo elimina correctamente
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DeleteBono_ConIdValido_RetornaResultadoOk()
        {
            // Arrange
            var bono = new Bono { IdBono = 1, Ticker = "BONO1", Nombre = "Bono 1" };
            _context.Bonos.Add(bono);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.DeleteBono(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);

            // Verificar que la acción fue eliminada
            var deletedAccion = await _context.Bonos.FindAsync(1);
            Assert.Null(deletedAccion); // Debería ser nulo ya que se eliminó
        }

        /// <summary>
        /// Intenta eliminar un bono enviando un ID igual acero
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DeleteBono_ConIdCero_RetornaBadRequest()
        {
            // Act
            var result = await _controller.DeleteBono(0);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        /// <summary>
        /// Intenta eliminar un bono que no existe en la base de datos
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DeleteBono_ConIdNoExistente_RetornaNotFound()
        {
            // Act
            var result = await _controller.DeleteBono(999);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        //-------------------------------------------------------------------------------------------------------------------- FIN DELETE ------------------------------------------------------------------------------------------------------//
    }
}
