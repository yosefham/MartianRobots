using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace MartianRobots
{
    public class Program
    {
        internal static Container Container { get; } = new();

        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args)
                .Build()
                .UseSimpleInjector(Container, x =>
                {
                    Container.Verify();
                });

            using (AsyncScopedLifestyle.BeginScope(Container))
            {
                host.Run();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
