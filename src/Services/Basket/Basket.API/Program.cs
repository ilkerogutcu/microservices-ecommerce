using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Basket.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var logFilePath = $"{projectDirectory + "/logs"}/{DateTime.Now:yyyy-MM-dd}.txt";
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(logFilePath,
                    retainedFileCountLimit: 1,
                    fileSizeLimitBytes: 5000000,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}")
                .CreateLogger();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}