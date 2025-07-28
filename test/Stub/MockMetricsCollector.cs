using InfluxDB.Collector;
using InfluxDB.Collector.Pipeline;

namespace IotedgeV2InfluxDBRegister.Test.Stub;

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
public class MockMetricsCollector : MetricsCollector
{
    private readonly List<WriteMethodArguments> _calledArguments = [];

    protected override void Emit(PointData[] points)
    {
        // 継承が必須のため、宣言のみ実施
    }

    public void StackWriteArguments(string measurement, IReadOnlyDictionary<string, object> fields, IReadOnlyDictionary<string, string>? tags, DateTime? timestamp)
    {
        _calledArguments.Add(WriteMethodArguments.Parse(measurement, fields, tags, timestamp));
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
                Assert.Equal(expected.Timestamp, actual.Timestamp);
        }

        _calledArguments.Clear();
    }
}
