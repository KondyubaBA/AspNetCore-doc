> Microsoft.AspNetCore.dll  
> Microsoft.AspNetCore.Builder.WebApplicationBuilder

#### Состояния
```csharp
public IWebHostEnvironment Environment { get; private set; }
public ConfigureHostBuilder Host { get; private set; }
public ConfigureWebHostBuilder WebHost { get; private set; }
public IServiceCollection Services => _hostApplicationBuilder.Services;
public ConfigurationManager Configuration => _hostApplicationBuilder.Configuration;

private readonly HostApplicationBuilder _hostApplicationBuilder;
private readonly ServiceDescriptor _genericWebHostServiceDescriptor;
```

#### ctor
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

#### Методы
```csharp
[MemberNotNull(nameof(Environment), nameof(Host), nameof(WebHost))]
private ServiceDescriptor InitializeHosting(BootstrapHostBuilder bootstrapHostBuilder)
{
    // This applies the config from ConfigureWebHostDefaults
    // Grab the GenericWebHostService ServiceDescriptor so we can append it after any user-added IHostedServices during Build();
    var genericWebHostServiceDescriptor = bootstrapHostBuilder.RunDefaultCallbacks();

    // Grab the WebHostBuilderContext from the property bag to use in the ConfigureWebHostBuilder. Then
    // grab the IWebHostEnvironment from the webHostContext. This also matches the instance in the IServiceCollection.
    var webHostContext = (WebHostBuilderContext)bootstrapHostBuilder.Properties[typeof(WebHostBuilderContext)];
    Environment = webHostContext.HostingEnvironment;

    Host = new ConfigureHostBuilder(bootstrapHostBuilder.Context, Configuration, Services);
    WebHost = new ConfigureWebHostBuilder(webHostContext, Configuration, Services);

    return genericWebHostServiceDescriptor;
}
```
