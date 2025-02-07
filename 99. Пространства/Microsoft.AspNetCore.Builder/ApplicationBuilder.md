> Microsoft.AspNetCore.Http

[src](https://github.com/dotnet/aspnetcore/blob/main/src/Http/Http/src/Builder/ApplicationBuilder.cs)

```csharp
public class ApplicationBuilder : IApplicationBuilder
{
  private const string ServerFeaturesKey = "server.Features";
  private const string ApplicationServicesKey = "application.Services";
  private const string MiddlewareDescriptionsKey = "__MiddlewareDescriptions";
  private const string RequestUnhandledKey = "__RequestUnhandled";
  private readonly List<Func<RequestDelegate, RequestDelegate>> _components = new List<Func<RequestDelegate, RequestDelegate>>();
  private readonly List<string> _descriptions;
  private readonly IDebugger _debugger;
  private int MiddlewareCount => _components.Count;
  public IServiceProvider ApplicationServices;
  public IFeatureCollection ServerFeatures;
  public IDictionary<string, object?> Properties;

  private T GetProperty<T>(string key);
  private void SetProperty<T>(string key, T value);
  public IApplicationBuilder Use(Func<RequestDelegate, RequestDelegate> middleware)
  {
	_components.Add(middleware);
	_descriptions?.Add(CreateMiddlewareDescription(middleware));
	return this;
  }
  private static string CreateMiddlewareDescription(Func<RequestDelegate, RequestDelegate> middleware);
  public IApplicationBuilder New();

public RequestDelegate Build()
{
	RequestDelegate requestDelegate = delegate(HttpContext context)
	{
		Endpoint endpoint = context.GetEndpoint();
		if (endpoint?.RequestDelegate != null)
		{
			throw new InvalidOperationException($"The request reached the end of the pipeline without executing the endpoint: '{endpoint.DisplayName}'. Please register the EndpointMiddleware using '{"IApplicationBuilder"}.UseEndpoints(...)' if using routing.");
		}
		if (!context.Response.HasStarted)
		{
			context.Response.StatusCode = 404;
		}
		context.Items["__RequestUnhandled"] = true;
		return Task.CompletedTask;
	};
	for (int num = _components.Count - 1; num >= 0; num--)
	{
		requestDelegate = _components[num](requestDelegate);
	}
	return requestDelegate;
}
}
```

1. WebApplication создает WebApplicationBuilder

```csharp
internal WebApplicationBuilder(WebApplicationOptions options, Action<IHostBuilder>? configureDefaults = null)
{
	ConfigurationManager configurationManager = new ConfigurationManager();
	configurationManager.AddEnvironmentVariables("ASPNETCORE_");
	_hostApplicationBuilder = new HostApplicationBuilder(new HostApplicationBuilderSettings
	{
		Args = options.Args,
		ApplicationName = options.ApplicationName,
		EnvironmentName = options.EnvironmentName,
		ContentRootPath = options.ContentRootPath,
		Configuration = configurationManager
	});
	if (options.WebRootPath != null)
	{
		Configuration.AddInMemoryCollection(new KeyValuePair<string, string>[1]
		{
			new KeyValuePair<string, string>(WebHostDefaults.WebRootKey, options.WebRootPath)
		});
	}
	BootstrapHostBuilder bootstrapHostBuilder = new BootstrapHostBuilder(_hostApplicationBuilder);
	configureDefaults?.Invoke(bootstrapHostBuilder);
	bootstrapHostBuilder.ConfigureWebHostDefaults(delegate(IWebHostBuilder webHostBuilder)
	{
		webHostBuilder.Configure(ConfigureApplication);
		InitializeWebHostSettings(webHostBuilder);
	}, delegate(WebHostBuilderOptions options)
	{
		options.SuppressEnvironmentConfiguration = true;
	});
	_genericWebHostServiceDescriptor = InitializeHosting(bootstrapHostBuilder);
}
```
