using EventStore.Domain.Entity;
using EventStore.Domain.Event.Impl;
using Marten.Events.Projections;
using Marten.Schema;
using System;

namespace EventStore.StreamListener.Projection
{
    public class UserProjection : ViewProjection<User, Guid>
    {
        #region Public Constructors

        public UserProjection()
        {
            ProjectEvent<UserDeleted>(@event => @event.EntityId, Persist);
            ProjectEvent<UserCreated>(created => created.EntityId, Persist);
            //ProjectEvent<ContentPlayed>(@event => @event.EntityId, Persist);
        }

        #endregion Public Constructors

        #region Public Properties

        [Identity]
        public Guid Id { get; set; }

        #endregion Public Properties

        #region Private Methods

        private void Persist(User view, UserCreated e)
        {
            view.Id = e.EntityId;
            view.UserName = e.Data?.UserName;
            view.Active = true;
        }

        private void Persist(User view, UserDeleted @event)
        {
            view.Active = false;
        }

        #endregion Private Methods
    }
}