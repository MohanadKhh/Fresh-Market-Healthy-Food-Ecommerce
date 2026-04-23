# Fresh Market - Healthy Food ECommerce API

A fully-featured RESTful backend API for an e-commerce platform built with **ASP.NET Core**, supporting product browsing, cart management, and order processing with JWT authentication.

---

## 📋 Table of Contents

- [Overview](#overview)
- [Architecture](#architecture)
- [Tech Stack](#tech-stack)
- [Features](#features)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Environment Variables](#environment-variables)
- [API Endpoints](#api-endpoints)
- [Authentication](#authentication)
- [Testing](#testing)

---

## Overview

ECommerce API is a backend-only system that allows users to browse products, manage their cart, and place orders. It supports role-based access control with **Admin** and **User** roles, JWT authentication, and clean N-Tier architecture.

---

## Architecture

The project follows **N-Tier Architecture** with 3 layers + Common:

```
ECommerce.APIs        → Controllers, Middleware, Program.cs
ECommerce.BLL         → Business Logic, Managers, Validators, DTOs
ECommerce.DAL         → DbContext, Models, Repositories, Migrations
ECommerce.Common      → Shared models (Result Pattern, Settings, Errors)
```

**Design Patterns Used:**
- Repository Pattern (Generic + Non-Generic)
- Unit of Work
- Result Pattern (General Response Wrapper)
- DTOs with FluentValidation
- Async/Await throughout

---

## Tech Stack

| Technology | Usage |
|---|---|
| ASP.NET Core 8 | Web API Framework |
| Entity Framework Core | ORM + Migrations |
| Microsoft Identity | User Management |
| JWT Bearer | Authentication |
| FluentValidation | Request Validation |
| SQL Server | Database |
| Scalar | API Documentation |

---

## Features

- ✅ User registration and login with JWT tokens
- ✅ Role-based authorization (Admin / User)
- ✅ UserId extracted from JWT claims — never passed in requests
- ✅ Product browsing with filtering, search, and pagination
- ✅ Category management
- ✅ Cart management (add, update, remove items)
- ✅ Order placement with stock validation
- ✅ Order history and details
- ✅ Image upload for products and categories
- ✅ Result pattern for consistent API responses
- ✅ CORS enabled
- ✅ Default Admin account and roles seeded on startup

---

## Project Structure

```
ECommerce-Backend/
│
├── ECommerce.APIs/
│   ├── Controllers/
│   │   ├── AuthController.cs
│   │   ├── ProductsController.cs
│   │   ├── CategoriesController.cs
│   │   ├── CartController.cs
│   │   ├── OrdersController.cs
│   │   └── ImageController.cs
│   ├── Files/
│   └── Program.cs
│
├── ECommerce.BLL/
│   ├── Managers/
│   │   ├── Auth/
│   │   ├── Products/
│   │   ├── Categories/
│   │   ├── Cart/
│   │   └── Orders/
│   ├── DTOs/
│   ├── Validators/
│   ├── Mapping/
│   └── Extensions/
│
├── ECommerce.DAL/
│   ├── Models/
│   │   ├── ApplicationUser.cs
│   │   ├── Product.cs
│   │   ├── Category.cs
│   │   ├── Cart.cs
│   │   ├── CartItem.cs
│   │   ├── Order.cs
│   │   └── OrderItem.cs
│   ├── Repositories/
│   │   ├── Generic/
│   │   └── NonGeneric/
│   ├── UnitOfWork/
│   ├── Seed/
│   ├── Migrations/
│   └── AppDbContext.cs
│
└── ECommerce.Common/
    ├── Results/
    ├── Errors/
    └── Settings/
        └── JwtSettings.cs
```

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server)
- [EF Core CLI](https://learn.microsoft.com/en-us/ef/core/cli/dotnet)

```bash
dotnet tool install --global dotnet-ef
```

### Installation

**1. Clone the repository**
```bash
git clone https://github.com/your-username/ECommerce-Backend.git
cd ECommerce-Backend
```

**2. Configure appsettings.json**

Update `ECommerce.APIs/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=ECommerceDB;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "JwtSettings": {
    "SecretKey": "YourStrongSecretKeyHereMinimum16Chars",
    "Issuer": "ECommerceAPI",
    "Audience": "ECommerceClient",
    "DurationInDays": 7
  }
}
```

**3. Apply migrations**
```bash
dotnet ef migrations add InitialCreate --project ECommerce.DAL --startup-project ECommerce.APIs
dotnet ef database update --project ECommerce.DAL --startup-project ECommerce.APIs
```

**4. Run the project**
```bash
dotnet run --project ECommerce.APIs
```

**5. Open API docs**
```
https://localhost:7021/scalar
```

---

## Environment Variables

| Key | Description | Example |
|---|---|---|
| `ConnectionStrings:DefaultConnection` | SQL Server connection string | `Server=.;Database=ECommerceDB;...` |
| `JwtSettings:SecretKey` | JWT signing key (min 16 chars) | `MyStrongSecretKey123!` |
| `JwtSettings:Issuer` | Token issuer name | `ECommerceAPI` |
| `JwtSettings:Audience` | Token audience name | `ECommerceClient` |
| `JwtSettings:DurationInDays` | Token expiry in days | `7` |

---

## API Endpoints

### Auth
| Method | Endpoint | Description | Auth |
|---|---|---|---|
| POST | `/api/auth/register` | Register new user | ❌ |
| POST | `/api/auth/login` | Login and get JWT token | ❌ |

### Categories
| Method | Endpoint | Description | Auth |
|---|---|---|---|
| GET | `/api/categories` | Get all categories | ❌ |
| GET | `/api/categories/{id}` | Get category by id | ❌ |
| POST | `/api/categories` | Create category | ✅ Admin |
| PUT | `/api/categories/{id}` | Update category | ✅ Admin |
| DELETE | `/api/categories/{id}` | Delete category | ✅ Admin |
| POST | `/api/categories/{id}/image` | Upload category image | ✅ Admin |

### Products
| Method | Endpoint | Description | Auth |
|---|---|---|---|
| GET | `/api/products` | Get products (filter + search + pagination) | ❌ |
| GET | `/api/products/{id}` | Get product by id | ❌ |
| POST | `/api/products` | Create product | ✅ Admin |
| PUT | `/api/products/{id}` | Update product | ✅ Admin |
| DELETE | `/api/products/{id}` | Delete product | ✅ Admin |
| POST | `/api/products/{id}/image` | Upload product image | ✅ Admin |

### Cart
| Method | Endpoint | Description | Auth |
|---|---|---|---|
| GET | `/api/cart` | Get current user cart | ✅ User |
| POST | `/api/cart` | Add item to cart | ✅ User |
| PUT | `/api/cart` | Update cart item quantity | ✅ User |
| DELETE | `/api/cart/{productId}` | Remove item from cart | ✅ User |

### Orders
| Method | Endpoint | Description | Auth |
|---|---|---|---|
| POST | `/api/orders` | Place a new order | ✅ User |
| GET | `/api/orders` | Get order history | ✅ User |
| GET | `/api/orders/{id}` | Get order details | ✅ User |

### Images
| Method | Endpoint | Description | Auth |
|---|---|---|---|
| POST | `/api/image/upload` | Upload image | ✅ Admin |

---

## Authentication

The API uses **JWT Bearer Authentication** with **Policy-Based Authorization**.

**How to authenticate:**

1. Register or login to get a token:
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "admin@ecommerce.com",
  "password": "Admin@123456"
}
```

2. Use the token in subsequent requests:
```http
GET /api/orders
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Default Admin Account (seeded on startup):**
```
Email:    admin@ecommerce.com
Password: Admin@123456
```

**Authorization Policies:**
| Policy | Roles | Used On |
|---|---|---|
| `AdminOnly` | Admin | Create/Update/Delete products & categories |
| `AdminOrUser` | Admin, User | Cart & Orders |

---

## Testing

A full Postman testing walkthrough is available here:

🎥 **[Postman Testing Video →](https://your-video-link-here)**

**To test locally:**
1. Run the project
2. Open Scalar at `https://localhost:7021/scalar`
3. Register a user or use the default admin account
4. Copy the JWT token from login response
5. Use `Bearer <token>` in Authorization header for protected endpoints
