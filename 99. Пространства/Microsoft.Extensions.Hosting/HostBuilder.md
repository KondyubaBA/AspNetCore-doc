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
