// Copyright © Portable EHR inc, 2021
// https://portableehr.com/

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace PortableEHRNetFeedDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}