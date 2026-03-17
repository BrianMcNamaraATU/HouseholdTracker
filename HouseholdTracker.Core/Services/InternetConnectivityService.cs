namespace HouseholdTracker.Core.Services;

/// <summary>
/// A class that provides checks for internet connectivity.
/// As the CentralDatabase is currently stored locally we need this implementation
/// for completion. It can be easily modified to provide actual results if the
/// CentralDabase is moved online.
/// It also allows forced simulation of true/false
/// </summary>
internal class InternetConnectivityService
{
    private readonly Func<bool> _connectivity;

    private ConnectivityType _connectivityType = ConnectivityType.Default;

    /// <summary>
    /// Constructor for the class
    /// </summary>
    /// <param name="connectivityCheck">Uses Func-bool to check connectivity each time its accessed</param>
    public InternetConnectivityService(Func<bool> connectivityCheck)
    {
        _connectivity = connectivityCheck;
    }

    internal bool HasInternetConnectivity
    {
        get
        {
            switch (_connectivityType)
            {
                case ConnectivityType.Connected: return true;
                case ConnectivityType.Disconnected: return false;
                default: return _connectivity();
            }
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
