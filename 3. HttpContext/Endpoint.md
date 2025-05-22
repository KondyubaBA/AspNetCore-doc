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
