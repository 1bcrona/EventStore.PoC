using System;
using EventStore.Domain.Entity;
using EventStore.Domain.Event.Impl;
using EventStore.Store.EventStore.Infrastructure;
using Marten.Events.Projections;
using Marten.Schema;

namespace EventStore.StreamListener.Projection.Marten
{
    public class UserProjection : ViewProjection<User, Guid>, IEventProjection
    {
        #region Public Constructors

        public UserProjection()
        {
            ProjectEvent<UserDeleted>(@event => @event.EntityId, Persist);
            ProjectEvent<UserCreated>(created => created.EntityId, Persist);
        }

        #endregion Public Constructors

        #region Public Properties

        [Identity] public Guid Id { get; set; }

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

        public event EventHandler<object> ProjectionUpdated;
    }
}