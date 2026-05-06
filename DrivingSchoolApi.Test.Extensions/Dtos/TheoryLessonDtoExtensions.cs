using DrivingSchoolApi.DTOs.TheoryLesson;
using DrivingSchoolApi.DTOs.ValueObject;
using Microsoft.AspNetCore.Http;

namespace DrivingSchoolApi.Test.Extensions.Dtos;

public static class TheoryLessonDtoExtensions
{
    extension(TheoryLessonRegistryDto registryDto)
    {
        public static TheoryLessonRegistryDto CreateTestTheoryLesson(
            Guid studentId, 
            DateTime? time = null,
            MoneyDto? price = null)
        {
            time ??= new DateTime(2000, 01, 01);
            price ??= new MoneyDto(500, "DKK");
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

            return new TheoryLessonRegistryDto(
                time.Value,
                price,
                studentId,
                instructorSignature,
                studentSignature);
        }
    }
}