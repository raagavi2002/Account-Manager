// <copyright file="AccountRelationshipNotFoundException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Exceptions
{
    using AccountManager.Domain.Errors;

    /// <summary>
    /// Exception that is thrown when an account relationship cannot be found.
    /// </summary>
    /// <remarks>
    /// This exception is typically used to indicate that a requested relationship
    /// between a head account and a subaccount does not exist in the system.
    /// </remarks>
    public class AccountRelationshipNotFoundException : BaseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountRelationshipNotFoundException"/> class
        /// with a specified error response.
        /// </summary>
        /// <param name="errorResponse">
        /// The error response object that contains details about the error condition.
        /// </param>
        public AccountRelationshipNotFoundException(ErrorResponses errorResponse)
            : base(errorResponse)
        {
        }
    }
}
