namespace EventStore.Domain.Entity.Infrastructure
{
    public interface IEntity
    {
        #region Public Properties

        public bool Active { get; }

        #endregion Public Properties
    }

    public abstract class BaseEntity<T> : IEntity
    {
        #region Public Properties

        public bool Active { get; set; }
        public T Id { get; set; }

        #endregion Public Properties
    }
}