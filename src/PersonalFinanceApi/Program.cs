using Scalar.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<FinanceDb>(options =>
    options.UseSqlite("Data Source=finance.db"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FinanceDb>();
    db.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapGet("/hello", () => "Hello from my Personal Finance API!");



app.MapGet("/expenses/{id}", (int id, FinanceDb db) =>
{
    var expense = db.Expenses.Find(id);
    if (expense is null)
    {
        return Results.NotFound();
    }
    return Results.Ok(expense);
});
app.MapPost("/expenses", (Expense expense, FinanceDb db) =>
{
    if (ExpenseValidator.Validate(expense) is string errorMessage)
    {
        return Results.BadRequest(errorMessage);
    }
    db.Expenses.Add(expense);
    db.SaveChanges();
    return Results.Created($"/expenses/{expense.Id}", expense);

});

app.MapDelete("/expenses/{id}", (int id, FinanceDb db) =>
{
    var expense = db.Expenses.Find(id);
    if (expense is null)
    {
        return Results.NotFound();

    }

    db.Expenses.Remove(expense);
    db.SaveChanges();
    return Results.NoContent();

});

app.MapPut("/expenses/{id}", (FinanceDb db, int id, Expense updated) =>
{
    var expense = db.Expenses.Find(id);
    if (expense is null)
    {
        return Results.NotFound();
    }



    if (ExpenseValidator.Validate(updated) is string errorMessage)
    {
        return Results.BadRequest(errorMessage);
    }


    expense.Description = updated.Description;
    expense.Amount = updated.Amount;
    expense.Category = updated.Category;

    db.SaveChanges();
    return Results.Ok(expense);
});
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}


public class Expense
{
    public int Id { get; set; }
    public string Description { get; set; } = "";
    public decimal Amount { get; set; }
    public string Category { get; set; } = "";


}

public static class ExpenseValidator
{
    public static string? Validate(Expense expense)
    {
        if (expense.Amount <= 0)
        {
            return "Amount must be more than 0";
        }
        if (string.IsNullOrWhiteSpace(expense.Description))
        {
            return "Description is required";
        }
        return null;
    }
}

class FinanceDb(DbContextOptions<FinanceDb> options) : DbContext(options)
{
    public DbSet<Expense> Expenses => Set<Expense>();
}
