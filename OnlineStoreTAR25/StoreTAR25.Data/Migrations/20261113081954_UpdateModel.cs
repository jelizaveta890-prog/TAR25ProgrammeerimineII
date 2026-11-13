using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopTARpe25.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RealEstatedId",
                table: "FileToDatabases",
                newName: "RealEstateId");

            migrationBuilder.RenameColumn(
                name: "ImageDate",
                table: "FileToDatabases",
                newName: "ImageData");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RealEstateId",
                table: "FileToDatabases",
                newName: "RealEstatedId");

            migrationBuilder.RenameColumn(
                name: "ImageData",
                table: "FileToDatabases",
                newName: "ImageDate");
        }
    }
}
