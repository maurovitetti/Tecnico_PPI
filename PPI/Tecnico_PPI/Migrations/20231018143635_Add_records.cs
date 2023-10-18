using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PPI.Migrations
{
    /// <inheritdoc />
    public partial class Add_records : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Acciones",
                columns: new[] { "IdAccion", "Nombre", "PrecioUnitario", "Ticker" },
                values: new object[,]
                {
                    { 1, "Apple", 177.97m, "AAPL" },
                    { 2, "Alphabet Inc", 138.21m, "GOOGL" },
                    { 3, "Microsoft", 329.04m, "MSFT" },
                    { 4, "Coca Cola", 58.3m, "KO" },
                    { 5, "Walmart", 163.42m, "WMT" }
                });

            migrationBuilder.InsertData(
                table: "Bonos",
                columns: new[] { "IdBono", "Nombre", "Ticker" },
                values: new object[,]
                {
                    { 1, "Bono ejemplo1", "Bono1" },
                    { 2, "Bono ejemplo2", "Bono2" },
                    { 3, "Bono ejemplo3", "Bono2" }
                });

            migrationBuilder.InsertData(
                table: "EstadoOrdenes",
                columns: new[] { "IdEstadoOrden", "DescripcionEstado" },
                values: new object[,]
                {
                    { 1, "En proceso" },
                    { 2, "Ejecutada" },
                    { 3, "Cancelada" }
                });

            migrationBuilder.InsertData(
                table: "FCIs",
                columns: new[] { "IdFCI", "Nombre", "Ticker" },
                values: new object[,]
                {
                    { 1, "FCI ejemplo1", "FCI1" },
                    { 2, "FCI ejemplo2", "FCI2" },
                    { 3, "FCI ejemplo3", "FCI3" }
                });

            migrationBuilder.InsertData(
                table: "TipoActivos",
                columns: new[] { "IdTipoActivo", "Comision", "Impuesto", "Nombre" },
                values: new object[,]
                {
                    { 1, 0m, 0m, "FCI" },
                    { 2, 0.6m, 21m, "Accion" },
                    { 3, 0.2m, 21m, "Bono" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Acciones",
                keyColumn: "IdAccion",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Acciones",
                keyColumn: "IdAccion",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Acciones",
                keyColumn: "IdAccion",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Acciones",
                keyColumn: "IdAccion",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Acciones",
                keyColumn: "IdAccion",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Bonos",
                keyColumn: "IdBono",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Bonos",
                keyColumn: "IdBono",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Bonos",
                keyColumn: "IdBono",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "EstadoOrdenes",
                keyColumn: "IdEstadoOrden",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "EstadoOrdenes",
                keyColumn: "IdEstadoOrden",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "EstadoOrdenes",
                keyColumn: "IdEstadoOrden",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "FCIs",
                keyColumn: "IdFCI",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "FCIs",
                keyColumn: "IdFCI",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "FCIs",
                keyColumn: "IdFCI",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TipoActivos",
                keyColumn: "IdTipoActivo",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TipoActivos",
                keyColumn: "IdTipoActivo",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TipoActivos",
                keyColumn: "IdTipoActivo",
                keyValue: 3);
        }
    }
}
