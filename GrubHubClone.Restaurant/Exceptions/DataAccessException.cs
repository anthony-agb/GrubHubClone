namespace GrubHubClone.Restaurant.Exceptions;

public class DataAccessException : Exception
{
    public DataAccessException(string? message) : base(message) { }
    public DataAccessException(string? message, Exception? exception) : base(message, exception) { }
}
