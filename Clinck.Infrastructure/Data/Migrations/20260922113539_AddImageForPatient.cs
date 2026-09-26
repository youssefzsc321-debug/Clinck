using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinck.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddImageForPatient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageThumnail",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "ImageThumnail",
                table: "Patients");
        }
    }
}
