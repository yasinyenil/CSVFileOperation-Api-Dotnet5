using AutoMapper;
using FileApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileApi.Service.Config
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Hotel, HotelDTO>();
        }
    }

    public class ObjectMapper
    {
        public static IMapper Mapper
        {
            get { return mapper.Value; }
        }

        public static IConfigurationProvider Configuration
        {
            get { return config.Value; }
        }

        public static Lazy<IMapper> mapper = new Lazy<IMapper>(() =>
        {
            var mapper = new Mapper(Configuration);
            return mapper;
        });

        public static Lazy<IConfigurationProvider> config = new Lazy<IConfigurationProvider>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MapperProfile>();
                //cfg.AddProfile<AppCore.Config.MapperProfile>();  // any other profiles you need to use
            });

            return config;
        });
    }
}
