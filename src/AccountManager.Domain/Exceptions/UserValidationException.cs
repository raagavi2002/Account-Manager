// <copyright file="UserValidationException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Exceptions
{
    using AccountManager.Domain.Errors;

    /// <summary>
    /// Exception that is thrown when user validation fails due to invalid input or business rule violations.
    /// </summary>
    /// <remarks>
    /// This exception is typically used in service or domain logic when user data does not meet
    /// required validation rules, ensuring that errors are handled consistently across application layers.
    /// </remarks>
    public class UserValidationException : BaseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserValidationException"/> class
        /// with the specified error response.
        /// </summary>
        /// <param name="error">
        /// The error response object containing details about the validation failure.
        /// </param>
        public UserValidationException(ErrorResponses error)
            : base(error)
        {
        }
    }
}
