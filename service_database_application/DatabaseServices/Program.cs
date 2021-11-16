using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace DatabaseServices
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webHostBuilder =>
                {
                    webHostBuilder
                    .UseKestrel(
                        options =>
                        {
                            options.Limits.MaxRequestBodySize = int.MaxValue;
                            options.Limits.KeepAliveTimeout = System.TimeSpan.FromMinutes(10);
                        }
                    )
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseIIS()
                    .UseIISIntegration()
                    .UseStartup<Startup>();
                });
    }
}
