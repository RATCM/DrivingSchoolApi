using DrivingSchoolApi.Domain.Primitives;

namespace DrivingSchoolApi.Domain.Keys;

public class DrivingLessonKey : EntityKey<DrivingLessonKey>
{
    public required Guid Value { get; init; }
    private DrivingLessonKey() {} // EF

    public override bool Equals(DrivingLessonKey? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value.Equals(other.Value);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static DrivingLessonKey Create(Guid value)
    {
        return new DrivingLessonKey
        {
            Value = value
        };
    }
}