using HarmonyLib;

namespace IotedgeV2InfluxDBRegister.Test;

[Collection(nameof(CollectionAttribute))]
public class MyApplicationMain_InitializeAsync : IDisposable
{
    private readonly MyApplicationMain _app = new ();
    private readonly Harmony _harmony;
    private readonly ITestOutputHelper _output;
    private readonly MultiTextWriter _logWriter;

    public MyApplicationMain_InitializeAsync(ITestOutputHelper output)
    {
        _output = output;
        _harmony = new Harmony("MyApplicationMain_InitializeAsync_Test");
        
        // SQLParser.InitParserAsyncをモック化
        var originalMethod = typeof(SQLParser).GetMethod("InitParserAsync", BindingFlags.Public | BindingFlags.Static);
        var prefixMethod = typeof(MyApplicationMain_InitializeAsync).GetMethod("MockInitParserAsyncPrefix", BindingFlags.NonPublic | BindingFlags.Static);
        _harmony.Patch(originalMethod, new HarmonyMethod(prefixMethod));
        
        // テスト実行前に環境変数をクリア
        ClearEnvironmentVariables();

        // ログキャプチャ設定
        _logWriter = LoggerHelper.SetupMyApplicationMainLogger();
    }

    public void Dispose()
    {
        _harmony.UnpatchAll("MyApplicationMain_InitializeAsync_Test");
        ClearEnvironmentVariables();
    }

    private void ClearEnvironmentVariables()
    {
        Environment.SetEnvironmentVariable("DBName", null);
        Environment.SetEnvironmentVariable("URI", null);
        Environment.SetEnvironmentVariable("Interval", null);
    }

    private static bool MockInitParserAsyncPrefix(ref Task __result)
    {
        __result = Task.CompletedTask;
        return false;
    }

    private void AssertLogContains(string expected)
    {
        LoggerHelper.AssertLogContains(_logWriter, _output, expected);
    }

    [Fact(DisplayName = "No.7_異常系：必須項目の確認_\"DBName\"が存在しない")]
    public async Task Case007()
    {
        // 未設定
        // Environment.SetEnvironmentVariable("DBName", "dummy-db");
        Environment.SetEnvironmentVariable("URI", "https://dummy-sv.net");
        Environment.SetEnvironmentVariable("Interval", "10");

        var result = await _app.InitializeAsync();

        // 結果確認
        Assert.False(result);
        AssertLogContains(LoggerHelper.RequiredEnvVarNotSpecified("DBName"));
    }

    [Fact(DisplayName = "No.9_異常系：不正値の確認（string型）_\"DBName\"が空文字")]
    public async Task Case009()
    {
        // 空文字
        Environment.SetEnvironmentVariable("DBName", "");
        Environment.SetEnvironmentVariable("URI", "https://dummy-sv.net");
        Environment.SetEnvironmentVariable("Interval", "10");

        var result = await _app.InitializeAsync();

        // 結果確認
        Assert.False(result);
        AssertLogContains(LoggerHelper.RequiredEnvVarNotSpecified("DBName"));
    }

    [Fact(DisplayName = "No.10_正常系：正常値の確認（string型）_\"DBName\"がnull/空文字以外")]
    public async Task Case010()
    {
        Environment.SetEnvironmentVariable("DBName", "dummy-db");
        Environment.SetEnvironmentVariable("URI", "https://dummy-sv.net");
        Environment.SetEnvironmentVariable("Interval", "10");

        var result = await _app.InitializeAsync();

        // 結果確認
        Assert.True(result);
        AssertLogContains(LoggerHelper.EndMethod("InitializeAsync"));
    }

    [Fact(DisplayName = "No.11_異常系：必須項目の確認_\"URI\"が存在しない")]
    public async Task Case011()
    {
        Environment.SetEnvironmentVariable("DBName", "dummy-db");
        // 未設定
        // Environment.SetEnvironmentVariable("URI", "https://dummy-sv.net");
        Environment.SetEnvironmentVariable("Interval", "10");

        var result = await _app.InitializeAsync();
        Assert.False(result);
        AssertLogContains(LoggerHelper.RequiredEnvVarNotSpecified("URI"));
    }

    [Fact(DisplayName = "No.13_異常系：不正値の確認（string型）_\"URI\"が空文字")]
    public async Task Case013()
    {
        Environment.SetEnvironmentVariable("DBName", "dummy-db");
        // 空文字
        Environment.SetEnvironmentVariable("URI", "");
        Environment.SetEnvironmentVariable("Interval", "10");

        var result = await _app.InitializeAsync();

        // 結果確認
        Assert.False(result);
        AssertLogContains(LoggerHelper.RequiredEnvVarNotSpecified("URI"));
    }

    [Fact(DisplayName = "No.14_正常系：正常値の確認（string型）_\"URI\"がnull/空文字以外")]
    public async Task Case014()
    {
        Environment.SetEnvironmentVariable("DBName", "dummy-db");
        Environment.SetEnvironmentVariable("URI", "https://dummy-sv.net");
        Environment.SetEnvironmentVariable("Interval", "10");

        var result = await _app.InitializeAsync();
        Assert.True(result);
        AssertLogContains(LoggerHelper.EndMethod("InitializeAsync"));
    }

    [Fact(DisplayName = "No.15_異常系：必須項目の確認_\"Interval\"が存在しない")]
    public async Task Case015()
    {
        Environment.SetEnvironmentVariable("DBName", "dummy-db");
        Environment.SetEnvironmentVariable("URI", "https://dummy-sv.net");
        // 未設定
        // Environment.SetEnvironmentVariable("Interval", "10");

        var result = await _app.InitializeAsync();

        // 結果確認
        Assert.False(result);
        AssertLogContains(LoggerHelper.RequiredEnvVarNotSpecified("Interval"));
    }

    [Fact(DisplayName = "No.16_異常系：不正値の確認（number型）_\"Interval\"が数値以外")]
    public async Task Case016()
    {
        Environment.SetEnvironmentVariable("DBName", "dummy-db");
        Environment.SetEnvironmentVariable("URI", "https://dummy-sv.net");
        // 不正値
        Environment.SetEnvironmentVariable("Interval", "ERROR_VALUE");

        var result = await _app.InitializeAsync();

        // 結果確認
        Assert.False(result);
        AssertLogContains(LoggerHelper.ErrorRetrievingEnvVar("Interval"));
    }

    [Fact(DisplayName = "No.17_異常系：不正値の確認（number型）_\"Interval\"が0未満の数値")]
    public async Task Case017()
    {
        Environment.SetEnvironmentVariable("DBName", "dummy-db");
        Environment.SetEnvironmentVariable("URI", "https://dummy-sv.net");
        // 不正値
        Environment.SetEnvironmentVariable("Interval", "-1");

        var result = await _app.InitializeAsync();

        // 結果確認
        Assert.False(result);
        AssertLogContains(LoggerHelper.EnvVarConditionNotMet("Interval"));
    }

    [Fact(DisplayName = "No.18_正常系：正常値の確認（number型）_\"Interval\"が0以上の数値")]
    public async Task Case018()
    {
        Environment.SetEnvironmentVariable("DBName", "dummy-db");
        Environment.SetEnvironmentVariable("URI", "https://dummy-sv.net");
        Environment.SetEnvironmentVariable("Interval", "0");

        var result = await _app.InitializeAsync();

        // 結果確認
        Assert.True(result);
        AssertLogContains(LoggerHelper.EndMethod("InitializeAsync"));
    }
}
