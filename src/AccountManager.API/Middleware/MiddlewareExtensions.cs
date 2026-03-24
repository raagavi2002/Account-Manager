// <copyright file="MiddlewareExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Middleware
{
    using Microsoft.AspNetCore.Builder;

    /// <summary>
    /// Extension methods for configuring middleware.
    /// </summary>
    public static class MiddlewareExtensions
    {
        /// <summary>
        /// Adds the global exception handling middleware to the application pipeline.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <returns>IApplicationBuilder.</returns>
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<GlobalExceptionMiddleware>();
        }
    }
}
