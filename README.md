# Flower Shop Online Order System

ASP.NET Core MVC application for browsing flower arrangements, placing orders, managing florist inventory, and processing customer orders with role-based access.

## Features

- Customer, Flower, Order, and OrderItem entities backed by EF Core and SQLite.
- Repository and service layers for inventory, customer lookup, order creation, and order status updates.
- AutoMapper profiles for entity/view model mapping.
- Data annotations plus a custom order-items validator.
- ASP.NET Core Identity roles: Administrator, Florist, and Customer.
- Email notification service that logs by default and can be switched to SMTP in configuration.
- Bootstrap-based responsive catalog, order form, inventory, and order management views.
- xUnit tests for order service business rules and repository data access.

## Setup

1. Install .NET 9 SDK or newer.
2. Restore and build:

   ```powershell
   dotnet restore "Flower Shop.slnx"
   dotnet build "Flower Shop.slnx"
   ```

3. Run the MVC app:

   ```powershell
   dotnet run --project "Flower Shop/Flower Shop.csproj"
   ```

4. Open the HTTPS or HTTP URL printed by `dotnet run`.

The app creates `flower-shop.db` automatically on first run and seeds sample flowers, roles, and demo accounts.

## Demo Accounts

| Role | Email | Password |
| --- | --- | --- |
| Administrator | admin@flowershop.local | Admin123! |
| Florist | florist@flowershop.local | Florist123! |
| Customer | customer@flowershop.local | Customer123! |

## User Guide

- Catalog: browse available arrangements and start an order.
- Order: enter customer details, choose quantities, and submit the order.
- Inventory: view flowers; Florist and Administrator accounts can add, edit, or delete inventory.
- Orders: Florist and Administrator accounts can review orders and update order status.
- Email notifications: when SMTP is disabled, notification content is written to application logs. To send real email, set `EmailSettings:EnableSmtp` to `true` and provide SMTP settings in user secrets or environment variables.

## Tests

Run all tests:

```powershell
dotnet test "Flower Shop.slnx"
```

Test coverage includes:

- Order creation calculates totals, decrements inventory, creates customers, and queues confirmation email.
- Order creation rejects insufficient inventory.
- Flower repository returns only in-stock flowers from SQLite-backed data access.

Latest local result: `dotnet test "Flower Shop.slnx" --no-build` passed on 2026-04-27 with 3 passed, 0 failed, 0 skipped.
