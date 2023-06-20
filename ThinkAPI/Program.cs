using Data.Interfaces;
using Data.Models;
using Data.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Services.Interfaces;
using Services.Services;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Agrega servicios al contenedor.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    #region Agrega el filtro de autenticación de Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
    });
    #endregion Agrega el filtro de autenticación de Swagger
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ThinkUs API",
        Version = "v1",
        Description = "API documentation for ThinkUs",
        Contact = new OpenApiContact
        {
            Name = "Alejandro Castellanos",
            Url = new Uri("https://www.linkedin.com/in/carlos-alejandro-castellanos-calero-9a717a92")
        },
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        },
        TermsOfService = new Uri("https://www.example.com/terms")
    });
    c.OperationFilter<AddAuthorizationHeaderOperationFilter>();
});


builder.Services.Configure<InsuranceStoreDatabaseSettings>(
    builder.Configuration.GetSection("InsurancestoreDatabase")
);

builder.Services.AddScoped<IInsuranceService, InsuranceService>();
builder.Services.AddScoped<IInsuranceRepository, InsuranceRepository>();

// Adding Authentication
#region JWT
builder.Services.AddAuthentication(options =>
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
        ValidIssuer = builder.Configuration["Jwt:ValidIssuer"],
        ValidAudience = builder.Configuration["Jwt:ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]))
    };
});
#endregion

var app = builder.Build();

// Configura la pipeline de solicitudes HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Think Us V1");
    });
}



app.UseHttpsRedirection();
app.UseRouting();

// Agrega la autenticación
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
