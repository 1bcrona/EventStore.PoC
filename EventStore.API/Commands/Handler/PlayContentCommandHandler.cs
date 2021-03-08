using System.Threading;
using System.Threading.Tasks;
using EventStore.Domain.Entity;
using EventStore.Domain.Event.Impl;
using EventStore.Domain.Event.Infrastructure;
using EventStore.Store.EventStore.Infrastructure;
using MediatR;

namespace EventStore.API.Commands.Handler
{
    public class PlayContentCommandHandler : IRequestHandler<PlayContentCommand, bool>
    {
        #region Private Fields

        private readonly IEventStore _DocumentStore;

        #endregion Private Fields

        #region Public Constructors

        public PlayContentCommandHandler(IEventStore documentStore)
        {
            _DocumentStore = documentStore;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<bool> Handle(PlayContentCommand request, CancellationToken cancellationToken)
        {
            var eventCollection = _DocumentStore.GetCollection();


            PlayedContent playedContent = new PlayedContent();

            var content = await eventCollection.Query<Domain.Entity.Content>(request.ContentId);
            var user = await eventCollection.Query<Domain.Entity.User>(request.UserId);

            playedContent.AssignContent(content);
            playedContent.AssignUser(user);

            var events = new IEvent[]
            {
                new ContentPlayed { AggregateId = playedContent.Id, EntityId = playedContent.Id, Data = playedContent },
            };

            foreach (var @event in events)
            {
                await eventCollection.AddEvent(playedContent.Id, @event);
            }

            return await Task.FromResult(true);
        }

        #endregion Public Methods
    }
}