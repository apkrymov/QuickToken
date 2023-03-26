using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using QuickToken.Database.Repositories;
using QuickToken.Shared.Worker.Options;

namespace QuickToken.Shared.Worker;

public abstract class AbstractWorker<TWork> : BackgroundService
{
    protected readonly ILogger<AbstractWorker<TWork>> Logger;
    private readonly BaseWorkerOptions _options;

    public AbstractWorker(ILogger<AbstractWorker<TWork>> logger, BaseWorkerOptions options)
    {
        _options = options;
        Logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var tworks = await GetWorkAsync(new Paging(count: _options.PrefetchCount), stoppingToken);

            if (tworks.Length == 0)
            {
                Logger.LogInformation("Empty batch, delay on {DelayOnEmptyBatchMs}",
                    _options.DelayOnEmptyBatchMs);
                await Task.Delay(_options.DelayOnEmptyBatchMs, stoppingToken);
            }
            else
            {
                await Parallel.ForEachAsync(tworks, new ParallelOptions
                    {
                        MaxDegreeOfParallelism = _options.MaxDegreeOfParallelism,
                        CancellationToken = stoppingToken
                    },
                    async (transaction, ct) => await ProcessWorkAsync(transaction, ct));
            }
        }
    }
    
    protected abstract Task<TWork[]> GetWorkAsync(Paging paging, CancellationToken ct);
    
    protected abstract Task ProcessWorkAsync(TWork transaction, CancellationToken ct);
}