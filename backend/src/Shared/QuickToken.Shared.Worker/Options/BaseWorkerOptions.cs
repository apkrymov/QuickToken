namespace QuickToken.Shared.Worker.Options;

public class BaseWorkerOptions
{
    public int PrefetchCount { get; set; }

    public int MaxDegreeOfParallelism { get; set; }

    public TimeSpan DelayOnEmptyBatchMs { get; set; }
}