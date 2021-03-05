namespace EventStore.PoC.Domain.Entity.Infrastructure
{
    public abstract class BaseEntity<T>
    {
        public T Id { get; internal set; }

        public abstract void SetId(T id);
    }
}