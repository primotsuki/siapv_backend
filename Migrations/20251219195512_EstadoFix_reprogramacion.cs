using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace siapv_backend.Migrations
{
    /// <inheritdoc />
    public partial class EstadoFix_reprogramacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "reprogramaciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    solicitudId = table.Column<int>(type: "integer", nullable: false),
                    linea = table.Column<string>(type: "text", nullable: false),
                    fecha_emsision = table.Column<DateOnly>(type: "date", nullable: false),
                    origenId = table.Column<int>(type: "integer", nullable: false),
                    destinoId = table.Column<int>(type: "integer", nullable: false),
                    nro_boleto = table.Column<string>(type: "text", nullable: false),
                    importe = table.Column<double>(type: "double precision", nullable: false),
                    justificacion = table.Column<string>(type: "text", nullable: false),
                    createAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reprogramaciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_reprogramaciones_lugarDestinos_destinoId",
                        column: x => x.destinoId,
                        principalTable: "lugarDestinos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_reprogramaciones_lugarDestinos_origenId",
                        column: x => x.origenId,
                        principalTable: "lugarDestinos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_reprogramaciones_solicitudViajes_solicitudId",
                        column: x => x.solicitudId,
                        principalTable: "solicitudViajes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_reprogramaciones_destinoId",
                table: "reprogramaciones",
                column: "destinoId");

            migrationBuilder.CreateIndex(
                name: "IX_reprogramaciones_origenId",
                table: "reprogramaciones",
                column: "origenId");

            migrationBuilder.CreateIndex(
                name: "IX_reprogramaciones_solicitudId",
                table: "reprogramaciones",
                column: "solicitudId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reprogramaciones");
        }
    }
}
