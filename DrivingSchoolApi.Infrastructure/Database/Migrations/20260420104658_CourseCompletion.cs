using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrivingSchoolApi.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class CourseCompletion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompletedCourses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: true),
                    Cost_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Cost_Currency = table.Column<string>(type: "text", nullable: false),
                    CompletionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Reason = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompletedCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompletedCourses_DrivingSchools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "DrivingSchools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompletedCourses_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompletedCourses_SchoolId",
                table: "CompletedCourses",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_CompletedCourses_StudentId",
                table: "CompletedCourses",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompletedCourses");
        }
    }
}
