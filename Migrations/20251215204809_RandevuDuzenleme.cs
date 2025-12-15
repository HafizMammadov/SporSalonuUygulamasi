using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SporSalonuUygulamasi.Migrations
{
    /// <inheritdoc />
    public partial class RandevuDuzenleme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AppointmentId",
                table: "Appointments",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Appointments",
                newName: "AppointmentId");
        }
    }
}
