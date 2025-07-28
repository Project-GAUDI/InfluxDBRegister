namespace IotedgeV2InfluxDBRegister.Test;

public class SQLparser_InitParser
{
    private readonly SQLParser _parser = new ();

    [Fact(DisplayName = "No.12 引数\"dbenv\"にnullが指定されていること。")]
    public void Case012()
    {
        Assert.ThrowsAny<Exception>(() => _parser.InitParser(null, null, null, "https://dummy-sv.net", 10));
    }

    [Fact(DisplayName = "No.13 引数\"urienv\"にnullが指定されていること。")]
    public void Case013()
    {
        Assert.ThrowsAny<Exception>(() => _parser.InitParser(null, null, "dummy-db", null, 10));
    }

    [Fact(DisplayName = "No.14 引数\"dbenv\"、\"urienv\"で参照するDBが存在しない。")]
    public void Case014()
    {
        _parser.InitParser(null, null, "dummy-db", "https://not-found-sv.net", 10);
    }

    [Fact(DisplayName = "No.15 引数\"interval\"に-1が指定されていること。")]
    public void Case015()
    {
        _parser.InitParser(null, null, "dummy-db", "https://dummy-sv.net", -1);
    }

    [Fact(DisplayName = "No.16 引数\"userenv\"が指定されていて、\"passenv\"が指定されていないこと。")]
    public void Case016()
    {
        _parser.InitParser("dummy-user", null, "dummy-db", "https://dummy-sv.net", 10);
    }

    [Fact(DisplayName = "No.17 引数\"passenv\"が指定されていて、\"userenv\"が指定されていないこと。")]
    public void Case017()
    {
        _parser.InitParser(null, "dummy-pass", "dummy-db", "https://dummy-sv.net", 10);
    }

    [Fact(DisplayName = "No.18 引数\"dbenv\"、\"urienv\"、\"userenv\"、\"passenv\"で参照するDBの認証が通過できない。")]
    public void Case018()
    {
        _parser.InitParser("dummy-user", "invalid-pass", "dummy-db", "https://dummy-sv.net", 10);
    }

    [Fact(DisplayName = "No.19 初期化関数が正常終了すること。")]
    public void Case019()
    {
        _parser.InitParser(null, null, "dummy-db", "https://dummy-sv.net", 10);
    }

    [Fact(DisplayName = "No.20 初期化関数が正常終了すること。(認証有)")]
    public void Case020()
    {
        _parser.InitParser("dummy-user", "valid-pass", "dummy-db", "https://dummy-sv.net", 10);
    }
}
