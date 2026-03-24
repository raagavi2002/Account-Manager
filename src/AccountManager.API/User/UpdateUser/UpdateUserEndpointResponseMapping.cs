using AutoMapper;
using AccountManager.Application.Commands.UpdateUserCommand;

namespace AccountManager.API.User.UpdateUser
{
    /// <summary>
    /// AutoMapper profile for mapping UpdateUserCommandResponse to UpdateUserResponse.
    /// </summary>
    public class UpdateUserEndpointResponseMapping : Profile
    {
        public UpdateUserEndpointResponseMapping()
        {
            CreateMap<UpdateUserCommandResponse, UpdateUserResponse>();
        }
    }
}
