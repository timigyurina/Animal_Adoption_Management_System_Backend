using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Animal_Adoption_Management_System_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddMorePropertiesToUserAndAnimalShelter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.AddColumn<DateTime>(
                name: "ExitDate",
                table: "AnimalShelters",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "aaaa1111-82b5-4ef3-95b9-f33f6130d5ac",
                column: "ConcurrencyStamp",
                value: "97d6c81b-7c9c-4586-beae-f7b544777adf");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "abcd1234-82b5-4ef3-95b9-f33f6130d5ac",
                column: "ConcurrencyStamp",
                value: "ddd0ef23-3dd0-4a58-b1f1-406757aec9cf");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bbbb2222-82b5-4ef3-95b9-f33f6130d5ac",
                column: "ConcurrencyStamp",
                value: "a5d45988-0dc7-4d53-81f9-639c93e7a67d");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1bdd9ba1-82b5-4ef3-95b9-f13f6150d5ac",
                columns: new[] { "ConcurrencyStamp", "DateOfBirth", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d1f9cef6-bbdd-47ac-8d03-1a176e87f63d", new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "AQAAAAEAACcQAAAAEEx5tYvN4xpJDZX9UPtRo5p+6UkVFp4HNW3i7D2jmMGnc0/JMLjYngFbEMGmEt7o/g==", "fc224c29-273c-4206-904a-795e1bffe89b" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ExitDate",
                table: "AnimalShelters");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "aaaa1111-82b5-4ef3-95b9-f33f6130d5ac",
                column: "ConcurrencyStamp",
                value: "10289375-cdd0-4b13-b78e-87a9a68c573f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "abcd1234-82b5-4ef3-95b9-f33f6130d5ac",
                column: "ConcurrencyStamp",
                value: "dddbf1a6-29df-498b-aeda-5d3d6c9d8d6a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bbbb2222-82b5-4ef3-95b9-f33f6130d5ac",
                column: "ConcurrencyStamp",
                value: "d875fc5b-b47e-40d7-9075-7011b1b454c6");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1bdd9ba1-82b5-4ef3-95b9-f13f6150d5ac",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "89c915f6-2c96-46b3-b042-00c0ca8586b6", "AQAAAAEAACcQAAAAELUEr/Ac4IgQa6H/G9ZmqZaf7gLh323Zpbou4RTQuO9T8awoulOHYL4AuhlTegniZA==", "490efe45-5baf-403f-9f28-0500de2c28cd" });
        }
    }
}
