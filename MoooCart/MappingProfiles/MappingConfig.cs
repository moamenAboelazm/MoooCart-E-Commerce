using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata;
using MoooCart.DB.Models;
using MoooCart.id.Identity;
using MoooCart.lib.DTOs;

namespace MoooCart.MappingProfiles
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<DtoCategory, ClsCategory>();
            CreateMap<ClsCategory, DtoGetCategory>();
            CreateMap<ClsCategory, DtoCategory>();
            CreateMap<DtoProduct, ClsProduct>();
            CreateMap<ClsProduct, DtoGetProducts>();
            CreateMap<ClsProduct, DtoProduct>();
            
            CreateMap<ClsProduct, DtoGetProducts>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

            CreateMap<DtoCreateUser, AppUser>();
            CreateMap<DtoLoginUser, AppUser>();

            CreateMap<DtoCart, ClsProductsHistory>().ReverseMap();

            CreateMap<DtoProductHistory, ClsProductsHistory>();
            
        }
    }
}