using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurassicPark.Core.Migrations
{
    /// <inheritdoc />
    public partial class SomePendingChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MapObjectTable_MapObjectType_MapObjectTypeId",
                table: "MapObjectTable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MapObjectType",
                table: "MapObjectType");

            migrationBuilder.RenameTable(
                name: "MapObjectType",
                newName: "MapObjectTypeTable");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MapObjectTypeTable",
                table: "MapObjectTypeTable",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MapObjectTable_MapObjectTypeTable_MapObjectTypeId",
                table: "MapObjectTable",
                column: "MapObjectTypeId",
                principalTable: "MapObjectTypeTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MapObjectTable_MapObjectTypeTable_MapObjectTypeId",
                table: "MapObjectTable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MapObjectTypeTable",
                table: "MapObjectTypeTable");

            migrationBuilder.RenameTable(
                name: "MapObjectTypeTable",
                newName: "MapObjectType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MapObjectType",
                table: "MapObjectType",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MapObjectTable_MapObjectType_MapObjectTypeId",
                table: "MapObjectTable",
                column: "MapObjectTypeId",
                principalTable: "MapObjectType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
