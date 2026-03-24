// <copyright file="AccountAlreadyArchivedException.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Exceptions
{
    using AccountManager.Domain.Errors;

    /// <summary>
    /// Exception that is thrown when an attempt is made to archive an account 
    /// that has already been archived.
    /// </summary>
    public class AccountAlreadyArchivedException : BaseException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountAlreadyArchivedException"/> class.
        /// </summary>
        /// <param name="error">
        /// The error response containing details about why the account is already archived.
        /// </param>
        public AccountAlreadyArchivedException(ErrorResponses error)
            : base(error)
        {
        }
    }
}
