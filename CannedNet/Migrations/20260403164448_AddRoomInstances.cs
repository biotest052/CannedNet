using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CannedNet.Migrations
{
    /// <inheritdoc />
    public partial class AddRoomInstances : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "room_instances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OwnerAccountId = table.Column<int>(type: "integer", nullable: false),
                    roomInstanceId = table.Column<int>(type: "integer", nullable: false),
                    roomId = table.Column<int>(type: "integer", nullable: false),
                    subRoomId = table.Column<int>(type: "integer", nullable: false),
                    roomInstanceType = table.Column<int>(type: "integer", nullable: false),
                    location = table.Column<string>(type: "text", nullable: false),
                    dataBlob = table.Column<string>(type: "text", nullable: false),
                    eventId = table.Column<int>(type: "integer", nullable: false),
                    clubId = table.Column<int>(type: "integer", nullable: false),
                    roomCode = table.Column<string>(type: "text", nullable: false),
                    photonRegionId = table.Column<string>(type: "text", nullable: false),
                    photonRoomId = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    maxCapacity = table.Column<int>(type: "integer", nullable: false),
                    isFull = table.Column<bool>(type: "boolean", nullable: false),
                    isPrivate = table.Column<bool>(type: "boolean", nullable: false),
                    isInProgress = table.Column<bool>(type: "boolean", nullable: false),
                    EncryptVoiceChat = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_room_instances", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_room_instances_OwnerAccountId",
                table: "room_instances",
                column: "OwnerAccountId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "room_instances");
        }
    }
}
