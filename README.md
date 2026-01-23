# Project Structure

## Folders
- **Controllers/**: HTTP endpoints (API routes)
- **Repositories/**: Data access layer (talks to databases)
  - **Interfaces/**: Repository contracts
- **Services/**: Business logic (game rules, calculations)
  - **Interfaces/**: Service contracts
- **Models/**: Domain entities (database models)
  - **Context/**: EF Core DbContext configurations
- **Dto/**: Data Transfer Objects (API response models)
- **Enums/**: Enumerations (Moves, Result)

## Pattern: Repository Pattern
Controllers → Repositories → DbContext → Database

Controllers don't know about DbContext or database details.
Repositories handle all data access.


## Migrations: 
dotnet ef migrations add RenamedModels --context SqlContext
dotnet ef database update --context SqlContext
