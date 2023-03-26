namespace QuickToken.Core.Shared.Services.Polling;

public interface ITransactionPollingService
{
    public Task<TResponse?> PollAsync<TResponse>(Guid id, CancellationToken ct) where TResponse : class;
}