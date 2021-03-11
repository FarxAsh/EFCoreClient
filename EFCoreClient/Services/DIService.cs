using Microsoft.Extensions.DependencyInjection;
using EFCoreClient.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Proxies;

namespace EFCoreClient.Services
{
    public static class DIService
    {

        static ServiceProvider serviceProvider;
        public static ServiceProvider ServiceProvider
        {
            get
            {
                return serviceProvider;
            }
        }

        public static void RegisterService(IConfiguration configuration)
        {
            var collection = new ServiceCollection();
            collection.AddSingleton<AppConfigService>();
            collection.AddDbContext<BookStoreContext>(options => options.UseSqlServer(configuration.GetConnectionString("MSSQLConnection"))
                                                                        .UseLazyLoadingProxies());
            collection.AddScoped<DiscountService>();
            collection.AddScoped<OrderRepository>();
            collection.AddScoped<UserRepository>();
            collection.AddScoped<BookRepository>();
            collection.AddScoped<DbClientConsoleWindowService>();
            serviceProvider = collection.BuildServiceProvider();
        }
    }
}
