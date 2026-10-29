using Scalar.AspNetCore;
using Microsoft.EntityFrameworkCore;
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
    db.Expenses.Add(expense);
    db.SaveChanges();
    return Results.Created($"/expenses/{expense.Id}", expense);

});
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

record Expense(int Id, string Description, decimal Amount, string Category);

class FinanceDb(DbContextOptions<FinanceDb> options) : DbContext(options)
{
    public DbSet<Expense> Expenses => Set<Expense>();
}
