using AuthenticatedWebApp.Auth.Models;
using AuthenticatedWebApp.Domain.Models;
using AutoMapper;

namespace AuthenticatedWebApp.Auth.Profiles;

public class AuthProfile : Profile
{
    public AuthProfile()
    {
        CreateMap<RegisterRequest, User>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
    }
}