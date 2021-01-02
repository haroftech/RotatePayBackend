using AutoMapper;
using Backend.Dtos;
using Backend.Entities;

namespace Backend.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDtoAll>();
            CreateMap<UserDtoAll, User>(); 
            CreateMap<User, UserDtoUser>();
            CreateMap<UserDtoUser, User>(); 
            CreateMap<User, UserDtoAdmin>();
            CreateMap<UserDtoAdmin, User>(); 
            CreateMap<PaymentNotification, PaymentNotificationDto>();
            CreateMap<PaymentNotificationDto, PaymentNotification>();                                     
        }
    }
}