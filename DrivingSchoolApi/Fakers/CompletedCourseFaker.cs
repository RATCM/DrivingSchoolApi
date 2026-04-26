using Bogus;
using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Enums;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Fakers;

public sealed class CompletedCourseFaker : Faker<CompletedCourse>
{
    private CompletedCourseFaker() {}

    public static CompletedCourseFaker Create(
        int seed,
        IEnumerable<TheoryLesson> theoryLessons,
        IEnumerable<DrivingLesson> drivingLessons)
    {
        // This is maybe a bit more complicated than it needs to be
        // But its mainly to ensure that it creates in a similar way
        // to how it would do so on real data
        var theoryLessonsList = theoryLessons.Where(x => x.StudentId is not null).ToList();
        var drivingLessonsList = drivingLessons.Where(x => x.StudentId is not null).ToList();
        var studentSchoolIdCombinations = theoryLessonsList.Select(x => 
                (StudentId: x.StudentId!, x.SchoolId))
            .Union(drivingLessonsList.Select(x => (StudentId: x.StudentId!, x.SchoolId)))
            .ToList();
        
        var studentTheoryLessons = studentSchoolIdCombinations
            .GroupJoin(theoryLessonsList,
                x => x,
                x => (x.StudentId, x.SchoolId),
                (x, y) => new
                {
                    StudentSchoolId = x,
                    TheoryLessons = y
                }).ToList();
        
        var studentDrivingLessons = studentSchoolIdCombinations
            .GroupJoin(drivingLessonsList,
                x => x,
                x => (x.StudentId, x.SchoolId),
                (x, y) => new
                {
                    StudentSchoolId = x,
                    DrivingLessons = y
                }).ToList();

        var faker = new CompletedCourseFaker();
        faker
            .UseSeed(seed)
            .CustomInstantiator(f =>
            {
                var studentSchoolId = f.PickRandom(studentSchoolIdCombinations);

                var initialSelectedTheoryLessons = studentTheoryLessons
                    .First(x => x.StudentSchoolId.Equals(studentSchoolId)).TheoryLessons;
                var initialSelectedDrivingLessons = studentDrivingLessons
                    .First(x => x.StudentSchoolId.Equals(studentSchoolId)).DrivingLessons;

                var lessonDates = initialSelectedTheoryLessons
                    .Select(x => x.LessonDateTime)
                    .Union(initialSelectedDrivingLessons.Select(x => x.Route.DateTimeRange.EndDateTime))
                    .ToList();

                var minDate = lessonDates.Min();
                var maxDate = lessonDates.Max();
                
                var selectedTimeFrom = f.Date.Between(minDate, maxDate);
                var selectedDateTime = f.Date.Soon(days: 100, refDate: selectedTimeFrom);

                var selectedTheoryLessons = studentTheoryLessons
                    .First(x => x.StudentSchoolId.Equals(studentSchoolId))
                    .TheoryLessons
                    .Where(x => x.LessonDateTime >= selectedTimeFrom &&
                                x.LessonDateTime < selectedDateTime).ToList();
                var selectedDrivingLessons = studentDrivingLessons
                    .First(x => x.StudentSchoolId.Equals(studentSchoolId))
                    .DrivingLessons
                    .Where(x => x.Route.DateTimeRange.EndDateTime >= selectedTimeFrom &&
                                x.Route.DateTimeRange.EndDateTime < selectedDateTime).ToList();

                var totalPrice = selectedTheoryLessons.Select(x => x.Price.Amount).Sum()
                                 + selectedDrivingLessons.Select(x => x.Price.Amount).Sum();

                return CompletedCourse.Create(
                    CompletedCourseKey.Create(Guid.NewGuid()),
                    studentSchoolId.SchoolId,
                    studentSchoolId.StudentId,
                    Money.Create(totalPrice, f.Finance.Currency().Code),
                    f.Date.Between(new DateTime(2000, 1, 1), new DateTime(2020, 1, 1)),
                    f.PickRandom<CourseCompletionReason>());
            });

        return faker;
    }
}