using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coworking.APP.Migrations
{
    public partial class AddRoomDeskHourlyRates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "HourlyRate",
                table: "Rooms",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "HourlyRate",
                table: "Desks",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HourlyRate",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "HourlyRate",
                table: "Desks");
        }
    }
}
