using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using AuthorizationService.DAL;
using VaultSharp;
using VaultSharp.V1.AuthMethods.AppRole;
using VaultSharp.V1.AuthMethods;
using VaultSharp.V1.Commons;
using VaultSharp.V1.AuthMethods.Token;
using System.Net.Http;

namespace AuthorizationService
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

        private async static Task<string> GetPassword()
		{
            var vaultUrl = "http://vault:8200";
            // Initialize one of the several auth methods.
            IAuthMethodInfo authMethod = new TokenAuthMethodInfo("s.R2gFHDiup5wCeHHksfc2zKUN");

            // Initialize settings. You can also set proxies, custom delegates etc. here.
            var vaultClientSettings = new VaultClientSettings(vaultUrl, authMethod);

            IVaultClient vaultClient = new VaultClient(vaultClientSettings);

            // Use client to read a key-value secret.
            Secret<SecretData> kv2Secret = await vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync("internal/data/database/config");

            object pass;
            kv2Secret.Data.Data.TryGetValue("password", out pass);
            return (string)pass;
        }

        private static async Task<HttpResponseMessage> RequestAsync()
        {
            HttpClient client = new HttpClient();
            return await client.GetAsync($"http://vault/v1/auth/kubernetes/login");
        }
    }

}
