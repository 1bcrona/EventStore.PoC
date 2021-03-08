using System;
using MediatR;

namespace EventStore.API.Commands.User
{
    public class DeleteUserCommand : IRequest<bool>
    {
        public Guid UserId { get; set; }
    }
}
