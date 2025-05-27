using HarmonyLib;
using IotedgeV2InfluxDBRegister.Test.Stub;

namespace IotedgeV2InfluxDBRegister.Test;

[Collection(nameof(CollectionAttribute))]
public abstract class MetricsCollectorTesterBase : IDisposable
{
    protected readonly ITestOutputHelper _output;

    protected readonly MockMetricsCollector _collector = new ();

    private bool _disposed = false;

    public MetricsCollectorTesterBase(ITestOutputHelper output) {
        _output = output;

        var harmony = new Harmony(Guid.NewGuid().ToString());
        harmony.PatchAll();

        SetMetricsCollector();
    }

    private static PropertyInfo GetMetricsCollector()
    {
        var t = typeof(SQLParser);
        var pi = t.GetProperty("metricsCollector", BindingFlags.Static | BindingFlags.NonPublic);
        return pi!;
    }

    private void SetMetricsCollector()
    {
        var pi = GetMetricsCollector();
        pi.SetValue(null, _collector);
    }

    private static void ReleaseMetricsCollector()
    {
        var pi = GetMetricsCollector();
        pi.SetValue(null, null);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            ReleaseMetricsCollector();
            _collector.Dispose();
        }

        _disposed = true;
    }
}
