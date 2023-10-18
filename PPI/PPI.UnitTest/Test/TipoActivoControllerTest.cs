using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PPI.Controllers;
using PPI.Data;
using PPI.DTOs;
using PPI.Models;
using Xunit;

namespace PPI.Test
{
    public class TipoActivoControllerTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly TipoActivoController _controller;

        public TipoActivoControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabaseTipoActivo")
                .Options;

            _context = new ApplicationDbContext(options);
            _controller = new TipoActivoController(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        //-------------------------------------------------------------------------------------------------------------------- INICIO GET -------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Crea tipos de activos en la base de datos y los lista
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetTiposDeActivos_OkResult_ReturnListTipoActivo()
        {
            // Arrange
            var tipoActivo1 = new TipoActivo { IdTipoActivo = 1, Nombre = "Tipo 1", Comision = 0.01m, Impuesto = 0.02m };
            var tipoActivo2 = new TipoActivo { IdTipoActivo = 2, Nombre = "Tipo 2", Comision = 0.03m, Impuesto = 0.04m };
            _context.TipoActivos.AddRange(tipoActivo1, tipoActivo2);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetTiposDeActivos();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var tiposDeActivos = Assert.IsType<List<TipoActivo>>(okResult.Value);
            Assert.Equal(2, tiposDeActivos.Count);
        }

        /// <summary>
        /// Intenta obtener un listado de tipo de activos cuando la tabla esta vacia
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetTiposDeActivos_SinInformacionEnBaseDatos_ReturnNotFound()
        {
            // Act
            var result = await _controller.GetTiposDeActivos();

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        //--------------------------------------------------------------------------------------------------------------------- FIN GET ---------------------------------------------------------------------------------------------------------//

        //--------------------------------------------------------------------------------------------------------------------- INICIO POST -----------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Crea un tipo de activo correcto
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PostTipoActivo_OkResult_DebeRetornarCreatedAtActionResult()
        {
            // Arrange
            var newTipoActivoDTO = new NewTipoActivoDTO
            {
                Nombre = "Nuevo Tipo",
                Comision = 0.05m,
                Impuesto = 0.06m
            };

            // Act
            var result = await _controller.PostTipoActivo(newTipoActivoDTO);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var tipoActivo = Assert.IsType<TipoActivo>(createdAtActionResult.Value);
            Assert.Equal(newTipoActivoDTO.Nombre, tipoActivo.Nombre);
        }

        /// <summary>
        /// Se intenta crear un tipo de activo enviando información incorrecta
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PostTipoActivo_ConDatosInvalidos_DebeRetornarBadRequestResult()
        {
            // Arrange
            var newTipoActivoDTO = new NewTipoActivoDTO
            {
                Nombre = "",
                Comision = -0.01m,
                Impuesto = 0.06m
            };

            // Act
            var result = await _controller.PostTipoActivo(newTipoActivoDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        //--------------------------------------------------------------------------------------------------------------------- FIN POST --------------------------------------------------------------------------------------------------------//

        //-------------------------------------------------------------------------------------------------------------------- INICIO GET CON PARÁMETRO -----------------------------------------------------------------------------------------//

        /// <summary>
        /// Crea un tipo de activo en la base de datos y la obtiene
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetTipoActivo_ConIdExistente_DebeRetornarOkResultConTipoActivo()
        {
            // Arrange
            var tipoActivo = new TipoActivo { IdTipoActivo = 1, Nombre = "Tipo 1", Comision = 0.1m, Impuesto = 0.05m };
            _context.TipoActivos.Add(tipoActivo);
            await _context.SaveChangesAsync();
            var id = tipoActivo.IdTipoActivo;

            // Act
            var result = await _controller.GetTipoActivo(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var tipoActivoObtenido = Assert.IsType<TipoActivo>(okResult.Value);
            Assert.Equal(id, tipoActivoObtenido.IdTipoActivo);
        }

        /// <summary>
        /// Intenta obtener un tipo de activo enviando un ID igual a cero 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetTipoActivo_ConIdInvalido_DebeRetornarBadRequestResult()
        {
            // Arrange
            var id = 0;

            // Act
            var result = await _controller.GetTipoActivo(id);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        /// <summary>
        /// Intenta obtener un tipo de activo que no existe en la DB
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetTipoActivo_ConIdNoExistente_DebeRetornarNotFoundResult()
        {
            // Arrange
            var id = 999;

            // Act
            var result = await _controller.GetTipoActivo(id);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }


        //-------------------------------------------------------------------------------------------------------------------- FIN GET CON PARÁMETRO --------------------------------------------------------------------------------------------//

        //-------------------------------------------------------------------------------------------------------------------- INICIO PUT -------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Se intenta modificar un tipo de activo con información correcta
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PutTipoActivo_ConDatosValidos_DebeRetornarOkResultConTipoActivoActualizado()
        {
            // Arrange
            var tipoActivo = new TipoActivo { IdTipoActivo = 1, Nombre = "Tipo de Prueba", Comision = 0.01m, Impuesto = 0.02m };
            _context.TipoActivos.Add(tipoActivo);
            await _context.SaveChangesAsync();

            var id = tipoActivo.IdTipoActivo;
            var updateTipoActivoDTO = new UpdateTipoActivoDTO
            {
                Nombre = "Nuevo Nombre",
                Comision = 0.03m,
                Impuesto = 0.04m
            };

            // Act
            var result = await _controller.PutTipoActivo(id, updateTipoActivoDTO);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var tipoActivoActualizado = Assert.IsType<TipoActivo>(okResult.Value);
            Assert.Equal(updateTipoActivoDTO.Nombre, tipoActivoActualizado.Nombre);
        }

        /// <summary>
        /// Se intenta modificar un tipo de activo enviando información incorrecta
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PutTipoActivo_ConDatosInvalidos_DebeRetornarBadRequestResult()
        {
            // Arrange
            var id = 1;
            var updateTipoActivoDTO = new UpdateTipoActivoDTO
            {
                Nombre = "",
                Comision = -0.01m,
                Impuesto = 0.04m
            };

            // Act
            var result = await _controller.PutTipoActivo(id, updateTipoActivoDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        /// <summary>
        /// Se intenta cambiar la información de un tipo de activo con un ID que no existe
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PutTipoActivo_ConIdNoExistente_DebeRetornarNotFoundResult()
        {
            // Arrange
            var id = 999;
            var updateTipoActivoDTO = new UpdateTipoActivoDTO
            {
                Nombre = "Nuevo Nombre",
                Comision = 0.03m,
                Impuesto = 0.04m
            };

            // Act
            var result = await _controller.PutTipoActivo(id, updateTipoActivoDTO);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        //-------------------------------------------------------------------------------------------------------------------- FIN PUT ----------------------------------------------------------------------------------------------------------//

        //-------------------------------------------------------------------------------------------------------------------- INICIO DELETE ----------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Se crea un tipo de activo correcto y se la elimina 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DeleteTipoActivo_ConIdExistente_DebeRetornarOkResultConTipoActivoEliminado()
        {
            // Arrange
            var tipoActivo = new TipoActivo { IdTipoActivo = 1, Nombre = "Tipo de Prueba", Comision = 0.01m, Impuesto = 0.02m };
            _context.TipoActivos.Add(tipoActivo);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.DeleteTipoActivo(tipoActivo.IdTipoActivo);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var tipoActivoEliminado = Assert.IsType<TipoActivo>(okResult.Value);
            Assert.Equal(tipoActivo.IdTipoActivo, tipoActivoEliminado.IdTipoActivo);
        }

        /// <summary>
        /// Intenta eliminar tipo de activo un ID igual a cero
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DeleteTipoActivo_ConIdCero_DebeRetornarBadRequestResult()
        {
            // Arrange
            var id = 0;

            // Act
            var result = await _controller.DeleteTipoActivo(id);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        /// <summary>
        /// Intenta eliminar un tipo de activo que no existe en la DB
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DeleteTipoActivo_ConIdNoExistente_DebeRetornarNotFoundResult()
        {
            // Arrange
            var id = 999;

            // Act
            var result = await _controller.DeleteTipoActivo(id);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        //-------------------------------------------------------------------------------------------------------------------- FIN DELETE ------------------------------------------------------------------------------------------------------//

    }
}
