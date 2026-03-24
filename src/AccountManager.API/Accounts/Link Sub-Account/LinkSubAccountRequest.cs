using AccountManager.API.Authorization;
using AccountManager.Domain.Enums.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccountManager.API.Accounts.Link_Sub_Account
{
    /// <summary>
    /// Request payload for creating a head-sub account relationship.
    /// </summary>
    public class LinkSubAccountRequest : IRequirePermission
    {
        /// <summary>
        /// Gets or sets the unique identifier of the account that will become the head account.
        /// </summary>
        [FromRoute]
        public Guid HeadAccountId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier of the account that will become the sub-account.
        /// </summary>
        public Guid SubAccountId { get; set; }

        /// <summary>
        /// Gets or sets the type of relationship to create between the head and sub account.
        /// </summary>
        /// <remarks>
        /// Must be set to <c>HEAD_SUB</c>.
        /// </remarks>
        public string RelationshipType { get; set; } = "HEAD_SUB";

        /// <summary>
        /// Gets the permission required to perform this request.
        /// </summary>
        public string RequiredPermission => Permissions.Administrative.Update.Account;

        /// <summary>
        /// Gets the account identifier associated with the permission requirement.
        /// </summary>
        string? IRequirePermission.AccountId => HeadAccountId.ToString();
    }
}
