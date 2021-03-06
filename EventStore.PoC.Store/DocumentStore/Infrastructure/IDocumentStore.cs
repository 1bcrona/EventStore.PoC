using EventStore.PoC.Domain.Entity.Infrastructure;
using System;
using System.Threading.Tasks;

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