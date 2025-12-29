using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace siapv_backend.Migrations
{
    /// <inheritdoc />
    public partial class operaciones_relations_Added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "operacionId",
                table: "certificacionPOAs",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_certificacionPOAs_operacionId",
                table: "certificacionPOAs",
                column: "operacionId");

            migrationBuilder.AddForeignKey(
                name: "FK_certificacionPOAs_operacionPOAs_operacionId",
                table: "certificacionPOAs",
                column: "operacionId",
                principalTable: "operacionPOAs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_certificacionPOAs_operacionPOAs_operacionId",
                table: "certificacionPOAs");

            migrationBuilder.DropIndex(
                name: "IX_certificacionPOAs_operacionId",
                table: "certificacionPOAs");

            migrationBuilder.DropColumn(
                name: "operacionId",
                table: "certificacionPOAs");
        }
    }
}
