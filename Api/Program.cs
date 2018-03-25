using App.Metrics;
using App.Metrics.AspNetCore;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using System;
using System.IO;

namespace Api
{
    public class Program
    {
        private const string InfluxDbDatabase = "testdb";
        private const string InfluxDbUri = "http://127.0.0.1:8086";


        public static IWebHost BuildWebHost(string[] args)
        {
            ConfigureLogging();

            var configuration = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true)
                                .Build();

            var influxOptions = new MetricsReportingInfluxDbOptions();
            configuration.GetSection(nameof(MetricsReportingInfluxDbOptions)).Bind(influxOptions);

            return WebHost.CreateDefaultBuilder(args)
                          .ConfigureMetricsWithDefaults(
                              builder =>
                              {
                                  builder.Report.ToInfluxDb(InfluxDbUri, InfluxDbDatabase, TimeSpan.FromSeconds(5));
                              })
                          .UseMetrics()
                          .UseSerilog()
                          .UseStartup<Startup>()
                          .Build();
        }

        public static void Main(string[] args) { BuildWebHost(args).Run(); }

        private static void ConfigureLogging()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Verbose)
                .WriteTo.File(@"E:\AppMetrics\Serilog.txt")
                .CreateLogger();
        }











    }


}
