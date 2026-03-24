using System;

namespace AccountManager.Application.Commands.UpdateUserCommand
{
    public class UpdateUserCommandResponse
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
