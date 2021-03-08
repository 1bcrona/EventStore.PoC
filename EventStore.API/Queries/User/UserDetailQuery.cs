using System;
using MediatR;

namespace EventStore.API.Queries.User
{
    public class UserDetailQuery : IRequest<Domain.Entity.User>
    {
        public Guid UserId { get; set; }
    }
}
