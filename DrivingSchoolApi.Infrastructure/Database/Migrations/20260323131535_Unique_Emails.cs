using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrivingSchoolApi.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Unique_Emails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Students_EmailAddress",
                table: "Students",
                column: "EmailAddress",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Instructors_EmailAddress",
                table: "Instructors",
                column: "EmailAddress",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Admins_EmailAddress",
                table: "Admins",
                column: "EmailAddress",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Students_EmailAddress",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Instructors_EmailAddress",
                table: "Instructors");

            migrationBuilder.DropIndex(
                name: "IX_Admins_EmailAddress",
                table: "Admins");
        }
    }
}
