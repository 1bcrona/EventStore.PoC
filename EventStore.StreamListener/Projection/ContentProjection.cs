using EventStore.Domain.Entity;
using EventStore.Domain.Event.Impl;
using Marten.Events.Projections;
using Marten.Schema;
using System;

namespace EventStore.StreamListener.Projection
{
    public class ContentProjection : ViewProjection<Content, Guid>
    {
        #region Public Constructors

        public ContentProjection()
        {
            ProjectEvent<ContentDeleted>(@event => @event.EntityId, Persist);
            ProjectEvent<ContentCreated>(created => created.EntityId, Persist);
            ProjectEvent<ContentPlayed>(@event => @event.Data.ViewContent.Id, Persist, onlyUpdate: true);
        }

        #endregion Public Constructors

        #region Public Properties

        [Identity]
        public Guid Id { get; set; }

        #endregion Public Properties

        #region Private Methods

        
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

        #endregion Private Methods
    }
}