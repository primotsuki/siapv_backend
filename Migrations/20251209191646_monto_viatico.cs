using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace siapv_backend.Migrations
{
    /// <inheritdoc />
    public partial class monto_viatico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "monto_viatico",
                table: "tiposViaje",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "monto_viatico",
                table: "tiposViaje");
        }
    }
}
