public interface IHostBuilder
{
    IDictionary<object, object> Properties { get; }

    IHostBuilder ConfigureHostConfiguration(Action<IConfigurationBuilder> configureDelegate);

    /// <summary>
    /// Sets up the configuration for the remainder of the build process and application. This can be called multiple times and
    /// the results will be additive. The results will be available at <see cref="HostBuilderContext.Configuration"/> for
    /// subsequent operations, as well as in <see cref="IHost.Services"/>.
    /// </summary>
    /// <param name="configureDelegate">The delegate for configuring the <see cref="IConfigurationBuilder"/> that will be used
    /// to construct the <see cref="IConfiguration"/> for the application.</param>
    /// <returns>The same instance of the <see cref="IHostBuilder"/> for chaining.</returns>
    IHostBuilder ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configureDelegate);

    /// <summary>
    /// Adds services to the container. This can be called multiple times and the results will be additive.
    /// </summary>
    /// <param name="configureDelegate">The delegate for configuring the <see cref="IServiceCollection"/> that will be used
    /// to construct the <see cref="IServiceProvider"/>.</param>
    /// <returns>The same instance of the <see cref="IHostBuilder"/> for chaining.</returns>
    IHostBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate);

    /// <summary>
    /// Overrides the factory used to create the service provider.
    /// </summary>
    /// <typeparam name="TContainerBuilder">The type of builder.</typeparam>
    /// <param name="factory">The factory to register.</param>
    /// <returns>The same instance of the <see cref="IHostBuilder"/> for chaining.</returns>
    IHostBuilder UseServiceProviderFactory<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory) where TContainerBuilder : notnull;

    /// <summary>
    /// Overrides the factory used to create the service provider.
    /// </summary>
    /// <typeparam name="TContainerBuilder">The type of builder.</typeparam>
    /// <param name="factory">The factory to register.</param>
    /// <returns>The same instance of the <see cref="IHostBuilder"/> for chaining.</returns>
    IHostBuilder UseServiceProviderFactory<TContainerBuilder>(Func<HostBuilderContext, IServiceProviderFactory<TContainerBuilder>> factory) where TContainerBuilder : notnull;

    /// <summary>
    /// Enables configuring the instantiated dependency container. This can be called multiple times and
    /// the results will be additive.
    /// </summary>
    /// <typeparam name="TContainerBuilder">The type of builder.</typeparam>
    /// <param name="configureDelegate">The delegate which configures the builder.</param>
    /// <returns>The same instance of the <see cref="IHostBuilder"/> for chaining.</returns>
    IHostBuilder ConfigureContainer<TContainerBuilder>(Action<HostBuilderContext, TContainerBuilder> configureDelegate);

    /// <summary>
    /// Runs the given actions to initialize the host. This can only be called once.
    /// </summary>
    /// <returns>An initialized <see cref="IHost"/>.</returns>
    IHost Build();
}
