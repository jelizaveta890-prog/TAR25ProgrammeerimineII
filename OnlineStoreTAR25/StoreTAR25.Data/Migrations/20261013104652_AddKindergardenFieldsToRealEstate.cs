using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopTARpe25.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddKindergardenFieldsToRealEstate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChildrenCount",
                table: "RealEstates",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "KindergartenName",
                table: "RealEstates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TeacherName",
                table: "RealEstates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChildrenCount",
                table: "RealEstates");

            migrationBuilder.DropColumn(
                name: "KindergartenName",
                table: "RealEstates");

            migrationBuilder.DropColumn(
                name: "TeacherName",
                table: "RealEstates");
        }
    }
}
