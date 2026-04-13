using DrivingSchoolApi.Domain.Keys;
using DrivingSchoolApi.Domain.Primitives;

namespace DrivingSchoolApi.Domain.Entities;

public class StudentInvite : Entity<StudentInviteKey>
{
    public required DrivingSchoolKey DrivingSchoolId { get; init; }
    public required DateTime ExpirationDateTime { get; init; }
    private StudentInvite(){ } // EF

    public static StudentInvite Create(
        StudentInviteKey inviteKey, 
        DrivingSchoolKey drivingSchoolId,
        DateTime expirationDateTime)
    {
        return new StudentInvite()
        {
            Id = inviteKey,
            DrivingSchoolId = drivingSchoolId,
            ExpirationDateTime = expirationDateTime
        };
    }
}