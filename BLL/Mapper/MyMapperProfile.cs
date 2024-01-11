using AutoMapper;
using DTO;
using DTO.AccountDto_s;
using DTO.CategoryDto_s;
using DTO.ComplaintDto_s;
using DTO.FavouriteDto_s;
using DTO.LocationDto_s;
using DTO.ProductDto_s;
using Entity.Entities;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Mapper
{
    public class MyMapperProfile : Profile
    {
        public MyMapperProfile()
        {
            //Location
            CreateMap<Location, LocationToListDto>().ReverseMap();
            CreateMap<Location, LocationtoAddDTO>().ReverseMap();
            CreateMap<Location, LocationToUpdateDTO>().ReverseMap();
            CreateMap<Location, DeleteLocationDto>().ReverseMap();
            CreateMap<Location, LocationFindIdDTO>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Location, LocationAddProductsGetDTO>().ReverseMap();
            //Category
            CreateMap<Category, CategoryToAddDto>().ReverseMap();
            CreateMap<Category, CategoryToListDto>().ReverseMap();
            CreateMap<Category, DeleteCategoryDTO>().ReverseMap();
            CreateMap<Category, UpdateCategoryDto>().ReverseMap();
            CreateMap<Category, CategoryFindIdDTO>().ReverseMap();
            //Account
            CreateMap<User, RegisterModelDto>().ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.UserName)).ReverseMap();
            CreateMap<User, UpdateUserDto>().ReverseMap();



            //Product
            CreateMap<ProductAddDto, Product>()
                .ForMember(d => d.CreateDate, opt => opt.MapFrom(s => DateTime.Now))
                .ForMember(d => d.ExpireDate, opt => opt.MapFrom(s => DateTime.Now.AddDays(14)))
              //.ForMember(d => d.IsActive, opt => opt.MapFrom(s => DateTime.Now < (DateTime.Now.AddDays(14))))
                .ReverseMap();
           

            CreateMap<Product, ProductToListDto>().ReverseMap();
            CreateMap<Product, ProductFindIdDto>().ReverseMap();
            CreateMap<Product, UpdateProductDto>().ReverseMap();

            //Complaint
            CreateMap<ComplaintAddDto, Complaint>()
                .ForMember(m => m.CreateDate, opt => opt.MapFrom(s => DateTime.Now))
                .ReverseMap();
            CreateMap<Complaint, ComplaintFindIdDto>().ReverseMap();
            CreateMap<Complaint, UpdateComplaintDto>().ReverseMap();
            //Favourite
            CreateMap<FavouriteAddDto, Favorite>().ReverseMap();
            CreateMap<Favorite, FavouriteGetProductDto>().ReverseMap();
            CreateMap<Favorite, DeleteFavouriteDto>().ReverseMap();



        }
    }
}
