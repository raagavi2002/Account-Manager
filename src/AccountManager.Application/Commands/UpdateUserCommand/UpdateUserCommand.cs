using System;
using MediatR;

namespace AccountManager.Application.Commands.UpdateUserCommand
{
    public class UpdateUserCommand : IRequest<UpdateUserCommandResponse>
    {
        public Guid UserId { get; set; }
        public Guid RequestorId { get; set; } // For permission checks and audit
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
