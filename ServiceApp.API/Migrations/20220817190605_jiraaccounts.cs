using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceApp.API.Migrations
{
    public partial class jiraaccounts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: new Guid("7ac425ec-f8ce-4a74-9322-327967212ec4"));

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("ef35f28a-5c2d-41ab-badc-1b120c33583c"));

            migrationBuilder.CreateTable(
                name: "JiraCredentials",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JiraCredentials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JiraCredentials_User_Id",
                        column: x => x.Id,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "Description", "DueTime", "JiraTicketId", "ModifiedAt", "ModifiedById", "Priority", "ResponsibleUserId", "TicketStatus", "Title" },
                values: new object[] { new Guid("9dafe656-6331-492e-a322-724a4dcbf962"), new DateTime(2022, 8, 17, 21, 6, 4, 858, DateTimeKind.Local).AddTicks(9132), null, "test ticket lorem ipsum", null, "", null, null, "Lowest", null, "Open", "TestTicket123" });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "CreatedAt", "Email", "FirstName", "IsActive", "LastName", "PasswordHash", "PasswordSalt", "PhoneNumber", "RefreshToken", "RefreshTokenCreated", "RefreshTokenExpires", "ResetPasswordToken", "ResetTokenExpires", "UserType" },
                values: new object[] { new Guid("ef35f28a-5c2d-41ab-badc-1b120c33583c"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "John@gmail.com", "John", true, "Test", new byte[] { 198, 5, 225, 182, 225, 60, 36, 164, 30, 111, 15, 20, 30, 49, 246, 65, 213, 83, 37, 119, 255, 44, 250, 177, 37, 129, 195, 138, 41, 145, 255, 123, 149, 110, 25, 132, 97, 58, 15, 33, 135, 245, 192, 219, 152, 201, 40, 186, 192, 78, 162, 68, 252, 70, 23, 229, 121, 13, 103, 79, 47, 86, 150, 203 }, new byte[] { 242, 36, 213, 46, 174, 0, 248, 143, 252, 58, 211, 207, 3, 59, 188, 127, 158, 232, 185, 250, 204, 142, 75, 22, 155, 217, 0, 41, 244, 23, 190, 184, 167, 238, 198, 220, 33, 170, 126, 144, 213, 103, 209, 175, 56, 61, 206, 219, 207, 100, 80, 76, 24, 45, 11, 253, 63, 167, 251, 192, 121, 228, 209, 220, 71, 233, 168, 2, 30, 182, 39, 80, 241, 52, 115, 230, 91, 155, 102, 126, 15, 192, 184, 93, 248, 143, 173, 35, 5, 140, 177, 42, 9, 46, 4, 17, 98, 209, 111, 50, 87, 139, 3, 218, 15, 174, 65, 144, 169, 231, 40, 55, 103, 253, 31, 28, 83, 71, 9, 213, 238, 181, 128, 25, 57, 160, 42, 42 }, "", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, 1 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JiraCredentials");

            migrationBuilder.DeleteData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: new Guid("9dafe656-6331-492e-a322-724a4dcbf962"));

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "Description", "DueTime", "JiraTicketId", "ModifiedAt", "ModifiedById", "Priority", "ResponsibleUserId", "TicketStatus", "Title" },
                values: new object[] { new Guid("7ac425ec-f8ce-4a74-9322-327967212ec4"), new DateTime(2022, 8, 17, 20, 58, 42, 301, DateTimeKind.Local).AddTicks(8688), null, "test ticket lorem ipsum", null, "", null, null, "Lowest", null, "Open", "TestTicket123" });
        }
    }
}
