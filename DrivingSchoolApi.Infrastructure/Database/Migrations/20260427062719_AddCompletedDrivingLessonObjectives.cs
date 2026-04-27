using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DrivingSchoolApi.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddCompletedDrivingLessonObjectives : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompletedObjective",
                table: "DrivingLessons",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedObjective",
                table: "DrivingLessons");
        }
    }
}
