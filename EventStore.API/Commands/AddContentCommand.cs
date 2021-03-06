using EventStore.Domain.Entity;
using MediatR;

namespace EventStore.API.Commands
{
    public class AddContentCommand : IRequest<Content>
    {
        #region Public Properties

        public string Title { get; set; }
        public string Url { get; set; }

        #endregion Public Properties
    }
}