// <copyright file="AccountValidationException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Exceptions
{
    using System;
    using AccountManager.Domain.Errors;

    /// <summary>
    /// Represents an error that occurs when account validation rules are violated.
    /// </summary>
    public sealed class AccountValidationException : BaseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountValidationException"/> class.
        /// </summary>
        /// <param name="errorResponse">The validation error message.</param>
        public AccountValidationException(ErrorResponses errorResponse)
            : base(errorResponse)
        {
        }
    }
}
