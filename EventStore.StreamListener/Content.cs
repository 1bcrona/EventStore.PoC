using EventStore.Domain.Event.Impl;
using EventStore.Domain.ValueObject;
using System;
using EventStore.Domain.Entity;
using Marten.Events.Projections;
using Marten.Schema;

namespace EventStore.StreamListener
{

    public class ContentProjection : ViewProjection<Content, Guid>
    {
        [Identity]
        public Guid Id { get; set; }
        public ContentProjection()
        {
            ProjectEvent<ContentDeleted>(@event => @event.EntityId, Persist);
            ProjectEvent<ContentCreated>(created => created.EntityId, Persist);
            ProjectEvent<ContentPlayed>(@event => @event.EntityId, Persist);
        }

        private void Persist(Content view, ContentCreated e)
        {
            view.Id = e.EntityId;
            view.ContentMetadata = e.Data?.ContentMetadata;
            view.ContentCdnLink = e.Data?.ContentCdnLink;
            view.PlayCount = e.Data?.PlayCount ?? 0;
            view.Active = true;



        }

        private void Persist(Content view, ContentDeleted @event)
        {
            view.Active = false;
        }

        private void Persist(Content view, ContentPlayed @event)
        {
            view.PlayCount++;
        }

    }
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
            Id = e.EntityId;
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