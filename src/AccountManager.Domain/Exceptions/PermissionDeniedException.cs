// <copyright file="PermissionDeniedException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Exceptions
{
    using AccountManager.Domain.Errors;

    /// <summary>
    /// Represents an exception that is thrown when a user attempts an action
    /// without the required permissions.
    /// </summary>
    public class PermissionDeniedException : BaseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionDeniedException"/> class
        /// with a specified error response.
        /// </summary>
        /// <param name="error">
        /// The error response that describes the permission denial.
        /// </param>
        public PermissionDeniedException(ErrorResponses error)
            : base(error)
        {
        }
    }
}
