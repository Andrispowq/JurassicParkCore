using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurassicPark.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddedDiscoveries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiscoveredTable",
                columns: table => new
                {
                    AnimalId = table.Column<long>(type: "INTEGER", nullable: false),
                    MapObjectId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscoveredTable", x => new { x.AnimalId, x.MapObjectId });
                    table.ForeignKey(
                        name: "FK_DiscoveredTable_AnimalTable_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "AnimalTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DiscoveredTable_MapObjectTable_MapObjectId",
                        column: x => x.MapObjectId,
                        principalTable: "MapObjectTable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiscoveredTable_MapObjectId",
                table: "DiscoveredTable",
                column: "MapObjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiscoveredTable");
        }
    }
}
