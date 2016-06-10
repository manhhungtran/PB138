using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using B_Layer.DTO;
using DA_Layer.Entities;
using DA_Layer.IdentityEntities;

namespace B_Layer
{
    public static class Mapping
    {
        public static IMapper Mapper { get; }
        static Mapping()
        {
            var config = new MapperConfiguration(c =>
            {
                c.CreateMap<Person, PersonDTO>()
                    .ForMember(dest => dest.IsMale, opt => opt.MapFrom(src => src.IsMale));
                c.CreateMap<PersonDTO, Person>()
                    .ForMember(dest => dest.IsMale, opt => opt.MapFrom(src => src.IsMale));

                c.CreateMap<AppUser, UserDTO>()
                    .ReverseMap();
                c.CreateMap<UserDTO, AppUser>()
                    .ReverseMap();
            });
            Mapper = config.CreateMapper();
        }
    }
}
