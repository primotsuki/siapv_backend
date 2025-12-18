using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace siapv_backend.Migrations
{
    /// <inheritdoc />
    public partial class EstadoFix_nullable_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "estadoId",
                table: "solicitudViajes",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "estadoSolicitudes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    estado = table.Column<string>(type: "varchar(30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_estadoSolicitudes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "estadoSolicitudes",
                columns: new[] { "Id", "estado" },
                values: new object[,]
                {
                    { 1, "Designado" },
                    { 2, "Solitado" },
                    { 3, "Pendiente" },
                    { 4, "Enviado a revisión" },
                    { 5, "Aprobado" },
                    { 6, "Finalizado" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_solicitudViajes_estadoId",
                table: "solicitudViajes",
                column: "estadoId");

            migrationBuilder.AddForeignKey(
                name: "FK_solicitudViajes_estadoSolicitudes_estadoId",
                table: "solicitudViajes",
                column: "estadoId",
                principalTable: "estadoSolicitudes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_solicitudViajes_estadoSolicitudes_estadoId",
                table: "solicitudViajes");

            migrationBuilder.DropTable(
                name: "estadoSolicitudes");

            migrationBuilder.DropIndex(
                name: "IX_solicitudViajes_estadoId",
                table: "solicitudViajes");

            migrationBuilder.DropColumn(
                name: "estadoId",
                table: "solicitudViajes");
        }
    }
}
