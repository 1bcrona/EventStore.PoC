using System.Threading.Tasks;

namespace EventStore.Store.EventStore.Infrastructure
{
    public interface IEventStore
    {
        #region Public Methods

        IEventCollection GetCollection();

        Task<bool> Open();

        #endregion Public Methods
    }
}