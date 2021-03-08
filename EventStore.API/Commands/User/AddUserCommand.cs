using MediatR;

namespace EventStore.API.Commands.User
{
    public class AddUserCommand : IRequest<Domain.Entity.User>
    {
        #region Public Properties

        public string UserName { get; set; }

        #endregion Public Properties
    }
}