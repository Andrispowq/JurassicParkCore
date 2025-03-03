using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurassicPark.Core.Migrations
{
    /// <inheritdoc />
    public partial class ForgotSavedGameTabke : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnimalGroupTable_SavedGame_SavedGameId",
                table: "AnimalGroupTable");

            migrationBuilder.DropForeignKey(
                name: "FK_AnimalTable_SavedGame_SavedGameId",
                table: "AnimalTable");

            migrationBuilder.DropForeignKey(
                name: "FK_JeepTable_SavedGame_SavedGameId",
                table: "JeepTable");

            migrationBuilder.DropForeignKey(
                name: "FK_MapObjectTable_SavedGame_SavedGameId",
                table: "MapObjectTable");

            migrationBuilder.DropForeignKey(
                name: "FK_SavedGame_PositionTable_MapSizeId",
                table: "SavedGame");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionTable_SavedGame_SavedGameId",
                table: "TransactionTable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SavedGame",
                table: "SavedGame");

            migrationBuilder.RenameTable(
                name: "SavedGame",
                newName: "SavedGameTable");

            migrationBuilder.RenameIndex(
                name: "IX_SavedGame_MapSizeId",
                table: "SavedGameTable",
                newName: "IX_SavedGameTable_MapSizeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SavedGameTable",
                table: "SavedGameTable",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AnimalGroupTable_SavedGameTable_SavedGameId",
                table: "AnimalGroupTable",
                column: "SavedGameId",
                principalTable: "SavedGameTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AnimalTable_SavedGameTable_SavedGameId",
                table: "AnimalTable",
                column: "SavedGameId",
                principalTable: "SavedGameTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JeepTable_SavedGameTable_SavedGameId",
                table: "JeepTable",
                column: "SavedGameId",
                principalTable: "SavedGameTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MapObjectTable_SavedGameTable_SavedGameId",
                table: "MapObjectTable",
                column: "SavedGameId",
                principalTable: "SavedGameTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SavedGameTable_PositionTable_MapSizeId",
                table: "SavedGameTable",
                column: "MapSizeId",
                principalTable: "PositionTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionTable_SavedGameTable_SavedGameId",
                table: "TransactionTable",
                column: "SavedGameId",
                principalTable: "SavedGameTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnimalGroupTable_SavedGameTable_SavedGameId",
                table: "AnimalGroupTable");

            migrationBuilder.DropForeignKey(
                name: "FK_AnimalTable_SavedGameTable_SavedGameId",
                table: "AnimalTable");

            migrationBuilder.DropForeignKey(
                name: "FK_JeepTable_SavedGameTable_SavedGameId",
                table: "JeepTable");

            migrationBuilder.DropForeignKey(
                name: "FK_MapObjectTable_SavedGameTable_SavedGameId",
                table: "MapObjectTable");

            migrationBuilder.DropForeignKey(
                name: "FK_SavedGameTable_PositionTable_MapSizeId",
                table: "SavedGameTable");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionTable_SavedGameTable_SavedGameId",
                table: "TransactionTable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SavedGameTable",
                table: "SavedGameTable");

            migrationBuilder.RenameTable(
                name: "SavedGameTable",
                newName: "SavedGame");

            migrationBuilder.RenameIndex(
                name: "IX_SavedGameTable_MapSizeId",
                table: "SavedGame",
                newName: "IX_SavedGame_MapSizeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SavedGame",
                table: "SavedGame",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AnimalGroupTable_SavedGame_SavedGameId",
                table: "AnimalGroupTable",
                column: "SavedGameId",
                principalTable: "SavedGame",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AnimalTable_SavedGame_SavedGameId",
                table: "AnimalTable",
                column: "SavedGameId",
                principalTable: "SavedGame",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JeepTable_SavedGame_SavedGameId",
                table: "JeepTable",
                column: "SavedGameId",
                principalTable: "SavedGame",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MapObjectTable_SavedGame_SavedGameId",
                table: "MapObjectTable",
                column: "SavedGameId",
                principalTable: "SavedGame",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SavedGame_PositionTable_MapSizeId",
                table: "SavedGame",
                column: "MapSizeId",
                principalTable: "PositionTable",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionTable_SavedGame_SavedGameId",
                table: "TransactionTable",
                column: "SavedGameId",
                principalTable: "SavedGame",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
