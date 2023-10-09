using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chirp.Razor.Migrations
{
    /// <inheritdoc />
    public partial class TestMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CheepId",
                table: "Cheeps",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Cheeps",
                newName: "CheepId");
        }
    }
}
