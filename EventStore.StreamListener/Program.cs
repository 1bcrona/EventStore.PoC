using System.Threading.Tasks;

namespace EventStore.StreamListener
{
    internal class Program
    {
        #region Private Fields

        private static readonly App _APP = new();

        #endregion Private Fields

        #region Private Methods

        private static async Task Main(string[] args)
        {
            await _APP.Run();
        }

        #endregion Private Methods
    }
}