namespace Domain.Primitives;

public abstract class Entity
{
    public Guid Id { get; protected set; }
}