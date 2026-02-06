# WebApi Project – Overview

This document explains how **.NET 8 (ASP.NET Core)** is used with **MySQL** in this project, the main commands, how models become database tables, the project structure, and what the product does.

---

## 1. .NET Core with MySQL in This Project

### What is used

- **.NET 8** – Framework (ASP.NET Core Web API).
- **Entity Framework Core 8** – ORM to talk to the database from C#.
- **Pomelo.EntityFrameworkCore.MySql 8.0.2** – EF Core provider for **MySQL** (and MariaDB).

So: the app is a **.NET Core Web API** that uses **EF Core** and **Pomelo** to connect to **MySQL**, run queries, and create/update the schema (tables) from your C# models.

### How MySQL is connected

1. **Connection string** (in `appsettings.json`):
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost;Port=3306;Database=testdb;User=root;Password=YOUR_PASSWORD;"
   }
   ```
   - `Server` / `Port` – MySQL server address and port.
   - `Database` – Database name (e.g. `testdb`).
   - `User` / `Password` – MySQL login.

2. **Registration in `Program.cs`**:
   ```csharp
   builder.Services.AddDbContext<AppDbContext>(options =>
       options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
   ```
   - `AppDbContext` is the EF Core context that uses MySQL.
   - `UseMySql` uses the Pomelo provider; `ServerVersion.AutoDetect` lets it detect the MySQL server version.

3. **Usage in code** – Controllers use services (e.g. `ProductService`), and services inject `AppDbContext` and use `_db.Products`, `_db.Orders`, etc. EF Core turns that into MySQL queries.

---

## 2. Commands Used in This Project

All commands are run from the **project folder** (where `WebApi.csproj` is), e.g. `e:\FFM\WebApi`.

### General .NET commands

| Command | Purpose |
|--------|---------|
| `dotnet restore` | Restore NuGet packages (run automatically by build/run if needed). |
| `dotnet build` | Compile the project. |
| `dotnet run` | Build and run the API (starts the server). |
| `dotnet clean` | Remove build outputs. |
| `dotnet run --urls "https://localhost:5001"` | Run with a specific URL. |

### Entity Framework Core (database) commands

These require the **dotnet-ef** global tool. Install once:

```bash
dotnet tool install --global dotnet-ef --version 8.0.11
```

| Command | Purpose |
|--------|---------|
| `dotnet ef migrations add <MigrationName>` | Generate a new migration from your current models (e.g. `AddOrdersTable`, `InitialCreate`). |
| `dotnet ef database update` | Apply pending migrations to MySQL (create/update database and tables). |
| `dotnet ef migrations list` | List applied and pending migrations. |
| `dotnet ef database update <MigrationName>` | Update database to a specific migration. |
| `dotnet ef migrations remove` | Remove the last migration (if not yet applied). |
| `dotnet ef --version` | Show installed EF Core tools version. |

### Summary of “model → database” commands

- **First time (no database):**  
  `dotnet run` (if using `EnsureCreated()`) **or**  
  `dotnet ef migrations add InitialCreate` then `dotnet ef database update`.
- **After adding a new model/table:**  
  `dotnet ef migrations add AddXxxTable` then `dotnet ef database update`.

---

## 3. How Models Create the Database in MySQL (Commands and Flow)

### From C# model to MySQL table

1. **You define a model** in `Models/` (e.g. `Product.cs`) with properties and attributes (`[Key]`, `[Required]`, `[MaxLength]`, `[Table("Products")]`, etc.).
2. **You register it** in `Data/AppDbContext.cs` with a `DbSet<T>` (e.g. `public DbSet<Product> Products => Set<Product>();`).
3. **You create/update the database** using either:
   - **Option A – `EnsureCreated()`:** On first run, `dotnet run` creates the database and tables (only when the database does not exist).
   - **Option B – Migrations:** You run EF commands to generate and apply migrations, which create/update tables in MySQL.

### Commands for creating/updating the database (model → MySQL)

| Step | Command | What it does |
|------|---------|--------------|
| 1. Install EF tools (once) | `dotnet tool install --global dotnet-ef --version 8.0.11` | Installs the CLI used for migrations. |
| 2. Create a migration | `dotnet ef migrations add InitialCreate` or `AddOrdersTable` | Compares your models with the last snapshot and generates a migration (C# code that creates/alters tables). |
| 3. Apply to MySQL | `dotnet ef database update` | Runs pending migrations against the MySQL database (creates/updates tables). |

**Alternative (first-time only, no migrations):**  
Run the app once: `dotnet run`. The code calls `db.Database.EnsureCreated()`, which creates the database and all tables defined in `AppDbContext` if they don’t exist. This does **not** create new tables when you add new models later; for that you must use migrations.

### Flow diagram (model → DB)

```
Models (e.g. Product.cs)  →  AppDbContext (DbSet<Product>)
                                    ↓
              dotnet ef migrations add AddOrdersTable
                                    ↓
              Migrations/XXXX_AddOrdersTable.cs (Up/Down methods)
                                    ↓
              dotnet ef database update
                                    ↓
              MySQL: CREATE TABLE ... (tables created/updated)
```

---

## 4. Main Files and Project Structure

### Folder and file layout

```
WebApi/
├── Program.cs                 # App entry; configures services, MySQL, JWT, middleware, routes
├── WebApi.csproj              # Project file (target framework, packages)
├── appsettings.json           # Config: connection string, JWT, logging
├── appsettings.Development.json
│
├── Controllers/               # HTTP API endpoints
│   ├── AuthController.cs      # Register, Login, ForgotPassword, ChangePassword
│   ├── ProductsController.cs  # CRUD for products
│   ├── CategoriesController.cs
│   ├── CustomersController.cs
│   └── OrdersController.cs
│
├── Models/                    # Entity classes (map to MySQL tables)
│   ├── User.cs
│   ├── Product.cs
│   ├── Category.cs
│   ├── Customer.cs
│   └── Order.cs
│
├── Data/                      # Database and migrations docs
│   ├── AppDbContext.cs        # EF Core context; DbSets and model config
│   ├── CreateTables.sql       # Optional manual SQL for tables
│   ├── CreateTablesFromModels.md
│   └── Migrations-StepByStep.md
│
├── Migrations/                # EF Core migrations (model → schema changes)
│   ├── XXXXX_AddOrdersTable.cs
│   ├── XXXXX_AddOrdersTable.Designer.cs
│   └── AppDbContextModelSnapshot.cs
│
├── DTOs/                      # Data transfer objects (request/response shapes)
│   ├── Auth/                  # RegisterRequest, LoginRequest, LoginResponse, etc.
│   ├── Product/               # ProductCreateDto, ProductReadDto, ProductUpdateDto
│   ├── Category/
│   ├── Customer/
│   └── Order/
│
├── Services/                  # Business logic; use AppDbContext to access MySQL
│   ├── IAuthService.cs, AuthService.cs
│   ├── IProductService.cs, ProductService.cs
│   ├── ICategoryService.cs, CategoryService.cs
│   ├── ICustomerService.cs, CustomerService.cs
│   └── IOrderService.cs, OrderService.cs
│
├── Middleware/
│   └── ExceptionMiddleware.cs # Global exception handling
│
└── Properties/
    └── launchSettings.json    # Run profiles and URLs
```

### Main files explained

| File | Role |
|------|------|
| **Program.cs** | Configures dependency injection (DbContext, services), MySQL via `UseMySql`, JWT authentication, authorization (authenticated by default), Swagger, middleware pipeline, and `EnsureCreated()` on startup (first-run DB creation). |
| **WebApi.csproj** | Defines target framework (net8.0), nullable, and packages: Pomelo.EntityFrameworkCore.MySql, EF Core Design, JWT Bearer, BCrypt, Swashbuckle. |
| **appsettings.json** | Holds `ConnectionStrings:DefaultConnection` (MySQL) and `JwtSettings` (key, issuer, audience, expiry). |
| **AppDbContext.cs** | Derives from `DbContext`; exposes `DbSet<User>`, `DbSet<Product>`, etc., and configures the model (e.g. unique index on `User.Email`). This is the bridge between C# and MySQL. |
| **Models/*.cs** | Entity classes with `[Table]`, `[Key]`, `[Required]`, `[MaxLength]`, etc. They map to MySQL tables. |
| **Controllers/*.cs** | Define HTTP routes (GET/POST/PUT/DELETE). Call services and return DTOs. Most require `[Authorize]` (JWT). |
| **Services/*.cs** | Contain business logic; inject `AppDbContext` and perform queries/updates (EF Core talks to MySQL). |
| **DTOs/** | Request/response models for the API (e.g. create/read/update DTOs per entity). |
| **Migrations/** | Generated by `dotnet ef migrations add`; the `Up` method applies schema changes to MySQL when you run `dotnet ef database update`. |

---

## 5. What the Product Does (Whole Product Explanation)

### In one sentence

This is a **REST API** built with **.NET 8** and **MySQL** that manages **users (auth)**, **products**, **categories**, **customers**, and **orders**, with **JWT authentication** so that only logged-in users can use the main resources.

### Main features

1. **Authentication (AuthController)**  
   - **Register** – Create account (email, password hashed with BCrypt).  
   - **Login** – Returns a JWT token.  
   - **Forgot password** – Request password reset.  
   - **Change password** – For authenticated users only.  

2. **Protected resources (only with valid JWT)**  
   - **Products** – CRUD (create, read, update, delete).  
   - **Categories** – CRUD.  
   - **Customers** – CRUD.  
   - **Orders** – CRUD (orders linked to customers).  

3. **Security**  
   - Global “authenticated by default” policy: all endpoints require login unless marked `[AllowAnonymous]`.  
   - Only Register, Login, and Forgot password are public; everything else (including Products, Categories, Customers, Orders) needs a valid JWT in the `Authorization: Bearer <token>` header.  

4. **Database**  
   - **MySQL** holds all data.  
   - **Entity Framework Core** + **Pomelo** map C# models to tables.  
   - Tables: `Users`, `Categories`, `Products`, `Customers`, `Orders`, and `__EFMigrationsHistory` (used by EF).  
   - Schema can be created/updated by `EnsureCreated()` on first run or by **migrations** (`dotnet ef migrations add` + `dotnet ef database update`).  

5. **API surface**  
   - REST over HTTP/HTTPS.  
   - JSON request/response.  
   - Swagger UI in development for testing (e.g. `/swagger`).  

### Request flow (example: add a product)

1. Client sends **POST** `/api/products` with JSON body and header `Authorization: Bearer <JWT>`.  
2. ASP.NET Core validates the JWT.  
3. `ProductsController` receives the request, validates the DTO, calls `ProductService.CreateAsync(dto)`.  
4. `ProductService` uses `AppDbContext`, creates a `Product` entity, adds it to `_db.Products`, calls `SaveChangesAsync()`.  
5. EF Core generates MySQL `INSERT` and runs it.  
6. Controller returns the created product (e.g. 201 Created) with the new product data.  

So: **.NET Core** handles HTTP and auth, **EF Core + Pomelo** handle MySQL, and **models** define the structure that becomes tables when you run the app (EnsureCreated) or the migration commands above.

---

## Quick reference – Commands list

| Purpose | Command(s) |
|--------|-------------|
| Run the API | `dotnet run` |
| Build | `dotnet build` |
| Install EF tools | `dotnet tool install --global dotnet-ef --version 8.0.11` |
| Add migration (new/changed model) | `dotnet ef migrations add <Name>` |
| Create/update DB in MySQL | `dotnet ef database update` |
| List migrations | `dotnet ef migrations list` |

For more detail on creating tables from models, see **Data/CreateTablesFromModels.md** and **Data/Migrations-StepByStep.md**.
