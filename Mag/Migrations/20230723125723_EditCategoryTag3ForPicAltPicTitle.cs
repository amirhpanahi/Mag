using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mag.Migrations
{
    public partial class EditCategoryTag3ForPicAltPicTitle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PicAlt",
                table: "CategoryTags",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PicTitle",
                table: "CategoryTags",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PicAlt",
                table: "CategoryTags");

            migrationBuilder.DropColumn(
                name: "PicTitle",
                table: "CategoryTags");
        }
    }
}
