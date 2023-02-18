using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Messenger.Core.Model.UserAggregate;

#nullable disable

namespace Messenger.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldsToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:gender", "not_stated,male,female,other")
                .Annotation("Npgsql:Enum:user_account_type", "teacher,student")
                .OldAnnotation("Npgsql:Enum:user_account_type", "teacher,student");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Gender>(
                name: "Gender",
                table: "Users",
                type: "gender",
                nullable: false,
                defaultValue: Gender.NotStated);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Users");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:user_account_type", "teacher,student")
                .OldAnnotation("Npgsql:Enum:gender", "not_stated,male,female,other")
                .OldAnnotation("Npgsql:Enum:user_account_type", "teacher,student");
        }
    }
}
