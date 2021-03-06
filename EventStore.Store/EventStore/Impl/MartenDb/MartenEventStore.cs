using EventStore.Store.EventStore.Infrastructure;
using Marten;
using Marten.Storage;
using System.Threading.Tasks;

namespace EventStore.Store.EventStore.Impl.MartenDb
{
    public class MartenEventStore : IEventStore
    {
        #region Private Fields

        private readonly Marten.DocumentStore _DocumentStore;

        #endregion Private Fields

        #region Public Constructors

        public MartenEventStore(string connectionString)
        {
            _DocumentStore = Marten.DocumentStore.For(f =>
            {
                f.Connection(connectionString);
                f.AutoCreateSchemaObjects = AutoCreate.All;
                f.DefaultTenantUsageEnabled = true;
                f.Events.TenancyStyle = TenancyStyle.Separate;
            });
        }

        #endregion Public Constructors

        #region Public Properties

        public Marten.DocumentStore DocumentStore => _DocumentStore;

        #endregion Public Properties

        #region Public Methods

        public IEventCollection GetCollection()
        {
            return new MartenEventCollection(_DocumentStore);
        }

        public async Task<bool> Open()
        {
            return await Task.FromResult(true);
        }

        #endregion Public Methods
    }
}