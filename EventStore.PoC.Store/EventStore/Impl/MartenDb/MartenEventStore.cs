using System.Threading.Tasks;
using EventStore.PoC.Store.EventStore.Infrastructure;
using Marten;
using Marten.Events.Projections;

namespace EventStore.PoC.Store.EventStore.Impl.MartenDb
{
    public class MartenEventStore : IEventStore
    {
        private readonly Marten.DocumentStore _DocumentStore;

        public Marten.DocumentStore DocumentStore
        {
            get
            {
                return _DocumentStore;
            }
        }

        public MartenEventStore(string connectionString)
        {
            _DocumentStore = Marten.DocumentStore.For(f =>
            {
                f.Connection(connectionString);
                f.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate;
            });
        }

       
        public async Task<bool> Open()
        {
            return await Task.FromResult(true);
        }


        public IEventCollection GetCollection()
        {
            return new MartenEventCollection(_DocumentStore);
        }
    }
}