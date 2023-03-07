using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Messenger.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGroupMembers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GroupChatMembers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    WasExcluded = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    WasBanned = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    MutedTill = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Permissions = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ConversationId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupChatMembers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupChatMembers_ConversationId_UserId",
                table: "GroupChatMembers",
                columns: new[] { "ConversationId", "UserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupChatMembers");
        }
    }
}
