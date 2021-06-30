using Marten;
using System.Data;

namespace EventStore.Store.EventStore.Impl.MartenDb
{
    public class CustomSessionFactory : ISessionFactory
    {
        #region Private Fields

        private readonly IDocumentStore _store;

        #endregion Private Fields

        #region Public Constructors

        public CustomSessionFactory(IDocumentStore store)
        {
            _store = store;
        }

        #endregion Public Constructors

        #region Public Methods

        public IDocumentSession OpenSession()
        {
            return _store.LightweightSession(IsolationLevel.Serializable);
        }

        public IQuerySession QuerySession()
        {
            return _store.QuerySession();
        }

        #endregion Public Methods
    }
}