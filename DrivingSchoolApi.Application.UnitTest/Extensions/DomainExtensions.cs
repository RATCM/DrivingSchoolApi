using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.ValueObjects;

namespace DrivingSchoolApi.Application.UnitTest.Extensions;

internal static class DomainExtensions
{
    extension(DrivingLesson _)
    {
        /// <summary>
        /// Generates a driving school with random values 
        /// </summary>
        /// <param name="instructorId">The Instructor id</param>
        /// <param name="studentId">The student id</param>
        /// <param name="schoolId">The driving school id</param>
        /// <param name="id">The driving lesson id</param>
        /// <param name="seed">The seed to use for generating the random values</param>
        /// <returns>The randomly generated driving school</returns>
        internal static DrivingLesson GenerateRandom(
            InstructorKey instructorId, 
            StudentKey studentId, 
            DrivingSchoolKey schoolId, 
            Guid id = default, 
            int seed = 0)
        {
            Random rng = new Random(seed);
            var letters = "abcdefghijklmnopqrstuvwxyz";
            return DrivingLesson.Create(
                DrivingLessonKey.Create(id),
                schoolId,
                DrivingRoute.Create(
                    DateTimeRange.Create(
                        new DateTime(2000, 1, 1),
                        new DateTime(2000, 1, 1).AddMinutes(45 * (rng.NextDouble() + rng.NextDouble() + 1))),
                    Enumerable.Range(1, rng.Next(2, 10)).Select(x =>
                        CoordinatePoint.Create(x, 180 * rng.NextSingle(), 180 * rng.NextSingle())).ToArray()
                ),
                Money.Create(decimal.CreateChecked(rng.Next(1, 100)), rng.GetString(letters.ToUpper(), 3)),
                instructorId,
                studentId);
        }

        internal static DrivingLesson[] GenerateRandomArray(
            int length,
            (DrivingSchoolKey schoolId, StudentKey[] studentIds, InstructorKey[] instructorIds)[] schoolStudentsAndInstructors,
            int seed = 0)
        {
            Random rng = new Random(seed);
            var letters = "abcdefghijklmnopqrstuvwxyz";
            
            return Enumerable.Range(0, length)
                .Select(_ => rng.GetItems(schoolStudentsAndInstructors, 1)[0])
                .Select(x => (
                    x.schoolId,
                    studentId: rng.GetItems(x.instructorIds, 1)[0],
                    instructorId: rng.GetItems(x.studentIds, 1)[0]))
                .Select(x => DrivingLesson.Create(
                        DrivingLessonKey.Create(Guid.NewGuid()),
                        x.schoolId,
                        DrivingRoute.Create(
                            DateTimeRange.Create(
                                new DateTime(2000, 1, 1),
                                new DateTime(2000, 1, 1).AddMinutes(45 * (rng.NextDouble() + rng.NextDouble() + 1))),
                            Enumerable.Range(1, rng.Next(2, 10)).Select(x =>
                                CoordinatePoint.Create(x, 180 * rng.NextSingle(), 180 * rng.NextSingle())).ToArray()
                        ),
                        Money.Create(decimal.CreateChecked(rng.Next(1, 100)), rng.GetString(letters.ToUpper(), 3)),
                        x.studentId,
                        x.instructorId) ).ToArray();
        }
    }

    extension(DrivingSchool _)
    {
        /// <summary>
        /// Generates a driving school with random values 
        /// </summary>
        /// <param name="id">The driving school id</param>
        /// <param name="seed">The seed to use for generating the random values</param>
        /// <returns>The randomly generated driving school</returns>
        internal static DrivingSchool GenerateRandom(Guid id = default, int seed = 0)
        {
            Random rng = new Random(seed);
            var letters = "abcdefghijklmnopqrstuvwxyz";
            var numbers = "0123456789";
            return DrivingSchool.Create(
                DrivingSchoolKey.Create(id),
                DrivingSchoolName.Create(rng.GetString(letters, rng.Next(2, 10))),
                StreetAddress.Create(
                    rng.GetString(letters, rng.Next(2, 10)), 
                    rng.GetString(letters, rng.Next(2, 10)), 
                    rng.GetString(letters, rng.Next(2, 10)), 
                    rng.GetString(letters, rng.Next(2, 10))),
                PhoneNumber.Create(rng.GetString(numbers, 8)),
                WebAddress.Create($"{rng.GetString(letters, rng.Next(2, 10))}.{rng.GetString(letters, rng.Next(2, 10))}"),
                Enumerable.Range(0, rng.Next(0,5))
                    .Select(_ => Package.Create(
                        rng.GetString(letters, rng.Next(2, 10)),
                        rng.GetString(letters, rng.Next(10, 20)),
                        Money.Create(decimal.CreateChecked(rng.Next(1, 100)), rng.GetString(letters.ToUpper(), 3))
                    )).ToArray());
        }

        internal static DrivingSchool[] GenerateRandomArray(int length, int seed = 0)
        {
            Random rng = new Random(seed);
            var letters = "abcdefghijklmnopqrstuvwxyz";
            var numbers = "0123456789";

            return Enumerable.Range(0, length).Select(_ => 
                    DrivingSchool.Create(
                        DrivingSchoolKey.Create(Guid.NewGuid()),
                        DrivingSchoolName.Create(rng.GetString(letters, rng.Next(2, 10))),
                        StreetAddress.Create(
                            rng.GetString(letters, rng.Next(2, 10)), 
                            rng.GetString(letters, rng.Next(2, 10)), 
                            rng.GetString(letters, rng.Next(2, 10)), 
                            rng.GetString(letters, rng.Next(2, 10))),
                        PhoneNumber.Create(rng.GetString(numbers, 8)),
                        WebAddress.Create($"{rng.GetString(letters, rng.Next(2, 10))}.{rng.GetString(letters, rng.Next(2, 10))}"),
                        Enumerable.Range(0, rng.Next(0,5))
                            .Select(_ => Package.Create(
                                rng.GetString(letters, rng.Next(2, 10)),
                                rng.GetString(letters, rng.Next(10, 20)),
                                Money.Create(decimal.CreateChecked(rng.Next(1, 100)), rng.GetString(letters.ToUpper(), 3))
                            )).ToArray())).ToArray();


        }
    }
}