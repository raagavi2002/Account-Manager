// <copyright file="GlobalExceptionMiddleware.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Middleware
{
    using System.Net;
    using System.Text.Json;
    using AccountManager.API.ErrorResponses;
    using AccountManager.Domain.Exceptions;
    using AccountManager.Shared.Logging;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Middleware for handling exceptions globally and returning
    /// standardized API error responses.
    /// </summary>
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IApplogger appLogger;

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalExceptionMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="appLogger">The application logger.</param>
        public GlobalExceptionMiddleware(RequestDelegate next, IApplogger appLogger)
        {
            this.next = next;
            this.appLogger = appLogger;
        }

        /// <summary>
        /// Invokes the middleware and handles any unhandled exception.
        /// </summary>
        /// <param name="context">The current HTTP context.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            try
            {
                await next(context).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await HandleAsync(context, ex).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Builds a standardized API error from a domain exception.
        /// </summary>
        /// <param name="exception">The domain exception.</param>
        /// <returns>A populated <see cref="ApiErrorResponse"/> instance.</returns>
        private static ApiErrorResponse BuildApiError(BaseException exception)
        {
            return new ApiErrorResponse
            {
                Code = exception?.Error?.Code,
                Message = exception?.Error?.Message ?? string.Empty,
                Details = new ApiErrorInfo
                {
                    AccountId = exception?.Error?.Details?.AccountId,
                    ProductId = exception?.Error?.Details?.ProductId,
                    AdditionalInfo = exception?.Error?.Details?.AdditionalInfo,
                },
                TimeStamp = exception?.Error?.TimeStamp ?? DateTime.Today.ToString(),
                CorrelationId = exception?.Error?.CorrelationId?.ToString() ?? string.Empty,
            };
        }

        /// <summary>
        /// Handles exceptions and writes standardized error responses.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <param name="exception">The exception that occurred.</param>
        private async Task HandleAsync(HttpContext context, Exception exception)
        {
            var (statusCode, apiError) = exception switch
            {
                AccountValidationException ex =>
                    (HttpStatusCode.BadRequest, BuildApiError(ex)),

                AccountAlreadyExistsException ex =>
                    (HttpStatusCode.Conflict, BuildApiError(ex)),

                AccountAlreadyLinkedException ex =>
                    (HttpStatusCode.Conflict, BuildApiError(ex)),

                InvalidAccountStatusTransitionException ex =>
                    (HttpStatusCode.Conflict, BuildApiError(ex)),

                UserNotFoundException ex =>
                    (HttpStatusCode.NotFound, BuildApiError(ex)),

                UserValidationException ex =>
                    (HttpStatusCode.BadRequest, BuildApiError(ex)),

                AccountRelationshipNotFoundException ex =>
                    (HttpStatusCode.NotFound, BuildApiError(ex)),

                AccountNotFoundException ex =>
                    (HttpStatusCode.NotFound, BuildApiError(ex)),

                UnauthorizedAccessException ex =>
                    (HttpStatusCode.Forbidden, new ApiErrorResponse
                    {
                        Code = "UNAUTHORIZED-EXCEPTION",
                        Message = ex.Message,
                        TimeStamp = DateTime.UtcNow.ToString(),
                        CorrelationId = Guid.NewGuid().ToString(),
                    }),

                ArgumentNullException ex =>
                    (HttpStatusCode.BadRequest, new ApiErrorResponse
                    {
                        Code = "BAD-REQUEST",
                        Message = $"Required field is missing: {ex.ParamName}",
                        TimeStamp = DateTime.UtcNow.ToString(),
                        CorrelationId = Guid.NewGuid().ToString(),
                    }),

                ArgumentException ex =>
                    (HttpStatusCode.BadRequest, new ApiErrorResponse
                    {
                        Code = "BAD-REQUEST",
                        Message = ex.Message,
                        TimeStamp = DateTime.UtcNow.ToString(),
                    }),
                Exception ex => (HttpStatusCode.BadRequest, new ApiErrorResponse
                {
                    Code = "BAD-REQUEST",
                    Message = ex.Message,
                    Details = new ApiErrorInfo
                    {
                        AdditionalInfo = new Dictionary<string, string>
                            {
                                { ex.Source, ex.StackTrace},
                            },
                    },
                    TimeStamp = DateTime.UtcNow.ToString(),
                }),
                _ =>
                    (HttpStatusCode.InternalServerError, new ApiErrorResponse
                    {
                        Code = "INTERNAL SERVER ERROR",
                        Message = "An unexpected error occurred.",
                        TimeStamp = DateTime.UtcNow.ToString(),
                    })
            };

            // Logging
            if ((int)statusCode >= 500)
            {
                appLogger.LogException(
                    exception,
                    "Unhandled server error");
            }
            else
            {
                appLogger.LogError(
                    $"Client error: {apiError.Message} | Code: {apiError.Code}");
            }

            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
            };
            await context.Response.WriteAsync(JsonSerializer.Serialize(apiError, jsonOptions)).ConfigureAwait(false);
        }
    }
}
