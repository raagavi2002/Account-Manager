namespace AccountManager.API.Authorization
{
    using System.Reflection;
    using System.Security.Claims;
    using AccountManager.Application.Authorization.Interfaces;
    using AccountManager.Application.Authorization.Models;
    using AccountManager.Shared.Logging;
    using FastEndpoints;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Primitives;

    /// <summary>
    /// FastEndpoints pre-processor that validates authentication, user context,
    /// and permissions before endpoint execution.
    /// </summary>
    /// <typeparam name="TRequest">
    /// The request type that requires permission validation. Must implement <see cref="IRequirePermission"/>.
    /// </typeparam>
    public class PermissionPreProcessor<TRequest> : IPreProcessor<TRequest>
        where TRequest : IRequirePermission
    {
        /// <summary>
        /// Executes pre-processing logic to validate authentication, user context, and permissions.
        /// </summary>
        /// <param name="context">The pre-processor context containing the request and HTTP context.</param>
        /// <param name="ct">Cancellation token to cancel the operation if needed.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if required services or HttpContext are not properly injected.
        /// </exception>
        public async Task PreProcessAsync(IPreProcessorContext<TRequest> context, CancellationToken ct)
        {
            var httpContext = context.HttpContext;
            var request = context.Request;

            // ----------------------------------------------------------------
            // Safety guards (prevents NullReferenceException)
            // ----------------------------------------------------------------
            if (httpContext is null)
            {
                throw new InvalidOperationException("HttpContext is null in PermissionPreProcessor.");
            }

            var permissionResolver = httpContext.RequestServices.GetService<IPermissionResolver>();
            var permissionValidator = httpContext.RequestServices.GetService<IPermissionValidator>();
            var logger = httpContext.RequestServices.GetService<IApplogger>();

            if (permissionResolver is null ||
                permissionValidator is null ||
                logger is null)
            {
                throw new InvalidOperationException("Services were not injected properly.");
            }

            if (request is null)
            {
                logger.LogError("Permission pre-processor received a null request for {Path}", httpContext.Request.Path);
                await SendErrorAsync(context, 400, "Invalid request payload", null, ct);
                return;
            }

            if (string.IsNullOrWhiteSpace(request.RequiredPermission))
            {
                logger.LogError("RequiredPermission was null or empty for request type {RequestType}", typeof(TRequest).Name);
                await SendErrorAsync(context, 500, "Permission configuration is invalid", null, ct);
                return;
            }

            HydrateRouteValues(httpContext, request);

            // ----------------------------------------------------------------
            // E-01: Authentication Check
            // ----------------------------------------------------------------
            /*var isAuthenticated = httpContext.User?.Identity?.IsAuthenticated == true;

            if (!isAuthenticated)
            {
                logger.LogError("Unauthenticated request to {Path}", httpContext.Request.Path);
                await SendErrorAsync(context, 401, "User not authenticated", null, ct);
                return;
            }*/

            // Extract user id from claims
            var userId = httpContext?.User?.FindFirst("sub")?.Value;

            if (string.IsNullOrWhiteSpace(userId))
            {
                logger.LogError("User ID not found in claims");
                await SendErrorAsync(context, 401, "Invalid authentication token", null, ct);
                return;
            }

            var sessionId = ResolveSessionId(httpContext);

            // ----------------------------------------------------------------
            // Resolve User Context
            // ----------------------------------------------------------------
            UserContext userContext;

            try
            {
                userContext = await permissionResolver.ResolveUserContextAsync(
                    userId,
                    request?.AccountId,
                    ct);
            }
            catch (UnauthorizedAccessException ex)
            {
                logger.LogException(ex, "Failed to resolve user context for {UserId}", userId);
                await SendErrorAsync(context, 401, ex.Message, null, ct);
                return;
            }
            catch (Exception ex)
            {
                logger.LogException(ex, "Unexpected error resolving user context for {UserId}", userId);
                await SendErrorAsync(context, 500, "Internal server error", null, ct);
                return;
            }

            if (userContext is null)
            {
                logger.LogError("UserContext resolved as null for {UserId}", userId);
                await SendErrorAsync(context, 401, "Invalid user context", null, ct);
                return;
            }

            userContext.SessionId = sessionId;
            httpContext.Items["SessionId"] = sessionId;

            // ----------------------------------------------------------------
            // E-04: Account Context Required
            // ----------------------------------------------------------------
            /*if (string.IsNullOrEmpty(request?.AccountId) && !userContext.IsAdmin)
            {
                logger.LogError(
                    "Account context ambiguity for user {UserId} - accountId not provided",
                    userId);

                await SendErrorAsync(
                    context,
                    400,
                    "Account ID is required for this operation",
                    null,
                    ct);

                return;
            }*/

            // ----------------------------------------------------------------
            // Validate Context (FIXED: now awaited)
            // ----------------------------------------------------------------
            var isValidContext = permissionResolver.ValidateContextAsync(userContext, ct);

            if (!isValidContext)
            {
                logger.LogError(
                    "Invalid user context for {UserId} accessing account {AccountId}",
                    userId,
                    userContext.AccountId);

                await SendErrorAsync(
                    context,
                    403,
                    "Access to this account is not allowed",
                    request.RequiredPermission,
                    ct);

                return;
            }

            // ----------------------------------------------------------------
            // Validate Permission
            // ----------------------------------------------------------------
            var permissionResult =
                await permissionValidator.ValidatePermissionAsync(
                    userContext,
                    request.RequiredPermission,
                    ct);

            if (permissionResult is null)
            {
                logger.LogError(
                    "Permission validator returned null for user {UserId} and permission {Permission}",
                    userId,
                    request.RequiredPermission);
                await SendErrorAsync(context, 500, "Permission validation failed", request.RequiredPermission, ct);
                return;
            }

            if (!permissionResult.IsGranted)
            {
                logger.LogError(
                    "Permission denied for user {UserId} on account {AccountId}. Required: {Permission}",
                    userId,
                    userContext.AccountId,
                    request.RequiredPermission);

                await SendErrorAsync(
                    context,
                    403,
                    permissionResult.DenialReason ?? "Permission denied",
                    request.RequiredPermission,
                    ct);

                return;
            }

            // ----------------------------------------------------------------
            // Success
            // ----------------------------------------------------------------
            httpContext.Items["UserContext"] = userContext;

            logger.LogInformation(
                "Permission granted for user {UserId} on account {AccountId}. Permission: {Permission}",
                userId,
                userContext.AccountId,
                request.RequiredPermission);
        }

        /// <summary>
        /// Sends a standardized error response to the client.
        /// </summary>
        /// <param name="context">The pre-processor context containing the HTTP response.</param>
        /// <param name="statusCode">The HTTP status code to return.</param>
        /// <param name="message">The error message to include in the response.</param>
        /// <param name="requiredPermission">The required permission, if applicable.</param>
        /// <param name="ct">Cancellation token to cancel the operation if needed.</param>
        private static async Task SendErrorAsync(
            IPreProcessorContext<TRequest> context,
            int statusCode,
            string message,
            string? requiredPermission,
            CancellationToken ct)
        {
            await context.HttpContext.Response.SendAsync(
                new ErrorResponse
                {
                    Message = message,
                    Errors = new Dictionary<string, List<string>>
                    {
                        {
                            "Permission",
                            requiredPermission is not null
                                ? new List<string> { $"Required permission: {requiredPermission}" }
                                : new List<string>()
                        },
                    },
                },
                statusCode,
                cancellation: ct);
        }

        private static void HydrateRouteValues(HttpContext httpContext, TRequest request)
        {
            foreach (var routeValue in httpContext.Request.RouteValues)
            {
                var rawValue = routeValue.Value?.ToString();
                if (string.IsNullOrWhiteSpace(rawValue))
                {
                    continue;
                }

                var property = typeof(TRequest).GetProperty(
                    routeValue.Key,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

                if (property is null || !property.CanWrite)
                {
                    continue;
                }

                var currentValue = property.GetValue(request);
                var targetType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

                if (targetType == typeof(Guid))
                {
                    var currentGuid = currentValue is Guid guid ? guid : Guid.Empty;
                    if (currentGuid != Guid.Empty || !Guid.TryParse(rawValue, out var parsedGuid))
                    {
                        continue;
                    }

                    property.SetValue(request, parsedGuid);
                    continue;
                }

                if (targetType == typeof(string) && currentValue is null)
                {
                    property.SetValue(request, rawValue);
                }
            }
        }

        private static string? ResolveSessionId(HttpContext httpContext)
        {
            if (httpContext.Request.Headers.TryGetValue("X-Session-Id", out StringValues headerValue) &&
                !StringValues.IsNullOrEmpty(headerValue))
            {
                return headerValue.ToString();
            }

            return httpContext.User?.FindFirstValue("sid")
                   ?? httpContext.User?.FindFirstValue("jti")
                   ?? httpContext.User?.FindFirstValue("session_id");
        }
    }
}
