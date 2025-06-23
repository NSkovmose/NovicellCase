using API.Services;
using API.Services.Interfaces;
using EntityFramework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PicoPlanner.Service.Middleware;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));

builder.Services.AddEndpointsApiExplorer();

var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

builder.Services.AddSwaggerGen(c =>
{
    c.IncludeXmlComments(xmlPath);
    OpenApiSecurityScheme securityScheme = new()
    {
        In = ParameterLocation.Header,
        Name = "XApiKey",
        Type = SecuritySchemeType.ApiKey,
    };
    c.AddSecurityDefinition("ApiKeyMiddleware", securityScheme);

    var apiKey = new OpenApiSecurityScheme()
    {
        Reference = new OpenApiReference()
        {
            Type = ReferenceType.SecurityScheme,
            Id = "ApiKeyMiddleware"
        },
    };
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { apiKey, new List<string>() }
    });
});

builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

app.UseMiddleware<ApiKeyMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();