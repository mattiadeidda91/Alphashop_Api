using Alphashop_ArticoliWebService.Models;
using Alphashop_ArticoliWebService.Models.Dtos;
using AutoMapper;

namespace Alphashop_ArticoliWebService.Mappers
{
    public class Mapper : Profile
    {
        public Mapper() 
        {
            _ = CreateMap<Articoli, ArticoliDto>();
            _ = CreateMap<BarCode, BarCodeDto>()
                .ForMember(dest => dest.IdTipoArt, src => src.MapFrom(src => src.IdTipoArt)); //non serve è solo per promemoria di come si fa
            _ = CreateMap<Iva, IvaDto>();
            _ = CreateMap<FamAssort, FamAssortDto>();

            CreateMap<string?, string?>()
                .ConvertUsing(src => ForattingString(src));
            CreateMap<string?, string>()
                .ConvertUsing(src => ForattingString(src));
            CreateMap<string, string>()
                .ConvertUsing(src => ForattingString(src));
            CreateMap<string, string?>()
                .ConvertUsing(src => ForattingString(src));
            CreateMap<int?, int?>()
                .ConvertUsing(src => src.HasValue ? src : 0);
        }

        private static string ForattingString(string? param)
        {
            if (!string.IsNullOrEmpty(param)) 
                return param.Trim();
            else
                return string.Empty;
        }
    }
}
