using DrivingSchoolApi.Domain.Entities;
using DrivingSchoolApi.Domain.Keys;

namespace DrivingSchoolApi.Application.UnitTest.Extensions;

internal static class StudentInviteExtensions
{
    extension(StudentInvite invite)
    {
        public static StudentInvite CreateTestInvite(DateTime dateTime, Guid? inviteId = null, Guid? schoolId = null)
        {
            inviteId ??= Guid.NewGuid();
            schoolId ??= Guid.NewGuid();
            
            return StudentInvite.Create(
                StudentInviteKey.Create(inviteId.Value),
                DrivingSchoolKey.Create(schoolId.Value),
                dateTime);
        }
    }
}
