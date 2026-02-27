namespace DrivingSchoolApi.Domain.Primitives;

public abstract class Entity<T> : IEquatable<Entity<T>> where T : EntityKey<T>
{
    public required T Id { get; init; }

    //protected Entity(T id)
    //{
    //    Id = id;
    //}

    public bool Equals(Entity<T>? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Entity<T>)obj);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}