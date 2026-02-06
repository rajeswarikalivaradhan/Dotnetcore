# Migrations – Step by Step (Create Orders Table in MySQL)

Follow these steps to create the **Orders** table (and any other new tables) on your MySQL server using EF Core migrations.

---

## Prerequisites

- MySQL server running (e.g. localhost).
- Connection string set in **appsettings.json** under `ConnectionStrings:DefaultConnection`.
- Project builds: `dotnet build` succeeds.

---

## Step 1: Install EF Core tools (one time only)

The tool version must match your project’s EF Core (8.0.x). In PowerShell or CMD:

```bash
dotnet tool install --global dotnet-ef --version 8.0.11
```

If you already have a different version (e.g. 9.x), uninstall then install 8.0.11:

```bash
dotnet tool uninstall --global dotnet-ef
dotnet tool install --global dotnet-ef --version 8.0.11
```

Check it works:

```bash
dotnet ef --version
```

You should see **8.0.11** (matching the project). Using a different major version (e.g. 9.x) can cause `TypeLoadException` when adding migrations.

---

## Step 2: Go to the project folder

```bash
cd e:\FFM\WebApi
```

(Use your actual project path if different.)

---

## Step 3: Create a migration from your models

This generates a migration that includes the new **Orders** table (and any other model changes).

```bash
dotnet ef migrations add AddOrdersTable
```

- **AddOrdersTable** is the migration name. You can use any name, e.g. `AddOrdersTable`, `InitialCreate` (if first time), etc.

You should see:

- A **Migrations** folder created (if first time).
- New files, e.g.:
  - `Migrations/20250206120000_AddOrdersTable.cs`
  - `Migrations/20250206120000_AddOrdersTable.Designer.cs`
  - `Migrations/AppDbContextModelSnapshot.cs` (or updated)

**If you get:** “No DbContext found” → run the command from the folder that contains the `.csproj` (e.g. `e:\FFM\WebApi`).

**If you get:** “dotnet-ef not found” → run Step 1 again and restart the terminal.

---

## Step 4: Apply the migration to MySQL (create/update tables)

This creates the database (if missing) and the **Orders** table (and any other pending changes) on MySQL.

```bash
dotnet ef database update
```

- EF uses the connection string from **appsettings.json** (for the environment you’re running under, e.g. Development).
- You should see output like: “Applying migration '...AddOrdersTable'” and “Done.”

**Optional – use a specific connection string:**

```bash
dotnet ef database update --connection "Server=localhost;Port=3306;Database=testdb;User=root;Password=YourPassword;"
```

---

## Step 5: Check that the Orders table exists in MySQL

**Option A – MySQL Workbench**

1. Connect to your MySQL server.
2. Select your database (e.g. `testdb`).
3. In the left panel, expand **Tables**.
4. You should see **Orders** (and **__EFMigrationsHistory** if this was the first migration).

**Option B – MySQL command line**

```bash
mysql -u root -p -e "USE testdb; SHOW TABLES;"
```

You should see `Orders` in the list.

**Option C – Run the API and call Orders**

1. Run the API: `dotnet run`
2. Call `GET /api/orders` (with a valid JWT if required).  
   If it returns 200 (even with an empty list), the **Orders** table is in use.

---

## Step 6: If you add more models later

Whenever you add a **new model** (new table) or change existing entities:

1. Add the entity class (e.g. `Models/OrderItem.cs`).
2. Add `DbSet<OrderItem>` in **AppDbContext**.
3. Create a new migration:
   ```bash
   dotnet ef migrations add AddOrderItemsTable
   ```
4. Apply it:
   ```bash
   dotnet ef database update
   ```

---

## Quick reference

| Step | Command / action |
|------|-------------------|
| 1 – Install tools (once) | `dotnet tool install --global dotnet-ef` |
| 2 – Open project folder | `cd e:\FFM\WebApi` |
| 3 – Create migration | `dotnet ef migrations add AddOrdersTable` |
| 4 – Create/update tables in MySQL | `dotnet ef database update` |
| 5 – Verify | Check **Orders** table in MySQL or call `GET /api/orders` |

---

## Troubleshooting

- **`TypeLoadException` / “Method 'Identifier' does not have an implementation”:** Version mismatch between the global `dotnet-ef` tool and the project’s EF Core. Install the matching tool: `dotnet tool uninstall --global dotnet-ef` then `dotnet tool install --global dotnet-ef --version 8.0.11`. The project uses EF Core 8 and Pomelo 8.0.2; the tool must be 8.0.x.
- **Connection errors:** Check `appsettings.json` (or `appsettings.Development.json`) and that MySQL is running.
- **“Build failed”:** Run `dotnet build` and fix any compile errors before running `dotnet ef`.
- **“Pending model changes”:** You changed the model after the last migration; run `dotnet ef migrations add YourMigrationName` again, then `dotnet ef database update`.
