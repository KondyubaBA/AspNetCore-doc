> Microsoft.Extensions.Hosting.dll
> Microsoft.Extensions.Hosting.HostBuilder

#### Методы
```csharp
internal static void PopulateServiceCollection(IServiceCollection services, HostBuilderContext hostBuilderContext, HostingEnvironment hostingEnvironment, PhysicalFileProvider defaultFileProvider, IConfiguration appConfiguration, Func<IServiceProvider> serviceProviderGetter)
{
	services.AddSingleton((IHostingEnvironment)hostingEnvironment);
	services.AddSingleton((IHostEnvironment)hostingEnvironment);
	services.AddSingleton(hostBuilderContext);
	services.AddSingleton((IServiceProvider _) => appConfiguration);
	services.AddSingleton((IServiceProvider s) => (IApplicationLifetime)s.GetRequiredService<IHostApplicationLifetime>());
	services.AddSingleton<IHostApplicationLifetime, ApplicationLifetime>();
	AddLifetime(services);
	services.AddSingleton((Func<IServiceProvider, IHost>)delegate
	{
		IServiceProvider serviceProvider = serviceProviderGetter();
		return new Microsoft.Extensions.Hosting.Internal.Host(serviceProvider, hostingEnvironment, defaultFileProvider, serviceProvider.GetRequiredService<IHostApplicationLifetime>(), serviceProvider.GetRequiredService<ILogger<Microsoft.Extensions.Hosting.Internal.Host>>(), serviceProvider.GetRequiredService<IHostLifetime>(), serviceProvider.GetRequiredService<IOptions<HostOptions>>());
	});
	services.AddOptions().Configure(delegate(HostOptions options)
	{
		options.Initialize(hostBuilderContext.Configuration);
	});
	services.AddLogging();
	services.AddMetrics();
}
```
Добавялются сервисы  
> Service Type: Microsoft.Extensions.Hosting.IHostingEnvironment, Lifetime: Singleton  
> Service Type: Microsoft.Extensions.Hosting.IHostEnvironment, Lifetime: Singleton  
> Service Type: Microsoft.Extensions.Hosting.HostBuilderContext, Lifetime: Singleton  
> Service Type: Microsoft.Extensions.Configuration.IConfiguration, Lifetime: Singleton  
> Service Type: Microsoft.Extensions.Hosting.IApplicationLifetime, Lifetime: Singleton  
> Service Type: Microsoft.Extensions.Hosting.IHostApplicationLifetime, Lifetime: Singleton  
> Service Type: Microsoft.Extensions.Hosting.IHostLifetime, Lifetime: Singleton  
> Service Type: Microsoft.Extensions.Hosting.IHost, Lifetime: Singleton    
> Service Type: Microsoft.Extensions.Options.IOptions1[TOptions], Lifetime: Singleton  
> Service Type: Microsoft.Extensions.Options.IOptionsSnapshot1[TOptions], Lifetime: Scoped  
> Service Type: Microsoft.Extensions.Options.IOptionsMonitor1[TOptions], Lifetime: Singleton  
> Service Type: Microsoft.Extensions.Options.IOptionsFactory1[TOptions], Lifetime: Transient  
> Service Type: Microsoft.Extensions.Options.IOptionsMonitorCache1[TOptions], Lifetime: Singleton  
> Service Type: Microsoft.Extensions.Options.IConfigureOptions1[Microsoft.Extensions.Hosting.HostOptions], Lifetime: Singleton  
> Service Type: Microsoft.Extensions.Logging.ILoggerFactory, Lifetime: Singleton  
> Service Type: Microsoft.Extensions.Logging.ILogger1[TCategoryName], Lifetime: Singleton  
> Service Type: Microsoft.Extensions.Options.IConfigureOptions1[Microsoft.Extensions.Logging.LoggerFilterOptions], Lifetime: Singleton
> Service Type: System.Diagnostics.Metrics.IMeterFactory, Lifetime: Singleton  
> Service Type: Microsoft.Extensions.Diagnostics.Metrics.MetricsSubscriptionManager, Lifetime: Singleton
> Service Type: Microsoft.Extensions.Options.IStartupValidator, Lifetime: Transient  
