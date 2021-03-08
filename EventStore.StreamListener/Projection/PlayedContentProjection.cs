using EventStore.Domain.Entity;
using EventStore.Domain.Event.Impl;
using Marten;
using Marten.Events.Projections;
using Marten.Schema;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventStore.StreamListener.Projection
{
    public class PlayedContentProjection : ViewProjection<PlayedContent, Guid>
    {
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
            view.AssignContent(e.Data?.ViewContent);
            view.Active = true;
        }

        #endregion Public Constructors

        #region Public Properties

        [Identity]
        public Guid Id { get; set; }

        #endregion Public Properties

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
    }
}