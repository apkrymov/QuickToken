using QuickToken.Core.Shared.Contracts.Responses;

namespace QuickToken.Core.Shared.Services.Exceptions;

public class TransactionNotFoundException: Exception
{

    public TransactionNotFoundException()
    {
    }

    public TransactionNotFoundException(string message) : base(message)
    {
    }

    public TransactionNotFoundException(string message, Exception inner) : base(message, inner)
    {
    }
}