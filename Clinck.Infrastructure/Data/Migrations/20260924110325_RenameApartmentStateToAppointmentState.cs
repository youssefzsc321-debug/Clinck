using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinck.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameApartmentStateToAppointmentState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ApartmentStateState",
                table: "Appointments",
                newName: "ApartmentState");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ApartmentState",
                table: "Appointments",
                newName: "ApartmentStateState");
        }
    }
}
