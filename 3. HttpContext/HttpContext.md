```csharp
public abstract class HttpContext
{
	public abstract IFeatureCollection Features { get; }
	public abstract HttpRequest Request { get; }
	public abstract HttpResponse Response { get; }
	public abstract ConnectionInfo Connection { get; }
	public abstract WebSocketManager WebSockets { get; }
	public abstract ClaimsPrincipal User { get; set; }
	public abstract IDictionary<object, object?> Items { get; set; }
	public abstract IServiceProvider RequestServices { get; set; }
	public abstract CancellationToken RequestAborted { get; set; }
	public abstract string TraceIdentifier { get; set; }
	public abstract ISession Session { get; set; }

	public abstract void Abort();
}
```
