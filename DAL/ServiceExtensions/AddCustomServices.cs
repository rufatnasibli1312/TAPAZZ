using DAL.Data;
using DAL.Persistence.Repository.Abstract;
using DAL.Persistence.Repository.Abstraction;
using DAL.Persistence.Repository.Concrete;
using DAL.Persistence.Repository.Implementation;
using Entity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DAL.ServiceExtensions
{
    public static class AddCustomServices
    {
        public static void AddDALServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IComplaintRepository, ComplaintRepository>();
            services.AddScoped<IFavouriteRepository, FavouriteRepository>();

            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddDbContext<MyDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("Default")));







        }
    }
}
