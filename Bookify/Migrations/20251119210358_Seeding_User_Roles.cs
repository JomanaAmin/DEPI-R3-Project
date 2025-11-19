using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bookify.Migrations
{
    /// <inheritdoc />
    public partial class Seeding_User_Roles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "703eac2a-3316-4194-bb65-06b747f757ec", "703eac2a-3316-4194-bb65-06b747f757ec", "Customer", "CUSTOMER" },
                    { "a1842f43-2a1a-4ed7-ba0a-54745390d7bc", "a1842f43-2a1a-4ed7-ba0a-54745390d7bc", "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "703eac2a-3316-4194-bb65-06b747f757ec");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a1842f43-2a1a-4ed7-ba0a-54745390d7bc");
        }
    }
}
