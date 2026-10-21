namespace IotedgeV2InfluxDBRegister.Test;

[Collection(nameof(CollectionAttribute))]
public class MyApplicationMain_DisposeAsync(ITestOutputHelper output) : IDisposable
{
    private readonly ITestOutputHelper _output = output;
    private readonly MyApplicationMain _app = new ();
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

    [Fact(DisplayName = "No.1_正常系：正常動作の確認_リソース解放を1回実行する")]
    public async Task Case001()
    {
        var app = new MyApplicationMain();
        
        // DisposeAsyncが例外なく完了することを検証
        var exception = await Record.ExceptionAsync(async () => await app.DisposeAsync());
        Assert.Null(exception);
        
        // _disposedフィールドが1に設定されていることを検証
        var disposedField = typeof(MyApplicationMain).GetField("_disposed", BindingFlags.NonPublic | BindingFlags.Instance);
        var disposedValue = disposedField?.GetValue(app);
        Assert.Equal(1, disposedValue);
        
        // ログ確認
        AssertLogContains(LoggerHelper.EndMethod("DisposeAsync"));
    }

    [Fact(DisplayName = "No.2_正常系：正常動作の確認_リソース解放を2回実行する")]
    public async Task Case002()
    {
        var app = new MyApplicationMain();
        
        // 1回目の呼び出し
        await app.DisposeAsync();
        
        // _disposedフィールドが1に設定されていることを検証
        var disposedField = typeof(MyApplicationMain).GetField("_disposed", BindingFlags.NonPublic | BindingFlags.Instance);
        var disposedValue = disposedField?.GetValue(app);
        Assert.Equal(1, disposedValue);
        
        // 1回目のログ確認
        AssertLogContains(LoggerHelper.EndMethod("DisposeAsync"));
        
        // 2回目以降の呼び出しも例外なく完了することを検証
        var exception2 = await Record.ExceptionAsync(async () => await app.DisposeAsync());
        Assert.Null(exception2);
        
        var exception3 = await Record.ExceptionAsync(async () => await app.DisposeAsync());
        Assert.Null(exception3);
        
        // _disposedフィールドが依然として1であることを検証
        disposedValue = disposedField?.GetValue(app);
        Assert.Equal(1, disposedValue);
        
        // 2回目以降はExit Methodが出力される
        Assert.Contains("Exit Method: Already disposed, skipping DisposeAsync", _logOutput.GetStringWriterContent());
    }
}
