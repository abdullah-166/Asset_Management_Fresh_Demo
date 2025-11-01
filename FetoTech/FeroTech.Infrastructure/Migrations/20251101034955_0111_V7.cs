using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FeroTech.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _0111_V7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Assets");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Assets",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
