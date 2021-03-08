using MediatR;

namespace EventStore.API.Commands.Content
{
    public class AddContentCommand : IRequest<Domain.Entity.Content>
    {
        #region Public Properties

        public string Title { get; set; }
        public string Url { get; set; }

        #endregion Public Properties
    }
}