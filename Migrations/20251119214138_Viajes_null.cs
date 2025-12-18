using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace siapv_backend.Migrations
{
    /// <inheritdoc />
    public partial class Viajes_null : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_solicitudViajes_docGenerados_citeId",
                table: "solicitudViajes");

            migrationBuilder.DropForeignKey(
                name: "FK_solicitudViajes_lugarDestinos_lugarOrigenId",
                table: "solicitudViajes");

            migrationBuilder.DropForeignKey(
                name: "FK_solicitudViajes_mediosTransportes_transporteId",
                table: "solicitudViajes");

            migrationBuilder.DropForeignKey(
                name: "FK_solicitudViajes_tiposViaje_tipoViajeId",
                table: "solicitudViajes");

            migrationBuilder.AlterColumn<int>(
                name: "transporteId",
                table: "solicitudViajes",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "tipoViajeId",
                table: "solicitudViajes",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "lugarOrigenId",
                table: "solicitudViajes",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "citeId",
                table: "solicitudViajes",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_solicitudViajes_docGenerados_citeId",
                table: "solicitudViajes",
                column: "citeId",
                principalTable: "docGenerados",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_solicitudViajes_lugarDestinos_lugarOrigenId",
                table: "solicitudViajes",
                column: "lugarOrigenId",
                principalTable: "lugarDestinos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_solicitudViajes_mediosTransportes_transporteId",
                table: "solicitudViajes",
                column: "transporteId",
                principalTable: "mediosTransportes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_solicitudViajes_tiposViaje_tipoViajeId",
                table: "solicitudViajes",
                column: "tipoViajeId",
                principalTable: "tiposViaje",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_solicitudViajes_docGenerados_citeId",
                table: "solicitudViajes");

            migrationBuilder.DropForeignKey(
                name: "FK_solicitudViajes_lugarDestinos_lugarOrigenId",
                table: "solicitudViajes");

            migrationBuilder.DropForeignKey(
                name: "FK_solicitudViajes_mediosTransportes_transporteId",
                table: "solicitudViajes");

            migrationBuilder.DropForeignKey(
                name: "FK_solicitudViajes_tiposViaje_tipoViajeId",
                table: "solicitudViajes");

            migrationBuilder.AlterColumn<int>(
                name: "transporteId",
                table: "solicitudViajes",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "tipoViajeId",
                table: "solicitudViajes",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "lugarOrigenId",
                table: "solicitudViajes",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "citeId",
                table: "solicitudViajes",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_solicitudViajes_docGenerados_citeId",
                table: "solicitudViajes",
                column: "citeId",
                principalTable: "docGenerados",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_solicitudViajes_lugarDestinos_lugarOrigenId",
                table: "solicitudViajes",
                column: "lugarOrigenId",
                principalTable: "lugarDestinos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_solicitudViajes_mediosTransportes_transporteId",
                table: "solicitudViajes",
                column: "transporteId",
                principalTable: "mediosTransportes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_solicitudViajes_tiposViaje_tipoViajeId",
                table: "solicitudViajes",
                column: "tipoViajeId",
                principalTable: "tiposViaje",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
