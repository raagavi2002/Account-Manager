// <copyright file="ClerkService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Infrastructure.Services
{
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.Json;
    using AccountManager.Application.Abstractions;
    using AccountManager.Shared.Logging;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Service for interacting with Clerk authentication service.
    /// Provides methods to create users, add them to organizations,
    /// and verify API connectivity.
    /// </summary>
    public class ClerkService : IClerkService
    {
        private readonly HttpClient httpClient;
        private readonly IApplogger logger;
        private readonly string baseUrl;
        private readonly string secretKey;
        private readonly string defaultOrgId;
        private readonly string defaultRole;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClerkService"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client used to send requests.</param>
        /// <param name="configuration">The application configuration containing Clerk settings.</param>
        /// <param name="logger">The logger for capturing application logs.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if required configuration values (SecretKey or OrgId) are missing.
        /// </exception>
        public ClerkService(HttpClient httpClient, IConfiguration configuration, IApplogger logger)
        {
            this.httpClient = httpClient;
            this.logger = logger;
            baseUrl = configuration["Clerk:BaseUrl"] ?? "https://api.clerk.dev/v1";
            secretKey = configuration["Clerk:SecretKey"] ?? throw new InvalidOperationException("Clerk:SecretKey is required");
            defaultOrgId = configuration["Clerk:OrgId"] ?? throw new InvalidOperationException("Clerk:OrgId is required");
            defaultRole = configuration["Clerk:DefaultRole"] ?? "org:member";

            this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", secretKey);
            this.httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// Creates a new user in Clerk and assigns them to an organization.
        /// </summary>
        /// <param name="user">The Active Directory user details.</param>
        /// <param name="orgId">Optional organization ID. Defaults to configured OrgId.</param>
        /// <param name="role">Optional role. Defaults to configured DefaultRole.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>The Clerk user ID of the newly created user.</returns>
        /// <exception cref="Exception">Thrown if user creation fails.</exception>
        public async Task<string> CreateUserAsync(AdUser user, string? orgId = null, string? role = null, CancellationToken cancellationToken = default)
        {
            var payload = new Dictionary<string, object?>
            {
                ["external_id"] = user.ObjectGuid.ToString(),
                ["username"] = Sanitize(user.Username),
                ["first_name"] = user.FirstName,
                ["last_name"] = user.LastName,
                ["password"] = user.Password,
                ["public_metadata"] = new
                {
                    roles = user.Roles
                },
                ["private_metadata"] = new
                {
                    internal_user_guid = user.ObjectGuid,
                    provisioned_by = "api",
                    created_at = DateTime.UtcNow,
                },
            };

            if (!string.IsNullOrWhiteSpace(user.Email))
            {
                payload["email_address"] = new[] { user.Email };
            }

            var response = await httpClient.PostAsync("users", Serialize(payload), cancellationToken);
            var body = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to create user in Clerk: {body}");
            }

            var userId = JsonDocument.Parse(body)
                .RootElement.GetProperty("id")
                .GetString()!;

            // Add to organization
            await AddUserToOrganizationAsync(userId, orgId ?? defaultOrgId, role ?? defaultRole, cancellationToken);

            return userId;
        }

        /// <summary>
        /// Adds an existing Clerk user to an organization with a specified role.
        /// </summary>
        /// <param name="userId">The Clerk user ID.</param>
        /// <param name="orgId">Optional organization ID. Defaults to configured OrgId.</param>
        /// <param name="role">Optional role. Defaults to configured DefaultRole.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <exception cref="Exception">Thrown if adding user to organization fails.</exception>
        public async Task AddUserToOrganizationAsync(string userId, string? orgId = null, string? role = null, CancellationToken cancellationToken = default)
        {
            var payload = new
            {
                user_id = userId,
                role = role ?? defaultRole,
            };

            var response = await httpClient.PostAsync($"organizations/{orgId ?? defaultOrgId}/memberships", Serialize(payload), cancellationToken);
            var body = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to add user to organization in Clerk: {body}");
            }
        }

        /// <summary>
        /// Verifies connectivity to the Clerk API by calling the ping endpoint.
        /// </summary>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns><c>true</c> if the API is reachable; otherwise, <c>false</c>.</returns>
        public async Task<bool> VerifyApiAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await httpClient.GetAsync("ping", cancellationToken);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Sanitizes a username to meet Clerk requirements:
        /// alphanumeric, underscore, dash, and max 64 characters.
        /// </summary>
        /// <param name="username">The input username.</param>
        /// <returns>A sanitized username string.</returns>
        private static string Sanitize(string username)
        {
            return new string(username.Where(c => char.IsLetterOrDigit(c) || c == '_' || c == '-').Take(64).ToArray()).ToLower();
        }

        /// <summary>
        /// Serializes an object into JSON content for HTTP requests.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>A <see cref="StringContent"/> containing JSON data.</returns>
        private static StringContent Serialize(object obj)
        {
            return new StringContent(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");
        }
    }
}
