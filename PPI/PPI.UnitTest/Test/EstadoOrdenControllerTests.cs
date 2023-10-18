using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using PPI.Controllers;
using PPI.Data;
using PPI.Models;
using Xunit;

namespace PPI.Test
{
    public class EstadoOrdenControllerTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly EstadoOrdenController _controller;

        public EstadoOrdenControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabaseEstadosOrden")
                .Options;

            _context = new ApplicationDbContext(options);
            _controller = new EstadoOrdenController(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        //-------------------------------------------------------------------------------------------------------------------- INICIO GET -------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Crea estados de orden en la base de datos y las lista
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetEstadosOrden_ResultadoOk_RetornaListaEstadoOrden()
        {
            // Arrange
            var estadosOrden = new List<EstadoOrden>
            {
                new EstadoOrden { IdEstadoOrden = 1, DescripcionEstado = "Estado 1" },
                new EstadoOrden { IdEstadoOrden = 2, DescripcionEstado = "Estado 2" }
            };

            _context.EstadoOrdenes.AddRange(estadosOrden);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetEstadosOrden();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var estadosOrdenResult = Assert.IsType<List<EstadoOrden>>(okResult.Value);
            Assert.Equal(estadosOrden.Count, estadosOrdenResult.Count);

            // Valida las propiedades de los estados de orden en la respuesta
            foreach (var estado in estadosOrden)
            {
                var estadoResult = estadosOrdenResult.FirstOrDefault(e => e.IdEstadoOrden == estado.IdEstadoOrden);
                Assert.NotNull(estadoResult); // Asegura que se encontró un estado de orden correspondiente
                Assert.Equal(estado.DescripcionEstado, estadoResult.DescripcionEstado);
            }
        }

        /// <summary>
        /// Intenta obtener un listado de estados de orden cuando la tabla esta vacia
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetEstadosOrden_SinInformacionEnBaseDatos_RetornaNotFound()
        {
            // Act
            var result = await _controller.GetEstadosOrden();

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        //--------------------------------------------------------------------------------------------------------------------- FIN GET ---------------------------------------------------------------------------------------------------------//


        //--------------------------------------------------------------------------------------------------------------------- INICIO POST -----------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Crea una estado de orden correcto
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PostEstadoOrden_ConDatosValidos_RetornaCreatedAtActionResult()
        {
            // Arrange
            var descripcionEstado = "Nuevo Estado";

            // Act
            var result = await _controller.PostEstadoOrden(descripcionEstado);

            // Assert
            var actionResult = Assert.IsType<ActionResult<EstadoOrden>>(result);
            var estadoObjectResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var estadoResult = Assert.IsType<EstadoOrden>(estadoObjectResult.Value);

            Assert.Equal(descripcionEstado, estadoResult.DescripcionEstado);
        }

        /// <summary>
        /// Se intenta crear una acción enviando información incompleta
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PostEstadoOrden_ConDatosIncompletos_RetornaBadRequest()
        {
            // Act
            var result = await _controller.PostEstadoOrden(string.Empty);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }


        //--------------------------------------------------------------------------------------------------------------------- FIN POST --------------------------------------------------------------------------------------------------------//


        //-------------------------------------------------------------------------------------------------------------------- INICIO GET CON PARÁMETRO -----------------------------------------------------------------------------------------//

        /// <summary>
        /// Crea un estado de orden en la base de datos y la obtiene
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetEstadoOrden_ConIdValido_RetornaResultadoOk()
        {
            // Arrange
            var estado = new EstadoOrden { IdEstadoOrden = 1, DescripcionEstado = "Estado 1" };
            _context.EstadoOrdenes.Add(estado);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetEstadoOrden(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var estadoResult = Assert.IsType<EstadoOrden>(okResult.Value);
            Assert.Equal(estado.IdEstadoOrden, estadoResult.IdEstadoOrden);
        }

        /// <summary>
        /// Intenta obtener un estado de orden enviando un ID igual a cero 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetEstadoOrden_ConIdInvalido_RetornaBadRequest()
        {
            // Act
            var result = await _controller.GetEstadoOrden(0);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        /// <summary>
        /// Intenta obtener un estado de orden que no existe en la DB
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetEstadoOrden_ConIdNoExistente_RetornaNotFound()
        {
            // Act
            var result = await _controller.GetEstadoOrden(999);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        //-------------------------------------------------------------------------------------------------------------------- FIN GET CON PARÁMETRO --------------------------------------------------------------------------------------------//


        //-------------------------------------------------------------------------------------------------------------------- INICIO PUT -------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Modifica un estado de orden con información correcta
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PutEstadoOrden_ConDatosValidos_RetornaResultadoOk()
        {
            // Arrange
            var estado = new EstadoOrden { IdEstadoOrden = 1, DescripcionEstado = "Estado 1" };
            _context.EstadoOrdenes.Add(estado);
            await _context.SaveChangesAsync();

            var nuevaDescripcionEstado = "Estado 1 (Actualizado)";

            // Act
            var result = await _controller.PutEstadoOrden(1, nuevaDescripcionEstado);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var updatedEstado = Assert.IsType<EstadoOrden>(okResult.Value);
            Assert.Equal(nuevaDescripcionEstado, updatedEstado.DescripcionEstado);
        }

        /// <summary>
        /// Intenta modificar un estado de orden sin enviar la informacion necesaria
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PutEstadoOrden_ConDatosIncompletos_RetornaBadRequest()
        {
            // Arrange
            var estado = new EstadoOrden { IdEstadoOrden = 1, DescripcionEstado = "Estado 1" };
            _context.EstadoOrdenes.Add(estado);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.PutEstadoOrden(1, string.Empty);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        /// <summary>
        /// Intenta modificar un estado de orden que no existe
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PutEstadoOrden_ConIdCero_RetornaBadRequest()
        {
            // Act
            var result = await _controller.PutEstadoOrden(0, "Nuevo Estado");

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        //-------------------------------------------------------------------------------------------------------------------- FIN PUT ----------------------------------------------------------------------------------------------------------//


        //-------------------------------------------------------------------------------------------------------------------- INICIO DELETE ----------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Se crea un estado de orden correcto y se la elimina 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DeleteEstadoOrden_ConIdValido_RetornaResultadoOk()
        {
            // Arrange
            var estado = new EstadoOrden { IdEstadoOrden = 1, DescripcionEstado = "Estado 1" };
            _context.EstadoOrdenes.Add(estado);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.DeleteEstadoOrden(1);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);

            // Asegura que el estado de orden se eliminó de la base de datos
            var deletedEstado = await _context.EstadoOrdenes.FindAsync(1);
            Assert.Null(deletedEstado);
        }

        /// <summary>
        /// Se intenta eliminar un estado de orden enviando un ID igual acero
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DeleteEstadoOrden_ConIdCero_RetornaBadRequest()
        {
            // Act
            var result = await _controller.DeleteEstadoOrden(0);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        /// <summary>
        /// Se intenta eliminar un estado de orden que no existe en la DB
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DeleteEstadoOrden_ConIdNoExistente_RetornaNotFound()
        {
            // Act
            var result = await _controller.DeleteEstadoOrden(999);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        //-------------------------------------------------------------------------------------------------------------------- FIN DELETE ------------------------------------------------------------------------------------------------------//
    }
}
