using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceWatcher.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedItemReferenceLinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemLinks",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Link = table.Column<string>(type: "TEXT", nullable: false),
                    ItemWatcherId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemLinks_ItemWatchers_ItemWatcherId",
                        column: x => x.ItemWatcherId,
                        principalTable: "ItemWatchers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemLinks_ItemWatcherId",
                table: "ItemLinks",
                column: "ItemWatcherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemLinks");
        }
    }
}
