using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DrivingSchoolApi.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DrivingSchools",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DrivingSchoolName = table.Column<string>(type: "text", nullable: false),
                    SchoolAddress_PostalCode = table.Column<string>(type: "text", nullable: false),
                    SchoolAddress_City = table.Column<string>(type: "text", nullable: false),
                    SchoolAddress_Region = table.Column<string>(type: "text", nullable: false),
                    SchoolAddress_AddressLine = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    WebAddress = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrivingSchools", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Instructors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: false),
                    InstructorName_FirstName = table.Column<string>(type: "text", nullable: false),
                    InstructorName_LastName = table.Column<string>(type: "text", nullable: false),
                    EmailAddress = table.Column<string>(type: "text", nullable: false),
                    HashedPassword = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instructors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Instructors_DrivingSchools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "DrivingSchools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentName_FirstName = table.Column<string>(type: "text", nullable: false),
                    StudentName_LastName = table.Column<string>(type: "text", nullable: false),
                    EmailAddress = table.Column<string>(type: "text", nullable: false),
                    HashedPassword = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_DrivingSchools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "DrivingSchools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TheoryLessons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: false),
                    InstructorId = table.Column<Guid>(type: "uuid", nullable: false),
                    LessonDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Price_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Price_Currency = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TheoryLessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TheoryLessons_DrivingSchools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "DrivingSchools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TheoryLessons_Instructors_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Instructors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DrivingLessons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uuid", nullable: false),
                    Route_DateTimeRange_StartDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Route_DateTimeRange_EndDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Price_Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    Price_Currency = table.Column<string>(type: "text", nullable: false),
                    InstructorId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrivingLessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DrivingLessons_DrivingSchools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "DrivingSchools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DrivingLessons_Instructors_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Instructors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DrivingLessons_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentTheoryLesson",
                columns: table => new
                {
                    TheoryLessonId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentTheoryLesson", x => new { x.TheoryLessonId, x.StudentId });
                    table.ForeignKey(
                        name: "FK_StudentTheoryLesson_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentTheoryLesson_TheoryLessons_TheoryLessonId",
                        column: x => x.TheoryLessonId,
                        principalTable: "TheoryLessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CoordinatePoint",
                columns: table => new
                {
                    Order = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RouteId = table.Column<Guid>(type: "uuid", nullable: false),
                    Latitude = table.Column<float>(type: "real", nullable: false),
                    Longitude = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoordinatePoint", x => new { x.RouteId, x.Order });
                    table.ForeignKey(
                        name: "FK_CoordinatePoint_DrivingLessons_RouteId",
                        column: x => x.RouteId,
                        principalTable: "DrivingLessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DrivingLessons_InstructorId",
                table: "DrivingLessons",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_DrivingLessons_SchoolId",
                table: "DrivingLessons",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_DrivingLessons_StudentId",
                table: "DrivingLessons",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Instructors_SchoolId",
                table: "Instructors",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_SchoolId",
                table: "Students",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentTheoryLesson_StudentId",
                table: "StudentTheoryLesson",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_TheoryLessons_InstructorId",
                table: "TheoryLessons",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_TheoryLessons_SchoolId",
                table: "TheoryLessons",
                column: "SchoolId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoordinatePoint");

            migrationBuilder.DropTable(
                name: "StudentTheoryLesson");

            migrationBuilder.DropTable(
                name: "DrivingLessons");

            migrationBuilder.DropTable(
                name: "TheoryLessons");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Instructors");

            migrationBuilder.DropTable(
                name: "DrivingSchools");
        }
    }
}
