// <copyright file="AccountAlreadyLinkedException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AccountManager.Domain.Errors;

namespace AccountManager.Domain.Exceptions
{
    /// <summary>
    /// Represents an error that occurs when an account is already linked
    /// to another account and a duplicate or conflicting relationship
    /// is attempted.
    /// </summary>
    /// <remarks>
    /// This exception is thrown when the system detects an attempt to
    /// create an account relationship that already exists or violates
    /// the rule that an account can only be linked once in a given role
    /// (for example, a sub-account already associated with a head account).
    /// </remarks>
    public class AccountAlreadyLinkedException : BaseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountAlreadyLinkedException"/> class.
        /// </summary>
        /// <param name="errorResponse">
        /// An error response containing details about the conflict,
        /// such as error codes and descriptive messages.
        /// </param>
        public AccountAlreadyLinkedException(ErrorResponses errorResponse)
            : base(errorResponse)
        {
        }
    }
}
