using System.Reflection;
using System.Text;
using AspNetCoreRateLimit;
using AuthenticatedWebApp.Api.Settings;
using AuthenticatedWebApp.Auth.Interfaces;
using AuthenticatedWebApp.Auth.Models;
using AuthenticatedWebApp.Auth.Profiles;
using AuthenticatedWebApp.Auth.Services;
using AuthenticatedWebApp.Auth.Settings;
using AuthenticatedWebApp.Auth.Validators;
using AuthenticatedWebApp.Domain.Db;
using AuthenticatedWebApp.Domain.Interfaces;
using AuthenticatedWebApp.Domain.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace AuthenticatedWebApp.Api.Configurations;

public static class ServiceCollectionExtension
{
    public static void AddDomainServices(this IServiceCollection services, IConfiguration configuration)
    {
        var dbConnection = configuration.GetConnectionString("MongoDbConnection") ??
                           throw new ArgumentException("MongoDbConnection not found");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMongoDB(dbConnection, "app-database"));

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
        services.AddFluentValidationRulesToSwagger(options =>
        {
            options.SetNotNullableIfMinLengthGreaterThenZero = true;
            options.UseAllOfForMultipleRules = true;
        });

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Authenticated Web App API", Version = "v1" });

            // Add documentation from all these assemblies
            // Must generate xml output for each one in the list
            foreach (var assembly in new[]
                     {
                         Assembly.GetExecutingAssembly(),
                         Assembly.GetAssembly(typeof(AuthenticationResult))
                     })
            {
                ArgumentNullException.ThrowIfNull(assembly);
                var xmlFile = $"{assembly.GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            }

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