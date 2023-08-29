using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mag.Migrations
{
    public partial class InsertNewsTableInDataBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionHtmlEditor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionSeo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KeyWords = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Categories = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VideoAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IndexImageAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegisterDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RegisterDatePersian = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DraftTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DraftTimePersain = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WriterId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: true),
                    CountSeeNews = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "News");
        }
    }
}
