namespace IotedgeV2InfluxDBRegister.Test;

[Collection(nameof(CollectionAttribute))]
public class MyApplicationMain_InitializeAsync
{
    private readonly MyApplicationMain _app = new ();

    [Fact(DisplayName = "No.1 環境変数\"DBName\"が指定されていないこと。")]
    public async Task Case001()
    {
        Environment.SetEnvironmentVariable("DBName", null);
        Environment.SetEnvironmentVariable("URI", "https://dummy-sv.net");
        Environment.SetEnvironmentVariable("Interval", "10");

        var result = await _app.InitializeAsync();
        Assert.False(result);
    }

    [Fact(DisplayName = "No.2 環境変数\"URI\"が指定されていないこと。")]
    public async Task Case002()
    {
        Environment.SetEnvironmentVariable("DBName", "dummy-db");
        Environment.SetEnvironmentVariable("URI", null);
        Environment.SetEnvironmentVariable("Interval", "10");

        var result = await _app.InitializeAsync();
        Assert.False(result);
    }

    [Fact(DisplayName = "No.3 環境変数\"DBName\"、\"URI\"で参照するDBが存在しない。")]
    public async Task Case003()
    {
        Environment.SetEnvironmentVariable("DBName", "dummy-db");
        Environment.SetEnvironmentVariable("URI", "https://not-found-sv.net");
        Environment.SetEnvironmentVariable("Interval", "10");

        var result = await _app.InitializeAsync();
        Assert.True(result);
    }

    [Fact(DisplayName = "No.4 環境変数\"Interval\"が指定されていないこと。")]
    public async Task Case004()
    {
        Environment.SetEnvironmentVariable("DBName", "dummy-db");
        Environment.SetEnvironmentVariable("URI", "https://not-found-sv.net");
        Environment.SetEnvironmentVariable("Interval", null);

        var result = await _app.InitializeAsync();
        Assert.True(result);
    }

    [Fact(DisplayName = "No.5 環境変数\"Interval\"に\"ERROR_VALUE\"が指定されていること。")]
    public async Task Case005()
    {
        Environment.SetEnvironmentVariable("DBName", "dummy-db");
        Environment.SetEnvironmentVariable("URI", "https://not-found-sv.net");
        Environment.SetEnvironmentVariable("Interval", "ERROR_VALUE");

        var result = await _app.InitializeAsync();
        Assert.True(result);
    }

    [Fact(DisplayName = "No.6 環境変数\"Interval\"に-1が指定されていること。")]
    public async Task Case006()
    {
        Environment.SetEnvironmentVariable("DBName", "dummy-db");
        Environment.SetEnvironmentVariable("URI", "https://not-found-sv.net");
        Environment.SetEnvironmentVariable("Interval", "-1");

        var result = await _app.InitializeAsync();
        Assert.True(result);
    }

    [Fact(DisplayName = "No.7 環境変数\"UserName\"が指定されていて、\"Password\"が指定されていないこと。")]
    public async Task Case007()
    {
        Environment.SetEnvironmentVariable("UserName", "dummy-user");
        Environment.SetEnvironmentVariable("Password", null);
        Environment.SetEnvironmentVariable("DBName", "dummy-db");
        Environment.SetEnvironmentVariable("URI", "https://not-found-sv.net");
        Environment.SetEnvironmentVariable("Interval", "10");

        var result = await _app.InitializeAsync();
        Assert.True(result);
    }

    [Fact(DisplayName = "No.8 環境変数\"Password\"が指定されていて、\"UserName\"が指定されていないこと。")]
    public async Task Case008()
    {
        Environment.SetEnvironmentVariable("UserName", null);
        Environment.SetEnvironmentVariable("Password", "dummy-pass");
        Environment.SetEnvironmentVariable("DBName", "dummy-db");
        Environment.SetEnvironmentVariable("URI", "https://not-found-sv.net");
        Environment.SetEnvironmentVariable("Interval", "10");

        var result = await _app.InitializeAsync();
        Assert.True(result);
    }

    [Fact(DisplayName = "No.9 環境変数\"DBName\"、\"URI\"、\"UserName\"、\"Password\"で参照するDBの認証が通過できない。")]
    public async Task Case009()
    {
        Environment.SetEnvironmentVariable("UserName", "dummy-user");
        Environment.SetEnvironmentVariable("Password", "invalid-pass");
        Environment.SetEnvironmentVariable("DBName", "dummy-db");
        Environment.SetEnvironmentVariable("URI", "https://not-found-sv.net");
        Environment.SetEnvironmentVariable("Interval", "10");

        var result = await _app.InitializeAsync();
        Assert.True(result);
    }

    [Fact(DisplayName = "No.10 初期化関数が正常終了すること。")]
    public async Task Case010()
    {
        Environment.SetEnvironmentVariable("DBName", "dummy-db");
        Environment.SetEnvironmentVariable("URI", "https://not-found-sv.net");
        Environment.SetEnvironmentVariable("Interval", "60");

        var result = await _app.InitializeAsync();
        Assert.True(result);
    }

    [Fact(DisplayName = "No.11 初期化関数が正常終了すること。 (ユーザー認証有)")]
    public async Task Case011()
    {
        Environment.SetEnvironmentVariable("UserName", "dummy-user");
        Environment.SetEnvironmentVariable("Password", "valid-pass");
        Environment.SetEnvironmentVariable("DBName", "dummy-db");
        Environment.SetEnvironmentVariable("URI", "https://not-found-sv.net");
        Environment.SetEnvironmentVariable("Interval", "60");

        var result = await _app.InitializeAsync();
        Assert.True(result);
    }
}
