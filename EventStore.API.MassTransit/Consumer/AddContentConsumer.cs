using EventStore.Domain.Entity;
using EventStore.Domain.Event.Impl;
using EventStore.Domain.Event.Infrastructure;
using EventStore.Domain.ValueObject;
using EventStore.Store.EventStore.Infrastructure;
using MassTransit;
using System.Threading.Tasks;

namespace EventStore.API.MassTransit.Consumer
{
    public class AddContentCommand
    {
        #region Public Properties

        public string Title { get; set; }
        public string Url { get; set; }

        #endregion Public Properties
    }

    public class AddContentCommandConsumer : IConsumer<AddContentCommand>
    {
        #region Private Fields

        private readonly IEventStore _DocumentStore;

        #endregion Private Fields

        #region Public Constructors

        public AddContentCommandConsumer(IEventStore documentStore)
        {
            _DocumentStore = documentStore;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task Consume(ConsumeContext<AddContentCommand> context)
        {
            var c = new Product
            {
                ProductLocation = new ProductLocation(context.Message.Url),
                ProductMetadata = new ProductMetadata(context.Message.Title)
            };

            var eventCollection = await _DocumentStore.GetCollection();

            var events = new IEvent[]
            {
                new ProductCreated {AggregateId = c.Id, EntityId = c.Id, Data = c}
            };

            foreach (var @event in events) await eventCollection.AddEvent(c.Id, @event);

            await context.RespondAsync(c);
        }

        #endregion Public Methods
    }
}