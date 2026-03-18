using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrivingSchoolApi.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Renaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SchoolAddress_Region",
                table: "DrivingSchools",
                newName: "StreetAddress_Region");

            migrationBuilder.RenameColumn(
                name: "SchoolAddress_PostalCode",
                table: "DrivingSchools",
                newName: "StreetAddress_PostalCode");

            migrationBuilder.RenameColumn(
                name: "SchoolAddress_City",
                table: "DrivingSchools",
                newName: "StreetAddress_City");

            migrationBuilder.RenameColumn(
                name: "SchoolAddress_AddressLine",
                table: "DrivingSchools",
                newName: "StreetAddress_AddressLine");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StreetAddress_Region",
                table: "DrivingSchools",
                newName: "SchoolAddress_Region");

            migrationBuilder.RenameColumn(
                name: "StreetAddress_PostalCode",
                table: "DrivingSchools",
                newName: "SchoolAddress_PostalCode");

            migrationBuilder.RenameColumn(
                name: "StreetAddress_City",
                table: "DrivingSchools",
                newName: "SchoolAddress_City");

            migrationBuilder.RenameColumn(
                name: "StreetAddress_AddressLine",
                table: "DrivingSchools",
                newName: "SchoolAddress_AddressLine");
        }
    }
}
