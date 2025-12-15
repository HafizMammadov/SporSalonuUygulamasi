using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SporSalonuUygulamasi.Migrations
{
    /// <inheritdoc />
    public partial class HizmetSalonaBaglandi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Services");

            migrationBuilder.AddColumn<int>(
                name: "GymId",
                table: "Services",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Gyms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Services_GymId",
                table: "Services",
                column: "GymId");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Gyms_GymId",
                table: "Services",
                column: "GymId",
                principalTable: "Gyms",
                principalColumn: "GymId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Services_Gyms_GymId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Services_GymId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "GymId",
                table: "Services");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Services",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Gyms",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
