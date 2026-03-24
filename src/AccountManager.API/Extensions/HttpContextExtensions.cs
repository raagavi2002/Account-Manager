// <copyright file="HttpContextExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.API.Extensions
{
    using AccountManager.Application.Authorization.Models;

    /// <summary>
    /// Provides extension methods for accessing <see cref="UserContext"/> from <see cref="HttpContext"/>.
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Gets the <see cref="UserContext"/> stored by the <c>PermissionPreProcessor</c>.
        /// </summary>
        /// <param name="httpContext">The current HTTP context.</param>
        /// <returns>The <see cref="UserContext"/> associated with the request.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the <see cref="UserContext"/> is not found in <paramref name="httpContext"/>.
        /// </exception>
        public static UserContext GetUserContext(this HttpContext httpContext)
        {
            if (httpContext.Items.TryGetValue("UserContext", out var context) &&
                context is UserContext userContext)
            {
                return userContext;
            }

            throw new InvalidOperationException("UserContext not found in HttpContext");
        }

        /// <summary>
        /// Attempts to retrieve the <see cref="UserContext"/> from the <see cref="HttpContext"/>.
        /// </summary>
        /// <param name="httpContext">The current HTTP context.</param>
        /// <param name="userContext">
        /// When this method returns, contains the <see cref="UserContext"/> if found; otherwise, <c>null</c>.
        /// </param>
        /// <returns>
        /// <c>true</c> if the <see cref="UserContext"/> was successfully retrieved; otherwise, <c>false</c>.
        /// </returns>
        public static bool TryGetUserContext(this HttpContext httpContext, out UserContext? userContext)
        {
            if (httpContext.Items.TryGetValue("UserContext", out var context) &&
                context is UserContext ctx)
            {
                userContext = ctx;
                return true;
            }

            userContext = null;
            return false;
        }
    }
}
