using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JurassicPark.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddedAnimalProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "AnimalTable",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "HasChip",
                table: "AnimalTable",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AnimalTable",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Sex",
                table: "AnimalTable",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "AnimalTable");

            migrationBuilder.DropColumn(
                name: "HasChip",
                table: "AnimalTable");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AnimalTable");

            migrationBuilder.DropColumn(
                name: "Sex",
                table: "AnimalTable");
        }
    }
}
