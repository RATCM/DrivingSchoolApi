using DrivingSchoolApi.Domain.Primitives;

namespace DrivingSchoolApi.Domain.Keys;

public class DrivingSchoolKey : EntityKey<DrivingSchoolKey>
{
    public required Guid Value { get; init; }
    
    private DrivingSchoolKey() {} // EF

    public override bool Equals(DrivingSchoolKey? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value.Equals(other.Value);
    }
    
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static DrivingSchoolKey Create(Guid value)
    {
        return new DrivingSchoolKey
        {
            Value = value
        };
    }
}