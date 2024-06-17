using AspNetCoreRateLimit;
using UTechLeague24.Backend.Api.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDomainServices(builder.Configuration);
builder.Services.AddAuthenticationServices(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddRateLimitingServices(builder.Configuration);
builder.Services.AddCorsServices(builder.Configuration);
builder.Services.AddFluentValidationServices();

// Other services and configurations.
builder.Services.AddOptions();
builder.Services.AddMemoryCache();
builder.Services.AddProblemDetails();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenWithAuthentication();

var app = builder.Build();

// Startup tasks.
using (var scope = app.Services.CreateScope())
{
    await scope.SeedClientUsersAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler();
    app.UseHsts();
}

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();
app.UseIpRateLimiting();

app.MapControllers();

app.Run();