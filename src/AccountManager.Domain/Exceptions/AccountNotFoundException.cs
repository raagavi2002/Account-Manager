// <copyright file="AccountNotFoundException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Exceptions
{
    using AccountManager.Domain.Errors;

    /// <summary>
    /// Represents an error that occurs when an account cannot be found.
    /// </summary>
    public sealed class AccountNotFoundException : BaseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountNotFoundException"/> class.
        /// </summary>
        /// <param name="errorResponse">errorResponse.</param>
        public AccountNotFoundException(ErrorResponses errorResponse)
            : base(errorResponse)
        {
        }
    }
}
