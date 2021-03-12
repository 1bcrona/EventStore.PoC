using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.Domain.Entity;
using EventStore.Store.EventStore.Impl.MartenDb;
using EventStore.Store.EventStore.Infrastructure;
using EventStore.StreamListener.Projection.Marten;

namespace EventStore.StreamListener
{
    public class App
    {
        private IEventStore _EventStore;
        public App(IEventStore eventStore)
        {
            _EventStore = eventStore;
        }


        public async Task Run()
        {
            await _EventStore.AddProjection(new ContentProjection());
            await _EventStore.AddProjection(new PlayedContentProjection());
            await _EventStore.AddProjection(new UserProjection());
            await _EventStore.StartProjectionDaemon();

            var collection = await _EventStore.GetCollection();

            var contents = await collection.Query<Content>();

            Console.WriteLine("Contents");
            foreach (var content in contents) Console.WriteLine(content.Id);

            Console.WriteLine("Users");

            var users = await collection.Query<User>();
            foreach (var user in users) Console.WriteLine(user.Id);
        }
    }
}
