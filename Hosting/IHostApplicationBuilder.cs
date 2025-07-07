public interface IHostApplicationBuilder
{
	IDictionary<object, object> Properties { get; }

	IConfigurationManager Configuration { get; }

	IHostEnvironment Environment { get; }

	ILoggingBuilder Logging { get; }

	IMetricsBuilder Metrics { get; }

	IServiceCollection Services { get; }

	void ConfigureContainer<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory, Action<TContainerBuilder>? configure = null) where TContainerBuilder : notnull;
}
