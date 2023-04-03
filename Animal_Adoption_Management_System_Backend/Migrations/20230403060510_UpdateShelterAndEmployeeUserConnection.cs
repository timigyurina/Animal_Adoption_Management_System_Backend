using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Animal_Adoption_Management_System_Backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateShelterAndEmployeeUserConnection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shelters_AspNetUsers_ContactPersonId",
                table: "Shelters");

            migrationBuilder.DropIndex(
                name: "IX_Shelters_ContactPersonId",
                table: "Shelters");

            migrationBuilder.DropColumn(
                name: "ContactPersonId",
                table: "Shelters");

            migrationBuilder.AddColumn<bool>(
                name: "IsContactOfShelter",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ShelterId",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "aaaa1111-82b5-4ef3-95b9-f33f6130d5ac",
                column: "ConcurrencyStamp",
                value: "175b3504-1234-4e95-8b8f-5a02e15f2589");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "abcd1234-82b5-4ef3-95b9-f33f6130d5ac",
                column: "ConcurrencyStamp",
                value: "6652b107-5655-4d9f-812c-f51200dbf437");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bbbb2222-82b5-4ef3-95b9-f33f6130d5ac",
                column: "ConcurrencyStamp",
                value: "6f75cbcb-0573-448c-a7fb-ce6f62e9389a");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1bdd9ba1-82b5-4ef3-95b9-f13f6150d5ac",
                columns: new[] { "ConcurrencyStamp", "IsContactOfShelter", "PasswordHash", "SecurityStamp", "ShelterId" },
                values: new object[] { "210ebb69-53d2-46cd-9bb4-7e6f3c84d311", false, "AQAAAAEAACcQAAAAECRoF9KHAcIq2ZGtix32ZX5mOLrhaIcHDohBvRQv+pdnLvAoFoDvqJqE+hkF6yB1Eg==", "ddea045a-d74f-474e-ac61-b9e84375972e", null });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ShelterId",
                table: "AspNetUsers",
                column: "ShelterId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Shelters_ShelterId",
                table: "AspNetUsers",
                column: "ShelterId",
                principalTable: "Shelters",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Shelters_ShelterId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ShelterId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsContactOfShelter",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ShelterId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "ContactPersonId",
                table: "Shelters",
                type: "text",
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
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "d1f9cef6-bbdd-47ac-8d03-1a176e87f63d", "AQAAAAEAACcQAAAAEEx5tYvN4xpJDZX9UPtRo5p+6UkVFp4HNW3i7D2jmMGnc0/JMLjYngFbEMGmEt7o/g==", "fc224c29-273c-4206-904a-795e1bffe89b" });

            migrationBuilder.CreateIndex(
                name: "IX_Shelters_ContactPersonId",
                table: "Shelters",
                column: "ContactPersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shelters_AspNetUsers_ContactPersonId",
                table: "Shelters",
                column: "ContactPersonId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
