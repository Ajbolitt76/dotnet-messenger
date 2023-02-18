# Messenger - Бэкэнд TEST

## Cheatsheet комманд

### EF
| Действие           | Комманда                                                                                                                |
|:-------------------|:------------------------------------------------------------------------------------------------------------------------|
| Установить EF.Tool | `dotnet tool install --global dotnet-ef`                                                                                |
| Создать миграцию   | Находясь в корне решения<br/> `dotnet ef migrations -p .\Messenger.Data\ -s .\Messenger.Api\ add AddFieldsToUser` |
| Создать секрет     | `dotnet user-secrets set "Movies:ServiceApiKey" "12345"`                                                                |

### [Секреты](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-7.0&tabs=windows)

Для секретов валидна стандартное представление уплощенного JSON-объекта. 

Например, для хранения строки подключения к БД, нужно создать секрет с именем `ConnectionStrings:DefaultConnection` и значением строки подключения.

Список секретов:
- `ConnectionStrings:DefaultConnection` - строка подключения к БД