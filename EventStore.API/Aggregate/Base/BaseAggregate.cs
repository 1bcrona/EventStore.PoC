using EventStore.Domain.Entity.Infrastructure;

namespace EventStore.API.Aggregate.Base
{
    public class BaseAggregate<T, K> where T : BaseEntity<K>
    {
        #region Public Properties

        public T Data { get; set; }
        public K Id { get; set; }

        #endregion Public Properties
    }
}