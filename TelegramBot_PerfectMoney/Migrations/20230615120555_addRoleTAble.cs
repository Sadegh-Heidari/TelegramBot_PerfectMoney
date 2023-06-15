using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TelegramBot_PerfectMoney.Migrations
{
    public partial class addRoleTAble : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "RoleId",
                table: "Users",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "RoleModel",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Role = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreationDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleModel", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "RoleModel",
                columns: new[] { "id", "CreationDate", "Role" },
                values: new object[] { 1L, new DateTime(2023, 6, 15, 16, 35, 55, 218, DateTimeKind.Local).AddTicks(6820), "Admin" });

            migrationBuilder.InsertData(
                table: "RoleModel",
                columns: new[] { "id", "CreationDate", "Role" },
                values: new object[] { 2L, new DateTime(2023, 6, 15, 16, 35, 55, 218, DateTimeKind.Local).AddTicks(6852), "Customer" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_RoleModel_RoleId",
                table: "Users",
                column: "RoleId",
                principalTable: "RoleModel",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_RoleModel_RoleId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "RoleModel");

            migrationBuilder.DropIndex(
                name: "IX_Users_RoleId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Users");
        }
    }
}
