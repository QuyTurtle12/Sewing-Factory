using AutoMapper;
using SewingFactory.Models;
using SewingFactory.Services.Dto.UserDto.RequestDto;
using SewingFactory.Services.Dto.UserDto.RespondDto;

namespace SewingFactory.Services.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name))
                .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.Group.Name));

            CreateMap<CreateDto, User>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.Password)));

            CreateMap<UpdateDto, User>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password != null ? BCrypt.Net.BCrypt.HashPassword(src.Password) : null));
        }
    }
}