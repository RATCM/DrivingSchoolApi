using DrivingSchoolApi.Domain.Primitives;

namespace DrivingSchoolApi.Domain.Keys;

public class InstructorKey : EntityKey<InstructorKey>
{
    public required Guid Value { get; init; }
    private InstructorKey() {} // EF

    public override bool Equals(InstructorKey? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value.Equals(other.Value);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static InstructorKey Create(Guid value)
    {
        return new InstructorKey
        {
            Value = value
        };
    }
}