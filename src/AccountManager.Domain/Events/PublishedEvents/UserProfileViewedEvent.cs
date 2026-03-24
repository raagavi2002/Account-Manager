using System.Diagnostics.CodeAnalysis;
using AccountManager.Domain.Events.Constants;
using AccountManager.Domain.Events.Models;

namespace AccountManager.Domain.Events.Published
{
    /// <summary>
    /// Represents an event published when a user profile is viewed/retrieved.
    /// </summary>
    public class UserProfileViewedEvent : BaseEvent<UserProfileViewedData>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfileViewedEvent"/> class.
        /// </summary>
        [SetsRequiredMembers]
        public UserProfileViewedEvent()
        {
            this.EventType = EventTypes.UserProfileViewed;
        }
    }
}
