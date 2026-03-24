// <copyright file="PermissionSource.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AccountManager.Domain.Enums
{
    /// <summary>
    /// Specifies the source from which a permission is granted to a user.
    /// </summary>
    public enum PermissionSource
    {
        /// <summary>
        /// The permission is not granted from any source.
        /// </summary>
        None = 0,

        /// <summary>
        /// The permission is granted through the user's role assignments.
        /// </summary>
        Role = 1,

        /// <summary>
        /// The permission is granted through a user-specific override.
        /// </summary>
        Override = 2,

        /// <summary>
        /// The permission is granted through both role assignment and override.
        /// </summary>
        Both = 3,
    }
}
