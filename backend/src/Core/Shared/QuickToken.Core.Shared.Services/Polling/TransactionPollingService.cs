using Microsoft.Extensions.Logging;

namespace QuickToken.Core.Shared.Services.Polling;

public class TransactionPollingService : ITransactionPollingService
{
    private readonly TransactionPollingOptions _options;
    private readonly IBlockchainService _blockchain;

    private readonly ILogger<TransactionPollingService> _logger;

    public TransactionPollingService(IBlockchainService blockchain, TransactionPollingOptions options,
        ILogger<TransactionPollingService> logger)
    {
        _blockchain = blockchain;
        _options = options;
        _logger = logger;
    }

    public async Task<TResponse?> PollAsync<TResponse>(Guid id, CancellationToken ct) where TResponse : class
    {
        while (true)
        {
            await Task.Delay(_options.Interval, ct);
            var (isCompleted, result) = await _blockchain.GetTransactionAsync(id, ct);
            if (isCompleted)
            {
                return result as TResponse;
            }

            _logger.LogInformation("Awaiting transaction {TransactionId} to complete", id);
        }
    }
}