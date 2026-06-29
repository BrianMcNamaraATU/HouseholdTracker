using System.Text;
using System.Text.Json;
using HouseholdTracker.Core.Models;
using HouseholdTracker.Core.Services;
using HouseholdTracker.Core;

namespace HouseholdTracker.Services;

/// <summary>
/// The real implementation of the API service, using HttpClient to
/// communicate with the central database REST API
/// </summary>
public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    /// <summary>
    /// Constructor for the ApiService
    /// </summary>
    /// <param name="httpClient">The HttpClient to use for API calls</param>
    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Register a new user with the central database
    /// </summary>
    /// <param name="firstName">The user's first name</param>
    /// <param name="lastName">The user's last name</param>
    /// <param name="email">The user's email address</param>
    /// <param name="password">The user's chosen password</param>
    /// <returns>The registered user if successful, or an error message if not</returns>
    public async Task<ApiResult<RegisteredUser>> RegisterAsync(string firstName, string lastName, string email, string password)
    {
        try
        {
            var payload = new { first_name = firstName, last_name = lastName, email, password };
            var url = $"{Constants.ApiBaseUrl}/add_user.php?key={Constants.AppApiKey}";
            var result = await PostAsync<RegisteredUserResponse>(url, payload);

            if (!result.Success || result.Data is null)
                return ApiResult<RegisteredUser>.Fail(result.ErrorMessage ?? "Registration failed.");

            return ApiResult<RegisteredUser>.Ok(new RegisteredUser(
                result.Data.Id,
                firstName,
                lastName,
                email,
                result.Data.ApiKey
            ));
        }
        catch (Exception ex)
        {
            return ApiResult<RegisteredUser>.Fail($"An error occurred: {ex.Message}");
        }
    }

    /// <summary>
    /// Login an existing user with the central database
    /// </summary>
    /// <param name="email">The user's email address</param>
    /// <param name="password">The user's password</param>
    /// <returns>The logged in user if successful, or an error message if not</returns>
    public async Task<ApiResult<RegisteredUser>> LoginAsync(string email, string password)
    {
        try
        {
            var payload = new { email, password };
            var url = $"{Constants.ApiBaseUrl}/login.php?key={Constants.AppApiKey}";
            var result = await PostAsync<LoginResponse>(url, payload);

            if (!result.Success || result.Data is null)
                return ApiResult<RegisteredUser>.Fail(result.ErrorMessage ?? "Login failed.");

            return ApiResult<RegisteredUser>.Ok(new RegisteredUser(
                result.Data.Id,
                result.Data.FirstName,
                result.Data.LastName,
                email,
                result.Data.ApiKey,
                result.Data.ForcePasswordReset,
                result.Data.EmailVerified
            ));
        }
        catch (Exception ex)
        {
            return ApiResult<RegisteredUser>.Fail($"An error occurred: {ex.Message}");
        }
    }

    /// <summary>
    /// Request a password reset for a user
    /// </summary>
    /// <param name="email">The user's email address</param>
    /// <returns>A result indicating success or failure</returns>
    public async Task<ApiResult<bool>> ForgotPasswordAsync(string email)
    {
        try
        {
            var payload = new { email };
            var url = $"{Constants.ApiBaseUrl}/forgot_password.php?key={Constants.AppApiKey}";
            var result = await PostAsync<object>(url, payload);

            return result.Success
                ? ApiResult<bool>.Ok(true)
                : ApiResult<bool>.Fail(result.ErrorMessage ?? "Request failed.");
        }
        catch (Exception ex)
        {
            return ApiResult<bool>.Fail($"An error occurred: {ex.Message}");
        }
    }

    /// <summary>
    /// Change the password for a logged in user
    /// </summary>
    /// <param name="userId">The user's Id</param>
    /// <param name="apiKey">The user's API key</param>
    /// <param name="email">The user's email address</param>
    /// <param name="currentPassword">The user's current password</param>
    /// <param name="newPassword">The user's new password</param>
    /// <returns>A result indicating success or failure</returns>
    public async Task<ApiResult<bool>> ChangePasswordAsync(int userId, string apiKey, string email, string currentPassword, string newPassword)
    {
        try
        {
            var payload = new { email, current_password = currentPassword, new_password = newPassword };
            var url = $"{Constants.ApiBaseUrl}/change_password.php?key={apiKey}&user_id={userId}";
            var result = await PostAsync<object>(url, payload);

            return result.Success
                ? ApiResult<bool>.Ok(true)
                : ApiResult<bool>.Fail(result.ErrorMessage ?? "Password change failed.");
        }
        catch (Exception ex)
        {
            return ApiResult<bool>.Fail($"An error occurred: {ex.Message}");
        }
    }

    /// <summary>
    /// Internal helper to POST JSON to an API endpoint and deserialise the response
    /// </summary>
    /// <typeparam name="T">The type to deserialise the response data into</typeparam>
    /// <param name="url">The full URL to POST to</param>
    /// <param name="payload">The object to serialise as the request body</param>
    /// <returns>An ApiResponse wrapping the deserialised data</returns>
    private async Task<ApiResponse<T>> PostAsync<T>(string url, object payload)
    {
        var json = JsonSerializer.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(url, content);
        var body = await response.Content.ReadAsStringAsync();
        System.Diagnostics.Debug.WriteLine($"Login Response: {body}");
        return JsonSerializer.Deserialize<ApiResponse<T>>(body, JsonOptions)
               ?? new ApiResponse<T> { Success = false, ErrorMessage = "Empty response from server." };
    }

    // Internal response shape classes matching the PHP API JSON structure

    private class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string? ErrorMessage { get; set; }
        public string? ApiVersion { get; set; }
    }

    private class RegisteredUserResponse
    {
        public int Id { get; set; }
        public string ApiKey { get; set; } = string.Empty;
    }

    private class LoginResponse
    {
        [System.Text.Json.Serialization.JsonPropertyName("id")]
        public int Id { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("first_name")]
        public string FirstName { get; set; } = string.Empty;

        [System.Text.Json.Serialization.JsonPropertyName("last_name")]
        public string LastName { get; set; } = string.Empty;

        [System.Text.Json.Serialization.JsonPropertyName("api_key")]
        public string ApiKey { get; set; } = string.Empty;

        [System.Text.Json.Serialization.JsonPropertyName("force_password_reset")]
        public bool ForcePasswordReset { get; set; }

        [System.Text.Json.Serialization.JsonPropertyName("email_verified")]
        public bool EmailVerified { get; set; }
    }
}
