using AutoMapper;
using AccountManager.Application.Queries.GetUserQuery;

namespace AccountManager.API.User.GetUser
{
    /// <summary>
    /// AutoMapper profile for mapping GetUserQueryResponse to GetUserResponse.
    /// </summary>
    public class GetUserEndpointResponseMapping : Profile
    {
        public GetUserEndpointResponseMapping()
        {
            CreateMap<GetUserQueryResponse, GetUserResponse>();
        }
    }
}
