namespace CareerSearcher.Domain.Entities;

public abstract class BaseEntity : IEquatable<BaseEntity>
{
    public Guid Id { get; }
    public DateTime CreatedAt { get; }

    public BaseEntity()
    {
        Id          = Guid.NewGuid();
        CreatedAt   = DateTime.UtcNow;
    }

    public bool Equals(BaseEntity? other)
    {
        if (other is null)
            return false;

        if (other.GetType() != GetType())
            return false;

        return other.Id == Id;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
            return false;

        if (obj.GetType() != this.GetType())
            return false;

        if (obj is not BaseEntity entity)
            return false;

        return entity.Id == Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static bool operator==(BaseEntity? left, BaseEntity? right)
    {
        return left is not null && right is not null && left.Equals(right);
    }

    public static bool operator!=(BaseEntity? left, BaseEntity? right)
    {
        return !(left == right);
    }
}
