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
