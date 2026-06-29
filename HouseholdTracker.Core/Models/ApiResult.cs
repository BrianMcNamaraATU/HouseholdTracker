namespace HouseholdTracker.Core.Models;

/// <summary>
/// A generic wrapper for API responses, containing the result data
/// and any error message returned from the central database
/// </summary>
/// <typeparam name="T">The type of data returned from the API</typeparam>
public class ApiResult<T>
{
    /// <summary>
    /// Whether or not the API call was successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// The data returned from the API, or null if the call failed
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// The error message returned from the API, or null if the call succeeded
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Create a successful API result with data
    /// </summary>
    /// <param name="data">The data returned from the API</param>
    /// <returns>A successful ApiResult</returns>
    public static ApiResult<T> Ok(T data) => new() { Success = true, Data = data };

    /// <summary>
    /// Create a failed API result with an error message
    /// </summary>
    /// <param name="message">The error message</param>
    /// <returns>A failed ApiResult</returns>
    public static ApiResult<T> Fail(string message) => new() { Success = false, ErrorMessage = message };
}
