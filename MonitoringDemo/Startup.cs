using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Prometheus;
using Serilog;

namespace MonitoringDemo
{
	public class Startup
	{
		public class HealthCheckTags
		{
			public const string Application = "Application";
			public const string Infrastructure = "Infrastructure";
			public const string ELK = "ELK";
			public const string Monitoring = "Monitoring";
			public const string Memory = "Memory";
		}

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
				.AddHealthChecksUI(setup =>
				{
					setup.AddHealthCheckEndpoint("Application", "http://localhost/health");
					setup.AddHealthCheckEndpoint("Infrastructure", "http://localhost/health-infrastructure");
				})
				.AddInMemoryStorage();

			var servicesLocation = Configuration.GetSection("Services").Get<ServicesLocation>();

			services.AddHealthChecks()
				.AddProcessAllocatedMemoryHealthCheck(1024, "Allocated Memory", tags: new [] { HealthCheckTags.Application, HealthCheckTags.Memory })
				.AddElasticsearch(servicesLocation.ElasticsearchUri, "Elasticsearch", tags: new [] { HealthCheckTags.Infrastructure, HealthCheckTags.ELK })
				.AddUrlGroup(new Uri(servicesLocation.LogstashUri), "Logstash", tags: new[] { HealthCheckTags.Infrastructure, HealthCheckTags.ELK })
				.AddUrlGroup(new Uri(servicesLocation.KibanaUri), "Kibana", tags: new[] { HealthCheckTags.Infrastructure, HealthCheckTags.ELK })
				.AddUrlGroup(new Uri(servicesLocation.PrometheusUri), "Prometheus", tags: new[] { HealthCheckTags.Infrastructure, HealthCheckTags.Monitoring })
				.AddUrlGroup(new Uri(servicesLocation.GrafanaUri), "Grafana", tags: new[] { HealthCheckTags.Infrastructure, HealthCheckTags.Monitoring })
				.AddUrlGroup(new Uri(servicesLocation.CAdvisorUri), "cAdvisor", tags: new[] { HealthCheckTags.Infrastructure, HealthCheckTags.Monitoring })
				.AddUrlGroup(new Uri(servicesLocation.NodeExporterUri), "Node Exporter", tags: new[] { HealthCheckTags.Infrastructure, HealthCheckTags.Monitoring });

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

			app.UseHttpMetrics();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();

				endpoints.MapHealthChecks("/health", new HealthCheckOptions()
				{
					ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
					Predicate = r => r.Tags.Contains(HealthCheckTags.Application)
				});
				endpoints.MapHealthChecks("/health-infrastructure", new HealthCheckOptions()
				{
					ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
					Predicate = r => r.Tags.Contains(HealthCheckTags.Infrastructure)
				});
				endpoints.MapHealthChecksUI(options =>
				{
					options.UIPath = "/health-ui";
				});

				endpoints.MapMetrics(); // /metrics (Prometheus)
			});
		}
	}
}
