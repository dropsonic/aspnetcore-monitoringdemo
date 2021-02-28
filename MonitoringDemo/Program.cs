using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using Serilog;
using Serilog.Exceptions;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Http.BatchFormatters;

namespace MonitoringDemo
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Serilog.Debugging.SelfLog.Enable(Console.Error);

			Log.Logger = new LoggerConfiguration()
				.Enrich.FromLogContext()
				.WriteTo.Console()
				.CreateBootstrapLogger();

			try
			{
				CreateHostBuilder(args).Build().Run();
			}
			catch (Exception ex)
			{
				Log.Fatal(ex, "An unhandled exception occurred during bootstrapping");
			}
			finally
			{
				Log.CloseAndFlush();
			}
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.UseSerilog((hostContext, serviceProvider, loggerConfiguration) =>
				{
					loggerConfiguration
						.ReadFrom.Configuration(hostContext.Configuration)
						.ReadFrom.Services(serviceProvider)
						.Enrich.FromLogContext()
						.Enrich.WithExceptionDetails()
						.Enrich.WithCorrelationId()
						.Enrich.WithClientIp()
						.Enrich.WithClientAgent();
				})
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}
}
