using TICO.GAUDI.Commons;

namespace IotedgeV2InfluxDBRegister.Test;

[Collection(nameof(CollectionAttribute))]
public class TagInfo_CreateInstance
{
    private readonly ITestOutputHelper _output;
    private readonly MultiTextWriter _logWriter;

    public TagInfo_CreateInstance(ITestOutputHelper output)
    {
        _output = output;
        // TagInfoのMyLoggerをキャプチャ設定
        var logger = (ILogger)typeof(TagInfo).GetProperty("MyLogger", BindingFlags.Static | BindingFlags.NonPublic)!.GetValue(null)!;
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

    [Fact(DisplayName = "No.61_正常系：正常値の確認（string型）_\"tag1\"がnull")]
    public void Case061()
    {
        string target = "tag1";
        var properties = JObject.Parse(@"{
            ""tag1"": null
        }");
        properties.TryGetValue(target, out var token);
        var instance = TagInfo.CreateInstance(token);
        
        // 結果確認
        Assert.Null(instance.TagName);
        var pi = typeof(TagInfo).GetProperty("TagValueType", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.Equal(0, (int)pi!.GetValue(instance)!);
        AssertLogContains(LoggerHelper.EndMethod("CreateInstance"));
    }

    [Fact(DisplayName = "No.62_正常系：正常値の確認（string型）_\"tag1\"が空文字")]
    public void Case062()
    {
        string target = "tag1";
        var properties = JObject.Parse(@"{
            ""tag1"": """"
        }");
        properties.TryGetValue(target, out var token);

        var instance = TagInfo.CreateInstance(token);
        
        // 結果確認
        Assert.Equal("", instance.TagName);
        var pi = typeof(TagInfo).GetProperty("TagValueType", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.Equal(0, (int)pi!.GetValue(instance)!);
        AssertLogContains(LoggerHelper.EndMethod("CreateInstance"));
    }

    [Fact(DisplayName = "No.39_正常系：正常値の確認（string型）_\"tag1\"が文字列(オブジェクト)以外")]
    public void Case039()
    {
        string target = "tag1";
        var properties = JObject.Parse(@"{
            ""tag1"": 1
        }");
        properties.TryGetValue(target, out var token);
        var instance = TagInfo.CreateInstance(token);
        
        // 結果確認
        Assert.Equal("1", instance.TagName);
        var pi = typeof(TagInfo).GetProperty("TagValueType", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.Equal(0, (int)pi!.GetValue(instance)!);
        AssertLogContains(LoggerHelper.EndMethod("CreateInstance"));
    }

    [Fact(DisplayName = "No.63_正常系：正常値の確認（string型）_\"tag1\"がnull/空文字以外")]
    public void Case063()
    {
        string target = "tag1";
        const string tag_name = "TEST_TAG1_STR";
        var properties = JObject.Parse($@"{{
            ""tag1"":""{tag_name}""
        }}");
        properties.TryGetValue(target, out var token);

        var instance = TagInfo.CreateInstance(token);
        
        // 結果確認
        Assert.Equal(tag_name, instance.TagName);
        var pi = typeof(TagInfo).GetProperty("TagValueType", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.Equal(0, (int)pi!.GetValue(instance)!);
        AssertLogContains(LoggerHelper.EndMethod("CreateInstance"));
    }

    [Fact(DisplayName = "No.64_異常系：必須項目の確認_\"tag_name\"が存在しない")]
    public void Case064()
    {
        string target = "tag_name";
        var properties = JObject.Parse(@"
        {
            ""tag1"":{
                ""value_type"": ""MessageProperty""
                } 
        }
        ");
        properties.TryGetValue("tag1", out var token);
        var ex = Assert.ThrowsAny<Exception>(() => TagInfo.CreateInstance(token));
        
        // 結果確認
        Assert.Contains(DesiredPropertiesHelper.RequiredPropertyNotSpecified(target), ex.Message);
    }

    [Fact(DisplayName = "No.65_異常系：不正値の確認（string型）_\"tag_name\"がnull")]
    public void Case065()
    {
        string target = "tag_name";
        var properties = JObject.Parse(@"
        {
            ""tag1"":{
                ""tag_name"": null,
                ""value_type"": ""MessageProperty""
                } 
        }
        ");

        properties.TryGetValue("tag1", out var token);
        var ex = Assert.ThrowsAny<Exception>(() => TagInfo.CreateInstance(token));
        
        // 結果確認
        Assert.Contains(DesiredPropertiesHelper.ErrorRetrievingProperty(target, DesiredPropertiesHelper.PropertyNullOrEmpty(target)), ex.Message);
    }

    [Fact(DisplayName = "No.66_異常系：不正値の確認（string型）_\"tag_name\"が空文字")]
    public void Case066()
    {
        string target = "tag_name";
        var properties = JObject.Parse(@"
        {
            ""tag1"":{
                ""tag_name"": """",
                ""value_type"": ""MessageProperty""
                } 
        }
        ");

        properties.TryGetValue("tag1", out var token);
        var ex = Assert.ThrowsAny<Exception>(() => TagInfo.CreateInstance(token));
        
        // 結果確認
        Assert.Contains(DesiredPropertiesHelper.ErrorRetrievingProperty(target, DesiredPropertiesHelper.PropertyNullOrEmpty(target)), ex.Message);
    }

    [Fact(DisplayName = "No.67_正常系：正常値の確認（string型）_\"tag_name\"がnull/空文字以外")]
    public void Case067()
    {
        var properties = JObject.Parse(@"
        {
            ""tag1"":{
                ""tag_name"": ""test"",
                ""value_type"": ""MessageProperty""
                } 
        }
        ");

        properties.TryGetValue("tag1", out var token);
        var instance = TagInfo.CreateInstance(token);
        
        // 結果確認
        Assert.Equal("test", instance.TagName);
        var pi = typeof(TagInfo).GetProperty("TagValueType", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.Equal(0, (int)pi!.GetValue(instance)!);
        AssertLogContains(LoggerHelper.EndMethod("CreateInstance"));
    }

    [Fact(DisplayName = "No.68_異常系：必須項目の確認_\"value_type\"が存在しない")]
    public void Case068()
    {
        string target = "value_type";
        var properties = JObject.Parse(@"
        {
            ""tag1"":{
                ""tag_name"": ""test""
                } 
        }
        ");
        properties.TryGetValue("tag1", out var token);
        var ex = Assert.ThrowsAny<Exception>(() => TagInfo.CreateInstance(token));
        
        // 結果確認
        Assert.Contains(DesiredPropertiesHelper.RequiredPropertyNotSpecified(target), ex.Message);
    }

    [Fact(DisplayName = "No.69_異常系：不正値の確認（string型）_\"value_type\"が無効値")]
    public void Case069()
    {
        string target = "value_type";
        var properties = JObject.Parse(@"
        {
            ""tag1"":{
                ""tag_name"": ""test"",
                ""value_type"": ""test""
                } 
        }
        ");

        properties.TryGetValue("tag1", out var token);
        var ex = Assert.ThrowsAny<Exception>(() => TagInfo.CreateInstance(token));
        
        // 結果確認
        Assert.Contains(DesiredPropertiesHelper.ErrorRetrievingProperty(target, DesiredPropertiesHelper.CouldNotConvert), ex.Message);
    }

    [Fact(DisplayName = "No.70_正常系：正常値の確認（string型）_\"value_type\"がMessageProperty")]
    public void Case070()
    {
        var properties = JObject.Parse(@"
        {
            ""tag1"":{
                ""tag_name"": ""test"",
                ""value_type"": ""MessageProperty""
                } 
        }
        ");

        properties.TryGetValue("tag1", out var token);
        var instance = TagInfo.CreateInstance(token);
        
        // 結果確認
        Assert.Equal("test", instance.TagName);
        var pi = typeof(TagInfo).GetProperty("TagValueType", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.Equal(0, (int)pi!.GetValue(instance)!);
        AssertLogContains(LoggerHelper.EndMethod("CreateInstance"));
    }

    [Fact(DisplayName = "No.71_正常系：正常値の確認（string型）_\"value_type\"がMessagePropertyAndRowIndex")]
    public void Case071()
    {
        var properties = JObject.Parse(@"
        {
            ""tag1"":{
                ""tag_name"": ""test"",
                ""value_type"": ""MessagePropertyAndRowIndex""
                } 
        }
        ");

        properties.TryGetValue("tag1", out var token);
        var instance = TagInfo.CreateInstance(token);
        
        // 結果確認
        Assert.Equal("test", instance.TagName);
        var pi = typeof(TagInfo).GetProperty("TagValueType", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.Equal(1, (int)pi!.GetValue(instance)!);
        AssertLogContains(LoggerHelper.EndMethod("CreateInstance"));
    }

    [Fact(DisplayName = "No.136_正常系：正常値の確認（string型）_\"value_type\"が\"messageproperty\"")]
    public void Case136()
    {
        var properties = JObject.Parse(@"
        {
            ""tag1"":{
                ""tag_name"": ""test"",
                ""value_type"": ""messageproperty""
                } 
        }
        ");

        properties.TryGetValue("tag1", out var token);
        var instance = TagInfo.CreateInstance(token);
        
        // 結果確認
        Assert.Equal("test", instance.TagName);
        var pi = typeof(TagInfo).GetProperty("TagValueType", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.Equal(0, (int)pi!.GetValue(instance)!);
        AssertLogContains(LoggerHelper.EndMethod("CreateInstance"));
    }

    [Fact(DisplayName = "No.137_正常系：正常値の確認（string型）_\"value_type\"が\"messagepropertyandrowindex\"")]
    public void Case137()
    {
        var properties = JObject.Parse(@"
        {
            ""tag1"":{
                ""tag_name"": ""test"",
                ""value_type"": ""messagepropertyandrowindex""
                } 
        }
        ");

        properties.TryGetValue("tag1", out var token);
        var instance = TagInfo.CreateInstance(token);
        
        // 結果確認
        Assert.Equal("test", instance.TagName);
        var pi = typeof(TagInfo).GetProperty("TagValueType", BindingFlags.Instance | BindingFlags.NonPublic);
        Assert.Equal(1, (int)pi!.GetValue(instance)!);
        AssertLogContains(LoggerHelper.EndMethod("CreateInstance"));
    }
}
