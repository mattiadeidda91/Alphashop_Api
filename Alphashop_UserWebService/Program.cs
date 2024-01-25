using Alphashop_UserWebService.Mapper;
using Alphashop_UserWebService.Options;
using Alphashop_UserWebService.Repository;
using Alphashop_UserWebService.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add Mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAutoMapper(typeof(Mapper));

//Add Services
builder.Services.AddDbContext<AlphaShopDbContext>();
builder.Services.AddScoped<IUserService, UserService>();

//JWT Token Settings Options
var jwtTokenOptions = builder.Configuration.GetRequiredSection(nameof(JwtTokenOptions));
builder.Services.Configure<JwtTokenOptions>(jwtTokenOptions);

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

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

app.UseAuthorization();

app.MapControllers();

app.Run();
