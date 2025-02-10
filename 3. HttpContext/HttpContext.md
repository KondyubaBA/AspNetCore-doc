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

**Features** - Позволяет получить доступ к различным дополнительным функциям, связанным с текущим контекстом, таким как поддержка различных протоколов или расширений.  
**Request** - Представляет текущий HTTP-запрос. Содержит информацию о методе запроса, заголовках, параметрах и теле запроса.  
**Response** - Представляет HTTP-ответ, который будет отправлен клиенту. Позволяет настраивать заголовки, статус код и содержимое ответа.  
**Connection** - Содержит информацию о соединении, через которое был выполнен запрос, например, IP-адрес клиента и протокол.  
**WebSockets** - Позволяет управлять WebSocket-соединениями, что полезно для реализации реального времени и двусторонней связи.  
**User** - Представляет текущего пользователя, выполняющего запрос. Содержит информацию о пользователе, включая его идентификацию и роли.  
**Items** - Позволяет хранить произвольные данные, связанные с текущим контекстом. Это может быть полезно для передачи информации между middleware.  
**RequestServices** - Позволяет получить доступ к сервисам, зарегистрированным в контейнере зависимостей для текущего запроса.  
**RequestAborted** - Позволяет отслеживать, был ли отменен текущий запрос, что полезно для управления длительными операциями.  
**TraceIdentifier** - Уникальный идентификатор для текущего запроса, который может быть использован для отслеживания и отладки.  
**Session** - Позволяет работать с сессиями пользователя, сохраняя данные между запросами.  
**Abort()** - Метод, который используется для немедленного завершения обработки текущего запроса. Это может быть полезно, если нужно прервать выполнение, например, при отмене запроса клиентом.

#### HttpRequest
```csharp
public abstract class HttpRequest
{
	public abstract HttpContext HttpContext { get; }
	public abstract string Method { get; set; }
	public abstract string Scheme { get; set; }
	public abstract bool IsHttps { get; set; }
	public abstract HostString Host { get; set; }
	public abstract PathString PathBase { get; set; }
	public abstract PathString Path { get; set; }
	public abstract QueryString QueryString { get; set; }
	public abstract IQueryCollection Query { get; set; }
	public abstract string Protocol { get; set; }
	public abstract IHeaderDictionary Headers { get; }
	public abstract IRequestCookieCollection Cookies { get; set; }
	public abstract long? ContentLength { get; set; }
	public abstract string? ContentType { get; set; }
	public abstract Stream Body { get; set; }
	public virtual PipeReader BodyReader
	public abstract bool HasFormContentType { get; }
	public abstract IFormCollection Form { get; set; }
	public virtual RouteValueDictionary RouteValues { get; set; }

	public abstract Task<IFormCollection> ReadFormAsync(CancellationToken cancellationToken = default(CancellationToken));
}
```
