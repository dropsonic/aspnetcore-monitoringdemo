namespace MonitoringDemo
{
	public class ServicesLocation
	{
		public string ElasticsearchUri { get; init; }
		public string LogstashUri { get; init; }
		public string KibanaUri { get; init; }
		public string PrometheusUri { get; init; }
		public string GrafanaUri { get; init; }
		public string CAdvisorUri { get; init; }
		public string NodeExporterUri { get; init; }
		public string AlertManagerUri { get; init; }
	}
}
