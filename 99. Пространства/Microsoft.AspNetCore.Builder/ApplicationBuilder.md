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
}
```
