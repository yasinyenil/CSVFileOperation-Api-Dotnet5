using AutoMapper;
using FileApi.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileApi.Extensions
{
    public static class MappingExtension
    {
        public static IServiceCollection Mapping(this IServiceCollection service)
        {
            var mappingConfig = new MapperConfiguration(i => i.AddProfile(new AutoMapperProfile()));
            IMapper mapper= mappingConfig.CreateMapper();
            service.AddSingleton(mapper);
            return service;
        }
    }

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //No need to set all property if it is same, automapper can set directly. But i write all
            CreateMap<Task<Hotel>, Task<HotelDTO>>().ReverseMap();
        }
    }

}
