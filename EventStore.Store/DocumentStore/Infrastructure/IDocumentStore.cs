using EventStore.Domain.Entity.Infrastructure;
using System;
using System.Threading.Tasks;

namespace EventStore.Store.DocumentStore.Infrastructure
{
    public interface IDocumentStore : IDisposable
    {
        #region Public Methods

         Task Connect();

         Task<IDocumentCollection<T, TKey>> GetCollection<T, TKey>() where T : BaseEntity<TKey>;

        #endregion Public Methods
    }
}