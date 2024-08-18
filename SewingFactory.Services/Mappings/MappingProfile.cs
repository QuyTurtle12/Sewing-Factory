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
            // Mapping configuration for User to UserDto
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => new RoleDto
                {
                    RoleID = src.Role.ID,         // Map RoleID from User's Role entity
                    RoleName = src.Role.Name      // Map RoleName from User's Role entity
                }))
                .ForMember(dest => dest.Group, opt => opt.MapFrom(src => new GroupDto
                {
                    GroupID = src.Group.ID,       // Map GroupID from User's Group entity
                    GroupName = src.Group.Name    // Map GroupName from User's Group entity
                }));

            // Mapping configuration for CreateDto to User
            // Hashes the password before mapping
            CreateMap<CreateDto, User>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => BCrypt.Net.BCrypt.HashPassword(src.Password)));

            // Mapping configuration for UpdateDto to User
            // Hashes the password if it's provided in the update request
            CreateMap<UpdateDto, User>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password != null ? BCrypt.Net.BCrypt.HashPassword(src.Password) : null));
        }
    }
}
