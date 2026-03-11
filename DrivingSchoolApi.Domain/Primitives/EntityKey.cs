namespace DrivingSchoolApi.Domain.Primitives;

public abstract class EntityKey<TSelf> : IEquatable<TSelf> where TSelf : EntityKey<TSelf>
{
    public abstract bool Equals(TSelf? other);
    
    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((TSelf)obj);
    }

    public abstract override int GetHashCode();
}