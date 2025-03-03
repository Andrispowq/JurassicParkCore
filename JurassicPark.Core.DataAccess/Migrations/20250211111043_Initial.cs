using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurassicPark.Core.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnimalTypeTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EatingHabit = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    VisitorSatisfaction = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalTypeTable", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PositionTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    X = table.Column<double>(type: "REAL", nullable: false),
                    Y = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PositionTable", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SavedGame",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Difficulty = table.Column<int>(type: "INTEGER", nullable: false),
                    MapSizeId = table.Column<int>(type: "INTEGER", nullable: false),
                    MapSeed = table.Column<string>(type: "TEXT", nullable: false),
                    TimeOfDay = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    DaysPassed = table.Column<int>(type: "INTEGER", nullable: false),
                    GameSpeed = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedGame", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SavedGame_PositionTable_MapSizeId",
                        column: x => x.MapSizeId,
                        principalTable: "PositionTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnimalGroupTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SavedGameId = table.Column<int>(type: "INTEGER", nullable: false),
                    NextPointOfInterestId = table.Column<int>(type: "INTEGER", nullable: true),
                    GroupTypeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalGroupTable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnimalGroupTable_AnimalTypeTable_GroupTypeId",
                        column: x => x.GroupTypeId,
                        principalTable: "AnimalTypeTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimalGroupTable_PositionTable_NextPointOfInterestId",
                        column: x => x.NextPointOfInterestId,
                        principalTable: "PositionTable",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AnimalGroupTable_SavedGame_SavedGameId",
                        column: x => x.SavedGameId,
                        principalTable: "SavedGame",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JeepTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SavedGameId = table.Column<int>(type: "INTEGER", nullable: false),
                    SeatedVisitors = table.Column<int>(type: "INTEGER", nullable: false),
                    PositionId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JeepTable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JeepTable_PositionTable_PositionId",
                        column: x => x.PositionId,
                        principalTable: "PositionTable",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_JeepTable_SavedGame_SavedGameId",
                        column: x => x.SavedGameId,
                        principalTable: "SavedGame",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MapObjectTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SavedGameId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MapObjectTable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MapObjectTable_SavedGame_SavedGameId",
                        column: x => x.SavedGameId,
                        principalTable: "SavedGame",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransactionTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SavedGameId = table.Column<int>(type: "INTEGER", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    IsCheckpoint = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionTable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionTable_SavedGame_SavedGameId",
                        column: x => x.SavedGameId,
                        principalTable: "SavedGame",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AnimalTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SavedGameId = table.Column<int>(type: "INTEGER", nullable: false),
                    AnimalTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    PositionId = table.Column<int>(type: "INTEGER", nullable: true),
                    HungerLevel = table.Column<decimal>(type: "TEXT", nullable: false),
                    ThirstLevel = table.Column<decimal>(type: "TEXT", nullable: false),
                    Health = table.Column<decimal>(type: "TEXT", nullable: false),
                    GroupId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalTable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnimalTable_AnimalGroupTable_GroupId",
                        column: x => x.GroupId,
                        principalTable: "AnimalGroupTable",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AnimalTable_AnimalTypeTable_AnimalTypeId",
                        column: x => x.AnimalTypeId,
                        principalTable: "AnimalTypeTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AnimalTable_PositionTable_PositionId",
                        column: x => x.PositionId,
                        principalTable: "PositionTable",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AnimalTable_SavedGame_SavedGameId",
                        column: x => x.SavedGameId,
                        principalTable: "SavedGame",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnimalGroupTable_GroupTypeId",
                table: "AnimalGroupTable",
                column: "GroupTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalGroupTable_NextPointOfInterestId",
                table: "AnimalGroupTable",
                column: "NextPointOfInterestId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalGroupTable_SavedGameId",
                table: "AnimalGroupTable",
                column: "SavedGameId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalTable_AnimalTypeId",
                table: "AnimalTable",
                column: "AnimalTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalTable_GroupId",
                table: "AnimalTable",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalTable_PositionId",
                table: "AnimalTable",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_AnimalTable_SavedGameId",
                table: "AnimalTable",
                column: "SavedGameId");

            migrationBuilder.CreateIndex(
                name: "IX_JeepTable_PositionId",
                table: "JeepTable",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_JeepTable_SavedGameId",
                table: "JeepTable",
                column: "SavedGameId");

            migrationBuilder.CreateIndex(
                name: "IX_MapObjectTable_SavedGameId",
                table: "MapObjectTable",
                column: "SavedGameId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedGame_MapSizeId",
                table: "SavedGame",
                column: "MapSizeId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionTable_SavedGameId",
                table: "TransactionTable",
                column: "SavedGameId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnimalTable");

            migrationBuilder.DropTable(
                name: "JeepTable");

            migrationBuilder.DropTable(
                name: "MapObjectTable");

            migrationBuilder.DropTable(
                name: "TransactionTable");

            migrationBuilder.DropTable(
                name: "AnimalGroupTable");

            migrationBuilder.DropTable(
                name: "AnimalTypeTable");

            migrationBuilder.DropTable(
                name: "SavedGame");

            migrationBuilder.DropTable(
                name: "PositionTable");
        }
    }
}
