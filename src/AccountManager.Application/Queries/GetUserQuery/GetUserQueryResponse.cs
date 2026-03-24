using System;
using System.Collections.Generic;

namespace AccountManager.Application.Queries.GetUserQuery
{
    public class GetUserQueryResponse
    {
        public Guid UserId { get; set; }
        public Guid AccountId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<string> Roles { get; set; }
        public object Permissions { get; set; } // Replace with actual PermissionSet type
        public bool IsActive { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public int LoginCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Version { get; set; }
    }
}
