using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Messenger.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserStatusAndSubscriptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "MessengerUsers",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UsersSubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Plan = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersSubscriptions_MessengerUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "MessengerUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConversationMessages_SenderId",
                table: "ConversationMessages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersSubscriptions_UserId",
                table: "UsersSubscriptions",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ConversationMessages_MessengerUsers_SenderId",
                table: "ConversationMessages",
                column: "SenderId",
                principalTable: "MessengerUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConversationMessages_MessengerUsers_SenderId",
                table: "ConversationMessages");

            migrationBuilder.DropTable(
                name: "UsersSubscriptions");

            migrationBuilder.DropIndex(
                name: "IX_ConversationMessages_SenderId",
                table: "ConversationMessages");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "MessengerUsers");
        }
    }
}
