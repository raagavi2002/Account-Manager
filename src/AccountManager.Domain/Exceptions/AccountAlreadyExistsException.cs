// <copyright file="AccountAlreadyExistsException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Exceptions
{
    using AccountManager.Domain.Errors;

    /// <summary>
    /// Represents an error that occurs when an account with name already exists.
    /// </summary>
    public class AccountAlreadyExistsException : BaseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountAlreadyExistsException"/> class.
        /// </summary>
        /// <param name="errorResponse">The validation error message.</param>
        public AccountAlreadyExistsException(ErrorResponses errorResponse)
            : base(errorResponse)
        {
        }
    }
}
