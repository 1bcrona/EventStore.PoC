using EventStore.Domain.Event.Impl;
using EventStore.Domain.ValueObject;
using System;

namespace EventStore.StreamListener
{
    public class ContentAggregation
    {
        #region Public Properties

        public Boolean Active { get; set; }
        public ContentCdnLink ContentCdnLink { get; set; }
        public ContentMetadata ContentMetadata { get; set; }
        public Guid Id { get; set; }
        public int PlayCount { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void Apply(ContentCreated e)
        {
            Id = Guid.Parse(e.EntityId.ToString() ?? Guid.Empty.ToString());
            ContentMetadata = e.Data?.ContentMetadata;
            ContentCdnLink = e.Data?.ContentCdnLink;
            PlayCount = e.Data?.PlayCount ?? 0;
            Active = true;
        }

        public void Apply(ContentDeleted e)
        {
            Active = false;
        }

        public void Apply(ContentPlayed e)
        {
            PlayCount += 1;
        }

        #endregion Public Methods
    }
}