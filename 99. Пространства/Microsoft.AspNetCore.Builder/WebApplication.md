> Microsoft.AspNetCore.dll  
> Microsoft.AspNetCore.Builder

#### Состояние
```csharp
public IServiceProvider Services => _host.Services;
public IConfiguration Configuration => _host.Services.GetRequiredService<IConfiguration>();
public IWebHostEnvironment Environment => _host.Services.GetRequiredService<IWebHostEnvironment>();
public IHostApplicationLifetime Lifetime => _host.Services.GetRequiredService<IHostApplicationLifetime>();
public ILogger Logger { get; }
public ICollection<string> Urls => ServerFeatures.GetRequiredFeature<IServerAddressesFeature>().Addresses;
```
```csharp
private readonly IHost _host;
private readonly List<EndpointDataSource> _dataSources = new List<EndpointDataSource>();
private bool IsRunning

internal const string GlobalEndpointRouteBuilderKey = "__GlobalEndpointRouteBuilder";
internal IFeatureCollection ServerFeatures => _host.Services.GetRequiredService<IServer>().Features;
internal IDictionary<string, object?> Properties => ApplicationBuilder.Properties;
internal ICollection<EndpointDataSource> DataSources => _dataSources;
internal ApplicationBuilder ApplicationBuilder { get; }

ICollection<EndpointDataSource> IEndpointRouteBuilder.DataSources => DataSources;
IServiceProvider IEndpointRouteBuilder.ServiceProvider => Services;

IServiceProvider IApplicationBuilder.ApplicationServices
IFeatureCollection IApplicationBuilder.ServerFeatures => ServerFeatures;
IDictionary<string, object?> IApplicationBuilder.Properties => Properties;
```


### Методы
#### Создает WebApplicationBuilder
```csharp
public static WebApplicationBuilder CreateBuilder()
{
  return new WebApplicationBuilder(new WebApplicationOptions());
}

public IApplicationBuilder Use(Func<RequestDelegate, RequestDelegate> middleware)
{
  ApplicationBuilder.Use(middleware);
  return this;
}

public void Run([StringSyntax(StringSyntaxAttribute.Uri)] string? url = null)
{
    Listen(url);
    HostingAbstractionsHostExtensions.Run(this);
}

private void Listen(string? url)
{
    if (url is null)
    {
        return;
    }

    var addresses = ServerFeatures.Get<IServerAddressesFeature>()?.Addresses;
    if (addresses is null)
    {
        throw new InvalidOperationException($"Changing the URL is not supported because no valid {nameof(IServerAddressesFeature)} was found.");
    }
    if (addresses.IsReadOnly)
    {
        throw new InvalidOperationException($"Changing the URL is not supported because {nameof(IServerAddressesFeature.Addresses)} {nameof(ICollection<string>.IsReadOnly)}.");
    }

    addresses.Clear();
    addresses.Add(url);
}
```
