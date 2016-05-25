using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using B_Layer.DTO;
using DA_Layer.Entities;

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
                    .ReverseMap();

                c.CreateMap<PersonDTO, Person>()
                    .ReverseMap();
            });
            Mapper = config.CreateMapper();
        }
    }
}
