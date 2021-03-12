using System;
using System.Threading.Tasks;

namespace EventStore.Store.EventStore.Infrastructure
{
    public interface IEventStore : IDisposable
    {
        #region Public Methods

        Task AddProjection(IEventProjection eventProjection);

        Task<IEventCollection> GetCollection();

        Task<bool> Open();

        Task StartProjectionDaemon();

        #endregion Public Methods
    }
}