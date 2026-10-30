# Personal Finance API

A backend API for tracking personal expenses, built with **C# and ASP.NET Core (.NET 10)**.

Part of my backend portfolio for the financial sector.

## Features
- `GET /expenses` — list all expenses
- `GET /expenses/{id}` — get one expense by its ID (404 if not found)
- `POST /expenses` — add a new expense
- `PUT /expenses/{id}` — edit an expense
- `DELETE /expenses/{id}` — delete an expense
- Validation: rejects amounts of 0 or less and empty descriptions (400 Bad Request)
- Data saved in a SQLite database (EF Core), so it survives restarts
- Unit tests (xUnit) for the expense validation rules
- Interactive API docs and testing page (Scalar)

## Tech
- C# / ASP.NET Core Minimal APIs
- EF Core + SQLite (database)
- xUnit (unit tests)
- `decimal` for all money values (exact to the cent)
- Git feature-branch workflow

## How to run
```
cd src/PersonalFinanceApi
dotnet run
```
Then open `http://localhost:5227/scalar` to see and test the API.

## Run the tests
```
dotnet test tests/PersonalFinanceApi.Tests
```

## Coming next
- User logins (JWT)