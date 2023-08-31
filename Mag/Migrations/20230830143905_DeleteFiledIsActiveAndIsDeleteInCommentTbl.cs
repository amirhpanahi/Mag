using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mag.Migrations
{
    public partial class DeleteFiledIsActiveAndIsDeleteInCommentTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Comments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Comments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Comments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
