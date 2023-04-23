using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Messenger.Data.Migrations
{
    /// <inheritdoc />
    public partial class Modify : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastMessage",
                table: "Conversations",
                newName: "LastMessageDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "PersonalChatInfos",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "GroupChatInfos",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "HardDeletedCount",
                table: "Conversations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<List<Guid>>(
                name: "DeletedFrom",
                table: "ConversationMessages",
                type: "uuid[]",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "ChannelInfos",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "ConversationUserStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ConversationId = table.Column<Guid>(type: "uuid", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    ReadTo = table.Column<long>(type: "bigint", nullable: false),
                    DeletedTo = table.Column<long>(type: "bigint", nullable: true),
                    SoftDeletedCount = table.Column<long>(type: "bigint", nullable: false),
                    IsDeletedByUser = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversationUserStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonalChatMembers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ConversationId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalChatMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalChatMembers_Conversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "Conversations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonalChatMembers_MessengerUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "MessengerUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PersonalChatInfos_ConversationId_LastUpdated",
                table: "PersonalChatInfos",
                columns: new[] { "ConversationId", "LastUpdated" },
                descending: new[] { false, true });

            migrationBuilder.CreateIndex(
                name: "IX_GroupChatMembers_UserId",
                table: "GroupChatMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupChatInfos_ConversationId_LastUpdated",
                table: "GroupChatInfos",
                columns: new[] { "ConversationId", "LastUpdated" },
                descending: new[] { false, true });

            migrationBuilder.CreateIndex(
                name: "IX_ChannelMembers_UserId",
                table: "ChannelMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelInfos_ConversationId_LastUpdated",
                table: "ChannelInfos",
                columns: new[] { "ConversationId", "LastUpdated" },
                descending: new[] { false, true });

            migrationBuilder.CreateIndex(
                name: "IX_ConversationUserStatuses_ConversationId_UserId",
                table: "ConversationUserStatuses",
                columns: new[] { "ConversationId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonalChatMembers_ConversationId",
                table: "PersonalChatMembers",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalChatMembers_UserId",
                table: "PersonalChatMembers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelMembers_Conversations_ConversationId",
                table: "ChannelMembers",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChannelMembers_MessengerUsers_UserId",
                table: "ChannelMembers",
                column: "UserId",
                principalTable: "MessengerUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupChatMembers_Conversations_ConversationId",
                table: "GroupChatMembers",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupChatMembers_MessengerUsers_UserId",
                table: "GroupChatMembers",
                column: "UserId",
                principalTable: "MessengerUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChannelMembers_Conversations_ConversationId",
                table: "ChannelMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_ChannelMembers_MessengerUsers_UserId",
                table: "ChannelMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupChatMembers_Conversations_ConversationId",
                table: "GroupChatMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupChatMembers_MessengerUsers_UserId",
                table: "GroupChatMembers");

            migrationBuilder.DropTable(
                name: "ConversationUserStatuses");

            migrationBuilder.DropTable(
                name: "PersonalChatMembers");

            migrationBuilder.DropIndex(
                name: "IX_PersonalChatInfos_ConversationId_LastUpdated",
                table: "PersonalChatInfos");

            migrationBuilder.DropIndex(
                name: "IX_GroupChatMembers_UserId",
                table: "GroupChatMembers");

            migrationBuilder.DropIndex(
                name: "IX_GroupChatInfos_ConversationId_LastUpdated",
                table: "GroupChatInfos");

            migrationBuilder.DropIndex(
                name: "IX_ChannelMembers_UserId",
                table: "ChannelMembers");

            migrationBuilder.DropIndex(
                name: "IX_ChannelInfos_ConversationId_LastUpdated",
                table: "ChannelInfos");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "PersonalChatInfos");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "GroupChatInfos");

            migrationBuilder.DropColumn(
                name: "HardDeletedCount",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "DeletedFrom",
                table: "ConversationMessages");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "ChannelInfos");

            migrationBuilder.RenameColumn(
                name: "LastMessageDate",
                table: "Conversations",
                newName: "LastMessage");
        }
    }
}
