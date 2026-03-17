using HouseholdTracker.Core.Services;

namespace HouseholdTracker.Tests.Services;

/// <summary>
/// Tests for the InternetConnectivity Service
/// </summary>
public class InternerConnectivityServiceTests
{
    private InternetConnectivityService? _service;

    /// <summary>
    /// Test the results when actual internet connectivity exists
    /// </summary>
    [Test]
    public async Task TestResults_WithActualInternetConnectivity()
    {
        _service = new InternetConnectivityService(() => true);
        var result1 = _service.HasInternetConnectivity;
        _service.ForceConnection();
        var result2 = _service.HasInternetConnectivity;
        _service.ForceNoConnection();
        var result3 = _service.HasInternetConnectivity;
        _service.ResetToDefault();
        var result4 = _service.HasInternetConnectivity;

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result1, Is.True);
            Assert.That(result2, Is.True);
            Assert.That(result3, Is.False);
            Assert.That(result4, Is.True);
        }
    }

    /// <summary>
    /// Test the results when no internet connectivity exists
    /// </summary>
    [Test]
    public async Task TestResults_WithNoInternetConnectivity()
    {
        _service = new InternetConnectivityService(() => false);
        var result1 = _service.HasInternetConnectivity;
        _service.ForceConnection();
        var result2 = _service.HasInternetConnectivity;
        _service.ForceNoConnection();
        var result3 = _service.HasInternetConnectivity;
        _service.ResetToDefault();
        var result4 = _service.HasInternetConnectivity;

        using (Assert.EnterMultipleScope())
        {
            Assert.That(result1, Is.False);
            Assert.That(result2, Is.True);
            Assert.That(result3, Is.False);
            Assert.That(result4, Is.False);
        }
    }
}
