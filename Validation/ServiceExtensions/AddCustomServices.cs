using DTO;
using DTO.AccountDto_s;
using DTO.CategoryDto_s;
using DTO.ComplaintDto_s;
using DTO.FavouriteDto_s;
using DTO.LocationDto_s;
using DTO.ProductDto_s;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Validation.CategoryValidator;
using Validation.ComplaintValidator;
using Validation.FavouriteValidator;
using Validation.LocationValidator;
using Validation.ProductValidator;
using Validation.UserValidator;

namespace Validation.ServiceExtensions
{
    public static class AddCustomServices
    {
        public static void AddValidatorServices(this IServiceCollection services)
        {
            services.AddScoped<IValidator<RegisterModelDto>, RegisterValidation>();
            services.AddScoped<IValidator<LoginDto>, LoginValidator>();
            services.AddScoped<IValidator<CategoryToAddDto>, CategoryToAddValidator>();
            services.AddScoped<IValidator<DeleteCategoryDTO>, DeleteCategoryValidator>();
            services.AddScoped<IValidator<UpdateCategoryDto>, UpdateCategoryValidator>();
            services.AddScoped<IValidator<ComplaintAddDto>, ComplaintAddValidator>();
            services.AddScoped<IValidator<DeleteComplaintDto>, DeleteComplaintValidator>();
            services.AddScoped<IValidator<UpdateComplaintDto>, UpdateComplaintValidator>();
            services.AddScoped<IValidator<DeleteFavouriteDto>, DeleteFavouriteValidator>();
            services.AddScoped<IValidator<FavouriteAddDto>, FavouriteAddValidator>();
            services.AddScoped<IValidator<DeleteLocationDto>, DeleteCityValidation>();
            services.AddScoped<IValidator<LocationtoAddDTO>, CitytoAddValidator>();
            services.AddScoped<IValidator<DeleteLocationDto>, DeleteCityValidation>();
            services.AddScoped<IValidator<LocationToUpdateDTO>, CityToUpdateValidator>();
            services.AddScoped<IValidator<DeleteProductDto>, DeleteProductValidator>();
            services.AddScoped<IValidator<ProductAddDto>, ProductAddValidator>();
            services.AddScoped<IValidator<UpdateProductDto>, UpdateProductValidator>();

            services.AddTransient<ProductAddValidator>();
            services.AddTransient<UpdateProductValidator>();



        }
    }
}
