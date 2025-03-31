#### AddRoutingCore
```csharp
ArgumentNullException.ThrowIfNull(services, "services");
services.AddMetrics();
services.TryAddTransient<IInlineConstraintResolver, DefaultInlineConstraintResolver>();
services.TryAddTransient<ObjectPoolProvider, DefaultObjectPoolProvider>();
services.TryAddSingleton((IServiceProvider s) => s.GetRequiredService<ObjectPoolProvider>().Create(new UriBuilderContextPooledObjectPolicy()));
services.TryAdd(ServiceDescriptor.Transient(delegate(IServiceProvider s)
{
  ILoggerFactory requiredService = s.GetRequiredService<ILoggerFactory>();
  ObjectPool<UriBuildingContext> requiredService2 = s.GetRequiredService<ObjectPool<UriBuildingContext>>();
  IInlineConstraintResolver requiredService3 = s.GetRequiredService<IInlineConstraintResolver>();
  return new TreeRouteBuilder(requiredService, requiredService2, requiredService3);
}));
services.TryAddSingleton(typeof(RoutingMarkerService));
ObservableCollection<EndpointDataSource> dataSources = new ObservableCollection<EndpointDataSource>();
services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<RouteOptions>, ConfigureRouteOptions>((IServiceProvider serviceProvider) => new ConfigureRouteOptions(dataSources)));
services.TryAddSingleton((Func<IServiceProvider, EndpointDataSource>)((IServiceProvider s) => new CompositeEndpointDataSource(dataSources)));
services.TryAddSingleton<ParameterPolicyFactory, DefaultParameterPolicyFactory>();
services.TryAddSingleton<MatcherFactory, DfaMatcherFactory>();
services.TryAddTransient<DfaMatcherBuilder>();
services.TryAddSingleton<DfaGraphWriter>();
services.TryAddTransient<DataSourceDependentMatcher.Lifetime>();
services.TryAddSingleton((IServiceProvider services) => new EndpointMetadataComparer(services));
services.TryAddSingleton<LinkGenerator, DefaultLinkGenerator>();
services.TryAddSingleton<IEndpointAddressScheme<string>, EndpointNameAddressScheme>();
services.TryAddSingleton<IEndpointAddressScheme<RouteValuesAddress>, RouteValuesAddressScheme>();
services.TryAddSingleton<LinkParser, DefaultLinkParser>();
services.TryAddSingleton<EndpointSelector, DefaultEndpointSelector>();
services.TryAddEnumerable(ServiceDescriptor.Singleton<MatcherPolicy, HttpMethodMatcherPolicy>());
services.TryAddEnumerable(ServiceDescriptor.Singleton<MatcherPolicy, HostMatcherPolicy>());
services.TryAddEnumerable(ServiceDescriptor.Singleton<MatcherPolicy, AcceptsMatcherPolicy>());
services.TryAddEnumerable(ServiceDescriptor.Singleton<MatcherPolicy, ContentEncodingNegotiationMatcherPolicy>());
services.TryAddSingleton<TemplateBinderFactory, DefaultTemplateBinderFactory>();
services.TryAddSingleton<RoutePatternTransformer, DefaultRoutePatternTransformer>();
services.TryAddSingleton<RoutingMetrics>();
services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<RouteHandlerOptions>, ConfigureRouteHandlerOptions>());
return services;
```
