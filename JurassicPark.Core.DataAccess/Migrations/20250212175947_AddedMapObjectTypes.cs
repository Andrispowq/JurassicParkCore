using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurassicPark.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddedMapObjectTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ResourceType",
                table: "MapObjectTypeTable",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "SavedGameId",
                table: "JeepRouteTable",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_JeepRouteTable_SavedGameId",
                table: "JeepRouteTable",
                column: "SavedGameId");

            migrationBuilder.AddForeignKey(
                name: "FK_JeepRouteTable_SavedGameTable_SavedGameId",
                table: "JeepRouteTable",
                column: "SavedGameId",
                principalTable: "SavedGameTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JeepRouteTable_SavedGameTable_SavedGameId",
                table: "JeepRouteTable");

            migrationBuilder.DropIndex(
                name: "IX_JeepRouteTable_SavedGameId",
                table: "JeepRouteTable");

            migrationBuilder.DropColumn(
                name: "ResourceType",
                table: "MapObjectTypeTable");

            migrationBuilder.DropColumn(
                name: "SavedGameId",
                table: "JeepRouteTable");
        }
    }
}
