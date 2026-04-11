using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gide.Migrations
{
    /// <inheritdoc />
    public partial class AddNameExe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "name_exe",
                table: "Game",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "name_exe",
                table: "Game");
        }
    }
}
