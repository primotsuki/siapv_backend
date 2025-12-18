using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using NpgsqlTypes;

#nullable disable

namespace siapv_backend.Migrations
{
    /// <inheritdoc />
    public partial class Viajes_empleado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_solicitudViajes_EmpleadosContrato_empleadoId",
                table: "solicitudViajes");

            migrationBuilder.DropTable(
                name: "EmpleadosContrato");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Dependencia");

            migrationBuilder.DropTable(
                name: "EscalaSalarial");

            migrationBuilder.DropTable(
                name: "Persona");

            migrationBuilder.DropIndex(
                name: "IX_solicitudViajes_empleadoId",
                table: "solicitudViajes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dependencia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    activo = table.Column<bool>(type: "boolean", nullable: false),
                    createAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    dependenciaId = table.Column<int>(type: "integer", nullable: true),
                    descripcion = table.Column<string>(type: "varchar(100)", nullable: false),
                    sigla = table.Column<string>(type: "varchar(30)", nullable: false),
                    updateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dependencia", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EscalaSalarial",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ResolucionEscalaId = table.Column<int>(type: "integer", nullable: false),
                    activo = table.Column<bool>(type: "boolean", nullable: false),
                    denominacion = table.Column<string>(type: "text", nullable: false),
                    haber_mensual = table.Column<string>(type: "text", nullable: false),
                    nivelSalarial = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EscalaSalarial", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Persona",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombres = table.Column<string>(type: "varchar(50)", nullable: false),
                    SearchVector = table.Column<NpgsqlTsVector>(type: "tsvector", nullable: false),
                    apellido_materno = table.Column<string>(type: "varchar(50)", nullable: false),
                    apellido_paterno = table.Column<string>(type: "varchar(50)", nullable: false),
                    carnet = table.Column<string>(type: "varchar(20)", nullable: false),
                    celular = table.Column<int>(type: "integer", nullable: true),
                    complemento_carnet = table.Column<string>(type: "varchar(5)", nullable: true),
                    completed = table.Column<bool>(type: "boolean", nullable: false),
                    direccion = table.Column<string>(type: "varchar(200)", nullable: true),
                    email_personal = table.Column<string>(type: "text", nullable: true),
                    fecha_nacimiento = table.Column<DateOnly>(type: "date", nullable: true),
                    localidad = table.Column<string>(type: "varchar(100)", nullable: true),
                    nro_cel_referencia = table.Column<string>(type: "varchar(20)", nullable: true),
                    nro_libreta_militar = table.Column<string>(type: "varchar(50)", nullable: true),
                    parentezco = table.Column<string>(type: "varchar(30)", nullable: true),
                    persona_referencia = table.Column<string>(type: "varchar(100)", nullable: true),
                    sexo = table.Column<char>(type: "character(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persona", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmpleadosContrato",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DependenciaId = table.Column<int>(type: "integer", nullable: false),
                    escalaSalarialId = table.Column<int>(type: "integer", nullable: false),
                    InmediatoSuperiorId = table.Column<int>(type: "integer", nullable: true),
                    personaId = table.Column<int>(type: "integer", nullable: false),
                    DenominacionCargo = table.Column<string>(type: "varchar(200)", nullable: false),
                    activo = table.Column<bool>(type: "boolean", nullable: false),
                    createdAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    fechaFin = table.Column<DateOnly>(type: "date", nullable: true),
                    fechaFinContrato = table.Column<DateOnly>(type: "date", nullable: true),
                    fechaInicio = table.Column<DateOnly>(type: "date", nullable: false),
                    fecha_baja = table.Column<DateOnly>(type: "date", nullable: true),
                    nro_contrato = table.Column<string>(type: "varchar(50)", nullable: false),
                    searchCargo = table.Column<NpgsqlTsVector>(type: "tsvector", nullable: false),
                    updatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmpleadosContrato", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmpleadosContrato_Dependencia_DependenciaId",
                        column: x => x.DependenciaId,
                        principalTable: "Dependencia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmpleadosContrato_EmpleadosContrato_InmediatoSuperiorId",
                        column: x => x.InmediatoSuperiorId,
                        principalTable: "EmpleadosContrato",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EmpleadosContrato_EscalaSalarial_escalaSalarialId",
                        column: x => x.escalaSalarialId,
                        principalTable: "EscalaSalarial",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmpleadosContrato_Persona_personaId",
                        column: x => x.personaId,
                        principalTable: "Persona",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PersonaId = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    email = table.Column<string>(type: "varchar(60)", nullable: false),
                    isActive = table.Column<bool>(type: "boolean", nullable: true),
                    last_login = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    username = table.Column<string>(type: "varchar(40)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuario_Persona_PersonaId",
                        column: x => x.PersonaId,
                        principalTable: "Persona",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_solicitudViajes_empleadoId",
                table: "solicitudViajes",
                column: "empleadoId");

            migrationBuilder.CreateIndex(
                name: "IX_EmpleadosContrato_DependenciaId",
                table: "EmpleadosContrato",
                column: "DependenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_EmpleadosContrato_escalaSalarialId",
                table: "EmpleadosContrato",
                column: "escalaSalarialId");

            migrationBuilder.CreateIndex(
                name: "IX_EmpleadosContrato_InmediatoSuperiorId",
                table: "EmpleadosContrato",
                column: "InmediatoSuperiorId");

            migrationBuilder.CreateIndex(
                name: "IX_EmpleadosContrato_personaId",
                table: "EmpleadosContrato",
                column: "personaId");

            migrationBuilder.CreateIndex(
                name: "Index_UserName",
                table: "Usuario",
                column: "username");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_PersonaId",
                table: "Usuario",
                column: "PersonaId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_solicitudViajes_EmpleadosContrato_empleadoId",
                table: "solicitudViajes",
                column: "empleadoId",
                principalTable: "EmpleadosContrato",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
