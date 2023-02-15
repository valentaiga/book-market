namespace Domain.Exceptions.Architecture;

public class ValidationException : Exception
{
    public Dictionary<string, string[]> Errors { get; }

    public ValidationException(string message, Dictionary<string, string[]> errors) : base(message) 
        => Errors = errors;
}