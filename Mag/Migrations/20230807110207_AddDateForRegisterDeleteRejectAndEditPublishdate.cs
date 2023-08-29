using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mag.Migrations
{
    public partial class AddDateForRegisterDeleteRejectAndEditPublishdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RegisterDatePersian",
                table: "News",
                newName: "RejecteTimePersain");

            migrationBuilder.RenameColumn(
                name: "RegisterDate",
                table: "News",
                newName: "RejecteTime");

            migrationBuilder.RenameColumn(
                name: "DraftTimePersain",
                table: "News",
                newName: "RegisterNewsDatePersian");

            migrationBuilder.RenameColumn(
                name: "DraftTime",
                table: "News",
                newName: "RegisterNewsDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeleteNewsDate",
                table: "News",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeleteNewsDatePersian",
                table: "News",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DraftNewsDate",
                table: "News",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DraftNewsDatePersian",
                table: "News",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishNewsDate",
                table: "News",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PublishNewsDatePersian",
                table: "News",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeleteNewsDate",
                table: "News");

            migrationBuilder.DropColumn(
                name: "DeleteNewsDatePersian",
                table: "News");

            migrationBuilder.DropColumn(
                name: "DraftNewsDate",
                table: "News");

            migrationBuilder.DropColumn(
                name: "DraftNewsDatePersian",
                table: "News");

            migrationBuilder.DropColumn(
                name: "PublishNewsDate",
                table: "News");

            migrationBuilder.DropColumn(
                name: "PublishNewsDatePersian",
                table: "News");

            migrationBuilder.RenameColumn(
                name: "RejecteTimePersain",
                table: "News",
                newName: "RegisterDatePersian");

            migrationBuilder.RenameColumn(
                name: "RejecteTime",
                table: "News",
                newName: "RegisterDate");

            migrationBuilder.RenameColumn(
                name: "RegisterNewsDatePersian",
                table: "News",
                newName: "DraftTimePersain");

            migrationBuilder.RenameColumn(
                name: "RegisterNewsDate",
                table: "News",
                newName: "DraftTime");
        }
    }
}
