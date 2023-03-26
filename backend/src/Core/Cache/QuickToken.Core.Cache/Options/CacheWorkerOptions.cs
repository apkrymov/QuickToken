using QuickToken.Shared.Worker.Options;

namespace QuickToken.Core.Cache.Options;

public class CacheWorkerOptions : BaseWorkerOptions
{
    public TimeSpan CacheExpirationPeriod { get; set; }
}