using System;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace FolioLibrary
{
    public class FolioServiceContextPool
    {
        private static ConcurrentStack<FolioServiceContext> pool = new ConcurrentStack<FolioServiceContext>();
        public static TimeSpan? Timeout { get; set; } = TimeSpan.FromSeconds(30);
        private readonly static TraceSource traceSource = new TraceSource("FolioLibrary", SourceLevels.All);

        public static FolioServiceContext GetFolioServiceContext()
        {
            if (pool.TryPop(out var fsc)) return fsc;
            traceSource.TraceEvent(TraceEventType.Information, 0, $"pool.Count = {pool.Count}");
            return new FolioServiceContext(timeout: Timeout, pool: true);
        }

        public static void AddFolioServiceContext(FolioServiceContext folioServiceContext)
        {
            pool.Push(folioServiceContext);
            traceSource.TraceEvent(TraceEventType.Information, 0, $"pool.Count = {pool.Count}");
        }
    }
}
