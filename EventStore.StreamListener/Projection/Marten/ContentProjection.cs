using System;
using System.Collections.Generic;
using System.Linq;
using EventStore.Domain.Entity;
using EventStore.Domain.Entity.Infrastructure;
using EventStore.Domain.Event.Impl;
using EventStore.Domain.Event.Infrastructure;
using EventStore.Store.EventStore.Infrastructure;
using Marten;
using Marten.Events.Projections;
using Marten.Schema;

namespace EventStore.StreamListener.Projection.Marten
{
    public class ContentProjection : ViewProjection<Content, Guid>, IEventProjection
    {
        #region Public Constructors

        public ContentProjection()
        {
            List<Guid> ContentsWillBeDeleted(IDocumentSession ds, UserDeleted @event)
            {
                var playedContentsByUser = ds.Query<PlayedContent>().Where(a => a.ViewedUser.Id == @event.EntityId)
                    .Select(a => a.ViewedContent.Id).ToList();
                return playedContentsByUser;
            }

            ProjectEvent<UserDeleted>(ContentsWillBeDeleted, Persist);
            ProjectEvent<ContentDeleted>(@event => @event.EntityId, Persist);
            ProjectEvent<ContentCreated>(created => created.EntityId, Persist);
            ProjectEvent<ContentPlayed>(@event => @event.Data.ViewedContent.Id, Persist, true);
        }

        #endregion Public Constructors

        #region Public Properties

        [Identity] public Guid Id { get; set; }

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

        private void Persist(Content view, UserDeleted @event)
        {
            view.PlayCount--;
        }

        #endregion Private Methods

        public event EventHandler<object> ProjectionUpdated;
    }
}