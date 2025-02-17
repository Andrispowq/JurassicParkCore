using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurassicParkCore.Migrations
{
    /// <inheritdoc />
    public partial class AddedThings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JeepTable_PositionTable_PositionId",
                table: "JeepTable");

            migrationBuilder.DropColumn(
                name: "IsCheckpoint",
                table: "TransactionTable");

            migrationBuilder.RenameColumn(
                name: "PositionId",
                table: "JeepTable",
                newName: "RouteId");

            migrationBuilder.RenameIndex(
                name: "IX_JeepTable_PositionId",
                table: "JeepTable",
                newName: "IX_JeepTable_RouteId");

            migrationBuilder.AddColumn<int>(
                name: "GameState",
                table: "SavedGameTable",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "HoursSinceGoalMet",
                table: "SavedGameTable",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "VisitorSatisfaction",
                table: "SavedGameTable",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "JeepRouteId",
                table: "PositionTable",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RouteProgression",
                table: "JeepTable",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "JeepRouteTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JeepRouteTable", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PositionTable_JeepRouteId",
                table: "PositionTable",
                column: "JeepRouteId");

            migrationBuilder.AddForeignKey(
                name: "FK_JeepTable_JeepRouteTable_RouteId",
                table: "JeepTable",
                column: "RouteId",
                principalTable: "JeepRouteTable",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PositionTable_JeepRouteTable_JeepRouteId",
                table: "PositionTable",
                column: "JeepRouteId",
                principalTable: "JeepRouteTable",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JeepTable_JeepRouteTable_RouteId",
                table: "JeepTable");

            migrationBuilder.DropForeignKey(
                name: "FK_PositionTable_JeepRouteTable_JeepRouteId",
                table: "PositionTable");

            migrationBuilder.DropTable(
                name: "JeepRouteTable");

            migrationBuilder.DropIndex(
                name: "IX_PositionTable_JeepRouteId",
                table: "PositionTable");

            migrationBuilder.DropColumn(
                name: "GameState",
                table: "SavedGameTable");

            migrationBuilder.DropColumn(
                name: "HoursSinceGoalMet",
                table: "SavedGameTable");

            migrationBuilder.DropColumn(
                name: "VisitorSatisfaction",
                table: "SavedGameTable");

            migrationBuilder.DropColumn(
                name: "JeepRouteId",
                table: "PositionTable");

            migrationBuilder.DropColumn(
                name: "RouteProgression",
                table: "JeepTable");

            migrationBuilder.RenameColumn(
                name: "RouteId",
                table: "JeepTable",
                newName: "PositionId");

            migrationBuilder.RenameIndex(
                name: "IX_JeepTable_RouteId",
                table: "JeepTable",
                newName: "IX_JeepTable_PositionId");

            migrationBuilder.AddColumn<bool>(
                name: "IsCheckpoint",
                table: "TransactionTable",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_JeepTable_PositionTable_PositionId",
                table: "JeepTable",
                column: "PositionId",
                principalTable: "PositionTable",
                principalColumn: "Id");
        }
    }
}
