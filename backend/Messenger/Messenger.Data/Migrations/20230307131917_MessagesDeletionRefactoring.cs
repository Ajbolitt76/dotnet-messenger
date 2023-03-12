using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Messenger.Data.Migrations
{
    /// <inheritdoc />
    public partial class MessagesDeletionRefactoring : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DeletedCount",
                table: "Conversations",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<List<Guid>>(
                name: "DeletedFrom",
                table: "ConversationMessages",
                type: "uuid[]",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ConversationUserStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    ConversationId = table.Column<Guid>(type: "uuid", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    ReadTo = table.Column<long>(type: "bigint", nullable: true),
                    DeletedTo = table.Column<long>(type: "bigint", nullable: true),
                    IsDeletedByUser = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversationUserStatuses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConversationUserStatuses_ConversationId_UserId",
                table: "ConversationUserStatuses",
                columns: new[] { "ConversationId", "UserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConversationUserStatuses");

            migrationBuilder.DropColumn(
                name: "DeletedCount",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "DeletedFrom",
                table: "ConversationMessages");
        }
    }
}
