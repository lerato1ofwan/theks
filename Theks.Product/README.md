### Useful project commands.

Running Migrations locally, from the "Theks.Product" main folder:
```
dotnet ef migrations add <Migration-Name> --project Theks.Product.Infrastructure/Theks.Product.Infrastructure.csproj --startup-project Theks.Product.Api/Theks.Product.Api.csproj --output-dir Data/Migrations
```