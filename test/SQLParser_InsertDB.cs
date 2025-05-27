using IotedgeV2InfluxDBRegister.Test.Stub;

namespace IotedgeV2InfluxDBRegister.Test;

[Collection(nameof(CollectionAttribute))]
public class SQLParser_InsertDB(ITestOutputHelper output): MetricsCollectorTesterBase(output)
{
    [Fact(DisplayName = "No.129, 130 引数をすべて指定した状態で正常に処理が終了すること。")]
    public void Case129()
    {
        var measurement = Guid.NewGuid().ToString();
        var fields = new Dictionary<string, object>();
        Enumerable.Range(1, 5).ToList().ForEach(i => fields.Add(i.ToString(), Guid.NewGuid().ToString()));
        var tags = new Dictionary<string, string>();
        Enumerable.Range(1, 5).ToList().ForEach(i => tags.Add($"tag-{i}", Guid.NewGuid().ToString()));
        var datetime = DateTime.UtcNow;

        _output.WriteLine($"{nameof(measurement)}: {measurement}");
        _output.WriteLine($"{nameof(fields)}: {JsonConvert.SerializeObject(fields)}");
        _output.WriteLine($"{nameof(tags)}: {JsonConvert.SerializeObject(tags)}");
        _output.WriteLine($"{nameof(datetime)}: {datetime:o}");

        SQLParser.InsertDB(measurement, fields, tags, datetime);

        _collector.ReviewCalledArguments(WriteMethodArguments.Parse(measurement, fields, tags, datetime));
    }
}
