// <copyright file="InvalidAccountStatusTransitionException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AccountManager.Domain.Errors;

namespace AccountManager.Domain.Exceptions
{
    /// <summary>
    /// Represents an error that occurs when an invalid account status transition is attempted.
    /// </summary>
    public sealed class InvalidAccountStatusTransitionException : BaseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidAccountStatusTransitionException"/> class.
        /// </summary>
        /// <param name="errorResponse">The validation error message.</param>
        public InvalidAccountStatusTransitionException(ErrorResponses errorResponse)
            : base(errorResponse)
        {
        }
    }
}
