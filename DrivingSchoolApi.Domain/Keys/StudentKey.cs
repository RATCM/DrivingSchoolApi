using DrivingSchoolApi.Domain.Primitives;

namespace DrivingSchoolApi.Domain.Keys;

public class StudentKey : EntityKey<StudentKey>
{
    public required Guid Value { get; init; }
    private StudentKey() {} // EF

    public override bool Equals(StudentKey? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value.Equals(other.Value);
    }
    
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static StudentKey Create(Guid value)
    {
        return new StudentKey
        {
            Value = value
        };
    }
}