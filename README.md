# Fresh Market - Healthy Food ECommerce API

A layered ASP.NET Core Web API project for an e-commerce system with authentication, product/category management, carts, orders, image upload, filtering, and pagination.

## Tech Stack

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- ASP.NET Core Identity
- JWT Authentication
- FluentValidation
- Scalar / OpenAPI

## Solution Structure

- `ECommerce.APIs` -> Presentation layer (controllers, startup)
- `ECommerce.BLL` -> Business logic (managers, DTOs, validators, mapping)
- `ECommerce.DAL` -> Data access (DbContext, repositories, unit of work, seeders)
- `ECommerce.Common` -> Shared models (general result, filtering, pagination)

## Features

- JWT-based authentication and authorization
- Role-based access (`Admin`, `User`)
- Product CRUD + filtering + pagination
- Category CRUD
- Cart operations (add/remove/update items)
- Order placement and retrieval
- Image upload for products and categories
- Seed data for roles/admin/catalog

## Getting Started

### 1) Prerequisites

- .NET 10 SDK
- SQL Server (LocalDB / SQLExpress / SQL Server)

### 2) Configure settings

Edit:

- `ECommerce.APIs/appsettings.Development.json`

Update:

- `ConnectionStrings:ECommerce`
- `JwtSettings:Issuer`
- `JwtSettings:Audience`
- `JwtSettings:ExpirationInMinutes`
- `JwtSettings:SecretKey` (Base64, minimum 16 bytes after decode)

> Do not use production secrets in source control.

### 3) Apply migrations

From solution root:

```powershell
dotnet ef database update --project ECommerce.DAL --startup-project ECommerce.APIs
```

### 4) Run the API

```powershell
dotnet run --project ECommerce.APIs
```

## API Documentation

In development, OpenAPI and Scalar are enabled by default in `Program.cs`.

After running, open the documented endpoints from the API host.

## Authentication Flow

1. `POST /api/Auth/register`
2. `POST /api/Auth/login`
3. Use returned bearer token in:
   - `Authorization: Bearer <token>`

## Main Endpoints

- `api/Auth`
- `api/Product`
- `api/Category`
- `api/Cart`
- `api/Order`

## Static Files

Uploaded files are served from:

- Physical path: `ECommerce.APIs/Files`
- Public path: `/Files/*`

## CORS

Configured in `Program.cs` with policy `AllowFrontend` (currently allowing `http://localhost:4200`).

## Notes

- Token lifetime is enforced with `ValidateLifetime = true` and `ClockSkew = TimeSpan.Zero`.
- This project uses layered architecture and a Unit of Work + Repository pattern.
- If you change domain models, create and apply new EF migrations.

## License

Add your license information here.
