using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using AspDotNetCoreCustomAuthenticationWIthIdentiy.Domain.AuthModel;
using AspDotNetCoreCustomAuthenticationWIthIdentiy.Infrustructure;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AspDotNetCoreCustomAuthenticationWIthIdentiy
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<AuthDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //services.Configure<IdentityOptions>(options =>
            //{
            //    options.Password.RequiredLength = 4;
            //    options.Password.RequireNonAlphanumeric = false;
            //    options.Password.RequiredUniqueChars = 0;
            //    options.Password.RequireUppercase = false;
            //    options.Password.RequireLowercase = false;
            //});


            //services.AddDbContext<AuthDbContext>();

            //services.AddDefaultIdentity<IdentityUser, AuthDbContext>()
            //    .AddEntityFrameworkStores<AuthDbContext>();

            //services.AddIdentityServer()
            //        .AddApiAuthorization<IdentityUser, AuthDbContext>();

            //services.AddIdentity<IdentityUser, AuthDbContext>()
            //    .AddEntityFrameworkStores<AuthDbContext>();
            var identity = services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();

            identity.AddClaimsPrincipalFactory<ClaimsPrincipalFactory>();

            services.AddAuthentication()
                    .AddIdentityServerJwt();

            services.AddAuthorization(options =>
            {

                options.AddPolicy("admin",
                    authBuilder =>
                    {
                        authBuilder.RequireRole("admin");
                    });

                options.AddPolicy("CreateRole", policy =>
                {
                    policy.RequireClaim("Create Role");
                });

            });

            services.AddIdentityServer()
                    .AddInMemoryCaching()
                    .AddClientStore<InMemoryClientStore>()
                    .AddResourceStore<InMemoryResourcesStore>();

            //CROS
            services.AddCors(options =>
            {
                options.AddPolicy("foo",
                builder =>
                {
                    // Not a permanent solution, but just trying to isolate the problem
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseIdentityServer();

            //CORS
            app.UseHttpsRedirection();

            // Use the CORS policy
            app.UseCors("foo");

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
