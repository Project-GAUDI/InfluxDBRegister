namespace IotedgeV2InfluxDBRegister.Test;

[Collection(nameof(CollectionAttribute))]
public class MyApplicationMain_OnDesiredPropertiesReceivedAsync
{
    private readonly MyApplicationMain _app = new ();
    private readonly ITestOutputHelper _output;
    private readonly MultiTextWriter _logWriter;

    public MyApplicationMain_OnDesiredPropertiesReceivedAsync(ITestOutputHelper output)
    {
        _output = output;
        _logWriter = LoggerHelper.SetupMyApplicationMainLogger();
    }

    private void AssertLogContains(string expected)
    {
        LoggerHelper.AssertLogContains(_logWriter, _output, expected);
    }

    [Fact(DisplayName = "No.22_異常系：必須項目の確認_\"dbparam1\"が存在しない")]
    public async Task Case022()
    {
        // 空のDesiredProperties
        var properties = JObject.Parse("{}");

        var result = await _app.OnDesiredPropertiesReceivedAsync(properties);

        // 結果確認
        Assert.False(result);
        AssertLogContains("DesiredProperties is empty.");
    }

    [Fact(DisplayName = "No.23_異常系：不正値の確認（object型）_\"dbparam1\"が存在せず、\"dbparam2\"が存在する")]
    public async Task Case023()
    {
        // "dbparam1"が存在せず、"dbparam2"が存在する
        var properties = JObject.Parse(@"
        {
            ""dbparam2"": {
                ""input"": ""TEST_INPUT"",
                ""measurement"": ""TEST_MEASUREMENT"",
                ""tags"": {
                    ""tag1"": ""TEST_TAG1_STR""
                },
                ""use_header"": true,
                ""use_data"": true,
                ""set_usertimestamp"": true,
                ""timestamp_index"": 1
            }
        }
        ");

        var result = await _app.OnDesiredPropertiesReceivedAsync(properties);

        // 結果確認
        Assert.False(result);
        AssertLogContains("DesiredProperties is empty.");
    }

    [Fact(DisplayName = "No.24_異常系：不正値の確認（object型）_\"dbparam1\"がオブジェクト以外")]
    public async Task Case024()
    {
        // "dbparam1"がオブジェクト以外
        var properties = JObject.Parse("{ \"dbparam1\": \"\" }");

        var result = await _app.OnDesiredPropertiesReceivedAsync(properties);

        // 結果確認
        Assert.False(result);
        AssertLogContains("Property dbparam1 is invalid.");
    }

    [Fact(DisplayName = "No.25_正常系：正常値の確認（object型）_\"dbparam1\"が存在し、正常終了する")]
    public async Task Case025()
    {
        // "dbparam1"が存在する
        var properties = JObject.Parse(@"
        {
            ""dbparam1"": {
                ""input"": ""TEST_INPUT"",
                ""measurement"": ""TEST_MEASUREMENT"",
                ""tags"": {
                    ""tag1"": ""TEST_TAG1_STR""
                },
                ""use_header"": true,
                ""use_data"": true,
                ""set_usertimestamp"": true,
                ""timestamp_index"": 1
            }
        }
        ");

        var result = await _app.OnDesiredPropertiesReceivedAsync(properties);

        // 結果確認
        Assert.True(result);
        AssertLogContains(LoggerHelper.EndMethod("OnDesiredPropertiesReceivedAsync"));
    }

    [Fact(DisplayName = "No.26_正常系：正常値の確認（object型）_\"dbparam1\"、\"dbparam2\"が存在し、正常終了する")]
    public async Task Case026()
    {
        // "dbparam1"、"dbparam2"が存在する
        var properties = JObject.Parse(@"
        {
            ""dbparam1"": {
                ""input"": ""TEST_INPUT1"",
                ""measurement"": ""TEST_MEASUREMENT1"",
                ""tags"": {
                    ""tag1"": ""TEST_TAG1_STR""
                },
                ""use_header"": true,
                ""use_data"": true,
                ""set_usertimestamp"": true,
                ""timestamp_index"": 1
            },
            ""dbparam2"": {
                ""input"": ""TEST_INPUT2"",
                ""measurement"": ""TEST_MEASUREMENT2"",
                ""tags"": {
                    ""tag1"": ""TEST_TAG2_STR""
                },
                ""use_header"": true,
                ""use_data"": true,
                ""set_usertimestamp"": true,
                ""timestamp_index"": 1
            }
        }
        ");

        var result = await _app.OnDesiredPropertiesReceivedAsync(properties);

        // 結果確認
        Assert.True(result);
        AssertLogContains(LoggerHelper.EndMethod("OnDesiredPropertiesReceivedAsync"));
    }
}
