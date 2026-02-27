using DrivingSchoolApi.Domain.Primitives;

namespace DrivingSchoolApi.Domain.Keys;

public class TheoryLessonKey : EntityKey<TheoryLessonKey>
{
    public required Guid Value { get; init; }
    private TheoryLessonKey() {} // EF

    public override bool Equals(TheoryLessonKey? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value.Equals(other.Value);
    }
    
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static TheoryLessonKey Create(Guid value)
    {
        return new TheoryLessonKey
        {
            Value = value
        };
    }
}