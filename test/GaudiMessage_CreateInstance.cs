using TICO.GAUDI.Commons;

namespace IotedgeV2InfluxDBRegister.Test;

[Collection(nameof(CollectionAttribute))]
public class GaudiMessage_CreateInstance
{
    private readonly ITestOutputHelper _output;
    private readonly MultiTextWriter _logWriter;

    public GaudiMessage_CreateInstance(ITestOutputHelper output)
    {
        _output = output;
        // GaudiMessageのMyLoggerをキャプチャ設定
        var logger = (ILogger)typeof(GaudiMessage).GetProperty("MyLogger", BindingFlags.Static | BindingFlags.NonPublic)!.GetValue(null)!;
        var stringWriter = new StringWriter();
        var consoleWriter = Console.Out;
        var multiWriter = new MultiTextWriter(consoleWriter, stringWriter);
        logger.SetOutputLogLevel(ILogger.LogLevel.TRACE.ToString());
        logger.SetOutputTextWriter(multiWriter);
        _logWriter = multiWriter;
    }

    private void AssertLogContains(string expected)
    {
        LoggerHelper.AssertLogContains(_logWriter, _output, expected);
    }

    [Fact(DisplayName = "No.116_異常系：異常動作の確認_RecordListを含まないメッセージを入力")]
    public void Case116()
    {
        var result = GaudiMessage.CreateInstance("{}");
        
        // 結果確認
        Assert.Null(result);
        AssertLogContains(LoggerHelper.EndMethod("CreateInstance"));
    }

    [Fact(DisplayName = "No.117_異常系：異常動作の確認_RecordListが配列ではないメッセージを入力")]
    public void Case117()
    {
        var inputRouteMsg = "{\"RecordList\":\"INVALID\"}";
        var result = GaudiMessage.CreateInstance(inputRouteMsg);
        
        // 結果確認
        Assert.Null(result);
        AssertLogContains(LoggerHelper.EndMethod("CreateInstance"));
    }

    [Fact(DisplayName = "No.118_正常系：正常動作の確認_GAUDI標準形式のメッセージを入力")]
    public void Case118()
    {
        string inputRouteMsg = "{\"RecordList\":[{\"RecordHeader\":[\"ABC\"],\"RecordData\":[\"あいう\"]}]}";
        GaudiMessage result = GaudiMessage.CreateInstance(inputRouteMsg);

        // 結果確認
        Assert.NotNull(result);
        Assert.IsAssignableFrom<GaudiMessage>(result);
        Assert.Single(result.RecordList);
        Assert.Equal("ABC", result.RecordList[0].RecordHeader[0]);
        Assert.Equal("あいう", result.RecordList[0].RecordData[0].ToString());
        AssertLogContains(LoggerHelper.EndMethod("CreateInstance"));
    }
}
