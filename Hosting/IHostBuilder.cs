public interface IHostBuilder
{
    IDictionary<object, object> Properties { get; }
    IHostBuilder ConfigureHostConfiguration(Action<IConfigurationBuilder> configureDelegate);
    IHostBuilder ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configureDelegate);
    IHostBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate);
    IHostBuilder UseServiceProviderFactory<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory) where TContainerBuilder : notnull;
    IHostBuilder UseServiceProviderFactory<TContainerBuilder>(Func<HostBuilderContext, IServiceProviderFactory<TContainerBuilder>> factory) where TContainerBuilder : notnull;
    IHostBuilder ConfigureContainer<TContainerBuilder>(Action<HostBuilderContext, TContainerBuilder> configureDelegate);
    IHost Build();
}

public static class HostingHostBuilderExtensions
{
    public static IHostBuilder UseEnvironment(this IHostBuilder hostBuilder, string environment)
    {
        return hostBuilder.ConfigureHostConfiguration(configBuilder =>
        {
            ArgumentNullException.ThrowIfNull(environment);

            configBuilder.AddInMemoryCollection(new[]
            {
                new KeyValuePair<string, string?>(HostDefaults.EnvironmentKey, environment)
            });
        });
    }

    public static IHostBuilder UseContentRoot(this IHostBuilder hostBuilder, string contentRoot)
    {
        return hostBuilder.ConfigureHostConfiguration(configBuilder =>
        {
            ArgumentNullException.ThrowIfNull(contentRoot);

            configBuilder.AddInMemoryCollection(new[]
            {
                new KeyValuePair<string, string?>(HostDefaults.ContentRootKey, contentRoot)
            });
        });
    }

    public static IHostBuilder UseDefaultServiceProvider(this IHostBuilder hostBuilder, Action<ServiceProviderOptions> configure)
        => hostBuilder.UseDefaultServiceProvider((context, options) => configure(options));

    public static IHostBuilder UseDefaultServiceProvider(this IHostBuilder hostBuilder, Action<HostBuilderContext, ServiceProviderOptions> configure)
    {
        return hostBuilder.UseServiceProviderFactory(context =>
        {
            var options = new ServiceProviderOptions();
            configure(context, options);
            return new DefaultServiceProviderFactory(options);
        });
    }

    public static IHostBuilder ConfigureLogging(this IHostBuilder hostBuilder, Action<HostBuilderContext, ILoggingBuilder> configureLogging)
    {
        return hostBuilder.ConfigureServices((context, collection) => collection.AddLogging(builder => configureLogging(context, builder)));
    }

    public static IHostBuilder ConfigureLogging(this IHostBuilder hostBuilder, Action<ILoggingBuilder> configureLogging)
    {
        return hostBuilder.ConfigureServices((context, collection) => collection.AddLogging(configureLogging));
    }

    public static IHostBuilder ConfigureHostOptions(this IHostBuilder hostBuilder, Action<HostBuilderContext, HostOptions> configureOptions)
    {
        return hostBuilder.ConfigureServices(
            (context, collection) => collection.Configure<HostOptions>(options => configureOptions(context, options)));
    }

    public static IHostBuilder ConfigureHostOptions(this IHostBuilder hostBuilder, Action<HostOptions> configureOptions)
    {
        return hostBuilder.ConfigureServices(collection => collection.Configure(configureOptions));
    }

    public static IHostBuilder ConfigureAppConfiguration(this IHostBuilder hostBuilder, Action<IConfigurationBuilder> configureDelegate)
    {
        return hostBuilder.ConfigureAppConfiguration((context, builder) => configureDelegate(builder));
    }

    public static IHostBuilder ConfigureServices(this IHostBuilder hostBuilder, Action<IServiceCollection> configureDelegate)
    {
        return hostBuilder.ConfigureServices((context, collection) => configureDelegate(collection));
    }

    public static IHostBuilder ConfigureContainer<TContainerBuilder>(this IHostBuilder hostBuilder, Action<TContainerBuilder> configureDelegate)
    {
        return hostBuilder.ConfigureContainer<TContainerBuilder>((context, builder) => configureDelegate(builder));
    }

    public static IHostBuilder ConfigureDefaults(this IHostBuilder builder, string[]? args)
    {
        return builder.ConfigureHostConfiguration(config => ApplyDefaultHostConfiguration(config, args))
                      .ConfigureAppConfiguration((hostingContext, config) => ApplyDefaultAppConfiguration(hostingContext, config, args))
                      .ConfigureServices(AddDefaultServices)
                      .UseServiceProviderFactory(context => new DefaultServiceProviderFactory(CreateDefaultServiceProviderOptions(context)));
    }

    private static void ApplyDefaultHostConfiguration(IConfigurationBuilder hostConfigBuilder, string[]? args)
    {
        SetDefaultContentRoot(hostConfigBuilder);

        hostConfigBuilder.AddEnvironmentVariables(prefix: "DOTNET_");
        AddCommandLineConfig(hostConfigBuilder, args);
    }

    internal static void SetDefaultContentRoot(IConfigurationBuilder hostConfigBuilder)
    {
        string cwd = Environment.CurrentDirectory;
        if (
#if NETFRAMEWORK
            Environment.OSVersion.Platform != PlatformID.Win32NT ||
#else
            !RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ||
#endif
            !string.Equals(cwd, Environment.SystemDirectory, StringComparison.OrdinalIgnoreCase))
        {
            hostConfigBuilder.AddInMemoryCollection(new[]
            {
                new KeyValuePair<string, string?>(HostDefaults.ContentRootKey, cwd),
            });
        }
    }

