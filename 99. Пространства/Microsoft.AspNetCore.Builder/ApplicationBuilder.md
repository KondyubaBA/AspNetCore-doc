> Microsoft.AspNetCore.Http

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
  public IApplicationBuilder Use(Func<RequestDelegate, RequestDelegate> middleware);
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
