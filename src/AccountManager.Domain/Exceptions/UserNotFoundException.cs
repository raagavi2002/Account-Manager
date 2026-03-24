// <copyright file="UserNotFoundException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Exceptions
{
    using AccountManager.Domain.Errors;

    /// <summary>
    /// Exception that is thrown when a requested user cannot be found in the system.
    /// </summary>
    /// <remarks>
    /// This exception is typically used in repository or service methods when a user lookup
    /// fails, ensuring that the error can be handled consistently across application layers.
    /// </remarks>
    public class UserNotFoundException : BaseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserNotFoundException"/> class
        /// with the specified error response.
        /// </summary>
        /// <param name="error">
        /// The error response object containing details about the failure.
        /// </param>
        public UserNotFoundException(ErrorResponses error)
            : base(error)
        {
        }
    }
}
