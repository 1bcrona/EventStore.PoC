using System;

namespace EventStore.Domain.Entity.Infrastructure
{
    public abstract class BaseEntity<T>
    {
        #region Public Properties

        public Boolean Active { get; set; }
        public T Id { get; internal set; }

        #endregion Public Properties

        #region Public Methods

        public abstract void SetId(T id);

        #endregion Public Methods
    }
}