    internal static void ApplyDefaultAppConfiguration(HostBuilderContext hostingContext, IConfigurationBuilder appConfigBuilder, string[]? args)
    {
        IHostEnvironment env = hostingContext.HostingEnvironment;
        bool reloadOnChange = GetReloadConfigOnChangeValue(hostingContext);

        appConfigBuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: reloadOnChange)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: reloadOnChange);

        if (env.IsDevelopment() && env.ApplicationName is { Length: > 0 })
        {
            try
            {
                var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
                appConfigBuilder.AddUserSecrets(appAssembly, optional: true, reloadOnChange: reloadOnChange);
            }
            catch (FileNotFoundException)
            {
                // The assembly cannot be found, so just skip it.
            }
        }

        appConfigBuilder.AddEnvironmentVariables();

        AddCommandLineConfig(appConfigBuilder, args);

        [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2026:RequiresUnreferencedCode", Justification = "Calling IConfiguration.GetValue is safe when the T is bool.")]
        static bool GetReloadConfigOnChangeValue(HostBuilderContext hostingContext) => hostingContext.Configuration.GetValue("hostBuilder:reloadConfigOnChange", defaultValue: true);
    }

    internal static void AddCommandLineConfig(IConfigurationBuilder configBuilder, string[]? args)
    {
        if (args is { Length: > 0 })
        {
            configBuilder.AddCommandLine(args);
        }
    }

    internal static void AddDefaultServices(HostBuilderContext hostingContext, IServiceCollection services)
    {
        services.AddLogging(logging =>
        {
            bool isWindows =
#if NET
                OperatingSystem.IsWindows();
#elif NETFRAMEWORK
                Environment.OSVersion.Platform == PlatformID.Win32NT;
#else
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
#endif

            // IMPORTANT: This needs to be added *before* configuration is loaded, this lets
            // the defaults be overridden by the configuration.
            if (isWindows)
            {
                // Default the EventLogLoggerProvider to warning or above
                logging.AddFilter<EventLogLoggerProvider>(level => level >= LogLevel.Warning);
            }

            logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
#if NET
            if (!OperatingSystem.IsBrowser() && !OperatingSystem.IsWasi())
#endif
            {
                logging.AddConsole();
            }
            logging.AddDebug();
            logging.AddEventSourceLogger();

            if (isWindows)
            {
                // Add the EventLogLoggerProvider on windows machines
                logging.AddEventLog();
            }

            logging.Configure(options =>
            {
                options.ActivityTrackingOptions =
                    ActivityTrackingOptions.SpanId |
                    ActivityTrackingOptions.TraceId |
                    ActivityTrackingOptions.ParentId;
            });
        });

        services.AddMetrics(metrics =>
        {
            metrics.AddConfiguration(hostingContext.Configuration.GetSection("Metrics"));
        });
    }

    internal static ServiceProviderOptions CreateDefaultServiceProviderOptions(HostBuilderContext context)
    {
        bool isDevelopment = context.HostingEnvironment.IsDevelopment();
        return new ServiceProviderOptions
        {
            ValidateScopes = isDevelopment,
            ValidateOnBuild = isDevelopment,
        };
    }

    [UnsupportedOSPlatform("android")]
    [UnsupportedOSPlatform("browser")]
    [UnsupportedOSPlatform("ios")]
    [UnsupportedOSPlatform("tvos")]
    public static IHostBuilder UseConsoleLifetime(this IHostBuilder hostBuilder)
    {
        return hostBuilder.ConfigureServices(collection => collection.AddSingleton<IHostLifetime, ConsoleLifetime>());
    }

    [UnsupportedOSPlatform("android")]
    [UnsupportedOSPlatform("browser")]
    [UnsupportedOSPlatform("ios")]
    [UnsupportedOSPlatform("tvos")]
    public static IHostBuilder UseConsoleLifetime(this IHostBuilder hostBuilder, Action<ConsoleLifetimeOptions> configureOptions)
    {
        return hostBuilder.ConfigureServices(collection =>
        {
            collection.AddSingleton<IHostLifetime, ConsoleLifetime>();
            collection.Configure(configureOptions);
        });
    }

    [UnsupportedOSPlatform("android")]
    [UnsupportedOSPlatform("browser")]
    [UnsupportedOSPlatform("ios")]
    [UnsupportedOSPlatform("tvos")]
    public static Task RunConsoleAsync(this IHostBuilder hostBuilder, CancellationToken cancellationToken = default)
    {
        return hostBuilder.UseConsoleLifetime().Build().RunAsync(cancellationToken);
    }

    [UnsupportedOSPlatform("android")]
    [UnsupportedOSPlatform("browser")]
    [UnsupportedOSPlatform("ios")]
    [UnsupportedOSPlatform("tvos")]
    public static Task RunConsoleAsync(this IHostBuilder hostBuilder, Action<ConsoleLifetimeOptions> configureOptions, CancellationToken cancellationToken = default)
    {
        return hostBuilder.UseConsoleLifetime(configureOptions).Build().RunAsync(cancellationToken);
    }

    public static IHostBuilder ConfigureMetrics(this IHostBuilder hostBuilder, Action<IMetricsBuilder> configureMetrics)
    {
        return hostBuilder.ConfigureServices((context, collection) => collection.AddMetrics(configureMetrics));
    }

    public static IHostBuilder ConfigureMetrics(this IHostBuilder hostBuilder, Action<HostBuilderContext, IMetricsBuilder> configureMetrics)
    {
        return hostBuilder.ConfigureServices((context, collection) => collection.AddMetrics(builder => configureMetrics(context, builder)));
    }
}
