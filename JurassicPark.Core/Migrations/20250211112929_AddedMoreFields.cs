using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurassicParkCore.Migrations
{
    /// <inheritdoc />
    public partial class AddedMoreFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MapObjectTypeId",
                table: "MapObjectTable",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PositionId",
                table: "MapObjectTable",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "ResourceAmount",
                table: "MapObjectTable",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AnimalTypeTable",
                type: "TEXT",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "MapObjectType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    ResourceAmount = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapObjectType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MapObjectTable_MapObjectTypeId",
                table: "MapObjectTable",
                column: "MapObjectTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MapObjectTable_PositionId",
                table: "MapObjectTable",
                column: "PositionId");

            migrationBuilder.AddForeignKey(
                name: "FK_MapObjectTable_MapObjectType_MapObjectTypeId",
                table: "MapObjectTable",
                column: "MapObjectTypeId",
                principalTable: "MapObjectType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MapObjectTable_PositionTable_PositionId",
                table: "MapObjectTable",
                column: "PositionId",
                principalTable: "PositionTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MapObjectTable_MapObjectType_MapObjectTypeId",
                table: "MapObjectTable");

            migrationBuilder.DropForeignKey(
                name: "FK_MapObjectTable_PositionTable_PositionId",
                table: "MapObjectTable");

            migrationBuilder.DropTable(
                name: "MapObjectType");

            migrationBuilder.DropIndex(
                name: "IX_MapObjectTable_MapObjectTypeId",
                table: "MapObjectTable");

            migrationBuilder.DropIndex(
                name: "IX_MapObjectTable_PositionId",
                table: "MapObjectTable");

            migrationBuilder.DropColumn(
                name: "MapObjectTypeId",
                table: "MapObjectTable");

            migrationBuilder.DropColumn(
                name: "PositionId",
                table: "MapObjectTable");

            migrationBuilder.DropColumn(
                name: "ResourceAmount",
                table: "MapObjectTable");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AnimalTypeTable");
        }
    }
}
