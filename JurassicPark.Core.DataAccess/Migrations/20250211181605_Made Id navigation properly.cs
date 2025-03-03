using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurassicPark.Core.Migrations
{
    /// <inheritdoc />
    public partial class MadeIdnavigationproperly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnimalGroupTable_PositionTable_NextPointOfInterestId",
                table: "AnimalGroupTable");

            migrationBuilder.RenameColumn(
                name: "NextPointOfInterestId",
                table: "AnimalGroupTable",
                newName: "PositionId");

            migrationBuilder.RenameIndex(
                name: "IX_AnimalGroupTable_NextPointOfInterestId",
                table: "AnimalGroupTable",
                newName: "IX_AnimalGroupTable_PositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnimalGroupTable_PositionTable_PositionId",
                table: "AnimalGroupTable",
                column: "PositionId",
                principalTable: "PositionTable",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnimalGroupTable_PositionTable_PositionId",
                table: "AnimalGroupTable");

            migrationBuilder.RenameColumn(
                name: "PositionId",
                table: "AnimalGroupTable",
                newName: "NextPointOfInterestId");

            migrationBuilder.RenameIndex(
                name: "IX_AnimalGroupTable_PositionId",
                table: "AnimalGroupTable",
                newName: "IX_AnimalGroupTable_NextPointOfInterestId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnimalGroupTable_PositionTable_NextPointOfInterestId",
                table: "AnimalGroupTable",
                column: "NextPointOfInterestId",
                principalTable: "PositionTable",
                principalColumn: "Id");
        }
    }
}
