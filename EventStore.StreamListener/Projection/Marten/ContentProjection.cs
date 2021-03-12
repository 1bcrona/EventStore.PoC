using EventStore.Domain.Entity;
using EventStore.Domain.Event.Impl;
using EventStore.StreamListener.Projection.Marten.Infrastructure;
using Marten;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventStore.StreamListener.Projection.Marten
{
    public class ContentProjection : BaseMartenProjection<Content, Guid>
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
        }

        private void Persist(Content content, UserDeleted @event)
        {
            content.PlayCount--;
        }

        #endregion Public Constructors
    }
}