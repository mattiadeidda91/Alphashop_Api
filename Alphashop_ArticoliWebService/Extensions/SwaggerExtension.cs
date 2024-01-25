using Microsoft.OpenApi.Models;

namespace Alphashop_ArticoliWebService.Extensions
{
    public static class SwaggerExtension
    {
        public static void ConfigureSwaggerAuthentication(this IServiceCollection services)
        {
            // Configurazione con JWT Token
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement 
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

            //NON SERVE CON L'USO DEL JWT TOKEN
            //Configurazione con Basic Auth
            //services.AddSwaggerGen(c =>
            //{
            //    // Aggiungi l'autenticazione di base a Swagger
            //    c.AddSecurityDefinition("Basic", new OpenApiSecurityScheme
            //    {
            //        Type = SecuritySchemeType.Http,
            //        Scheme = "basic",
            //        In = ParameterLocation.Header,
            //        Description = "Basic Authorization header using the Bearer scheme."
            //    });

            //    c.AddSecurityRequirement(new OpenApiSecurityRequirement
            //    {
            //        {
            //            new OpenApiSecurityScheme
            //            {
            //                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Basic" }
            //            },
            //            new string[] { }
            //        }
            //    });
            //});
        }
    }
}
