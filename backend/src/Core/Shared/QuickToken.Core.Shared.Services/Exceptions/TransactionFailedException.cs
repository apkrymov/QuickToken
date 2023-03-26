using QuickToken.Core.Shared.Contracts.Responses;

namespace QuickToken.Core.Shared.Services.Exceptions;

public class TransactionFailedException : Exception
{
    public ErrorResponse Response { get; }

    public TransactionFailedException()
    {
    }

    public TransactionFailedException(string message) : base(message)
    {
    }

    public TransactionFailedException(string message, Exception inner) : base(message, inner)
    {
    }
    
    public TransactionFailedException(string message, ErrorResponse response) : base(message)
    {
        Response = response;
    }
}