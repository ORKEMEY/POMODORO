using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using AuthorizationService.DAL;

namespace Messages
{
    public static class ServiceModule
    {
        public static string GetConnectionStringFromConfig(string connectionName = "DefaultConnection")
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
            return config.GetConnectionString(connectionName);
        }

        public static DbContextOptions<AuthorizationContext> GetDbContextOptions(string connectionName = "DefaultConnection")
        {

            var optionsBuilder = new DbContextOptionsBuilder<AuthorizationContext>();

            var connectionString = GetConnectionStringFromConfig(connectionName);

            return optionsBuilder.UseSqlServer(connectionString).Options;
        }
    }
}
