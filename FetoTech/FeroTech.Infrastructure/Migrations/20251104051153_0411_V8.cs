using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FeroTech.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _0411_V8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "QRCodes");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Assets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_QRCodes_AssetId",
                table: "QRCodes",
                column: "AssetId");

            migrationBuilder.AddForeignKey(
                name: "FK_QRCodes_Assets_AssetId",
                table: "QRCodes",
                column: "AssetId",
                principalTable: "Assets",
                principalColumn: "AssetId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QRCodes_Assets_AssetId",
                table: "QRCodes");

            migrationBuilder.DropIndex(
                name: "IX_QRCodes_AssetId",
                table: "QRCodes");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Assets");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "QRCodes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
