# Flower Shop Online Order System

A professional ASP.NET Core MVC application for browsing flower arrangements, placing customer orders, managing florist inventory, and processing order fulfillment with role-based access.

## Project Snapshot

| Area | Implementation |
| --- | --- |
| Framework | ASP.NET Core MVC on .NET 9 |
| Data | EF Core with SQLite |
| Security | ASP.NET Core Identity with Administrator, Florist, and Customer roles |
| Architecture | Controllers, ViewModels, Services, Repositories, AutoMapper |
| UI | Responsive Bootstrap interface with catalog, ordering, inventory, and order management screens |
| Notifications | Email service abstraction with logging by default and SMTP-ready configuration |
| Tests | xUnit service and repository tests |

## Main Features

- Modern catalog page with product imagery, stock counts, pricing, and quick ordering.
- Guided order form with customer details, quantity controls, and live order summary.
- Inventory dashboard with stock metrics, low-stock highlighting, and inventory value.
- Staff order dashboard with open orders, delivered orders, revenue, and status updates.
- Repository and service layers that keep business logic out of controllers.
- AutoMapper mappings between EF entities and view models.
- Data annotations plus custom order validation.
- Seeded demo users, roles, and starter flower catalog.

## User Roles

| Role | Access |
| --- | --- |
| Customer | Browse catalog and place orders |
| Florist | Manage flower inventory and process orders |
| Administrator | Full inventory and order management access |

## Demo Accounts

| Role | Email | Password |
| --- | --- | --- |
| Administrator | `admin@flowershop.local` | `Admin123!` |
| Florist | `florist@flowershop.local` | `Florist123!` |
| Customer | `customer@flowershop.local` | `Customer123!` |

## Setup

1. Install the .NET 9 SDK or newer.
2. Restore packages:

   ```powershell
   dotnet restore "Flower Shop.slnx"
   ```

3. Build the solution:

   ```powershell
   dotnet build "Flower Shop.slnx"
   ```

4. Run the web app:

   ```powershell
   dotnet run --project "Flower Shop/Flower Shop.csproj"
   ```

5. Open the local URL printed by the terminal.

The first run creates `flower-shop.db`, seeds Identity roles, adds demo users, and inserts starter flowers.

## Workflows

### Customer Order

1. Open the catalog.
2. Select **Start an order**.
3. Enter customer details.
4. Use quantity controls to choose flowers.
5. Review the live total and submit the order.

### Inventory Management

1. Log in as Florist or Administrator.
2. Open **Inventory**.
3. Add, edit, or delete flowers.
4. Use stock counts and low-stock badges to keep inventory current.

### Order Processing

1. Log in as Florist or Administrator.
2. Open **Orders**.
3. Review open orders.
4. Open an order and update its fulfillment status.
5. The email service logs or sends the status update notification.

## Email Configuration

By default, email notifications are written to application logs. To send real email, configure SMTP in `appsettings.json`, user secrets, or environment variables:

```json
"EmailSettings": {
  "EnableSmtp": true,
  "Host": "smtp.example.com",
  "Port": 587,
  "EnableSsl": true,
  "UserName": "smtp-user",
  "Password": "smtp-password",
  "FromAddress": "orders@example.com",
  "FromName": "Flower Shop"
}
```

Do not commit production SMTP credentials.

## Project Structure

```text
Flower Shop/
  Controllers/        MVC request handling
  Data/               EF Core DbContext, role constants, seed data
  Mapping/            AutoMapper profile
  Models/             Domain entities
  Repositories/       Data access interfaces and implementations
  Services/           Business logic and email notifications
  Validation/         Custom validation attributes
  ViewModels/         Strongly typed UI models
  Views/              Razor views for catalog, orders, inventory, shared layout
  wwwroot/            CSS, JavaScript, and client assets
Flower Shop.Tests/    xUnit tests
```

## Testing

Run all tests:

```powershell
dotnet test "Flower Shop.slnx"
```

Current test coverage:

- Order creation calculates totals, decrements inventory, creates customers, and queues confirmation email.
- Order creation rejects insufficient inventory.
- Flower repository returns only in-stock flowers from SQLite-backed data access.

Latest local verification: `dotnet build "Flower Shop.slnx"` succeeded and `dotnet test "Flower Shop.slnx" --no-build` passed with 3 tests on April 28, 2026.

## Troubleshooting

- If the app cannot start because the port is busy, run with a different URL:

  ```powershell
  dotnet run --project "Flower Shop/Flower Shop.csproj" --urls http://localhost:5123
  ```

- If you want a fresh demo database, stop the app and delete `Flower Shop/flower-shop.db`; the app recreates it on the next run.
- If emails do not send, confirm `EmailSettings:EnableSmtp` is `true` and the SMTP host, port, credentials, and SSL settings are correct.
