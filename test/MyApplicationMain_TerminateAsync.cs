namespace IotedgeV2InfluxDBRegister.Test;

[Collection(nameof(CollectionAttribute))]
public class MyApplicationMain_TerminateAsync
{
    private readonly MyApplicationMain _app = new ();
    private readonly ITestOutputHelper _output;
    private readonly MultiTextWriter _logWriter;

    public MyApplicationMain_TerminateAsync(ITestOutputHelper output)
    {
        _output = output;
        _logWriter = LoggerHelper.SetupMyApplicationMainLogger();
    }

    private void AssertLogContains(string expected)
    {
        LoggerHelper.AssertLogContains(_logWriter, _output, expected);
    }

    [Fact(DisplayName = "No.21_正常系：正常動作の確認_正常終了する")]
    public async Task Case021()
    {
        var result = await _app.TerminateAsync();

        // 結果確認
        Assert.True(result);
        AssertLogContains(LoggerHelper.EndMethod("TerminateAsync"));
    }
}
