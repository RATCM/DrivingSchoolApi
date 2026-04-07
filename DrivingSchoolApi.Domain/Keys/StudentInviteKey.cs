using DrivingSchoolApi.Domain.Primitives;

namespace DrivingSchoolApi.Domain.Keys;

public class StudentInviteKey : EntityKey<StudentInviteKey>
{
    public required Guid Value { get; init; }
    private StudentInviteKey(){ } // EF
    
    public override bool Equals(StudentInviteKey? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value.Equals(other.Value);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static StudentInviteKey Create(Guid value)
    {
        return new StudentInviteKey()
        {
            Value = value
        };
    }
}