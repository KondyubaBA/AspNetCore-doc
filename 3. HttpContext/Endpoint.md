#### IEndpointRouteBuilder
  - MapGet
  - MapPost
  - MapPut
  - MapDelete
  - MapPatch  
Выполняются через Map  

MapGroup - служит для добавления префикса группе endpoints

### 1.RequireHost
Ограничивает на каком хосте отработает точка.  
Добавляет метаданные:
```
"type": "HostAttribute",
"value": "Hosts: wed.admsurgut.ru:*"
```
