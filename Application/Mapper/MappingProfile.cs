using AutoMapper;
using Domain.Entities;

namespace Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<NatureDepense, string>().ConvertUsing(src => src.ToString());
        }
    }
}
