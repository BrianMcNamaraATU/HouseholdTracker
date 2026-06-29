namespace HouseholdTracker.Core.Services;

/// <summary>
/// Holds transient navigation state that needs to be passed between pages.
/// Values should be read once and reset after use.
/// </summary>
internal static class NavigationState
{
    /// <summary>
    /// Whether the user is being forced to change their password
    /// </summary>
    internal static bool ForcePasswordChange { get; set; } = false;
}
