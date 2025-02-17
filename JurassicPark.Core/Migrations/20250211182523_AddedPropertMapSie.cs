using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurassicPark.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddedPropertMapSie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SavedGameTable_PositionTable_MapSizeId",
                table: "SavedGameTable");

            migrationBuilder.DropIndex(
                name: "IX_SavedGameTable_MapSizeId",
                table: "SavedGameTable");

            migrationBuilder.RenameColumn(
                name: "MapSizeId",
                table: "SavedGameTable",
                newName: "MapWidth");

            migrationBuilder.AddColumn<long>(
                name: "MapHeight",
                table: "SavedGameTable",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MapHeight",
                table: "SavedGameTable");

            migrationBuilder.RenameColumn(
                name: "MapWidth",
                table: "SavedGameTable",
                newName: "MapSizeId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedGameTable_MapSizeId",
                table: "SavedGameTable",
                column: "MapSizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_SavedGameTable_PositionTable_MapSizeId",
                table: "SavedGameTable",
                column: "MapSizeId",
                principalTable: "PositionTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
