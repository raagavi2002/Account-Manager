using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccountManager.Domain.Errors;

namespace AccountManager.Domain.Exceptions
{
    /// <summary>
    /// Base class for all the exceptions.
    /// </summary>
    [SuppressMessage(
        "Design",
        "CA1032:Implement standard exception constructors",
        Justification = "Intentionally suppressed")]
    public abstract class BaseException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseException"/> class
        /// using a structured error result and optional details.
        /// </summary>
        /// <param name="error">Structured error information.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="error"/> is <c>null</c>.
        /// </exception>
        protected BaseException(ErrorResponses error)
            : base((error ?? throw new ArgumentNullException(nameof(error))).Message)
        {
            Error = error;
        }

        /// <summary>
        /// Get and set the Error.
        /// </summary>
        public ErrorResponses? Error { get; set; }
    }
}
