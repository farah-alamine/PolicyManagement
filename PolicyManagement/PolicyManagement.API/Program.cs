using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PolicyManagement.API.Extensions;
using PolicyManagement.Core.Interfaces.Services;
using PolicyManagement.Core.Settings;
using PolicyManagement.Core.Validators.Policies;
using PolicyManagement.Infrastructure;
using PolicyManagement.Infrastructure.Persistence;
using PolicyManagement.Infrastructure.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token."
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
            Array.Empty<string>()
        }
    });
});
builder.Services.AddDbContext<AuthenticationDbContext>(
    options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString(
                "AuthenticationConnection")));
builder.Services.AddDbContext<PolicyManagementDbContext>(
    (serviceProvider, options) =>
    {
        var currentTenantService =
            serviceProvider.GetRequiredService<ICurrentTenantService>();

        var configuration =
            serviceProvider.GetRequiredService<IConfiguration>();

        var connectionString =
            currentTenantService.IsResolved
                ? currentTenantService.ConnectionString
                : configuration.GetConnectionString(
                    "DefaultTenantConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "No tenant database connection string is configured.");
        }

        options.UseSqlServer(connectionString);
    });
builder.Services.AddScoped<
    ICurrentTenantService,
    CurrentTenantService>();
builder.Services.AddValidatorsFromAssemblyContaining<
    CreatePolicyRequestValidator>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt"));
var jwtSettings = builder.Configuration
    .GetSection("Jwt")
    .Get<JwtSettings>()!;

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.Key)
                    ),

                ClockSkew = TimeSpan.Zero
            };
    });
builder.Services.AddInfrastructureServices();

var app = builder.Build();

await app.Services.SeedDatabaseAsync();

app.UseHttpsRedirection();

app.UseGlobalExceptionHandling();

app.UseCors("AllowAngular");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseTenantResolution();

app.UseAuthorization();

app.MapControllers();

app.Run();
