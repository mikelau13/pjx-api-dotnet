using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Pjx_Api
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            IConfigurationRoot configurationRoot = new ConfigurationBuilder().AddEnvironmentVariables("PJX_").Build();
            IConfigurationSection section = configurationRoot.GetSection("SSO"); 
            string authAuthority = section["AUTHORITY"] ?? "http://localhost:5001"; // "http://pjx-sso-identityserver:80";

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.RequireHttpsMetadata = false; // TODO: non-SSL for testing purpose and local development
                    options.Authority = authAuthority;
                    options.MetadataAddress = authAuthority + "/.well-known/openid-configuration";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });

            // allows checking for the presence of the scope in the access token that the client asked for (and got granted)
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "api1", "web_sso"); // web_sso from the pjx-web-react
                });
            });

            services.AddCors(options =>
            {
                // this defines a CORS policy called "default"
                options.AddPolicy("default", policy =>
                {
                    // allow Ajax calls to be made from https://localhost:3000 (pjx-web-react)
                    policy.WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseCors("default"); // CORS middleware to the pipeline

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // setup the policy for all API endpoints in the routing system
                endpoints.MapControllers()
                    .RequireAuthorization("ApiScope");
            });
        }
    }
}
