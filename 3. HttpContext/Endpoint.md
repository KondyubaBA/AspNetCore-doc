#### IEndpointRouteBuilder
  - MapGet
  - MapPost
  - MapPut
  - MapDelete
  - MapPatch  
Выполняются через Map  

MapGroup - служит для добавления префикса группе endpoints

### 1. RequireHost
Ограничивает на каком хосте отработает точка.  
Добавляет метаданные:
```
"type": "HostAttribute",
"value": "Hosts: wed.admsurgut.ru:*"
```

### 2. WithDisplayName
Устанавливает WithDisplayName для endpoint

### 3. WithMetadata
Добавляет кастомные метаданные

### 4. WithName
В ASP.NET Core метод WithName используется для установки имени маршрута. Это позволяет вам задавать уникальные идентификаторы для маршрутов, что может быть полезно для генерации URL или при перенаправлении на определенные маршруты в приложении.
Добавляет метаданные:
```
"type": "RouteNameMetadata",
"value": "RouteName: GetProducts"
```

### 5. WithGroupName
Группирует маршруты для групповой настройки и пр.
Добавявлет метаданные:
```
"type": "EndpointGroupNameAttribute",
"value": "Microsoft.AspNetCore.Routing.EndpointGroupNameAttribute"
```

### 6. WithOrder
Настраивает порядок выполнения маршрутов если они совпадают
Устанавливает свойтво order в строителе

### 7. DisableAntiforgery
Отключает проверку
Добавляет метаданные:
```
"type": "AntiforgeryMetadata",
"value": "Microsoft.AspNetCore.Http.Metadata.AntiforgeryMetadata"
```

### 8. WithFormMappingOptions
Служит для нстройки маппига формы или ограничения
Добавляет метаданные:
```
```

### 9. WithFormOptions
Ограничение для формы - память, кол-во полей и пр.
Добавялет метаданные
```
```

### OpenApiRouteHandlerBuilderExtensions
#### Accepts
IAcceptsMetadata — это интерфейс в ASP.NET Core, который используется для определения, какие типы данных может принимать конечная точка (endpoint) в HTTP-запросах. Он позволяет разработчикам явно указывать, какие форматы данных (например, JSON, XML, формы и т.д.) поддерживаются конкретной конечной точкой.

#### ExcludeFromDescription
Исключить из описания endpoint

#### Produces
типы контента, которые будут возвращены, с помощью метода Produces. Это делается для настройки ответов вашего API на основе формата, который ожидает клиент.

#### ProducesProblem
#### ProducesValidationProblem
#### WithTags
Атрибут WithTags в ASP.NET Core минимальных API используется для группировки эндпоинтов по тегам в документации, сгенерированной с помощью Swagger (или OpenAPI). Это позволяет организовать и структурировать документацию вашего API, делая её более понятной и удобной для пользователей.

#### WithDescription
 добавлять описания к эндпоинтам вашего API в документации Swagger
#### WithSummary
добавлять краткие описания (суммарии) к эндпоинтам вашего API в документации Swagger. Это описание появляется в виде краткого текста над описанием метода в Swagger UI 
