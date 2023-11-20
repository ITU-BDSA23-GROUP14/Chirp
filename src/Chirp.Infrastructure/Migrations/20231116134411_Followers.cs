using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chirp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Followers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthorAuthor",
                columns: table => new
                {
                    FollowersAuthorId = table.Column<int>(type: "int", nullable: false),
                    FollowingAuthorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorAuthor", x => new { x.FollowersAuthorId, x.FollowingAuthorId });
                    table.ForeignKey(
                        name: "FK_AuthorAuthor_Authors_FollowersAuthorId",
                        column: x => x.FollowersAuthorId,
                        principalTable: "Authors",
                        principalColumn: "AuthorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorAuthor_Authors_FollowingAuthorId",
                        column: x => x.FollowingAuthorId,
                        principalTable: "Authors",
                        principalColumn: "AuthorId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorAuthor_FollowingAuthorId",
                table: "AuthorAuthor",
                column: "FollowingAuthorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorAuthor");
        }
    }
}
