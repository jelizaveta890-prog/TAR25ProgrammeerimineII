using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopTARpe25.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSpaceshipModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BuildDate",
                table: "Spaceships",
                newName: "BuiltDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BuiltDate",
                table: "Spaceships",
                newName: "BuildDate");
        }
    }
}
