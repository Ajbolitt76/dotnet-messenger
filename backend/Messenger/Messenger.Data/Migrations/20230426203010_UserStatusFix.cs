using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Messenger.Data.Migrations
{
    /// <inheritdoc />
    public partial class UserStatusFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "ConversationUserStatuses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "SoftDeletedCount",
                table: "ConversationUserStatuses",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "ReadTo",
                table: "ConversationUserStatuses",
                type: "integer",
                nullable: false,
                defaultValue: -1,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<Guid>(
                name: "ConversationId",
                table: "ConversationUserStatuses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConversationMessages_SenderId",
                table: "ConversationMessages",
                column: "SenderId");

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

            migrationBuilder.DropIndex(
                name: "IX_ConversationMessages_SenderId",
                table: "ConversationMessages");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "ConversationUserStatuses",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<long>(
                name: "SoftDeletedCount",
                table: "ConversationUserStatuses",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldDefaultValue: 0L);

            migrationBuilder.AlterColumn<long>(
                name: "ReadTo",
                table: "ConversationUserStatuses",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: -1);

            migrationBuilder.AlterColumn<Guid>(
                name: "ConversationId",
                table: "ConversationUserStatuses",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");
        }
    }
}
