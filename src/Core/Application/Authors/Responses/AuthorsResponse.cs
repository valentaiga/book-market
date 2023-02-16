namespace Application.Authors.Responses;

public sealed class AuthorsResponse
{
    public Author[] Authors { get; set; } = Array.Empty<Author>();
}