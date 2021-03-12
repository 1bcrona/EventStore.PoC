using System;
using System.Collections.Generic;
using System.Linq;
using EventStore.Domain.Entity;
using EventStore.Domain.Event.Impl;
using EventStore.Store.EventStore.Infrastructure;
using Marten;
using Marten.Events.Projections;
using Marten.Schema;

namespace EventStore.StreamListener.Projection.Marten
{
    public class PlayedContentProjection : ViewProjection<PlayedContent, Guid>, IEventProjection
    {
        #region Public Properties

        [Identity] public Guid Id { get; set; }

        #endregion Public Properties

        #region Public Constructors

        public PlayedContentProjection()
        {
            ProjectEvent<ContentPlayed>(created => created.EntityId, Persist);

            List<Guid> ContentsWillBeDeleted(IDocumentSession ds, UserDeleted @event)
            {
                var allPlayedContents = ds.Query<PlayedContent>();

                var contents = allPlayedContents.Where(a => a.ViewedUser.Id == @event.EntityId)
                    .Select(a => a.Id)
                    .ToList();
                return contents;
            }

            ProjectEvent<UserDeleted>(ContentsWillBeDeleted, Persist);
            ProjectEvent<ContentPlayDeleted>(@event => @event.EntityId, Persist);
        }

        private void Persist
        (
            IDocumentSession documentSession,
            PlayedContent view,
            ContentPlayed e
        )
        {
            view.Id = e.EntityId;
            view.AssignUser(e.Data?.ViewedUser);
            view.AssignContent(e.Data?.ViewedContent);
            view.Active = true;
        }

        #endregion Public Constructors

        #region Private Methods

        private void Persist(PlayedContent view, UserDeleted e)
        {
            view.Active = false;
        }

        private void Persist(PlayedContent view, ContentPlayDeleted e)
        {
            view.Active = false;
        }

        #endregion Private Methods

        public event EventHandler<object> ProjectionUpdated;
    }
}