using EventStore.Domain.Entity;
using EventStore.Domain.Event.Impl;
using Marten;
using Marten.Events.Projections;
using Marten.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventStore.StreamListener.Projection
{
    public class PlayedContentProjection : ViewProjection<PlayedContent, Guid>
    {
        #region Public Constructors

        public PlayedContentProjection()
        {
            ProjectEvent<ContentPlayed>(created => created.EntityId, Persist);

            Func<IDocumentSession, UserDeleted, List<Guid>> deletedContents =
                (ds, @event) =>
                {
                    var allPlayedContents = ds.Query<PlayedContent>();

                    var contents = allPlayedContents
                        .Where(a => a.ViewedUser.Id == @event.EntityId)
                        .Select(a => a.Id).ToList();
                    return contents;
                };

            DeleteEvent<UserDeleted>(deletedContents);
            DeleteEvent<ContentPlayDeleted>(@event => @event.EntityId);
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

        #endregion Private Methods
    }
}