using AutoMapper;
using RadzenTemplate.EntityFrameworkCore.SqlServer.Models;
using RadzenTemplate.Shared;

namespace RadzenTemplate.Server.AutoMapperProfiles
{
    public class UserConfigurationProfile : Profile
    {
        public UserConfigurationProfile()
        {
            CreateMap<UserConfiguration, UserConfigurationDto>().ReverseMap();
        }
    }
}
