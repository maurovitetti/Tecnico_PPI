using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PPI.Controllers;
using PPI.Data;
using PPI.Models;
using PPI.DTOs;
using Xunit;

namespace PPI.Test
{
    public class OrdenInversionControllerTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly OrdenInversionController _controller;

        public OrdenInversionControllerTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabaseOrdenInversion")
                .Options;

            _context = new ApplicationDbContext(options);
            _controller = new OrdenInversionController(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        //-------------------------------------------------------------------------------------------------------------------- INICIO GET -------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Crea ordenes de inversion en la db y las lista
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetOrdenesInversion_OkResult_ReturnListOrdenInversion()
        {
            // Arrange
            var ordenesInversion = CreateSampleOrdenesInversion();

            // Act
            var result = await _controller.GetOrdenesInversion();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var ordenesInversionResult = Assert.IsType<List<OrdenInversion>>(okResult.Value);
            Assert.Equal(ordenesInversion.Count, ordenesInversionResult.Count);
        }

        /// <summary>
        /// Intenta obtener un listado de ordenes de inversion cuando la tabla esta vacia
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetOrdenesInversion_SinInformacionEnBaseDatos_ReturnNotFound()
        {
            // Act
            var result = await _controller.GetOrdenesInversion();

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        //--------------------------------------------------------------------------------------------------------------------- FIN GET ---------------------------------------------------------------------------------------------------------//


        //--------------------------------------------------------------------------------------------------------------------- INICIO POST -----------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Crea una orden de inversion correcta
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PostOrdenInversionOtroTipo_OkResult_DebeRetornarCreatedAtActionResult()
        {
            // Arrange
            var ordenesInversion = CreateSampleOrdenesInversion();

            var newOrdenInversionDTO = new NewOrdenInversionDTO
            {
                IdTipoActivo = 105,
                IdCuenta = 2,
                CantidadActivos = 10,
                Operacion = 'C',
                IdActivo = 564,   
            };

            // Act
            var result = await _controller.PostOrdenInversion(newOrdenInversionDTO);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var ordenInversion = Assert.IsType<OrdenInversion>(createdAtActionResult.Value);
            Assert.Equal(newOrdenInversionDTO.IdTipoActivo, ordenInversion.IdTipoActivo);
            Assert.Equal(newOrdenInversionDTO.IdCuenta, ordenInversion.IdCuenta);
            Assert.Equal(newOrdenInversionDTO.Operacion, ordenInversion.Operacion);
        }

        /// <summary>
        /// Se intenta crear una orden de inversion enviando información incompleta
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PostOrdenInversion_ConInformacionInvalida_DebeRetornarBadRequestResult()
        {
            // Arrange
            var ordenesInversion = CreateSampleOrdenesInversion();

            //No se envia el precio unitario, al no ser una accion es requerido
            var newOrdenInversionDTO = new NewOrdenInversionDTO
            {
                IdTipoActivo = 432,
                IdCuenta = 2,
                CantidadActivos = 10,
                Operacion = 'C',
                IdActivo = 333,
            };

            // Act
            var result = await _controller.PostOrdenInversion(newOrdenInversionDTO);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        //--------------------------------------------------------------------------------------------------------------------- FIN POST --------------------------------------------------------------------------------------------------------//


        //-------------------------------------------------------------------------------------------------------------------- INICIO GET CON PARÁMETRO -----------------------------------------------------------------------------------------//

        /// <summary>
        /// Crea una orden de inversion en la base de datos y la obtiene
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetOrdenInversion_ConIdExistente_DebeRetornarOkResultConOrdenInversion()
        {
            // Arrange
            var ordenesInversion = CreateSampleOrdenesInversion();

            var id = 4645; //Mismo ID que crea el método CreateSampleOrdenesInversion()
            // Act
            var result = await _controller.GetOrdenInversion(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var ordenInversionObtenida = Assert.IsType<OrdenInversion>(okResult.Value);
            Assert.Equal(id, ordenInversionObtenida.IdOrdenInversion);
        }

        /// <summary>
        /// Intenta obtener una orden de inversion enviando un ID igual a cero 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetOrdenInversion_ConIdInvalido_DebeRetornarBadRequestResult()
        {
            // Arrange
            var id = 0;

            // Act
            var result = await _controller.GetOrdenInversion(id);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        /// <summary>
        /// Intenta obtener una orden de inversion que no existe en la DB
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetOrdenInversion_ConIdNoExistente_DebeRetornarNotFoundResult()
        {
            // Arrange
            var id = 999;

            // Act
            var result = await _controller.GetOrdenInversion(id);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        //-------------------------------------------------------------------------------------------------------------------- FIN GET CON PARÁMETRO --------------------------------------------------------------------------------------------//


        //-------------------------------------------------------------------------------------------------------------------- INICIO PUT -------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Modifica una orden de inversion con información correcta
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PutOrdenInversion_ConIdExistente_DebeRetornarOkResultConOrdenInversionActualizada()
        {
            // Arrange
            var ordenesInversion = CreateSampleOrdenesInversion();

            // Act
            var newIdEstado = 43;
            var result = await _controller.PutOrdenInversion(6532, newIdEstado);

            // Assert
            var createdAtActionResult = Assert.IsType<OkObjectResult>(result.Result);
            var ordenInversion = Assert.IsType<OrdenInversion>(createdAtActionResult.Value);
            Assert.Equal(newIdEstado, ordenInversion.IdEstado);
        }

        /// <summary>
        /// Intenta modificar una orden de inversion enviando ID igual a cero 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PutOrdenInversion_ConIdInvalido_DebeRetornarBadRequestResult()
        {
            // Arrange
            var id = 0;
            var nuevoIdEstado = 2;

            // Act
            var result = await _controller.PutOrdenInversion(id, nuevoIdEstado);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        /// <summary>
        /// Intenta modificar una orden de inversion enviandoID que no existe
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task PutOrdenInversion_ConIdNoExistente_DebeRetornarNotFoundResult()
        {
            // Arrange
            var id = 999;
            var nuevoIdEstado = 2;

            // Act
            var result = await _controller.PutOrdenInversion(id, nuevoIdEstado);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        //-------------------------------------------------------------------------------------------------------------------- FIN PUT ----------------------------------------------------------------------------------------------------------//


        //-------------------------------------------------------------------------------------------------------------------- INICIO DELETE ----------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Se crea una orden de inversion correcta y se la elimina 
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DeleteOrdenInversion_ConIdExistente_DebeRetornarOkResultConOrdenInversionEliminada()
        {
            // Arrange
            var ordenInversion = new OrdenInversion { IdOrdenInversion = 1, IdEstado = 1 };
            _context.OrdenInversiones.Add(ordenInversion);
            await _context.SaveChangesAsync();
            var id = ordenInversion.IdOrdenInversion;

            // Act
            var result = await _controller.DeleteOrdenInversion(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var ordenInversionEliminada = Assert.IsType<OrdenInversion>(okResult.Value);
            Assert.Equal(id, ordenInversionEliminada.IdOrdenInversion);
        }

        /// <summary>
        /// Se intenta eliminar una orden de inversion enviando ID igual acero
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DeleteOrdenInversion_ConIdInvalido_DebeRetornarBadRequestResult()
        {
            // Arrange
            var id = 0;

            // Act
            var result = await _controller.DeleteOrdenInversion(id);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        /// <summary>
        /// Se intenta borrar un orden de inversion enviando un ID que no existe
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task DeleteOrdenInversion_ConIdNoExistente_DebeRetornarNotFoundResult()
        {
            // Arrange
            var id = 999;

            // Act
            var result = await _controller.DeleteOrdenInversion(id);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        //-------------------------------------------------------------------------------------------------------------------- FIN DELETE ------------------------------------------------------------------------------------------------------//

        private List<OrdenInversion> CreateSampleOrdenesInversion()
        {
            var estadoOrden1 = new EstadoOrden
            {
                IdEstadoOrden = 43,
                DescripcionEstado = "En proceso"
            };

            var estadoOrden2 = new EstadoOrden
            {
                IdEstadoOrden = 21,
                DescripcionEstado = "Finalizado"
            };

            var tipoActivo1 = new TipoActivo
            {
                IdTipoActivo = 105,
                Nombre = "Accion",      
            };

            var tipoActivo2 = new TipoActivo
            {
                IdTipoActivo = 432,
                Nombre = "FCI"
            };

            var accion1 = new Accion
            {
                IdAccion = 4534,
                Ticker = "APPL",
                Nombre = "Apple",
                PrecioUnitario = 100.0M
            };

            var accion2 = new Accion
            {
                IdAccion = 564,
                Ticker = "NEO",
                Nombre = "Empresa X",
                PrecioUnitario = 32.0M
            };

            var ordenInversion1 = new OrdenInversion
            {
                IdOrdenInversion = 4645,
                IdCuenta = 234,
                IdEstado = 43,
                IdTipoActivo = 105,
                Cantidad = 10,
                Operacion = 'C',
                MontoTotal = 1000.0M,
                EstadoOrden = estadoOrden1,
                TipoActivo = tipoActivo1
            };

            var ordenInversion2 = new OrdenInversion
            {
                IdOrdenInversion = 6532,
                IdCuenta = 3432,
                IdEstado = 21,
                IdTipoActivo = 432,
                Cantidad = 5,
                Operacion = 'V',
                MontoTotal = 500.0M,
                EstadoOrden = estadoOrden2,
                TipoActivo = tipoActivo2
            };

            var fci1 = new FCI
            {
                IdFCI = 333,
                Nombre = "Nombre FCI",
                Ticker = "FFFCI"
            };

            // Insertar las entidades en la base de datos
            _context.EstadoOrdenes.Add(estadoOrden1);
            _context.EstadoOrdenes.Add(estadoOrden2);
            _context.TipoActivos.Add(tipoActivo1);
            _context.TipoActivos.Add(tipoActivo2);
            _context.Acciones.Add(accion1);
            _context.Acciones.Add(accion2);
            _context.OrdenInversiones.Add(ordenInversion1);
            _context.OrdenInversiones.Add(ordenInversion2);
            _context.FCIs.Add(fci1);

            // Guardar los cambios en la base de datos
            _context.SaveChanges();

            return new List<OrdenInversion> { ordenInversion1, ordenInversion2 };
        }

    }
}
