using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mag.Migrations
{
    public partial class addNewsSummaryFildInNewsTBL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NewsSummary",
                table: "News",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NewsSummary",
                table: "News");
        }
    }
}
