using System;
using System.Threading.Tasks;
using EventStore.PoC.Domain.Entity.Infrastructure;

namespace EventStore.PoC.Store.DocumentStore.Infrastructure
{
    public interface IDocumentStore : IDisposable
    {
        #region Public Methods

        public Task Connect();

        public Task<IDocumentCollection<T, TKey>> GetCollection<T, TKey>() where T : BaseEntity<TKey>;

        #endregion Public Methods
    }
}