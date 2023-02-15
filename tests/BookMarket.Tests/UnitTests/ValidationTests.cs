using Application.Books.Commands.CreateBook;
using Xunit;
using FluentValidation.TestHelper;
using static System.Guid;

namespace BookMarket.Tests.UnitTests;

public class ValidationTests
{
    public ValidationTests()
    {
        
    }
    
    [Theory]
    [InlineData("title", "desc", "2022-02-01", 321, "en", "3fa85f64-5717-4562-b3fc-2c963f66afa6", true)]
    [InlineData(null, "desc", "2022-02-01", 321, "en", "3fa85f64-5717-4562-b3fc-2c963f66afa6", false)]
    [InlineData("title", "", "2022-02-01", 321, "en", "3fa85f64-5717-4562-b3fc-2c963f66afa6", false)]
    [InlineData("title", "desc", "0001-01-01", 321, "en", "3fa85f64-5717-4562-b3fc-2c963f66afa6", false)]
    [InlineData("title", "desc", "2022-02-01", 0, "en", "3fa85f64-5717-4562-b3fc-2c963f66afa6", false)]
    [InlineData("title", "desc", "2022-02-01", 321, "", "3fa85f64-5717-4562-b3fc-2c963f66afa6", false)]
    [InlineData("title", "desc", "2022-02-01", 321, "en", "00000000-0000-0000-0000-000000000000", false)]
    public void CreateBookCommand(string title, string description, string pDate, short pages, string lang, string author, bool expectedResult)
    {
        DateTime.TryParse(pDate, out var publishDate);
        Guid.TryParse(author, out var authorId);

        var r = new CreateBookCommand(
            title,
            description,
            publishDate,
            pages,
            lang,
            authorId);

        var validator = new CreateBookCommandValidator();

        var result = validator.TestValidate(r);
        Assert.Equal(expectedResult, result.IsValid);
    }
}