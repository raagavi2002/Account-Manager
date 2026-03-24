// <copyright file="IApplogger.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Shared.Logging
{
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Defines a contract for application logging functionality, including exceptions, errors, and informational messages.
    /// </summary>
    public interface IApplogger
    {
        /// <summary>
        /// Logs an exception with an optional custom message and additional information.
        /// </summary>
        /// <param name="ex">The exception to be logged.</param>
        /// <param name="message">A descriptive message associated with the exception.</param>
        /// <param name="additionalInfo">Optional additional context information to log.</param>
        /// <param name="filePath">
        /// The full source file path of the caller. Automatically populated by the compiler.
        /// </param>
        /// <param name="methodName">
        /// The name of the calling method. Automatically populated by the compiler.
        /// </param>
        void LogException (
            System.Exception ex,
            string message,
            object? additionalInfo = null,
            [CallerFilePath] string? filePath = "",
            [CallerMemberName] string methodName = "");

        /// <summary>
        /// Logs an error message with optional additional information.
        /// </summary>
        /// <param name="message">The error message to log.</param>
        /// <param name="additionalInfo">Optional additional context information to log.</param>
        /// <param name="filePath">
        /// The full source file path of the caller. Automatically populated by the compiler.
        /// </param>
        /// <param name="methodName"> The name of the calling method. Automatically populated by the compiler.
        /// </param>
        void LogError (
            string message,
            object? additionalInfo = null,
            [CallerFilePath] string filePath = "",
            [CallerMemberName] string methodName = "");

        /// <summary>
        /// Logs an informational message with optional additional information.
        /// </summary>
        /// <param name="message">The informational message to log.</param>
        /// <param name="additionalInfo">Optional additional context information to log.</param>
        /// <param name="filePath">
        /// The full source file path of the caller. Automatically populated by the compiler.
        /// </param>
        /// <param name="methodName">
        /// The name of the calling method. Automatically populated by the compiler.
        /// </param>
        void LogInformation (
            string message,
            object? additionalInfo = null,
            [CallerFilePath] string filePath = "",
            [CallerMemberName] string methodName = "");
    }
}
