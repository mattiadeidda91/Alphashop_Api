using Alphashop_ArticoliWebService.Extensions;
using Alphashop_ArticoliWebService.Mappers;
using Alphashop_ArticoliWebService.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCors();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add Services
builder.Services.AddServices();

/* START JWT Token Authentication */

//Gestione del JwtTokenOptions dall'appsettings.json
var jwtTokenOptionsConfig = builder.Configuration.GetRequiredSection(nameof(JwtTokenOptions));
builder.Services.Configure<JwtTokenOptions>(jwtTokenOptionsConfig);

//Lettura della Key Signature
var jwtTokenOptions = jwtTokenOptionsConfig.Get<JwtTokenOptions>();
var key = Encoding.ASCII.GetBytes(jwtTokenOptions.Secret);

//Gestione dell'autenticzione tramite JWT Token e la Key di Signature
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata= false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

/* FINE JWT Token Authentication */

//NON SERVE CON L'USO DEL JWT TOKEN
////Add Basic Auth - Commentato per utilizzare il JWT Token
//builder.Services.AddAuthentication("BasicAuthentication")
//    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication",null);



//Come disabilitare i DataAnnotation validation automatici(quindi necessario valutare se ModelState.IsValid)
//Per non ritornare tutto l'oggetto di errore tramite ModelState, ma solo il messaggio presente nelle DataAnnotationValidation
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
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

app.UseCors(options =>
        options.WithOrigins("http://localhost:4200") //url app front-end
            .WithMethods("POST", "PUT", "DELETE", "GET")
            .AllowAnyHeader()
);

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
