using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CannedNet.Migrations
{
    /// <inheritdoc />
    public partial class AddPlayerCountAndLastHeartbeat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "lastHeartbeat",
                table: "room_instances",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "playerCount",
                table: "room_instances",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "lastHeartbeat",
                table: "room_instances");

            migrationBuilder.DropColumn(
                name: "playerCount",
                table: "room_instances");
        }
    }
}
