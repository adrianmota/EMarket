using Microsoft.EntityFrameworkCore.Migrations;

namespace EMarket.Infrastructure.Persistence.Migrations
{
    public partial class ChangeToNameOfPropertyImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image4",
                table: "Advertisements",
                newName: "ImageUrl4");

            migrationBuilder.RenameColumn(
                name: "Image3",
                table: "Advertisements",
                newName: "ImageUrl3");

            migrationBuilder.RenameColumn(
                name: "Image2",
                table: "Advertisements",
                newName: "ImageUrl2");

            migrationBuilder.RenameColumn(
                name: "Image1",
                table: "Advertisements",
                newName: "ImageUrl1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl4",
                table: "Advertisements",
                newName: "Image4");

            migrationBuilder.RenameColumn(
                name: "ImageUrl3",
                table: "Advertisements",
                newName: "Image3");

            migrationBuilder.RenameColumn(
                name: "ImageUrl2",
                table: "Advertisements",
                newName: "Image2");

            migrationBuilder.RenameColumn(
                name: "ImageUrl1",
                table: "Advertisements",
                newName: "Image1");
        }
    }
}
