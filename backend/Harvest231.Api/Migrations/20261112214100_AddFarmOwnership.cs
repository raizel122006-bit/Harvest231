using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Harvest231.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddFarmOwnership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Farms",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Farms_OwnerId",
                table: "Farms",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Farms_AspNetUsers_OwnerId",
                table: "Farms",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Farms_AspNetUsers_OwnerId",
                table: "Farms");

            migrationBuilder.DropIndex(
                name: "IX_Farms_OwnerId",
                table: "Farms");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Farms");
        }
    }
}
