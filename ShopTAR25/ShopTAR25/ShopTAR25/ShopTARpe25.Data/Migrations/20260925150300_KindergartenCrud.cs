using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopTARpe25.Data.Migrations
{
    public partial class KindergartenCrud : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Spaceships",
                newName: "Kindergartens");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Kindergartens",
                newName: "GroupName");

            migrationBuilder.RenameColumn(
                name: "Classification",
                table: "Kindergartens",
                newName: "KindergartenName");

            migrationBuilder.RenameColumn(
                name: "Crew",
                table: "Kindergartens",
                newName: "ChildrenCount");

            migrationBuilder.RenameColumn(
                name: "EnginePower",
                table: "Kindergartens",
                newName: "TeacherName");

            migrationBuilder.RenameColumn(
                name: "ModifiedAt",
                table: "Kindergartens",
                newName: "UpdatedAt");

            migrationBuilder.DropColumn(
                name: "BuiltDate",
                table: "Kindergartens");

            migrationBuilder.AlterColumn<string>(
                name: "TeacherName",
                table: "Kindergartens",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TeacherName",
                table: "Kindergartens",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "BuiltDate",
                table: "Kindergartens",
                type: "datetime2",
                nullable: true);

            migrationBuilder.RenameColumn(
                name: "GroupName",
                table: "Kindergartens",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "KindergartenName",
                table: "Kindergartens",
                newName: "Classification");

            migrationBuilder.RenameColumn(
                name: "ChildrenCount",
                table: "Kindergartens",
                newName: "Crew");

            migrationBuilder.RenameColumn(
                name: "TeacherName",
                table: "Kindergartens",
                newName: "EnginePower");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Kindergartens",
                newName: "ModifiedAt");

            migrationBuilder.RenameTable(
                name: "Kindergartens",
                newName: "Spaceships");
        }
    }
}
