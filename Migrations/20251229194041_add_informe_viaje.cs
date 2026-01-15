using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace siapv_backend.Migrations
{
    /// <inheritdoc />
    public partial class add_informe_viaje : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "informeViajes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    solicitudId = table.Column<int>(type: "integer", nullable: false),
                    cite_doc = table.Column<string>(type: "varchar(100)", nullable: false),
                    antecedentes = table.Column<string>(type: "text", nullable: false),
                    desarrollo = table.Column<string>(type: "text", nullable: false),
                    conclusion = table.Column<string>(type: "text", nullable: false),
                    createdAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_informeViajes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_informeViajes_solicitudViajes_solicitudId",
                        column: x => x.solicitudId,
                        principalTable: "solicitudViajes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_informeViajes_solicitudId",
                table: "informeViajes",
                column: "solicitudId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "informeViajes");
        }
    }
}
