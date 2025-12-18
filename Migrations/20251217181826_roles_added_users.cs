using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace siapv_backend.Migrations
{
    /// <inheritdoc />
    public partial class roles_added_users : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "userId",
                table: "userRoles",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "userId",
                table: "userRoles");
        }
    }
}
