using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceApp.API.Migrations
{
    public partial class midifieduser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_User_CustomerId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_CustomerId",
                table: "Tickets");

            migrationBuilder.DeleteData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: new Guid("97b1ee4c-9d3f-4120-8fed-fcd263d818d1"));

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("ef35f28a-5c2d-41ab-badc-1b120c33583c"));

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Tickets");

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "CreatedAt", "Description", "DueTime", "JiraTicketId", "Priority", "ResponsibleUserId", "Title" },
                values: new object[] { new Guid("8c51ae86-66ba-475f-899c-8a230bdda89d"), new DateTime(2022, 6, 26, 22, 44, 29, 203, DateTimeKind.Local).AddTicks(4699), "test ticket lorem ipsum", null, "", (short)1, null, "TestTicket123" });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "Email", "FirstName", "IsActive", "LastName", "PasswordHash", "PasswordSalt", "UserType" },
                values: new object[] { new Guid("ef35f28a-5c2d-41ab-badc-1b120c33583c"), "John@gmail.com", "John", true, "Test", new byte[] { 198, 5, 225, 182, 225, 60, 36, 164, 30, 111, 15, 20, 30, 49, 246, 65, 213, 83, 37, 119, 255, 44, 250, 177, 37, 129, 195, 138, 41, 145, 255, 123, 149, 110, 25, 132, 97, 58, 15, 33, 135, 245, 192, 219, 152, 201, 40, 186, 192, 78, 162, 68, 252, 70, 23, 229, 121, 13, 103, 79, 47, 86, 150, 203 }, new byte[] { 242, 36, 213, 46, 174, 0, 248, 143, 252, 58, 211, 207, 3, 59, 188, 127, 158, 232, 185, 250, 204, 142, 75, 22, 155, 217, 0, 41, 244, 23, 190, 184, 167, 238, 198, 220, 33, 170, 126, 144, 213, 103, 209, 175, 56, 61, 206, 219, 207, 100, 80, 76, 24, 45, 11, 253, 63, 167, 251, 192, 121, 228, 209, 220, 71, 233, 168, 2, 30, 182, 39, 80, 241, 52, 115, 230, 91, 155, 102, 126, 15, 192, 184, 93, 248, 143, 173, 35, 5, 140, 177, 42, 9, 46, 4, 17, 98, 209, 111, 50, 87, 139, 3, 218, 15, 174, 65, 144, 169, 231, 40, 55, 103, 253, 31, 28, 83, 71, 9, 213, 238, 181, 128, 25, 57, 160, 42, 42 }, 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: new Guid("8c51ae86-66ba-475f-899c-8a230bdda89d"));

            migrationBuilder.AddColumn<Guid>(
                name: "CustomerId",
                table: "Tickets",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "CreatedAt", "CustomerId", "Description", "DueTime", "JiraTicketId", "Priority", "ResponsibleUserId", "Title" },
                values: new object[] { new Guid("97b1ee4c-9d3f-4120-8fed-fcd263d818d1"), new DateTime(2022, 6, 23, 22, 30, 7, 570, DateTimeKind.Local).AddTicks(1171), null, "test ticket lorem ipsum", null, "", (short)1, null, "TestTicket123" });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_CustomerId",
                table: "Tickets",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_User_CustomerId",
                table: "Tickets",
                column: "CustomerId",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
