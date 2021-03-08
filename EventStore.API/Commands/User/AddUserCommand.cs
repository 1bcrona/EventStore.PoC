using MediatR;

namespace EventStore.API.Commands.User
{
    public class AddUserCommand : IRequest<Domain.Entity.User>
    {
        public string UserName { get; set; }
    }
}
