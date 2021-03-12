using System;
using MediatR;

namespace EventStore.API.Queries.User
{
    public class UserDetailQuery : IRequest<Domain.Entity.User>
    {
        #region Public Properties

        public Guid UserId { get; set; }

        #endregion Public Properties
    }
}