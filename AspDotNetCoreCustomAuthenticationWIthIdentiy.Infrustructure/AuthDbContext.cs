using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AspDotNetCoreCustomAuthenticationWIthIdentiy.Infrustructure
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        IConfigurationRoot configuration = new ConfigurationBuilder()
        //           .SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
        //        var connectionString = configuration.GetConnectionString("DefaultConnection");
        //        optionsBuilder.UseSqlServer(connectionString);

        //        //optionsBuilder.UseLazyLoadingProxies();
        //    }
        //}
    }
}
