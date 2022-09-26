using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ServiceApp.API.Migrations
{
    public partial class cleanup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DeleteData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: new Guid("8c51ae86-66ba-475f-899c-8a230bdda89d"));

            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: new Guid("ef35f28a-5c2d-41ab-badc-1b120c33583c"));

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "CreatedAt", "Description", "DueTime", "JiraTicketId", "Priority", "ResponsibleUserId", "Title" },
                values: new object[] { new Guid("0c01b6f9-9157-4311-806f-26f41dbdd62d"), new DateTime(2022, 6, 27, 22, 5, 32, 669, DateTimeKind.Local).AddTicks(3759), "test ticket lorem ipsum", null, "", (short)1, null, "TestTicket123" });

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
                keyValue: new Guid("0c01b6f9-9157-4311-806f-26f41dbdd62d"));

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    DateOfBrith = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhotoPath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_Employee_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "DepartmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Employee",
                columns: new[] { "EmployeeId", "DateOfBrith", "DepartmentId", "Email", "FirstName", "Gender", "LastName", "PhotoPath" },
                values: new object[,]
                {
                    { 1, new DateTime(1980, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "David@pragimtech.com", "John", 0, "Hastings", "images/john.png" },
                    { 2, new DateTime(1981, 12, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Sam@pragimtech.com", "Sam", 0, "Galloway", "images/sam.jpg" },
                    { 3, new DateTime(1979, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "mary@pragimtech.com", "Mary", 1, "Smith", "images/mary.png" },
                    { 4, new DateTime(1982, 9, 23, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "sara@pragimtech.com", "Sara", 1, "Longway", "images/sara.png" }
                });

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "CreatedAt", "Description", "DueTime", "JiraTicketId", "Priority", "ResponsibleUserId", "Title" },
                values: new object[] { new Guid("8c51ae86-66ba-475f-899c-8a230bdda89d"), new DateTime(2022, 6, 26, 22, 44, 29, 203, DateTimeKind.Local).AddTicks(4699), "test ticket lorem ipsum", null, "", (short)1, null, "TestTicket123" });

            migrationBuilder.CreateIndex(
                name: "IX_Employee_DepartmentId",
                table: "Employee",
                column: "DepartmentId");
        }
    }
}
