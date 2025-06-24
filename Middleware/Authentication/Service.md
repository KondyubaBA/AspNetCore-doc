### AddAuthenticationCore
```csharp
public static AuthenticationBuilder AddAuthentication(this IServiceCollection services)
{
  ArgumentNullException.ThrowIfNull(services, "services");
  services.AddAuthenticationCore();
  services.AddDataProtection();
  services.AddWebEncoders();
  services.TryAddSingleton(TimeProvider.System);
  services.TryAddSingleton<ISystemClock, SystemClock>();
  services.TryAddSingleton<IAuthenticationConfigurationProvider, DefaultAuthenticationConfigurationProvider>();
  return new AuthenticationBuilder(services);
}

public static IServiceCollection AddAuthenticationCore(this IServiceCollection services)
{
	ArgumentNullException.ThrowIfNull(services, "services");
	services.TryAddScoped<IAuthenticationService, AuthenticationService>();
	services.TryAddSingleton<IClaimsTransformation, NoopClaimsTransformation>();
	services.TryAddScoped<IAuthenticationHandlerProvider, AuthenticationHandlerProvider>();
	services.TryAddSingleton<IAuthenticationSchemeProvider, AuthenticationSchemeProvider>();
	return services;
}
```
