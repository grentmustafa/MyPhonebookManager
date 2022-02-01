using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MyPhoneBookManager.DTO;
using MyPhoneBookManager.Entitites.Models;

namespace MyPhoneBookManager.Domain.MappingProfile
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<PhoneNumberRecord, PhoneNumberInDTO>().ReverseMap();
            CreateMap<PhoneNumberRecord, PhoneNumberOutDTO>().ReverseMap();
            
            CreateMap<User, UsersOutDTO>()
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumbers))
                .ReverseMap();

            CreateMap<UsersInDTO,User>()
                .ForMember(dest => dest.PhoneNumbers, opt => opt.MapFrom(src => src.PhoneNumbers))
                .ReverseMap();
        }
    }
}
