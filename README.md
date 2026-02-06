# WebApi – .NET 8 + EF Core + MySQL + JWT

ASP.NET Core Web API with Entity Framework Core (Code-First), MySQL, and JWT authentication.

## Setup

### 1. MySQL

- Create database (optional; EF can create it): `CREATE DATABASE testdb;`
- Do **not** create tables manually; they are created by EF Core migrations.

### 2. Connection string

Edit `appsettings.json` (or `appsettings.Development.json`):

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Port=3306;Database=testdb;User=root;Password=YOUR_PASSWORD;"
}
```

Replace `YOUR_PASSWORD` with your MySQL root password.

### 3. JWT (optional)

Default in `appsettings.json`:

```json
"JwtSettings": {
  "Key": "YourSuperSecretKeyThatIsAtLeast32CharactersLong!",
  "Issuer": "WebApi",
  "Audience": "WebApi",
  "ExpiryMinutes": 60
}
```

Change `Key` in production.

### 4. Migrations and run

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
dotnet run
```

- Swagger: **https://localhost:5001/swagger** (or the port in `launchSettings.json`).

## API overview

### Auth (no token required for register/login)

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/auth/register` | Register; returns JWT |
| POST | `/api/auth/login` | Login; returns JWT |
| POST | `/api/auth/forgot-password` | Forgot password (mock email) |
| POST | `/api/auth/change-password` | Change password (requires Bearer token) |

### CRUD (require `Authorization: Bearer <token>`)

- **Products**: `GET/POST/PUT/DELETE` `/api/products` and `/api/products/{id}`
- **Categories**: `GET/POST/PUT/DELETE` `/api/categories` and `/api/categories/{id}`
- **Customers**: `GET/POST/PUT/DELETE` `/api/customers` and `/api/customers/{id}`

### Sample request/response

**Register**

```json
POST /api/auth/register
{ "name": "Test User", "email": "test@test.com", "password": "password123" }
→ { "token": "...", "email": "test@test.com", "name": "Test User", "expiresAt": "..." }
```

**Login**

```json
POST /api/auth/login
{ "email": "test@test.com", "password": "password123" }
→ { "token": "...", "email": "test@test.com", "name": "Test User", "expiresAt": "..." }
```

**Create product (with Bearer token)**

```json
POST /api/products
{ "name": "Widget", "price": 9.99, "description": "A widget" }
→ 201 + { "id": 1, "name": "Widget", "price": 9.99, "description": "A widget" }
```

## Project structure

- **Controllers** – Auth, Products, Categories, Customers
- **Models** – User, Product, Category, Customer
- **DTOs** – Auth and CRUD request/response DTOs
- **Data** – `AppDbContext` and EF configuration
- **Services** – Auth and CRUD business logic
- **Middleware** – Global exception handling

## Tools

- EF Core CLI: `dotnet ef` (install with `dotnet tool install --global dotnet-ef` if needed).
