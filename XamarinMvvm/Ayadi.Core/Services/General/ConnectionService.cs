
using Ayadi.Core.Contracts.Services;
using Plugin.Connectivity;


namespace Ayadi.Core.Services.General
{
    class ConnectionService : IConnectionService
    {
        public bool CheckOnline()
        {
            return CrossConnectivity.Current.IsConnected;
        }
    }
}
