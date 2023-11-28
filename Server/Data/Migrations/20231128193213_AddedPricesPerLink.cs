using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceWatcher.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedPricesPerLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemLinks_ItemWatchers_ItemWatcherId",
                table: "ItemLinks");

            migrationBuilder.AlterColumn<string>(
                name: "ItemWatcherId",
                table: "ItemLinks",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "ItemLinks",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "ItemLinks",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemLinks_ItemWatchers_ItemWatcherId",
                table: "ItemLinks",
                column: "ItemWatcherId",
                principalTable: "ItemWatchers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemLinks_ItemWatchers_ItemWatcherId",
                table: "ItemLinks");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "ItemLinks");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "ItemLinks");

            migrationBuilder.AlterColumn<string>(
                name: "ItemWatcherId",
                table: "ItemLinks",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemLinks_ItemWatchers_ItemWatcherId",
                table: "ItemLinks",
                column: "ItemWatcherId",
                principalTable: "ItemWatchers",
                principalColumn: "Id");
        }
    }
}
