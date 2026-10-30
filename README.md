# Personal Finance API

A backend API for tracking personal expenses, built with **C# and ASP.NET Core (.NET 10)**.

Part of my backend portfolio for the financial sector.

## Features
- `GET /expenses` — list all expenses
- `POST /expenses` — add a new expense
- Interactive API docs and testing page (Scalar)
- `GET /expenses/{id}` — get one expense by its ID (404 if not found)
- Data saved in a SQLite database (EF Core), so it survives restarts
- `PUT /expenses/{id}` — edit an expense
- `DELETE /expenses/{id}` — delete an expense
-    - Validation: rejects amounts of 0 or less and empty descriptions (400 Bad Request)
## Tech
- C# / ASP.NET Core Minimal APIs
- `decimal` for all money values (exact to the cent)
- Git feature-branch workflow

## How to run
```
cd src/PersonalFinanceApi
dotnet run
```
Then open `http://localhost:5227/scalar` to see and test the API.

## Coming next
- Tests (xUnit)
- User logins (JWT)