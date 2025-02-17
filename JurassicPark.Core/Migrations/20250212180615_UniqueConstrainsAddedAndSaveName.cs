using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurassicParkCore.Migrations
{
    /// <inheritdoc />
    public partial class UniqueConstrainsAddedAndSaveName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "SavedGameTable",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_SavedGameTable_Name",
                table: "SavedGameTable",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MapObjectTypeTable_Name",
                table: "MapObjectTypeTable",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AnimalTypeTable_Name",
                table: "AnimalTypeTable",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SavedGameTable_Name",
                table: "SavedGameTable");

            migrationBuilder.DropIndex(
                name: "IX_MapObjectTypeTable_Name",
                table: "MapObjectTypeTable");

            migrationBuilder.DropIndex(
                name: "IX_AnimalTypeTable_Name",
                table: "AnimalTypeTable");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "SavedGameTable");
        }
    }
}
