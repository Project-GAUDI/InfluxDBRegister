using HarmonyLib;
using InfluxDB.Collector;
using InfluxDBRegister.Test.Stub;
using IotedgeV2InfluxDBRegister;
using IotedgeV2InfluxDBRegister.Test;

namespace InfluxDBRegister.Test;

[Collection(nameof(CollectionAttribute))]
public abstract class MetricsCollectorTesterBase : IDisposable
{
    protected readonly ITestOutputHelper _output;

    protected readonly MockMetricsCollector _collector = null!;

    protected readonly MultiTextWriter _logOutput;

    private bool _disposed = false;

    public MetricsCollectorTesterBase(ITestOutputHelper output) {
        _output = output;
        _logOutput = LoggerHelper.SetupMyApplicationMainLogger();

        try
        {
            // Harmonyパッチを適用
            var harmony = new Harmony(Guid.NewGuid().ToString());
            harmony.PatchAll();

            // MockMetricsCollectorを初期化（Harmonyパッチ後）
            _collector = CreateMockCollector();
            if (_collector == null)
                throw new InvalidOperationException("CreateMockCollector returned null");
            
            SetMetricsCollector();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"MetricsCollectorTesterBase constructor failed: {ex.Message}", ex);
        }
    }

    private MockMetricsCollector CreateMockCollector()
    {
        try
        {
            // 直接インスタンスを作成
            var collector = new MockMetricsCollector();
            if (collector == null)
                throw new InvalidOperationException("MockMetricsCollector constructor returned null");
            return collector;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to create MockMetricsCollector: {ex.Message}", ex);
        }
    }

    private static PropertyInfo GetMetricsCollector()
    {
        var t = typeof(SQLParser);
        var pi = t.GetProperty("MetricsCollector", BindingFlags.Static | BindingFlags.NonPublic);
        return pi!;
    }

    private void SetMetricsCollector()
    {
        var pi = GetMetricsCollector();
        if (pi == null)
        {
            throw new InvalidOperationException("PropertyInfo for metricsCollector is null");
        }
        if (_collector == null)
        {
            throw new InvalidOperationException("_collector is null. CreateMockCollector() failed to initialize collector.");
        }
        // MockMetricsCollectorを暗黙的にMetricsCollectorにキャスト
        MetricsCollector metricsCollector = _collector;
        pi.SetValue(null, metricsCollector);
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
            _logOutput.Dispose();
        }

        _disposed = true;
    }

    protected void AssertLogContains(string expected)
    {
        LoggerHelper.AssertLogContains(_logOutput, _output, expected);
    }
}
