using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mag.Migrations
{
    public partial class EditUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PicByte",
                table: "AspNetUsers",
                newName: "PicTitle");

            migrationBuilder.AddColumn<string>(
                name: "PicAlt",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PicAlt",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "PicTitle",
                table: "AspNetUsers",
                newName: "PicByte");
        }
    }
}
