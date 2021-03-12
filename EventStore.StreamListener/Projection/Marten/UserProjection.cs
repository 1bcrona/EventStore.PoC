using EventStore.Domain.Entity;
using EventStore.Store.EventStore.Infrastructure;
using EventStore.StreamListener.Projection.Marten.Infrastructure;
using System;

namespace EventStore.StreamListener.Projection.Marten
{
    public class UserProjection : BaseMartenProjection<User, Guid>, IEventProjection
    {
        #region Public Constructors

        public UserProjection()
        {
        }

        #endregion Public Constructors

        #region Private Methods

        //private void Persist(User view, UserCreated e)
        //{
        //    view.Id = e.EntityId;
        //    view.UserName = e.Data?.UserName;
        //    view.Active = true;
        //}

        //private void Persist(User view, UserDeleted @event)
        //{
        //    view.Active = false;
        //}

        #endregion Private Methods
    }
}