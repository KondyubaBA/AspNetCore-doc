[DebuggerDisplay("{DebuggerToString(),nq}")]
[DebuggerTypeProxy(typeof(WebApplicationDebugView))]
public sealed class WebApplication : IHost, IApplicationBuilder, IEndpointRouteBuilder, IAsyncDisposable
{
    internal const string GlobalEndpointRouteBuilderKey = "__GlobalEndpointRouteBuilder";

    private readonly IHost _host;
    private readonly List<EndpointDataSource> _dataSources = new();

    internal WebApplication(IHost host)
    {
        _host = host;
        ApplicationBuilder = new ApplicationBuilder(host.Services, ServerFeatures);
        Logger = host.Services.GetRequiredService<ILoggerFactory>().CreateLogger(Environment.ApplicationName ?? nameof(WebApplication));

        Properties[GlobalEndpointRouteBuilderKey] = this;
    }

    public IServiceProvider Services => _host.Services;
    public IConfiguration Configuration => _host.Services.GetRequiredService<IConfiguration>();
    public IWebHostEnvironment Environment => _host.Services.GetRequiredService<IWebHostEnvironment>();
    public IHostApplicationLifetime Lifetime => _host.Services.GetRequiredService<IHostApplicationLifetime>();
    public ILogger Logger { get; }
    public ICollection<string> Urls => ServerFeatures.GetRequiredFeature<IServerAddressesFeature>().Addresses;

    IServiceProvider IApplicationBuilder.ApplicationServices
    {
        get => ApplicationBuilder.ApplicationServices;
        set => ApplicationBuilder.ApplicationServices = value;
    }

    internal IFeatureCollection ServerFeatures => _host.Services.GetRequiredService<IServer>().Features;
    IFeatureCollection IApplicationBuilder.ServerFeatures => ServerFeatures;

    internal IDictionary<string, object?> Properties => ApplicationBuilder.Properties;
    IDictionary<string, object?> IApplicationBuilder.Properties => Properties;

    internal ICollection<EndpointDataSource> DataSources => _dataSources;
    ICollection<EndpointDataSource> IEndpointRouteBuilder.DataSources => DataSources;

    internal ApplicationBuilder ApplicationBuilder { get; }

    IServiceProvider IEndpointRouteBuilder.ServiceProvider => Services;

    public static WebApplication Create(string[]? args = null) =>
        new WebApplicationBuilder(new() { Args = args }).Build();

    public static WebApplicationBuilder CreateBuilder() =>
        new(new());

    public static WebApplicationBuilder CreateSlimBuilder() =>
        new(new(), slim: true);

    public static WebApplicationBuilder CreateBuilder(string[] args) =>
        new(new() { Args = args });

    public static WebApplicationBuilder CreateSlimBuilder(string[] args) =>
        new(new() { Args = args }, slim: true);

    public static WebApplicationBuilder CreateBuilder(WebApplicationOptions options) =>
        new(options);

    public static WebApplicationBuilder CreateSlimBuilder(WebApplicationOptions options) =>
        new(options, slim: true);

    public static WebApplicationBuilder CreateEmptyBuilder(WebApplicationOptions options) =>
        new(options, slim: false, empty: true);

    public Task StartAsync(CancellationToken cancellationToken = default) =>
        _host.StartAsync(cancellationToken);

    public Task StopAsync(CancellationToken cancellationToken = default) =>
        _host.StopAsync(cancellationToken);

    public Task RunAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? url = null)
    {
        Listen(url);
        return HostingAbstractionsHostExtensions.RunAsync(this);
    }

    public void Run([StringSyntax(StringSyntaxAttribute.Uri)] string? url = null)
    {
        Listen(url);
        HostingAbstractionsHostExtensions.Run(this);
    }

    void IDisposable.Dispose() => _host.Dispose();

    public ValueTask DisposeAsync() => ((IAsyncDisposable)_host).DisposeAsync();

    internal RequestDelegate BuildRequestDelegate() => ApplicationBuilder.Build();
    RequestDelegate IApplicationBuilder.Build() => BuildRequestDelegate();

    // REVIEW: Should this be wrapping another type?
    IApplicationBuilder IApplicationBuilder.New()
    {
        var newBuilder = ApplicationBuilder.New();
        // Remove the route builder so branched pipelines have their own routing world
        newBuilder.Properties.Remove(GlobalEndpointRouteBuilderKey);
        return newBuilder;
    }

    public IApplicationBuilder Use(Func<RequestDelegate, RequestDelegate> middleware)
    {
        ApplicationBuilder.Use(middleware);
        return this;
    }

    IApplicationBuilder IEndpointRouteBuilder.CreateApplicationBuilder() => ((IApplicationBuilder)this).New();

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

    private string DebuggerToString()
    {
        return $@"ApplicationName = ""{Environment.ApplicationName}"", IsRunning = {(IsRunning ? "true" : "false")}";
    }

    // Web app is running if the app has been started and hasn't been stopped.
    private bool IsRunning => Lifetime.ApplicationStarted.IsCancellationRequested && !Lifetime.ApplicationStopped.IsCancellationRequested;

    internal sealed class WebApplicationDebugView(WebApplication webApplication)
    {
        private readonly WebApplication _webApplication = webApplication;

        public IServiceProvider Services => _webApplication.Services;
        public IConfiguration Configuration => _webApplication.Configuration;
        public IWebHostEnvironment Environment => _webApplication.Environment;
        public IHostApplicationLifetime Lifetime => _webApplication.Lifetime;
        public ILogger Logger => _webApplication.Logger;
        public string Urls => string.Join(", ", _webApplication.Urls);
        public IReadOnlyList<Endpoint> Endpoints
        {
            get
            {
                var dataSource = _webApplication.Services.GetRequiredService<EndpointDataSource>();
                if (dataSource is CompositeEndpointDataSource compositeEndpointDataSource)
                {
                    // The web app's data sources aren't registered until the routing middleware is. That often happens when the app is run.
                    // We want endpoints to be available in the debug view before the app starts. Test if all the web app's the data sources are registered.
                    if (compositeEndpointDataSource.DataSources.Intersect(_webApplication.DataSources).Count() == _webApplication.DataSources.Count)
                    {
                        // Data sources are centrally registered.
                        return dataSource.Endpoints;
                    }
                    else
                    {
                        // Fallback to just the web app's data sources to support debugging before the web app starts.
                        return new CompositeEndpointDataSource(_webApplication.DataSources).Endpoints;
                    }
                }

                return dataSource.Endpoints;
            }
        }
        public bool IsRunning => _webApplication.IsRunning;
        public IList<string>? Middleware
        {
            get
            {
                if (_webApplication.Properties.TryGetValue("__MiddlewareDescriptions", out var value) &&
                    value is IList<string> descriptions)
                {
                    return descriptions;
                }

                throw new NotSupportedException("Unable to get configured middleware.");
            }
        }
    }
}
