using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CannedNet.Migrations
{
    /// <inheritdoc />
    public partial class AddPendingJoinColumn2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "pendingJoin",
                table: "room_instances",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "pendingJoin",
                table: "room_instances");
        }
    }
}
