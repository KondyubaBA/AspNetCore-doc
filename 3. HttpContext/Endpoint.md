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
