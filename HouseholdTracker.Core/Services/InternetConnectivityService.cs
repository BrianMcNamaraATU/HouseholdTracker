namespace HouseholdTracker.Core.Services;

/// <summary>
/// A class that provides checks for internet connectivity.
/// As the CentralDatabase is currently stored locally we need this implementation
/// for completion. It can be easily modified to provide actual results if the
/// CentralDabase is moved online.
/// It also allows forced simulation of true/false
/// </summary>
/// <remarks>
/// Constructor for the class
/// </remarks>
/// <param name="connectivityCheck">Uses Func-bool to check connectivity each time its accessed</param>
internal class InternetConnectivityService(Func<bool> connectivityCheck)
{
    private readonly Func<bool> _connectivity = connectivityCheck;

    private ConnectivityType _connectivityType = ConnectivityType.Default;

    internal bool HasInternetConnectivity
    {
        get
        {
            return _connectivityType switch
            {
                ConnectivityType.Connected => true,
                ConnectivityType.Disconnected => false,
                _ => _connectivity(),
            };
        }
    }

    internal void ForceConnection() => _connectivityType = ConnectivityType.Connected;

    internal void ForceNoConnection() => _connectivityType = ConnectivityType.Disconnected;

    internal void ResetToDefault() => _connectivityType = ConnectivityType.Default;

    private enum ConnectivityType
    {
        Default = 0,
        Connected = 1,
        Disconnected = 2
    }
}
