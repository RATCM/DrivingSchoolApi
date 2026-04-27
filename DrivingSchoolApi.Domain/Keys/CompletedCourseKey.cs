using DrivingSchoolApi.Domain.Primitives;

namespace DrivingSchoolApi.Domain.Keys;

public class CompletedCourseKey : EntityKey<CompletedCourseKey>
{
    public required Guid Value { get; init; }
    
    private CompletedCourseKey() {}
    
    public override bool Equals(CompletedCourseKey? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value.Equals(other.Value);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
    
    public static CompletedCourseKey Create(Guid value)
    {
        return new CompletedCourseKey
        {
            Value = value
        };
    }
}