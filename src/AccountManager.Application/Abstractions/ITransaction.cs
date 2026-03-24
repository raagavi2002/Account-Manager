// <copyright file="ITransaction.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Application.Abstractions
{
    /// <summary>
    /// Represents an abstraction for a database transaction.
    /// </summary>
    public interface ITransaction : IAsyncDisposable
    {
        /// <summary>
        /// Gets a value indicating whether the transaction has been completed (committed or rolled back).
        /// </summary>
        bool IsCompleted { get; }
    }
}
