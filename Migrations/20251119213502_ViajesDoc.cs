using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using NpgsqlTypes;

#nullable disable

namespace siapv_backend.Migrations
{
    /// <inheritdoc />
    public partial class ViajesDoc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dependencia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    descripcion = table.Column<string>(type: "varchar(100)", nullable: false),
                    sigla = table.Column<string>(type: "varchar(30)", nullable: false),
                    dependenciaId = table.Column<int>(type: "integer", nullable: true),
                    activo = table.Column<bool>(type: "boolean", nullable: false),
                    createAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dependencia", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "docGenerados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cite_documento = table.Column<string>(type: "varchar(30)", nullable: false),
                    id_documento = table.Column<int>(type: "integer", nullable: false),
                    createdAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_docGenerados", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EscalaSalarial",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nivelSalarial = table.Column<int>(type: "integer", nullable: false),
                    denominacion = table.Column<string>(type: "text", nullable: false),
                    haber_mensual = table.Column<string>(type: "text", nullable: false),
                    ResolucionEscalaId = table.Column<int>(type: "integer", nullable: false),
                    activo = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EscalaSalarial", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "lugarDestinos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    destino = table.Column<string>(type: "varchar(60)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lugarDestinos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Persona",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombres = table.Column<string>(type: "varchar(50)", nullable: false),
                    apellido_paterno = table.Column<string>(type: "varchar(50)", nullable: false),
                    apellido_materno = table.Column<string>(type: "varchar(50)", nullable: false),
                    carnet = table.Column<string>(type: "varchar(20)", nullable: false),
                    complemento_carnet = table.Column<string>(type: "varchar(5)", nullable: true),
                    fecha_nacimiento = table.Column<DateOnly>(type: "date", nullable: true),
                    sexo = table.Column<char>(type: "character(1)", nullable: true),
                    celular = table.Column<int>(type: "integer", nullable: true),
                    localidad = table.Column<string>(type: "varchar(100)", nullable: true),
                    direccion = table.Column<string>(type: "varchar(200)", nullable: true),
                    email_personal = table.Column<string>(type: "text", nullable: true),
                    nro_libreta_militar = table.Column<string>(type: "varchar(50)", nullable: true),
                    persona_referencia = table.Column<string>(type: "varchar(100)", nullable: true),
                    parentezco = table.Column<string>(type: "varchar(30)", nullable: true),
                    nro_cel_referencia = table.Column<string>(type: "varchar(20)", nullable: true),
                    completed = table.Column<bool>(type: "boolean", nullable: false),
                    SearchVector = table.Column<NpgsqlTsVector>(type: "tsvector", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persona", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tiposViaje",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    tipo = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tiposViaje", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmpleadosContrato",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DenominacionCargo = table.Column<string>(type: "varchar(200)", nullable: false),
                    nro_contrato = table.Column<string>(type: "varchar(50)", nullable: false),
                    fechaInicio = table.Column<DateOnly>(type: "date", nullable: false),
                    fechaFinContrato = table.Column<DateOnly>(type: "date", nullable: true),
                    fechaFin = table.Column<DateOnly>(type: "date", nullable: true),
                    DependenciaId = table.Column<int>(type: "integer", nullable: false),
                    InmediatoSuperiorId = table.Column<int>(type: "integer", nullable: true),
                    escalaSalarialId = table.Column<int>(type: "integer", nullable: false),
                    personaId = table.Column<int>(type: "integer", nullable: false),
                    activo = table.Column<bool>(type: "boolean", nullable: false),
                    createdAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    searchCargo = table.Column<NpgsqlTsVector>(type: "tsvector", nullable: false),
                    fecha_baja = table.Column<DateOnly>(type: "date", nullable: true)
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
                    username = table.Column<string>(type: "varchar(40)", nullable: false),
                    email = table.Column<string>(type: "varchar(60)", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PersonaId = table.Column<int>(type: "integer", nullable: false),
                    last_login = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    isActive = table.Column<bool>(type: "boolean", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "solicitudViajes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fechaInicio = table.Column<DateOnly>(type: "date", nullable: false),
                    fechaFin = table.Column<DateOnly>(type: "date", nullable: false),
                    horaInicio = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    horaFin = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    descripcion_viaje = table.Column<string>(type: "text", nullable: false),
                    cite_memo = table.Column<string>(type: "varchar(30)", nullable: false),
                    citeId = table.Column<int>(type: "integer", nullable: false),
                    proyectoId = table.Column<int>(type: "integer", nullable: false),
                    empleadoId = table.Column<int>(type: "integer", nullable: false),
                    lugarOrigenId = table.Column<int>(type: "integer", nullable: false),
                    lugarDestinoId = table.Column<int>(type: "integer", nullable: false),
                    fuenteId = table.Column<int>(type: "integer", nullable: false),
                    transporteId = table.Column<int>(type: "integer", nullable: false),
                    tipoViajeId = table.Column<int>(type: "integer", nullable: false),
                    createdAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_solicitudViajes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_solicitudViajes_EmpleadosContrato_empleadoId",
                        column: x => x.empleadoId,
                        principalTable: "EmpleadosContrato",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_solicitudViajes_docGenerados_citeId",
                        column: x => x.citeId,
                        principalTable: "docGenerados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_solicitudViajes_fuenteFinanciamientos_fuenteId",
                        column: x => x.fuenteId,
                        principalTable: "fuenteFinanciamientos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_solicitudViajes_lugarDestinos_lugarDestinoId",
                        column: x => x.lugarDestinoId,
                        principalTable: "lugarDestinos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_solicitudViajes_lugarDestinos_lugarOrigenId",
                        column: x => x.lugarOrigenId,
                        principalTable: "lugarDestinos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_solicitudViajes_mediosTransportes_transporteId",
                        column: x => x.transporteId,
                        principalTable: "mediosTransportes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_solicitudViajes_proyectos_proyectoId",
                        column: x => x.proyectoId,
                        principalTable: "proyectos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_solicitudViajes_tiposViaje_tipoViajeId",
                        column: x => x.tipoViajeId,
                        principalTable: "tiposViaje",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_solicitudViajes_citeId",
                table: "solicitudViajes",
                column: "citeId");

            migrationBuilder.CreateIndex(
                name: "IX_solicitudViajes_empleadoId",
                table: "solicitudViajes",
                column: "empleadoId");

            migrationBuilder.CreateIndex(
                name: "IX_solicitudViajes_fuenteId",
                table: "solicitudViajes",
                column: "fuenteId");

            migrationBuilder.CreateIndex(
                name: "IX_solicitudViajes_lugarDestinoId",
                table: "solicitudViajes",
                column: "lugarDestinoId");

            migrationBuilder.CreateIndex(
                name: "IX_solicitudViajes_lugarOrigenId",
                table: "solicitudViajes",
                column: "lugarOrigenId");

            migrationBuilder.CreateIndex(
                name: "IX_solicitudViajes_proyectoId",
                table: "solicitudViajes",
                column: "proyectoId");

            migrationBuilder.CreateIndex(
                name: "IX_solicitudViajes_tipoViajeId",
                table: "solicitudViajes",
                column: "tipoViajeId");

            migrationBuilder.CreateIndex(
                name: "IX_solicitudViajes_transporteId",
                table: "solicitudViajes",
                column: "transporteId");

            migrationBuilder.CreateIndex(
                name: "Index_UserName",
                table: "Usuario",
                column: "username");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_PersonaId",
                table: "Usuario",
                column: "PersonaId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "solicitudViajes");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "EmpleadosContrato");

            migrationBuilder.DropTable(
                name: "docGenerados");

            migrationBuilder.DropTable(
                name: "lugarDestinos");

            migrationBuilder.DropTable(
                name: "tiposViaje");

            migrationBuilder.DropTable(
                name: "Dependencia");

            migrationBuilder.DropTable(
                name: "EscalaSalarial");

            migrationBuilder.DropTable(
                name: "Persona");
        }
    }
}
