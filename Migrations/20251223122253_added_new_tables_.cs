using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace siapv_backend.Migrations
{
    /// <inheritdoc />
    public partial class added_new_tables_ : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "revisiones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    solicitudId = table.Column<int>(type: "integer", nullable: false),
                    fucav = table.Column<bool>(type: "boolean", nullable: false),
                    poa = table.Column<bool>(type: "boolean", nullable: false),
                    presupuesto = table.Column<bool>(type: "boolean", nullable: false),
                    memo = table.Column<bool>(type: "boolean", nullable: false),
                    informe = table.Column<bool>(type: "boolean", nullable: false),
                    estadoId = table.Column<int>(type: "integer", nullable: false),
                    createdAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_revisiones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_revisiones_estadoSolicitudes_estadoId",
                        column: x => x.estadoId,
                        principalTable: "estadoSolicitudes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_revisiones_solicitudViajes_solicitudId",
                        column: x => x.solicitudId,
                        principalTable: "solicitudViajes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_revisiones_estadoId",
                table: "revisiones",
                column: "estadoId");

            migrationBuilder.CreateIndex(
                name: "IX_revisiones_solicitudId",
                table: "revisiones",
                column: "solicitudId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "revisiones");
        }
    }
}
