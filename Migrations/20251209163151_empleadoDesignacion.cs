using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace siapv_backend.Migrations
{
    /// <inheritdoc />
    public partial class empleadoDesignacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "designadorId",
                table: "solicitudViajes",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "designadorId",
                table: "solicitudViajes");
        }
    }
}
