using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using B_Layer.DTO;

namespace P_Layer.Models
{
    public class ModelMapping
    {
        public static IMapper Mapper { get; }
        static ModelMapping()
        {
            var config = new MapperConfiguration(c =>
            {
                c.CreateMap<PersonModel, PersonDTO>()
                    .ReverseMap();
                c.CreateMap<PersonDTO, PersonModel>()
                    .ReverseMap();
            });
            Mapper = config.CreateMapper();
        }
    }
}