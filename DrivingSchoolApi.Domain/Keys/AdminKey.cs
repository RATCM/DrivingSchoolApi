using DrivingSchoolApi.Domain.Primitives;

namespace DrivingSchoolApi.Domain.Keys;

public class AdminKey : EntityKey<AdminKey>
{
    public required Guid Value { get; init; }
    
    private AdminKey() {}
    
    public override bool Equals(AdminKey? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Value.Equals(other.Value);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }
    
    public static AdminKey Create(Guid value)
    {
        return new AdminKey
        {
            Value = value
        };
    }

}