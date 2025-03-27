using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurassicPark.Core.Migrations
{
    /// <inheritdoc />
    public partial class RemovedNullablePositions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnimalTable_PositionTable_PositionId",
                table: "AnimalTable");

            migrationBuilder.DropForeignKey(
                name: "FK_MapObjectTable_PositionTable_PositionId",
                table: "MapObjectTable");

            migrationBuilder.AlterColumn<long>(
                name: "PositionId",
                table: "MapObjectTable",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "PositionId",
                table: "AnimalTable",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AnimalTable_PositionTable_PositionId",
                table: "AnimalTable",
                column: "PositionId",
                principalTable: "PositionTable",
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
                name: "FK_AnimalTable_PositionTable_PositionId",
                table: "AnimalTable");

            migrationBuilder.DropForeignKey(
                name: "FK_MapObjectTable_PositionTable_PositionId",
                table: "MapObjectTable");

            migrationBuilder.AlterColumn<long>(
                name: "PositionId",
                table: "MapObjectTable",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<long>(
                name: "PositionId",
                table: "AnimalTable",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_AnimalTable_PositionTable_PositionId",
                table: "AnimalTable",
                column: "PositionId",
                principalTable: "PositionTable",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MapObjectTable_PositionTable_PositionId",
                table: "MapObjectTable",
                column: "PositionId",
                principalTable: "PositionTable",
                principalColumn: "Id");
        }
    }
}
