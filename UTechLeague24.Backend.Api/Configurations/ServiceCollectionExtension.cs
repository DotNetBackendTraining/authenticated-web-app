using System.Reflection;
using System.Text;
using AspNetCoreRateLimit;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using UTechLeague24.Backend.Api.Settings;
using UTechLeague24.Backend.Auth.Interfaces;
using UTechLeague24.Backend.Auth.Profiles;
using UTechLeague24.Backend.Auth.Services;
using UTechLeague24.Backend.Auth.Settings;
using UTechLeague24.Backend.Auth.Validators;
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
        services.Configure<ClientUserSettings>(configuration.GetSection("ClientUserSettings"));

        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IUserSeeder, UserSeeder>();

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
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "UTechLeague24 API", Version = "v1" });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);

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

    public static void AddRateLimitingServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        services.AddInMemoryRateLimiting();
    }

    public static void AddCorsServices(this IServiceCollection services, IConfiguration configuration)
    {
        var corsSettings = configuration.GetSection("CorsSettings").Get<CorsSettings>() ??
                           throw new ArgumentException("CorsSettings");

        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                builder.WithOrigins(corsSettings.AllowedOrigins.ToArray())
                    .WithMethods(corsSettings.AllowedMethods.ToArray())
                    .WithHeaders(corsSettings.AllowedHeaders.ToArray());

                if (corsSettings.AllowCredentials)
                {
                    builder.AllowCredentials();
                }
            });
        });
    }

    public static void AddFluentValidationServices(this IServiceCollection services)
    {
        services
            .AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters()
            .AddValidatorsFromAssemblyContaining<LoginRequestValidator>();
    }
}