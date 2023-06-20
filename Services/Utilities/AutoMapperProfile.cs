using AutoMapper;
using Domain.Entities;
using DTOs;
using System.Globalization;

namespace Services.Utilities
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {

            #region Poliza
            CreateMap<Poliza, PolizaDTO>().ReverseMap();
            
            #endregion
        }

    }
}
