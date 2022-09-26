using AutoMapper;
using ServiceApp.Models.DTO;
using ServiceApp.Models.Entities;

namespace ServiceApp.API.Utils
{
    public static class AutoMapperConfig
    {
        public static IMapper Initialize()
        {
            return new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Role, RoleDto>();
                    cfg.CreateMap<RoleDto, Role>();
                    cfg.CreateMap<User, UserDto>();
                    cfg.CreateMap<User, CustomerDto>();
                    cfg.CreateMap<UserRegisterDto, User>();
                    cfg.CreateMap<TicketDto, Ticket>();
                    cfg.CreateMap<CreateTicketDto, Ticket>();
                    cfg.CreateMap<CustomerDto, Customer>();
                    cfg.CreateMap<JiraCredentials, JiraCredentialsDto>();
                    cfg.CreateMap<Ticket, TicketDto>()
                        .ForMember(dest => dest.ResponsibleUser, opt => opt.MapFrom(src => src.ResponsibleUser))
                        .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments))
                        .ForMember(dest => dest.TicketStatus, opt => opt.MapFrom(src => src.TicketStatus));

                    cfg.CreateMap<Comment, CommentDto>()
                        .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author));

                })
                .CreateMapper();
        }
    }
}
