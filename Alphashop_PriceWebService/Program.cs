using Alphashop_PriceWebService.Extensions;
using Alphashop_PriceWebService.Mapper;
using Alphashop_PriceWebService.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
builder.Services.AddCors();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add Services
builder.Services.AddServices();

/* JWT Configuration */

//Gestione del JwtTokenOptions dall'appsettings.json
var jwtTokenOptionsConfig = builder.Configuration.GetRequiredSection(nameof(JwtTokenOptions));
builder.Services.Configure<JwtTokenOptions>(jwtTokenOptionsConfig);

//Lettura della Key Signature
var jwtTokenOptions = jwtTokenOptionsConfig.Get<JwtTokenOptions>();
var key = Encoding.ASCII.GetBytes(jwtTokenOptions.Secret);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

//Add Mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAutoMapper(typeof(Mapper));

//Configurazione Swagger con JWT Token e Basic Auth
builder.Services.ConfigureSwaggerAuthentication();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
