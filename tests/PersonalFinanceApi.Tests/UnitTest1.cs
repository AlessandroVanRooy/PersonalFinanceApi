namespace PersonalFinanceApi.Tests;

public class ExpenseValidatorTests
{
    [Fact]
    public void Negative_amount_is_rejected()
    {
        var expense = new Expense { Description = "Taxi", Amount = -5, Category = "Transport" };

        var error = ExpenseValidator.Validate(expense);

        Assert.Equal("Amount must be more than 0", error);
    }


    [Fact]
    public void Empty_description_is_rejected()
    {
        var expense = new Expense { Description = "", Amount = 5, Category = "Transport" };

        var error = ExpenseValidator.Validate(expense);

        Assert.Equal("Description is required", error);
    }

    [Fact]
    public void Valid_expense_is_accepted()
    {
        var expense = new Expense { Description = "Taxi", Amount = 5, Category = "Transport" };

        var error = ExpenseValidator.Validate(expense);

        Assert.Null(error);
    }
}

