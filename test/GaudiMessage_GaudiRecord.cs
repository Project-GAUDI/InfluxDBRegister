namespace IotedgeV2InfluxDBRegister.Test;

[Collection(nameof(CollectionAttribute))]
public class GaudiMessage_GaudiRecord(ITestOutputHelper output) : IDisposable
{
    private readonly ITestOutputHelper _output = output;
    private readonly MultiTextWriter _logOutput = LoggerHelper.SetupMyApplicationMainLogger();

    public void Dispose()
    {
        _logOutput.Dispose();
        GC.SuppressFinalize(this);
    }

    private void AssertLogContains(string expected)
    {
        LoggerHelper.AssertLogContains(_logOutput, _output, expected);
    }

    [Fact(DisplayName = "No.109_異常系：異常動作の確認_RecordHeaderを含まないメッセージを入力")]
    public void Case109()
    {
        JObject inputRouteMsg = JObject.Parse("{\"RecordData\":[\"あいう\"]}");
        var ex = Assert.Throws<Exception>(() => new GaudiMessage.GaudiRecord(inputRouteMsg));

        // 結果確認
        Assert.Contains("RecordHeader Required.", ex.Message);
        AssertLogContains(LoggerHelper.ExitMethod("GaudiRecord"));
    }

    [Fact(DisplayName = "No.110_異常系：異常動作の確認_RecordHeaderが配列以外のメッセージを入力")]
    public void Case110()
    {
        JObject inputRouteMsg = JObject.Parse("{\"RecordHeader\":\"INVALID\",\"RecordData\":[\"あいう\"]}");
        var ex = Assert.Throws<Exception>(() => new GaudiMessage.GaudiRecord(inputRouteMsg));

        // 結果確認
        Assert.Contains("Invalid RecordHeader datatype", ex.Message);
        AssertLogContains(LoggerHelper.ExitMethod("GaudiRecord"));
    }

    [Fact(DisplayName = "No.111_異常系：異常動作の確認_RecordHeaderが配列かつJValue以外の要素を含むメッセージを入力")]
    public void Case111()
    {
        JObject inputRouteMsg = JObject.Parse("{\"RecordHeader\":[{}],\"RecordData\":[\"あいう\"]}");
        var ex = Assert.Throws<Exception>(() => new GaudiMessage.GaudiRecord(inputRouteMsg));
        
        // 結果確認
        Assert.Contains("Invalid RecordHeader value's datatype", ex.Message);
        AssertLogContains(LoggerHelper.ExitMethod("GaudiRecord"));
    }

    [Fact(DisplayName = "No.112_異常系：異常動作の確認_RecordDataを含まないメッセージを入力")]
    public void Case112()
    {
        JObject inputRouteMsg = JObject.Parse("{\"RecordHeader\":[\"ABC\"]}");
        var ex = Assert.Throws<Exception>(() => new GaudiMessage.GaudiRecord(inputRouteMsg));

        // 結果確認
        Assert.Contains("RecordData Required.", ex.Message);
        AssertLogContains(LoggerHelper.ExitMethod("GaudiRecord"));
    }

    [Fact(DisplayName = "No.113_異常系：異常動作の確認_RecordDataが配列以外のメッセージを入力")]
    public void Case113()
    {
        JObject inputRouteMsg = JObject.Parse("{\"RecordHeader\":[\"ABC\"],\"RecordData\":\"INVALID\"}");
        var ex = Assert.Throws<Exception>(() => new GaudiMessage.GaudiRecord(inputRouteMsg));

        // 結果確認
        Assert.Contains("Invalid RecordData datatype", ex.Message);
        AssertLogContains(LoggerHelper.ExitMethod("GaudiRecord"));
    }

    [Fact(DisplayName = "No.114_異常系：異常動作の確認_RecordDataが配列かつJValue以外の要素を含むメッセージを入力")]
    public void Case114()
    {
        JObject inputRouteMsg = JObject.Parse("{\"RecordHeader\":[\"ABC\"],\"RecordData\":[{}]}");
        var ex = Assert.Throws<Exception>(() => new GaudiMessage.GaudiRecord(inputRouteMsg));

        // 結果確認
        Assert.Contains("Invalid RecordData value's datatype", ex.Message);
        AssertLogContains(LoggerHelper.ExitMethod("GaudiRecord"));
    }

    [Fact(DisplayName = "No.115_正常系：正常動作の確認_RecordHeader、RecordDataを含むメッセージを入力")]
    public void Case115()
    {
        JObject inputRouteMsg = JObject.Parse("{\"RecordHeader\":[\"ABC\"],\"RecordData\":[\"あいう\"]}");
        GaudiMessage.GaudiRecord result = new GaudiMessage.GaudiRecord(inputRouteMsg);
        
        // 結果確認        
        Assert.NotNull(result);
        Assert.IsAssignableFrom<GaudiMessage.GaudiRecord>(result);
        Assert.Single(result.RecordHeader);
        Assert.Single(result.RecordData);
        Assert.Equal("ABC", result.RecordHeader[0]);
        Assert.Equal("あいう", result.RecordData[0].ToString());
    }
}
