using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using EFCoreClient.Services;
using Microsoft.Extensions.DependencyInjection;
using EFCoreClient.Data;
using System.Threading.Tasks;
using EFCoreClient.Data.Entities;
using System.Collections.Generic;

namespace EFCoreClient
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var configuration = new AppConfigService().Configuration;
            DIService.RegisterService(configuration);
            using (var scope = DIService.ServiceProvider.CreateScope())
            {
                try
                {
                    await scope.ServiceProvider.GetRequiredService<DbClientConsoleWindowService>().RunDbAndClientCommunication();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + ex.StackTrace);
                }
            }
        }
    }
}
