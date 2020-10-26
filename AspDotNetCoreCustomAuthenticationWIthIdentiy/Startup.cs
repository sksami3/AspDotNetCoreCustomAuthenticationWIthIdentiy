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
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
                .AddEntityFrameworkStores<AuthDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication()
                    .AddIdentityServerJwt();

            services.AddIdentityServer()
                    .AddInMemoryCaching()
                    .AddClientStore<InMemoryClientStore>()
                    .AddResourceStore<InMemoryResourcesStore>();

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

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
