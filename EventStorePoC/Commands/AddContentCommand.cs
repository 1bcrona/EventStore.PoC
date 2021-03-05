using MediatR;

namespace EventStore.PoC.API.Commands
{
    public class AddContentCommand : IRequest<bool>
    {
        public string Title { get; set; }
        public string Url { get; set; }
    }
}