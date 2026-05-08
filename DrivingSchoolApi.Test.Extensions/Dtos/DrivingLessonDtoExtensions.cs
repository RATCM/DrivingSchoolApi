using DrivingSchoolApi.DTOs.DrivingLesson;
using DrivingSchoolApi.DTOs.ValueObject;
using Microsoft.AspNetCore.Http;

namespace DrivingSchoolApi.Test.Extensions.Dtos;

public static class DrivingLessonDtoExtensions
{
    extension(DrivingLessonRegistryDto registryDto)
    {
        public static DrivingLessonRegistryDto CreateTestDrivingLesson(
            Guid studentId,
            Guid schoolId,
            MoneyDto? price = null)
        {
            price ??= new MoneyDto(1000, "DKK");
            var instructorSignature = new FormFile(
                new MemoryStream([1,2,3,4]),
                0,
                4,
                "instructorSignature",
                "instructorSignature.png");
            var studentSignature = new FormFile(
                new MemoryStream([1,2,3,4]),
                0,
                4,
                "studentSignature",
                "studentSignature.png");
            var route = new DrivingRouteDto(
                new DateTimeRangeDto(DateTime.Now, DateTime.Now.AddHours(1)), [
                    new CoordinatePointDto(1, 55.6761f, 12.5683f), 
                    new CoordinatePointDto(2, 55.6762f, 12.5684f)]);
            var objectives = new DrivingLessonObjectiveDto(
                true, 
                false, 
                false, 
                false, 
                false, 
                false);

            return new DrivingLessonRegistryDto(
                instructorSignature,
                studentSignature,
                schoolId,
                studentId,
                route,
                price,
                objectives);
        }
    }
}