using Microsoft.EntityFrameworkCore.Migrations;

namespace Online_Book_Store.BookStore.Data.Migrations
{
    public partial class RelitionShio : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "Books",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "Books");
        }
    }
}
