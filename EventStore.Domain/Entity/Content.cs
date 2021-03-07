using EventStore.Domain.Entity.Infrastructure;
using EventStore.Domain.ValueObject;
using System;

namespace EventStore.Domain.Entity
{
    public class Content : BaseEntity<Guid>, IAggregateRoot
    {
        #region Public Constructors

        public Content(Guid id)
        {
            this.Id = id;
        }

        public Content() : this(Guid.NewGuid())
        {
        }

        #endregion Public Constructors

        #region Public Properties

        public ContentCdnLink ContentCdnLink { get; set; }
        public ContentMetadata ContentMetadata { get; set; }
        public int PlayCount { get; set; }

        #endregion Public Properties

        #region Public Methods


        #endregion Public Methods
    }
}