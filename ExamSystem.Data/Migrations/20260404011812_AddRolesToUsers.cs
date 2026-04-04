using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ExamSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRolesToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "users");

            migrationBuilder.AddColumn<Guid>(
                name: "RoleId",
                table: "users",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("3f2504e0-4f89-11d3-9a0c-0305e82c3301"), "Admin" },
                    { new Guid("6ba7b810-9dad-11d1-80b4-00c04fd430c8"), "Teacher" },
                    { new Guid("7d793037-a076-4c9c-a43f-4f2b8b3f7c9a"), "Student" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_users_RoleId",
                table: "users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_roles_Name",
                table: "roles",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_users_roles_RoleId",
                table: "users",
                column: "RoleId",
                principalTable: "roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_roles_RoleId",
                table: "users");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropIndex(
                name: "IX_users_RoleId",
                table: "users");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "users");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
