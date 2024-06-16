using AutoMapper;
using UTechLeague24.Backend.Auth.Models;
using UTechLeague24.Backend.Domain.Models;

namespace UTechLeague24.Backend.Auth.Profiles;

public class AuthProfile : Profile
{
    public AuthProfile()
    {
        CreateMap<RegisterRequest, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
    }
}