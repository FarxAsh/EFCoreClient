using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EFCoreClient.Services
{
    public class AppConfigService
    {
        public IConfiguration Configuration { get;}

        public AppConfigService()
        {
            Configuration = BuildAppConfig();
        }
        
        private IConfiguration BuildAppConfig()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            return builder.Build();
        }
    }
}
