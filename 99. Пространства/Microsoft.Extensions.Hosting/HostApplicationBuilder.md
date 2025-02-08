> Microsoft.Extensions.Hosting.dll  
> Microsoft.Extensions.Hosting

#### Состояние
```csharp

```
#### .ctor
```csharp
public HostApplicationBuilder(HostApplicationBuilderSettings? settings)
{
	HostApplicationBuilder hostApplicationBuilder = this;
	if (settings == null)
	{
		settings = new HostApplicationBuilderSettings();
	}
	Configuration = settings.Configuration ?? new ConfigurationManager();
	if (!settings.DisableDefaults)
	{
		if (settings.ContentRootPath == null && Configuration[HostDefaults.ContentRootKey] == null)
		{
			HostingHostBuilderExtensions.SetDefaultContentRoot(Configuration);
		}
		Configuration.AddEnvironmentVariables("DOTNET_");
	}
	Initialize(settings, out _hostBuilderContext, out _environment, out _logging, out _metrics);
	ServiceProviderOptions serviceProviderOptions = null;
	if (!settings.DisableDefaults)
	{
		HostingHostBuilderExtensions.ApplyDefaultAppConfiguration(_hostBuilderContext, Configuration, settings.Args);
		HostingHostBuilderExtensions.AddDefaultServices(_hostBuilderContext, Services);
		serviceProviderOptions = HostingHostBuilderExtensions.CreateDefaultServiceProviderOptions(_hostBuilderContext);
	}
	_createServiceProvider = delegate
	{
		hostApplicationBuilder._configureContainer(hostApplicationBuilder.Services);
		return (serviceProviderOptions != null) ? hostApplicationBuilder.Services.BuildServiceProvider(serviceProviderOptions) : hostApplicationBuilder.Services.BuildServiceProvider();
	};
}
```

#### Методы
```csharp
private void Initialize(HostApplicationBuilderSettings settings, out HostBuilderContext hostBuilderContext, out IHostEnvironment environment, out LoggingBuilder logging, out MetricsBuilder metrics)
{
	HostingHostBuilderExtensions.AddCommandLineConfig(Configuration, settings.Args);
	List<KeyValuePair<string, string>> list = null;
	if (settings.ApplicationName != null)
	{
		if (list == null)
		{
			list = new List<KeyValuePair<string, string>>();
		}
		list.Add(new KeyValuePair<string, string>(HostDefaults.ApplicationKey, settings.ApplicationName));
	}
	if (settings.EnvironmentName != null)
	{
		if (list == null)
		{
			list = new List<KeyValuePair<string, string>>();
		}
		list.Add(new KeyValuePair<string, string>(HostDefaults.EnvironmentKey, settings.EnvironmentName));
	}
	if (settings.ContentRootPath != null)
	{
		if (list == null)
		{
			list = new List<KeyValuePair<string, string>>();
		}
		list.Add(new KeyValuePair<string, string>(HostDefaults.ContentRootKey, settings.ContentRootPath));
	}
	if (list != null)
	{
		Configuration.AddInMemoryCollection(list);
	}
	var (hostingEnvironment, physicalFileProvider) = HostBuilder.CreateHostingEnvironment(Configuration);
	Configuration.SetFileProvider(physicalFileProvider);
	hostBuilderContext = new HostBuilderContext(new Dictionary<object, object>())
	{
		HostingEnvironment = hostingEnvironment,
		Configuration = Configuration
	};
	environment = hostingEnvironment;
	HostBuilder.PopulateServiceCollection(Services, hostBuilderContext, hostingEnvironment, physicalFileProvider, Configuration, () => _appServices);
	logging = new LoggingBuilder(Services);
	metrics = new MetricsBuilder(Services);
}
```
