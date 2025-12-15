using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SporSalonuUygulamasi.Migrations
{
    /// <inheritdoc />
    public partial class EgitmenSaatleri : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "WorkEnd",
                table: "Trainers",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "WorkStart",
                table: "Trainers",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkEnd",
                table: "Trainers");

            migrationBuilder.DropColumn(
                name: "WorkStart",
                table: "Trainers");
        }
    }
}
