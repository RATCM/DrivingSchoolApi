using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrivingSchoolApi.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class TheoryLessonReconfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DrivingLessons_Instructors_InstructorId",
                table: "DrivingLessons");

            migrationBuilder.DropForeignKey(
                name: "FK_DrivingLessons_Students_StudentId",
                table: "DrivingLessons");

            migrationBuilder.DropForeignKey(
                name: "FK_TheoryLessons_Instructors_InstructorId",
                table: "TheoryLessons");

            migrationBuilder.DropTable(
                name: "StudentTheoryLesson");

            migrationBuilder.AlterColumn<Guid>(
                name: "InstructorId",
                table: "TheoryLessons",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<byte[]>(
                name: "InstructorSignature",
                table: "TheoryLessons",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<Guid>(
                name: "StudentId",
                table: "TheoryLessons",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "StudentSignature",
                table: "TheoryLessons",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AlterColumn<Guid>(
                name: "StudentId",
                table: "DrivingLessons",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "InstructorId",
                table: "DrivingLessons",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateIndex(
                name: "IX_TheoryLessons_StudentId",
                table: "TheoryLessons",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_DrivingLessons_Instructors_InstructorId",
                table: "DrivingLessons",
                column: "InstructorId",
                principalTable: "Instructors",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_DrivingLessons_Students_StudentId",
                table: "DrivingLessons",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_TheoryLessons_Instructors_InstructorId",
                table: "TheoryLessons",
                column: "InstructorId",
                principalTable: "Instructors",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_TheoryLessons_Students_StudentId",
                table: "TheoryLessons",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DrivingLessons_Instructors_InstructorId",
                table: "DrivingLessons");

            migrationBuilder.DropForeignKey(
                name: "FK_DrivingLessons_Students_StudentId",
                table: "DrivingLessons");

            migrationBuilder.DropForeignKey(
                name: "FK_TheoryLessons_Instructors_InstructorId",
                table: "TheoryLessons");

            migrationBuilder.DropForeignKey(
                name: "FK_TheoryLessons_Students_StudentId",
                table: "TheoryLessons");

            migrationBuilder.DropIndex(
                name: "IX_TheoryLessons_StudentId",
                table: "TheoryLessons");

            migrationBuilder.DropColumn(
                name: "InstructorSignature",
                table: "TheoryLessons");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "TheoryLessons");

            migrationBuilder.DropColumn(
                name: "StudentSignature",
                table: "TheoryLessons");

            migrationBuilder.AlterColumn<Guid>(
                name: "InstructorId",
                table: "TheoryLessons",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "StudentId",
                table: "DrivingLessons",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "InstructorId",
                table: "DrivingLessons",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_StudentTheoryLesson_StudentId",
                table: "StudentTheoryLesson",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_DrivingLessons_Instructors_InstructorId",
                table: "DrivingLessons",
                column: "InstructorId",
                principalTable: "Instructors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DrivingLessons_Students_StudentId",
                table: "DrivingLessons",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TheoryLessons_Instructors_InstructorId",
                table: "TheoryLessons",
                column: "InstructorId",
                principalTable: "Instructors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
