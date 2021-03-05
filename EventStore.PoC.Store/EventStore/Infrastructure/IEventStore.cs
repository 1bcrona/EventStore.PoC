using System.Threading.Tasks;

namespace EventStore.PoC.Store.EventStore.Infrastructure
{
    public interface IEventStore
    {
        Task<bool> Open();

        IEventCollection GetCollection();
    }
}