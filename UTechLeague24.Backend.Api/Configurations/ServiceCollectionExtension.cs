using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using UTechLeague24.Backend.Auth.Interfaces;
using UTechLeague24.Backend.Auth.Models;
using UTechLeague24.Backend.Auth.Profiles;
using UTechLeague24.Backend.Auth.Services;
using UTechLeague24.Backend.Domain.Db;
using UTechLeague24.Backend.Domain.Interfaces;
using UTechLeague24.Backend.Domain.Repositories;

namespace UTechLeague24.Backend.Api.Configurations;

public static class ServiceCollectionExtension
{
    public static void AddDomainServices(this IServiceCollection services, IConfiguration configuration)
    {
        var dbConnection = configuration.GetConnectionString("MongoDbConnection") ??
                           throw new ArgumentException("MongoDbConnection not found");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMongoDB(dbConnection, "utechleague24"));

        services.AddScoped<IUserRepository, UserRepository>();
    }

    public static void AddAuthenticationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddAutoMapper(Assembly.GetAssembly(typeof(AuthProfile)));

        var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>() ??
                          throw new ArgumentException(nameof(JwtSettings));

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                };
            });
    }

    public static void AddSwaggerGenWithAuthentication(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });
    }
}