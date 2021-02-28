using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

namespace MonitoringDemo
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();

			services
				// Using an absolute URI with localhost because of https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks/issues/410
				.AddHealthChecksUI(setup => setup.AddHealthCheckEndpoint("main", "http://localhost/health"))
				.AddInMemoryStorage();

			services.AddHealthChecks()
				.AddProcessAllocatedMemoryHealthCheck(1024, "Allocated Memory");

			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "MonitoringDemo", Version = "v1" });
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			app.UseSerilogRequestLogging();

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MonitoringDemo v1"));
			}

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.MapHealthChecks("/health", new HealthCheckOptions() { ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse });
				endpoints.MapHealthChecksUI(options =>
				{
					options.UIPath = "/health-ui";
				});
			});
		}
	}
}
