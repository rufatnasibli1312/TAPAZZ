
using BLL.Mapper;
using BLL.Persistence.Service.Abstract;
using BLL.Persistence.Service.Abstraction;
using BLL.Persistence.Service.Concrete;
using BLL.Persistence.Service.Implementation;
using BLL.ServiceExtensions;
using DAL.Data;
using DAL.Filter.ActionFilter;
using DAL.Persistence.Repository.Abstract;
using DAL.Persistence.Repository.Abstraction;
using DAL.Persistence.Repository.Concrete;
using DAL.Persistence.Repository.Implementation;
using DAL.ServiceExtensions;
using DTO.AccountDto_s;
using Entity.Entities;
using FluentValidation;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

using Validation.ServiceExtensions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDALServices(builder.Configuration);
builder.Services.AddBLLServices();
builder.Services.AddValidatorServices();



builder.Services.AddIdentity<User, IdentityRole>(op =>
{

    op.Password = new PasswordOptions
    {
        RequireDigit = false,
        RequiredLength = 3,
        RequireLowercase = true,
        RequireUppercase = false,
        RequireNonAlphanumeric = false
    };

})
          .AddEntityFrameworkStores<MyDbContext>()
.AddDefaultTokenProviders();



var Configuration = builder.Configuration;

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
    configuration.Filter.ByExcluding(logEvent =>
    {

        return false;
    });
});




builder.Services.AddAuthentication(options =>
{

    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

})
            .AddJwtBearer(options =>
            {
                var key = Encoding.UTF8.GetBytes(builder.Configuration["JWTToken:Key"]);
                options.SaveToken = true;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["JWTToken:Issuer"],
                    ValidAudience = builder.Configuration["JWTToken:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("J-HEADER-TOKEN-EXPIRED", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });



builder.Services.AddSwaggerGen(swagger =>
{
    swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "TAPAZ", Version = "v1" });
    swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                Enter 'Bearer' [space] and then your token in the text input below.
                \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement()
{
  {
    new OpenApiSecurityScheme
    {
      Reference = new OpenApiReference
        {
          Type = ReferenceType.SecurityScheme,
          Id = "Bearer"
        },
        Scheme = "oauth2",
        Name = "Bearer",
        In = ParameterLocation.Header,

      },
      new List<string>()
    }
  });
});

//////////////////////////////////
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/TAPAZ
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//AddedIdentity

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();
app.MapControllers();

app.Run();
