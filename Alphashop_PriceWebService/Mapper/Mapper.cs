using Alphashop_PriceWebService.Models;
using Alphashop_PriceWebService.Models.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Routing.Constraints;

namespace Alphashop_PriceWebService.Mapper
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            _ = CreateMap<DettListino, DettListinoDto>().ReverseMap();
            _ = CreateMap<Listino, ListinoDto>()
                .ForMember(dest => dest.Obsoleto, src => src.MapFrom(src => MapObsoletoField(src.Obsoleto)));

            _ = CreateMap<ListinoDto, Listino>()
               .ForMember(dest => dest.Obsoleto, src => src.MapFrom(src => MapObsoletoField(src.Obsoleto)));
        }

        private static bool MapObsoletoField(string obsolete)
        {
            if (string.IsNullOrEmpty(obsolete))
                return false;
            else
                return obsolete.ToLower() == "si";
        }

        private static string MapObsoletoField(bool obsolete)
        {
            return obsolete ? "Si" : "No";
        }
    }
}
