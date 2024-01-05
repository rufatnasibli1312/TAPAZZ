using BLL.Mapper;
using BLL.Persistence.Service.Abstract;
using BLL.Persistence.Service.Abstraction;
using BLL.Persistence.Service.Concrete;
using BLL.Persistence.Service.Implementation;
using Entity.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ServiceExtensions
{
    public static class AddCustomServices
    {
        public static void AddBLLServices(this IServiceCollection services)
        {

            services.AddAutoMapper(typeof(MyMapperProfile));

            services.AddScoped<IAccountService, AccountService>();

            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IComplaintService, ComplaintService>();
            services.AddScoped<IFavouriteService,FavouriteService>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<JwtTokenExtractor>();
           


        }
    }
}
