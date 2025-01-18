using AutoMapper;
using WinFormFramework.BLL.DTOs;
using WinFormFramework.DAL.Entities;

namespace WinFormFramework.BLL.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => 
                    src.UserRoles.Select(ur => ur.Role.RoleName).ToList()));

            CreateMap<UserDTO, User>()
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore());

            CreateMap<SystemLog, SystemLogDTO>();
            CreateMap<SystemLogDTO, SystemLog>();
        }
    }
} 