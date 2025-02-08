> Microsoft.AspNetCore.dll  
> Microsoft.AspNetCore.Builder.WebApplicationBuilder

[src](https://github.com/dotnet/aspnetcore/blob/main/src/DefaultBuilder/src/WebApplicationBuilder.cs)

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
internal WebApplicationBuilder(WebApplicationOptions options, bool slim, Action<IHostBuilder>? configureDefaults = null)
{
    Debug.Assert(slim, "should only be called with slim: true");

    var configuration = new ConfigurationManager();

    configuration.AddEnvironmentVariables(prefix: "ASPNETCORE_");

    // SetDefaultContentRoot needs to be added between 'ASPNETCORE_' and 'DOTNET_' in order to match behavior of the non-slim WebApplicationBuilder.
    SetDefaultContentRoot(options, configuration);

    // Add the default host environment variable configuration source.
    // This won't be added by CreateEmptyApplicationBuilder.
    configuration.AddEnvironmentVariables(prefix: "DOTNET_");

    _hostApplicationBuilder = Microsoft.Extensions.Hosting.Host.CreateEmptyApplicationBuilder(new HostApplicationBuilderSettings
    {
        Args = options.Args,
        ApplicationName = options.ApplicationName,
        EnvironmentName = options.EnvironmentName,
        ContentRootPath = options.ContentRootPath,
        Configuration = configuration,
    });

    // Ensure the same behavior of the non-slim WebApplicationBuilder by adding the default "app" Configuration sources
    ApplyDefaultAppConfigurationSlim(_hostApplicationBuilder.Environment, configuration, options.Args);
    AddDefaultServicesSlim(configuration, _hostApplicationBuilder.Services);

    // configure the ServiceProviderOptions here since CreateEmptyApplicationBuilder won't.
    var serviceProviderFactory = GetServiceProviderFactory(_hostApplicationBuilder);
    _hostApplicationBuilder.ConfigureContainer(serviceProviderFactory);

    // Set WebRootPath if necessary
    if (options.WebRootPath is not null)
    {
        Configuration.AddInMemoryCollection(new[]
        {
            new KeyValuePair<string, string?>(WebHostDefaults.WebRootKey, options.WebRootPath),
        });
    }

    // Run methods to configure web host defaults early to populate services
    var bootstrapHostBuilder = new BootstrapHostBuilder(_hostApplicationBuilder);

    // This is for testing purposes
    configureDefaults?.Invoke(bootstrapHostBuilder);

    bootstrapHostBuilder.ConfigureSlimWebHost(
        webHostBuilder =>
        {
            AspNetCore.WebHost.ConfigureWebDefaultsSlim(webHostBuilder);

            // Runs inline.
            webHostBuilder.Configure(ConfigureApplication);

            InitializeWebHostSettings(webHostBuilder);
        },
        options =>
        {
            // We've already applied "ASPNETCORE_" environment variables to hosting config
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
