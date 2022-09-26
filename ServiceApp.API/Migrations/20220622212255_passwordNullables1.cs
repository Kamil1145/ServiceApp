using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceApp.API.Migrations
{
    public partial class passwordNullables1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: new Guid("405b7cbc-b648-4055-b665-eaef663a94cd"));

            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordSalt",
                table: "User",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordHash",
                table: "User",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "AssignedUserId", "CreatedAt", "CustomerId", "Description", "DueTime", "JiraTicketId", "Priority", "Title" },
                values: new object[] { new Guid("b71ed573-fc95-40a6-8e2e-b4d54dce0aa1"), null, new DateTime(2022, 6, 22, 23, 22, 55, 550, DateTimeKind.Local).AddTicks(2433), null, "test ticket lorem ipsum", null, "", (short)1, "TestTicket123" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: new Guid("b71ed573-fc95-40a6-8e2e-b4d54dce0aa1"));

            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordSalt",
                table: "User",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordHash",
                table: "User",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "AssignedUserId", "CreatedAt", "CustomerId", "Description", "DueTime", "JiraTicketId", "Priority", "Title" },
                values: new object[] { new Guid("405b7cbc-b648-4055-b665-eaef663a94cd"), null, new DateTime(2022, 6, 22, 23, 21, 29, 270, DateTimeKind.Local).AddTicks(2210), null, "test ticket lorem ipsum", null, "", (short)1, "TestTicket123" });
        }
    }
}
