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

## HttpRequest
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
**HttpContext** - Возвращает контекст HTTP, к которому относится текущий запрос. Это позволяет получить доступ ко всем аспектам текущего запроса и ответа.  
**Method** - Получает или устанавливает HTTP-метод запроса (например, GET, POST, PUT, DELETE).  
**Scheme** - Получает или устанавливает схему запроса (например, http или https).  
**IsHttps** - Указывает, является ли запрос защищенным (через HTTPS).  
**Host** - Получает или устанавливает заголовок Host запроса, который содержит информацию о хосте и порте.  
**PathBase** - Получает или устанавливает базовый путь запроса, который может использоваться для маршрутизации.  
**Path** - Получает или устанавливает путь к ресурсу, запрашиваемому в текущем запросе.  
**QueryString** - Получает или устанавливает строку запроса, содержащую параметры, переданные в URL.  
**Query** - Получает коллекцию параметров запроса, которые были переданы в URL.  
**Protocol** - Получает или устанавливает протокол, используемый в запросе (например, HTTP/1.1).  
**Headers** - Получает коллекцию заголовков запроса, которые были отправлены клиентом. 
**Cookies** - Получает или устанавливает коллекцию куки, отправленных с запросом.  
**ContentLength** - Получает или устанавливает длину содержимого запроса, если она известна.  
**ContentType** - Получает или устанавливает тип содержимого запроса (например, application/json).  
**Body** - Получает или устанавливает поток, содержащий тело запроса. Это может быть полезно для чтения данных, отправленных в запросе.  
**BodyReader** - Позволяет асинхронно читать тело запроса с использованием PipeReader.  
**HasFormContentType** - Указывает, имеет ли запрос тип содержимого, соответствующий форме (например, application/x-www-form-urlencoded или multipart/form-data).  
**Form** -  Получает или устанавливает коллекцию данных формы, отправленных с запросом.  
**RouteValues** - Получает или устанавливает значения маршрута, которые могут быть использованы для маршрутизации.
**ReadFormAsync()** - Асинхронно читает данные формы из тела запроса и возвращает их в виде IFormCollection. Это полезно, когда вы ожидаете, что запрос содержит данные формы.  

## HttpResponse
```csharp
public abstract class HttpResponse
{
	public abstract HttpContext HttpContext { get; }
	public abstract int StatusCode { get; set; }
	public abstract IHeaderDictionary Headers { get; }
	public abstract Stream Body { get; set; }
	public virtual PipeWriter BodyWriter
	public abstract long? ContentLength { get; set; }
	public abstract string? ContentType { get; set; }
	public abstract IResponseCookies Cookies { get; }
	public abstract bool HasStarted { get; }
	public abstract void OnStarting(Func<object, Task> callback, object state);

	public virtual void OnStarting(Func<Task> callback)
	public abstract void OnCompleted(Func<object, Task> callback, object state);
	public virtual void RegisterForDispose(IDisposable disposable)
	public virtual void RegisterForDisposeAsync(IAsyncDisposable disposable)
	public virtual void OnCompleted(Func<Task> callback)
	public virtual void Redirect([StringSyntax("Uri")] string location)
	public abstract void Redirect([StringSyntax("Uri")] string location, bool permanent);
	public virtual Task StartAsync(CancellationToken cancellationToken = default(CancellationToken))
	public virtual Task CompleteAsync()
}
```
**HttpContext** - Возвращает контекст HTTP, связанный с текущим ответом. Это позволяет получить доступ ко всем аспектам текущего запроса и ответа.  
**StatusCode** -  Получает или устанавливает код состояния ответа (например, 200 для успешного запроса, 404 для не найдено).  
**Headers** - Получает коллекцию заголовков, которые будут отправлены с ответом. Это позволяет настраивать заголовки, такие как Content-Type, Cache-Control и другие.  
**Body** - Получает или устанавливает поток, который будет использоваться для записи содержимого ответа. Это полезно для отправки данных, таких как файлы или другие потоки данных.  
**BodyWriter** - Позволяет асинхронно записывать данные в тело ответа с использованием PipeWriter.  
**ContentLength** - Получает или устанавливает длину содержимого ответа, если она известна. Это может быть полезно для указания размера содержимого в заголовках.  
**ContentType** - Получает или устанавливает тип содержимого ответа (например, application/json, text/html).  
**Cookies** - Получает коллекцию куки, которые будут отправлены с ответом. Это позволяет добавлять, изменять или удалять куки.  
**HasStarted** - Указывает, был ли уже начат процесс отправки ответа. Это может быть полезно для проверки, можно ли еще изменять ответ.  

**OnStarting()** -  Позволяет зарегистрировать обратный вызов, который будет выполнен перед началом отправки ответа. Это может быть полезно для выполнения дополнительных действий, таких как изменение заголовков.  
**OnCompleted()** - Позволяет зарегистрировать обратный вызов, который будет выполнен после завершения обработки ответа. Это может быть полезно для выполнения действий после отправки ответа, например, логирования.  
**Redirect()** - Перенаправляет клиента на указанный URL. Метод может принимать флаг, указывающий, является ли перенаправление постоянным (301) или временным (302).  
**StartAsync()** - Асинхронно начинает процесс отправки ответа. Это может быть полезно для обработки ответов, которые требуют асинхронной обработки.  
**CompleteAsync()** - Асинхронно завершает отправку ответа. Это может быть полезно для завершения обработки и освобождения ресурсов.  
**RegisterForDispose()** **RegisterForDisposeAsync()** - Позволяют зарегистрировать объекты, которые должны быть освобождены после завершения обработки ответа. Это может быть полезно для управления ресурсами и предотвращения утечек памяти.  










