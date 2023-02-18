using System;
using System.Net;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Messenger.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFileEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "RepetUsers");

            migrationBuilder.RenameIndex(
                name: "IX_Users_PhoneNumber",
                table: "RepetUsers",
                newName: "IX_RepetUsers_PhoneNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Users_IdentityUserId",
                table: "RepetUsers",
                newName: "IX_RepetUsers_IdentityUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RepetUsers",
                table: "RepetUsers",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FileLocation = table.Column<string>(type: "jsonb", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatorIp = table.Column<IPAddress>(type: "inet", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Files_RepetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "RepetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UploadingFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IsFinished = table.Column<bool>(type: "boolean", nullable: false),
                    TusId = table.Column<string>(type: "text", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: false),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatorIp = table.Column<IPAddress>(type: "inet", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadingFiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UploadingFiles_RepetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "RepetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Files_CreatedById",
                table: "Files",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UploadingFiles_CreatedById",
                table: "UploadingFiles",
                column: "CreatedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "UploadingFiles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RepetUsers",
                table: "RepetUsers");

            migrationBuilder.RenameTable(
                name: "RepetUsers",
                newName: "Users");

            migrationBuilder.RenameIndex(
                name: "IX_RepetUsers_PhoneNumber",
                table: "Users",
                newName: "IX_Users_PhoneNumber");

            migrationBuilder.RenameIndex(
                name: "IX_RepetUsers_IdentityUserId",
                table: "Users",
                newName: "IX_Users_IdentityUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");
        }
    }
}
