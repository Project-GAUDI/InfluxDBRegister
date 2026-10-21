using InfluxDB.Collector;
using InfluxDB.Collector.Pipeline;
using InfluxDB.Collector.Configuration;
using System.Reflection;
using IotedgeV2InfluxDBRegister.Test.Stub;

namespace InfluxDBRegister.Test.Stub;

/// <summary>
/// MetricsCollectorのモッククラス
/// </summary>
/// <remarks>
/// MetricsCollectorの基底挙動を利用したクラスであることから、改修の際は
/// <see href="https://github.com/influxdata/influxdb-csharp/blob/dev/src/InfluxDB.Collector/MetricsCollector.cs">
/// GitHub
/// </see>
/// を参照すること
/// </remarks>
public class MockMetricsCollector : IDisposable
{
    private readonly List<WriteMethodArguments> _calledArguments = [];
    private readonly MetricsCollector _realCollector;
    private static MockMetricsCollector? _instance;

    /// <summary>  
    /// MockMetricsCollectorのコンストラクタ
    /// </summary>
    public MockMetricsCollector()
    {
        // 実際のMetricsCollectorを作成（テスト用のダミー設定で）
        var config = new CollectorConfiguration();
        config.WriteTo.InfluxDB("http://localhost:8086", "testdb");
        _realCollector = config.CreateCollector();
        
        // シングルトンパターンでインスタンスを管理
        _instance = this;
    }

    /// <summary>
    /// 現在のインスタンスを取得
    /// </summary>
    public static MockMetricsCollector? Current => _instance;

    /// <summary>
    /// MetricsCollectorとしてキャストするためのオペレータ
    /// </summary>
    public static implicit operator MetricsCollector(MockMetricsCollector mock)
    {
        return mock._realCollector;
    }









    public void Emit(PointData[] points)
    {
        // テスト用のEmit実装
    }

    /// <summary>
    /// WriteメソッドをMetricsCollectorに転送
    /// </summary>
    public void Write(string measurement, IReadOnlyDictionary<string, object> fields, IReadOnlyDictionary<string, string>? tags, DateTime? timestamp)
    {
        // 引数をテスト用に保存
        StackWriteArguments(measurement, fields, tags, timestamp);
        
        // 実際のMetricsCollectorには送信しない（テスト環境では不要）
    }

    public void StackWriteArguments(string measurement, IReadOnlyDictionary<string, object> fields, IReadOnlyDictionary<string, string>? tags, DateTime? timestamp)
    {
        _calledArguments.Add(WriteMethodArguments.Parse(measurement, fields, tags, timestamp));
    }

    public void ReviewCalledArguments()
    {
        Assert.Empty(_calledArguments);
    }

    public void ReviewCalledArguments(WriteMethodArguments expected)
    {
        Assert.Single(_calledArguments);
        ReviewCalledArguments([expected]);
    }

    public void ReviewCalledArguments(List<WriteMethodArguments> expectedList)
    {
        Assert.Equal(expectedList.Count, _calledArguments.Count);
        for (var i = 0; i < expectedList.Count; ++i)
        {
            var expected = expectedList[i];
            var actual = _calledArguments[i];

            Assert.Equal(expected.Measurement, actual.Measurement);
            Assert.Equal(JsonConvert.SerializeObject(expected.Fields), JsonConvert.SerializeObject(actual.Fields));
            Assert.Equal(JsonConvert.SerializeObject(expected.Tags), JsonConvert.SerializeObject(actual.Tags));
            if (expected.Timestamp == null)
                Assert.Null(actual.Timestamp);
            else
            {
                // タイムゾーンを統一してUTCで比較
                var expectedUtc = expected.Timestamp?.ToUniversalTime();
                var actualUtc = actual.Timestamp?.ToUniversalTime();
                Assert.Equal(expectedUtc, actualUtc);
            }
        }

        _calledArguments.Clear();
    }

    /// <summary>
    /// IDisposableの実装
    /// </summary>
    public void Dispose()
    {
        _realCollector?.Dispose();
        _instance = null;
    }
}
