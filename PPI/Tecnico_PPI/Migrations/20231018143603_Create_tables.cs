using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PPI.Migrations
{
    /// <inheritdoc />
    public partial class Create_tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Acciones",
                columns: table => new
                {
                    IdAccion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ticker = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Acciones", x => x.IdAccion);
                });

            migrationBuilder.CreateTable(
                name: "Bonos",
                columns: table => new
                {
                    IdBono = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ticker = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bonos", x => x.IdBono);
                });

            migrationBuilder.CreateTable(
                name: "EstadoOrdenes",
                columns: table => new
                {
                    IdEstadoOrden = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DescripcionEstado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoOrdenes", x => x.IdEstadoOrden);
                });

            migrationBuilder.CreateTable(
                name: "FCIs",
                columns: table => new
                {
                    IdFCI = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ticker = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FCIs", x => x.IdFCI);
                });

            migrationBuilder.CreateTable(
                name: "TipoActivos",
                columns: table => new
                {
                    IdTipoActivo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Comision = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Impuesto = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoActivos", x => x.IdTipoActivo);
                });

            migrationBuilder.CreateTable(
                name: "OrdenInversiones",
                columns: table => new
                {
                    IdOrdenInversion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCuenta = table.Column<int>(type: "int", nullable: false),
                    IdEstado = table.Column<int>(type: "int", nullable: false),
                    IdTipoActivo = table.Column<int>(type: "int", nullable: false),
                    IdActivo = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Operacion = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    MontoTotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdenInversiones", x => x.IdOrdenInversion);
                    table.ForeignKey(
                        name: "FK_OrdenInversiones_Acciones_IdActivo",
                        column: x => x.IdActivo,
                        principalTable: "Acciones",
                        principalColumn: "IdAccion",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrdenInversiones_Bonos_IdActivo",
                        column: x => x.IdActivo,
                        principalTable: "Bonos",
                        principalColumn: "IdBono",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrdenInversiones_EstadoOrdenes_IdEstado",
                        column: x => x.IdEstado,
                        principalTable: "EstadoOrdenes",
                        principalColumn: "IdEstadoOrden",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrdenInversiones_FCIs_IdActivo",
                        column: x => x.IdActivo,
                        principalTable: "FCIs",
                        principalColumn: "IdFCI",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrdenInversiones_TipoActivos_IdTipoActivo",
                        column: x => x.IdTipoActivo,
                        principalTable: "TipoActivos",
                        principalColumn: "IdTipoActivo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bonos_Nombre",
                table: "Bonos",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EstadoOrdenes_DescripcionEstado",
                table: "EstadoOrdenes",
                column: "DescripcionEstado",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FCIs_Nombre",
                table: "FCIs",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrdenInversiones_IdActivo",
                table: "OrdenInversiones",
                column: "IdActivo");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenInversiones_IdEstado",
                table: "OrdenInversiones",
                column: "IdEstado");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenInversiones_IdTipoActivo",
                table: "OrdenInversiones",
                column: "IdTipoActivo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrdenInversiones");

            migrationBuilder.DropTable(
                name: "Acciones");

            migrationBuilder.DropTable(
                name: "Bonos");

            migrationBuilder.DropTable(
                name: "EstadoOrdenes");

            migrationBuilder.DropTable(
                name: "FCIs");

            migrationBuilder.DropTable(
                name: "TipoActivos");
        }
    }
}
