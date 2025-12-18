using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace siapv_backend.Migrations
{
    /// <inheritdoc />
    public partial class certificacion_poa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "descripcion",
                table: "fuenteFinanciamientos",
                type: "varchar(10)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "codigo_fuente",
                table: "fuenteFinanciamientos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "literal",
                table: "fuenteFinanciamientos",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "actividadPOAs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    codigo = table.Column<string>(type: "varchar(30)", nullable: false),
                    tipo = table.Column<string>(type: "varchar(10)", nullable: false),
                    descripcion = table.Column<string>(type: "text", nullable: false),
                    activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_actividadPOAs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "categoriasProgramaticas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    categoria = table.Column<string>(type: "varchar(50)", nullable: false),
                    descripcion_categoria = table.Column<string>(type: "text", nullable: false),
                    partida = table.Column<int>(type: "integer", nullable: false),
                    presupuesto_vigente = table.Column<float>(type: "real", nullable: false),
                    saldo = table.Column<float>(type: "real", nullable: false),
                    activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categoriasProgramaticas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "correlativos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    procesointernoId = table.Column<int>(type: "integer", nullable: false),
                    correlativo = table.Column<int>(type: "integer", nullable: false),
                    gestion = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_correlativos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "operacionPOAs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    codigo_op = table.Column<string>(type: "varchar(15)", nullable: false),
                    nombre_operacion = table.Column<string>(type: "varchar(200)", nullable: false),
                    prog = table.Column<string>(type: "text", nullable: false),
                    proyecto = table.Column<string>(type: "text", nullable: false),
                    fuente = table.Column<string>(type: "text", nullable: false),
                    activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operacionPOAs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "certificacionPOAs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    gestion = table.Column<int>(type: "integer", nullable: false),
                    correlativo = table.Column<int>(type: "integer", nullable: false),
                    solicitudId = table.Column<int>(type: "integer", nullable: false),
                    actividadAmpId = table.Column<int>(type: "integer", nullable: false),
                    actividadAcpId = table.Column<int>(type: "integer", nullable: false),
                    createdAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_certificacionPOAs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_certificacionPOAs_actividadPOAs_actividadAcpId",
                        column: x => x.actividadAcpId,
                        principalTable: "actividadPOAs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_certificacionPOAs_actividadPOAs_actividadAmpId",
                        column: x => x.actividadAmpId,
                        principalTable: "actividadPOAs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_certificacionPOAs_solicitudViajes_solicitudId",
                        column: x => x.solicitudId,
                        principalTable: "solicitudViajes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "certificacionPresupuestarias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    gestion = table.Column<int>(type: "integer", nullable: false),
                    correlativo = table.Column<int>(type: "integer", nullable: false),
                    solicitudId = table.Column<int>(type: "integer", nullable: false),
                    categoriaId = table.Column<int>(type: "integer", nullable: false),
                    importe_solicitado = table.Column<float>(type: "real", nullable: false),
                    saldo_categoria = table.Column<float>(type: "real", nullable: false),
                    concepto = table.Column<string>(type: "text", nullable: false),
                    createdAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_certificacionPresupuestarias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_certificacionPresupuestarias_categoriasProgramaticas_catego~",
                        column: x => x.categoriaId,
                        principalTable: "categoriasProgramaticas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_certificacionPresupuestarias_solicitudViajes_solicitudId",
                        column: x => x.solicitudId,
                        principalTable: "solicitudViajes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_certificacionPOAs_actividadAcpId",
                table: "certificacionPOAs",
                column: "actividadAcpId");

            migrationBuilder.CreateIndex(
                name: "IX_certificacionPOAs_actividadAmpId",
                table: "certificacionPOAs",
                column: "actividadAmpId");

            migrationBuilder.CreateIndex(
                name: "IX_certificacionPOAs_solicitudId",
                table: "certificacionPOAs",
                column: "solicitudId");

            migrationBuilder.CreateIndex(
                name: "IX_certificacionPresupuestarias_categoriaId",
                table: "certificacionPresupuestarias",
                column: "categoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_certificacionPresupuestarias_solicitudId",
                table: "certificacionPresupuestarias",
                column: "solicitudId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "certificacionPOAs");

            migrationBuilder.DropTable(
                name: "certificacionPresupuestarias");

            migrationBuilder.DropTable(
                name: "correlativos");

            migrationBuilder.DropTable(
                name: "operacionPOAs");

            migrationBuilder.DropTable(
                name: "actividadPOAs");

            migrationBuilder.DropTable(
                name: "categoriasProgramaticas");

            migrationBuilder.DropColumn(
                name: "codigo_fuente",
                table: "fuenteFinanciamientos");

            migrationBuilder.DropColumn(
                name: "literal",
                table: "fuenteFinanciamientos");

            migrationBuilder.AlterColumn<string>(
                name: "descripcion",
                table: "fuenteFinanciamientos",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(10)");
        }
    }
}
