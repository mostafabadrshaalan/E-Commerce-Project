using Microsoft.OpenApi.Models;

namespace API.Extensions
{
    public static class SwaggerDocumentationExtenions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo(){ Title = "Api",Version = "v1"});

                var securityScheme = new OpenApiSecurityScheme()
                {
                    Description="Jwt Auth Bearer Scheme",
                    Name="Authorization",
                    Type=SecuritySchemeType.ApiKey,
                    In=ParameterLocation.Header,
                    Scheme="Bearer",
                    Reference=new OpenApiReference()
                    {
                        Id="Bearer",
                        Type=ReferenceType.SecurityScheme
                    }
                };

                c.AddSecurityDefinition("Bearer", securityScheme);
                var securityRequirement = new OpenApiSecurityRequirement()
                {
                    {securityScheme,new string[]{} }
                };

                c.AddSecurityRequirement(securityRequirement);
            });
            return services;
        }
    }
}
