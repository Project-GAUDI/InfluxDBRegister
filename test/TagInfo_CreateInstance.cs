namespace IotedgeV2InfluxDBRegister.Test;

public class TagInfo_CreateInstance
{
    [Fact(DisplayName = "No.67 引数 jtok に配列（非文字列 or オブジェクト）が設定される")]
    public void Case067()
    {
        var token = JToken.Parse(@"[]");

        Assert.ThrowsAny<Exception>(() => TagInfo.CreateInstance(token));
    }

    [Fact(DisplayName = "No.68 引数 jtok の tag_name プロパティにオブジェクト（非文字列）が設定される")]
    public void Case068()
    {
        var token = JToken.Parse(@"
        {
            ""tag_name"": {},
            ""value_type"": ""MessageProperty""
        }
        ");

        Assert.ThrowsAny<Exception>(() => TagInfo.CreateInstance(token));
    }

    [Fact(DisplayName = "No.69 引数 jtok の value_type プロパティにオブジェクト（非文字列）が設定される")]
    public void Case069()
    {
        var token = JToken.Parse(@"
        {
            ""tag_name"": ""TEST_TAG1_NAME"",
            ""value_type"": {}
        }
        ");

        Assert.ThrowsAny<Exception>(() => TagInfo.CreateInstance(token));
    }

    [Fact(DisplayName = "No.70 引数 jtok の value_type プロパティに受け入れ不可能な文字列（\"MessagePropertyAndRowIndex\", \"MessageProperty\"以外の文字列）が設定される")]
    public void Case070()
    {
        var token = JToken.Parse(@"
        {
            ""tag_name"": ""TEST_TAG1_NAME"",
            ""value_type"": ""INVALID_VALUE""
        }
        ");

        Assert.ThrowsAny<Exception>(() => TagInfo.CreateInstance(token));
    }

    [Fact(DisplayName = "No.71 引数 jtok に 受け入れ可能な形のJSONオブジェクトが設定される（文字列データが渡されるパターン）")]
    public void Case071()
    {
        const string TAG_NAME = "TEST_TAG1_STR";
        var token = JToken.Parse($"\"{TAG_NAME}\"");

        var instance = TagInfo.CreateInstance(token);
        Assert.NotNull(instance);
        Assert.IsType<TagInfo>(instance);

        var pi = typeof(TagInfo).GetProperty("TagValueType", BindingFlags.Instance | BindingFlags.NonPublic);

        Assert.Equal(TAG_NAME, instance.TagName);
        Assert.Equal(0, (int)pi!.GetValue(instance)!);
    }

    [Fact(DisplayName = "No.72 引数 jtok に 受け入れ可能な形のJSONオブジェクトが設定される（\"MessagePropertyAndRowIndex\"の文字列が渡されるパターン）")]
    public void Case072()
    {
        const string TAG_NAME = "TEST_TAG1_NAME";
        var token = JToken.Parse($@"{{
            ""tag_name"": ""{TAG_NAME}"",
            ""value_type"": ""MessagePropertyAndRowIndex""
        }}");

        var instance = TagInfo.CreateInstance(token);
        Assert.NotNull(instance);
        Assert.IsType<TagInfo>(instance);

        var pi = typeof(TagInfo).GetProperty("TagValueType", BindingFlags.Instance | BindingFlags.NonPublic);

        Assert.Equal(TAG_NAME, instance.TagName);
        Assert.Equal(1, (int)pi!.GetValue(instance)!);
    }

    [Fact(DisplayName = "No.73 引数 jtok に 受け入れ可能な形のJSONオブジェクトが設定される（\"MessageProperty\"の文字列が渡されるパターン）")]
    public void Case073()
    {
        const string TAG_NAME = "TEST_TAG1_NAME";
        var token = JToken.Parse($@"{{
            ""tag_name"": ""{TAG_NAME}"",
            ""value_type"": ""MessageProperty""
        }}");

        var instance = TagInfo.CreateInstance(token);
        Assert.NotNull(instance);
        Assert.IsType<TagInfo>(instance);

        var pi = typeof(TagInfo).GetProperty("TagValueType", BindingFlags.Instance | BindingFlags.NonPublic);

        Assert.Equal(TAG_NAME, instance.TagName);
        Assert.Equal(0, (int)pi!.GetValue(instance)!);
    }
}
