using Entity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data
{
    public class MyDbContext : IdentityDbContext<User>
    {
        private readonly IConfiguration _configuration;
        public MyDbContext(DbContextOptions<MyDbContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<ImagesPath> Files { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18, 2)");



            modelBuilder.Entity<City>().HasData(
                new City { Id = 27, Name = "Bakı" },
                new City { Id = 28, Name = "Gəncə" },
                new City { Id = 29, Name = "Sumqayıt" },
                new City { Id = 30, Name = "Mingəçevir" },
                new City { Id = 31, Name = "Naxçıvan" },
                new City { Id = 32, Name = "Şirvan" },
                new City { Id = 33, Name = "Şəki" },
                new City { Id = 34, Name = "Lənkəran" },
                new City { Id = 35, Name = "Xaçmaz" },
                new City { Id = 36, Name = "Göyçay" },
                new City { Id = 37, Name = "Quba" },
                new City { Id = 38, Name = "Ağcabədi" },
                new City { Id = 39, Name = "İmişli" },
                new City { Id = 40, Name = "Bərdə" },
                new City { Id = 41, Name = "Sabirabad" },
                new City { Id = 42, Name = "Xızı" },
                new City { Id = 43, Name = "Yevlax" },
                new City { Id = 44, Name = "Qusar" },
                new City { Id = 45, Name = "Yardımlı" },
                new City { Id = 46, Name = "Zaqatala" }
            );
            modelBuilder.Entity<Category>().HasData(
            new Category
            {
                Id = 37,
                Name = "Digər",
                ParentCategoryId = null,
            },
            new Category
            {
                Id = 38,
                Name = "Elektronika",
                ParentCategoryId = null,
            },
            new Category
            {
                Id = 39,
                Name = "Daşınmaz Əmlak",
                ParentCategoryId = null,
            },
            new Category
            {
                Id = 40,
                Name = "Ev və bağ üçün",
                ParentCategoryId = null,
            },
            new Category
            {
                Id = 41,
                Name = "Şəxsi Əşyalar",
                ParentCategoryId = null,
            },
            new Category
            {
                Id = 42,
                Name = "Nəqliyyat",
                ParentCategoryId = null
            },
            new Category
            {
                Id = 43,
                Name = "Telefonlar",
                ParentCategoryId = 38,
            },
            new Category
            {
                Id = 44,
                Name = "Smart Saatlar",
                ParentCategoryId = 38,
            },
            new Category
            {
                Id = 45,
                Name = "Elektronik Aksesuarlar",
                ParentCategoryId = 38,
            },
            new Category
            {
                Id = 46,
                Name = "FotoTexnika",
                ParentCategoryId = 38,
            },
            new Category
            {
                Id = 47,
                Name = "Konsollar",
                ParentCategoryId = 38,
            },
            new Category
            {
                Id = 48,
                Name = "Oyunlar",
                ParentCategoryId = 38,
            },
            new Category
            {
                Id = 49,
                Name = "Kompüterlər",
                ParentCategoryId = 38,
            },
            new Category
            {
                Id = 50,
                Name = "Tv",
                ParentCategoryId = 38,
            },
            new Category
            {
                Id = 51,
                Name = "Ofis",
                ParentCategoryId = 39,
            },
            new Category
            {
                Id = 52,
                Name = "Evlər",
                ParentCategoryId = 39
            },
            new Category
            {
                Id = 53,
                Name = "Torpaq",
                ParentCategoryId = 39,
            },
            new Category
            {
                Id = 54,
                Name = "Mənzillər",
                ParentCategoryId = 39,
            },
            new Category
            {
                Id = 55,
                Name = "Mebellər",
                ParentCategoryId = 40,
            },
            new Category
            {
                Id = 56,
                Name = "Təmir tikinti",
                ParentCategoryId = 40,
            },
            new Category
            {
                Id = 57,
                Name = "Qab-qacaqlar",
                ParentCategoryId = 40
            },
            new Category
            {
                Id = 58,
                Name = "Xalçalar",
                ParentCategoryId = 40,
            },
            new Category
            {
                Id = 59,
                Name = "Geyimlər",
                ParentCategoryId = 41
            },
            new Category
            {
                Id = 60,
                Name = "Ayaqqabılar",
                ParentCategoryId = 41
            },
            new Category
            {
                Id = 61,
                Name = "Aksesuarlar",
                ParentCategoryId = 41
            },
            new Category
            {
                Id = 62,
                Name = "Saat və zinət əşyaları",
                ParentCategoryId = 41
            },
            new Category
            {
                Id = 63,
                Name = "Avtomobillər",
                ParentCategoryId = 42
            },
            new Category
            {
                Id = 64,
                Name = "Motosikletlər",
                ParentCategoryId = 42
            },
            new Category
            {
                Id = 65,
                Name = "Avtobuslar",
                ParentCategoryId = 42
            },
            new Category
            {
                Id = 66,
                Name = "Yük maşınları",
                ParentCategoryId = 42
            }
            );

            modelBuilder.Entity<IdentityRole>().HasData(

                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "2", Name = "User", NormalizedName = "USER" }

                );
            var roleId = _configuration["SeedData:RoleId"];
            var userId = _configuration["SeedData:UserId"];

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { RoleId = roleId, UserId = userId }

                );


       


        }
    }
}
