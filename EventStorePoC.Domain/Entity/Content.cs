using System;
using EventStore.PoC.Domain.Entity.Infrastructure;

namespace EventStore.PoC.Domain.Entity
{
    public class Content : BaseEntity<string>, IAggregateRoot
    {
        public Content(string id)
        {
            SetId(id);
        }

        public Content() : this(null)
        {
        }

        public ContentMetadata ContentMetadata { get; set; }
        public ContentCdnLink ContentCdnLink { get; set; }

        public int PlayCount { get; set; }

        public override void SetId(string id)
        {
            Id = id ?? Guid.NewGuid().ToString();
        }
    }
}