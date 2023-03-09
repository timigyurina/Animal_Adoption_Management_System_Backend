using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Animal_Adoption_Management_System_Backend.Migrations
{
    /// <inheritdoc />
    public partial class SeedDefaultUserAndRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "aaaa1111-82b5-4ef3-95b9-f33f6130d5ac", "10289375-cdd0-4b13-b78e-87a9a68c573f", "ShelterEmployee", "SHELTEREMPLOYEE" },
                    { "abcd1234-82b5-4ef3-95b9-f33f6130d5ac", "dddbf1a6-29df-498b-aeda-5d3d6c9d8d6a", "Administrator", "ADMINISTRATOR" },
                    { "bbbb2222-82b5-4ef3-95b9-f33f6130d5ac", "d875fc5b-b47e-40d7-9075-7011b1b454c6", "Adopter", "ADOPTER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "IsActive", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "1bdd9ba1-82b5-4ef3-95b9-f13f6150d5ac", 0, "89c915f6-2c96-46b3-b042-00c0ca8586b6", "admin@localhost.com", true, "System", true, "Admin", false, null, "ADMIN@LOCALHOST.COM", "ADMIN@LOCALHOST.COM", "AQAAAAEAACcQAAAAELUEr/Ac4IgQa6H/G9ZmqZaf7gLh323Zpbou4RTQuO9T8awoulOHYL4AuhlTegniZA==", null, false, "490efe45-5baf-403f-9f28-0500de2c28cd", false, "admin@localhost.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "abcd1234-82b5-4ef3-95b9-f33f6130d5ac", "1bdd9ba1-82b5-4ef3-95b9-f13f6150d5ac" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "aaaa1111-82b5-4ef3-95b9-f33f6130d5ac");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bbbb2222-82b5-4ef3-95b9-f33f6130d5ac");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "abcd1234-82b5-4ef3-95b9-f33f6130d5ac", "1bdd9ba1-82b5-4ef3-95b9-f13f6150d5ac" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "abcd1234-82b5-4ef3-95b9-f33f6130d5ac");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1bdd9ba1-82b5-4ef3-95b9-f13f6150d5ac");
        }
    }
}
