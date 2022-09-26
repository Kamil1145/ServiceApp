using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceApp.API.Migrations
{
    public partial class passwordNullables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: new Guid("7b56d122-32b9-43f6-8a49-8ae94b5524cc"));

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "AssignedUserId", "CreatedAt", "CustomerId", "Description", "DueTime", "JiraTicketId", "Priority", "Title" },
                values: new object[] { new Guid("405b7cbc-b648-4055-b665-eaef663a94cd"), null, new DateTime(2022, 6, 22, 23, 21, 29, 270, DateTimeKind.Local).AddTicks(2210), null, "test ticket lorem ipsum", null, "", (short)1, "TestTicket123" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: new Guid("405b7cbc-b648-4055-b665-eaef663a94cd"));

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "AssignedUserId", "CreatedAt", "CustomerId", "Description", "DueTime", "JiraTicketId", "Priority", "Title" },
                values: new object[] { new Guid("7b56d122-32b9-43f6-8a49-8ae94b5524cc"), null, new DateTime(2022, 6, 22, 22, 41, 57, 199, DateTimeKind.Local).AddTicks(782), null, "test ticket lorem ipsum", null, "", (short)1, "TestTicket123" });
        }
    }
}
