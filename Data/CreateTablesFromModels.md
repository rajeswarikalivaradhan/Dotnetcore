# Create MySQL Tables from Your Models (One-Command Process)

Your tables are created from the **models** in `WebApi.Models` (User, Product, Category, Customer). Use either option below.

---

## Option 1: Run the app (simplest – one command)

Tables are created automatically when the app starts.

1. Set your MySQL connection in **appsettings.json**:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost;Port=3306;Database=testdb;User=root;Password=YOUR_PASSWORD;"
   }
   ```

2. From the project folder, run:
   ```bash
   dotnet run
   ```

3. On first run, the app creates the database (if missing) and the tables **Users**, **Categories**, **Products**, **Customers** to match your models.

**Important:** `EnsureCreated()` runs only when the database does **not** exist. It **does not** create new tables when you add a new model later. For any new table, use **Option 2** (migrations) below.

---

## When you add a new table (new model)

Whenever you add a new entity class and `DbSet` in `AppDbContext`, create the table in MySQL with **migrations**:

### Step 1: Add your model and register it

1. Create the new model in **Models/** (e.g. `Models/Order.cs`).
2. Add a `DbSet` in **Data/AppDbContext.cs**:
   ```csharp
   public DbSet<Order> Orders => Set<Order>();
   ```

### Step 2: Create and apply the migration

From the **project folder** in terminal:

```bash
# Create a migration for your new table (use a descriptive name)
dotnet ef migrations add AddOrdersTable

# Apply it – this creates the new table on the MySQL server
dotnet ef database update
```

That’s it. The new table will exist in MySQL and match your model.

**One-time requirement:** If you haven’t already, install the EF Core tools:
```bash
dotnet tool install --global dotnet-ef
```

---

## Option 2: EF Core migrations (recommended for production and new tables)

Use this when you want versioned schema changes and to run updates with a single command.

### One-time setup: install EF Core tools

```bash
dotnet tool install --global dotnet-ef
```

(If already installed: `dotnet tool update --global dotnet-ef`.)

### Create tables (or apply latest migrations)

From the **project folder** (e.g. `e:\FFM\WebApi`):

```bash
# 1. Add a migration from your current models (only when you add/change models)
dotnet ef migrations add InitialCreate

# 2. Create/update the database and tables on MySQL (this is the command that creates tables)
dotnet ef database update
```

- **First time:** Run both. `database update` creates the database and all tables.
- **Later:** After changing models, run `migrations add YourMigrationName`, then `dotnet ef database update` again.

### Connection string

EF uses the same connection as the app. Set it in **appsettings.json** (or pass it):

```bash
dotnet ef database update --connection "Server=localhost;Port=3306;Database=testdb;User=root;Password=YOUR_PASSWORD;"
```

---

## Summary

| Goal | Command / action |
|------|------------------|
| First-time: create all tables by running app | `dotnet run` (Option 1) |
| **Add a new table** (after adding a new model) | `dotnet ef migrations add AddXxxTable` then `dotnet ef database update` |
| Apply existing migrations | `dotnet ef database update` |

**Adding a new table next time:** Add model → add `DbSet` in `AppDbContext` → `dotnet ef migrations add YourMigrationName` → `dotnet ef database update`.
