using System;
using System.Threading.Tasks;

namespace EventStore.Store.EventStore.Infrastructure
{
    public interface IEventStore : IDisposable
    {
        #region Public Methods

        Task<IEventCollection> GetCollection();

        Task<bool> Open();

        Task AddProjection(IEventProjection eventProjection);


        Task StartProjectionDaemon();

        #endregion Public Methods
    }
}