using Alphashop_UserWebService.Models;
using Alphashop_UserWebService.Models.Dtos;
using AutoMapper;

namespace Alphashop_UserWebService.Mapper
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<Utenti, UtentiDto>();
            CreateMap<Profili, ProfiliDto>();
        }
    }
}
