using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mag.Migrations
{
    public partial class EditRejectFiledTBLNews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RejecteTimePersain",
                table: "News",
                newName: "RejecteNewsDate");

            migrationBuilder.RenameColumn(
                name: "RejecteTime",
                table: "News",
                newName: "RejecteNewsDatePersian");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RejecteNewsDatePersian",
                table: "News",
                newName: "RejecteTime");

            migrationBuilder.RenameColumn(
                name: "RejecteNewsDate",
                table: "News",
                newName: "RejecteTimePersain");
        }
    }
}
