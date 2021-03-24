using EventStore.Domain.Entity;
using EventStore.Domain.Event.Impl;
using EventStore.StreamListener.Projection.Marten.Infrastructure;
using Marten;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventStore.StreamListener.Projection.Marten
{
    public class PlayedContentProjection : BaseMartenProjection<PlayedContent, Guid>
    {
        #region Public Constructors

        public PlayedContentProjection()
        {
            List<Guid> ContentsWillBeDeleted(IDocumentSession ds, UserDeleted @event)
            {
                var allPlayedContents = ds.Query<PlayedContent>();

                var contents = allPlayedContents.Where(a => a.ViewedUser.Id == @event.EntityId)
                    .Select(a => a.Id)
                    .ToList();
                return contents;
            }

            ProjectEvent<UserDeleted>(ContentsWillBeDeleted, Persist);
        }

        #endregion Public Constructors

        #region Private Methods

        private void Persist(PlayedContent view, UserDeleted e)
        {
            view.Active = false;
        }

        #endregion Private Methods
    }
}