using System;
using MediatR;

namespace AccountManager.Application.Queries.GetUserQuery
{
    public class GetUserQueryRequest : IRequest<GetUserQueryResponse>
    {
        public Guid UserId { get; set; }
        public Guid RequestorId { get; set; } // For permission checks
    }
}
