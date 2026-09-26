using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinck.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameApartmentStateToAppointmentState2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ApartmentState",
                table: "Appointments",
                newName: "AppointmentState");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AppointmentState",
                table: "Appointments",
                newName: "ApartmentState");
        }
    }
}
