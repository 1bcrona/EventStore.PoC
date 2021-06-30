namespace EventStore.Domain.Entity.Infrastructure
{
    public abstract class BaseEntity<T> : IEntity
    {
        #region Public Properties

        public bool Active { get; set; }
        public T Id { get; set; }

        #endregion Public Properties
    }

    public interface IEntity
    {
        public bool Active { get; }
    }
}