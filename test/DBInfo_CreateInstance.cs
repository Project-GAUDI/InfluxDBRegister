using TICO.GAUDI.Commons;

namespace IotedgeV2InfluxDBRegister.Test;

[Collection(nameof(CollectionAttribute))]
public class DBInfo_CreateInstance
{
    private readonly ITestOutputHelper _output;
    private readonly MultiTextWriter _logWriter;

    public DBInfo_CreateInstance(ITestOutputHelper output)
    {
        _output = output;
        // DBInfoのMyLoggerをキャプチャ設定
        var logger = (ILogger)typeof(DBInfo).GetProperty("MyLogger", BindingFlags.Static | BindingFlags.NonPublic)!.GetValue(null)!;
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

    [Fact(DisplayName = "No.27_異常系：必須項目の確認_\"input\"が存在しない")]
    public void Case027()
    {
        string target = "input";
        // プロパティ作成(inputが存在しない)
        var properties = JObject.Parse(@"{}");

        var ex = Assert.ThrowsAny<Exception>(() => DBInfo.CreateInstance(properties));
        
        // 結果確認
        Assert.Contains(DesiredPropertiesHelper.RequiredPropertyNotSpecified(target), ex.Message);
    }

    [Fact(DisplayName = "No.28_異常系：不正値の確認（string型）_\"input\"がnull")]
    public void Case028()
    {
        string target = "input";
        // プロパティ作成(inputがnull)
        var properties = JObject.Parse(@"
        {
            ""input"": null,
            ""measurement"": ""TEST_MEASUREMENT"",
            ""tags"": {
                ""tag1"": ""TEST_TAG1_STR""
            },
            ""use_header"": true,
            ""use_data"": true,
            ""set_usertimestamp"": true,
            ""timestamp_index"": 1
        }
        ");

        var ex = Assert.ThrowsAny<Exception>(() => DBInfo.CreateInstance(properties));

        // 結果確認
        Assert.Contains(DesiredPropertiesHelper.ErrorRetrievingProperty(target, DesiredPropertiesHelper.PropertyNullOrEmpty(target)), ex.Message);
    }

    [Fact(DisplayName = "No.29_異常系：不正値の確認（string型）_\"input\"が空文字")]
    public void Case029()
    {
        string target = "input";
        // プロパティ作成(inputが空文字)
        var properties = JObject.Parse(@"
        {
            ""input"": """",
            ""measurement"": ""TEST_MEASUREMENT"",
            ""tags"": {
                ""tag1"": ""TEST_TAG1_STR""
            },
            ""use_header"": true,
            ""use_data"": true,
            ""set_usertimestamp"": true,
            ""timestamp_index"": 1
        }
        ");

        var ex = Assert.ThrowsAny<Exception>(() => DBInfo.CreateInstance(properties));

        // 結果確認
        Assert.Equal(DesiredPropertiesHelper.ErrorRetrievingProperty(target, DesiredPropertiesHelper.PropertyNullOrEmpty(target)), ex.Message);
    }

    [Fact(DisplayName = "No.131_異常系：不正値の確認（string型）_\"input\"がオブジェクト")]
    public void Case131()
    {
        string target = "input";
        // プロパティ作成(inputがオブジェクト)
        var properties = JObject.Parse(@"
        {
            ""input"": {},
            ""measurement"": ""TEST_MEASUREMENT"",
            ""tags"": {
                ""tag1"": ""TEST_TAG1_STR""
            },
            ""use_header"": true,
            ""use_data"": true,
            ""set_usertimestamp"": true,
            ""timestamp_index"": 1
        }
        ");

        var ex = Assert.ThrowsAny<Exception>(() => DBInfo.CreateInstance(properties));

        // 結果確認
        Assert.Contains(DesiredPropertiesHelper.ErrorRetrievingProperty(target, DesiredPropertiesHelper.CannotConvert), ex.Message);
    }

    [Fact(DisplayName = "No.30_正常系：正常値の確認（string型）_\"input\"がnull/空文字以外")]
    public void Case030()
    {
        string value = "TEST_INPUT";
        // プロパティ作成(inputがnull/空文字以外)
        var properties = JObject.Parse($@"
        {{
            ""input"": ""{value}"",
            ""measurement"": ""TEST_MEASUREMENT"",
            ""tags"": {{
                ""tag1"": ""TEST_TAG1_STR""
            }},
            ""use_header"": true,
            ""use_data"": true,
            ""set_usertimestamp"": true,
            ""timestamp_index"": 1
        }}
        ");

        var instance = DBInfo.CreateInstance(properties);

        // 結果確認
        Assert.Equal(value, instance.Input);
        AssertLogContains(LoggerHelper.EndMethod("CreateInstance"));
    }

    [Fact(DisplayName = "No.31_異常系：必須項目の確認_\"measurement\"が存在しない")]
    public void Case031()
    {
        string target = "measurement";
        // プロパティ作成(measurementが存在しない)
        var properties = JObject.Parse(@"
        {
            ""input"": ""TEST_INPUT"",
            ""tags"": {
                ""tag1"": ""TEST_TAG1_STR""
            },
            ""use_header"": true,
            ""use_data"": true,
            ""set_usertimestamp"": true,
            ""timestamp_index"": 1
        }
        ");

        var ex = Assert.ThrowsAny<Exception>(() => DBInfo.CreateInstance(properties));

        // 結果確認
        Assert.Contains(DesiredPropertiesHelper.RequiredPropertyNotSpecified(target), ex.Message);
    }

    [Fact(DisplayName = "No.32_異常系：不正値の確認（string型）_\"measurement\"がnull")]
    public void Case032()
    {
        string target = "measurement";
        // プロパティ作成(measurementがnull)
        var properties = JObject.Parse(@"
        {
            ""input"": ""TEST_INPUT"",
            ""measurement"": null,
            ""tags"": {
                ""tag1"": ""TEST_TAG1_STR""
            },
            ""use_header"": true,
            ""use_data"": true,
            ""set_usertimestamp"": true,
            ""timestamp_index"": 1
        }
        ");

        var ex = Assert.ThrowsAny<Exception>(() => DBInfo.CreateInstance(properties));

        // 結果確認
        Assert.Contains(DesiredPropertiesHelper.ErrorRetrievingProperty(target, DesiredPropertiesHelper.PropertyNullOrEmpty(target)), ex.Message);
    }

    [Fact(DisplayName = "No.33_異常系：不正値の確認（string型）_\"measurement\"が空文字")]
    public void Case033()
    {
        string target = "measurement";
        // プロパティ作成(measurementが空文字)
        var properties = JObject.Parse(@"
        {
            ""input"": ""TEST_INPUT"",
            ""measurement"": """",
            ""tags"": {
                ""tag1"": ""TEST_TAG1_STR""
            },
            ""use_header"": true,
            ""use_data"": true,
            ""set_usertimestamp"": true,
            ""timestamp_index"": 1
        }
        ");

        var ex = Assert.ThrowsAny<Exception>(() => DBInfo.CreateInstance(properties));

        // 結果確認
        Assert.Contains(DesiredPropertiesHelper.ErrorRetrievingProperty(target, DesiredPropertiesHelper.PropertyNullOrEmpty(target)), ex.Message);
    }

    [Fact(DisplayName = "No.132_異常系：不正値の確認（string型）_\"measurement\"がオブジェクト")]
    public void Case132()
    {
        string target = "measurement";
        // プロパティ作成(measurementがオブジェクト)
        var properties = JObject.Parse(@"
        {
            ""input"": ""TEST_INPUT"",
            ""measurement"": {},
            ""tags"": {
                ""tag1"": ""TEST_TAG1_STR""
            },
            ""use_header"": true,
            ""use_data"": true,
            ""set_usertimestamp"": true,
            ""timestamp_index"": 1
        }
        ");

        var ex = Assert.ThrowsAny<Exception>(() => DBInfo.CreateInstance(properties));

        // 結果確認
        Assert.Contains(DesiredPropertiesHelper.ErrorRetrievingProperty(target, DesiredPropertiesHelper.CannotConvert), ex.Message);
    }

    [Fact(DisplayName = "No.34_正常系：正常値の確認（string型）_\"measurement\"がnull/空文字以外")]
    public void Case034()
    {
        string value = "TEST_MEASUREMENT";
        // プロパティ作成(measurementがnull/空文字以外)
        var properties = JObject.Parse($@"
        {{
            ""input"": ""TEST_INPUT"",
            ""measurement"": ""{value}"",
            ""tags"": {{
                ""tag1"": ""TEST_TAG1_STR""
            }},
            ""use_header"": true,
            ""use_data"": true,
            ""set_usertimestamp"": true,
            ""timestamp_index"": 1
        }}
        ");

        var instance = DBInfo.CreateInstance(properties);

        // 結果確認
        Assert.Equal(value, instance.Measurement);
        AssertLogContains(LoggerHelper.EndMethod("CreateInstance"));
    }

    [Fact(DisplayName = "No.35_異常系：必須項目の確認_\"tags\"が存在しない")]
    public void Case035()
    {
        string target = "tags";
        // プロパティ作成(tagsが存在しない)
        var properties = JObject.Parse(@"
        {
            ""input"": ""TEST_INPUT"",
            ""measurement"": ""TEST_MEASUREMENT"",
            ""use_header"": true,
            ""use_data"": true,
            ""set_usertimestamp"": true,
            ""timestamp_index"": 1
        }
        ");

        var ex = Assert.ThrowsAny<Exception>(() => DBInfo.CreateInstance(properties));

        // 結果確認
        Assert.Contains(DesiredPropertiesHelper.RequiredPropertyNotSpecified(target), ex.Message);
    }

    [Fact(DisplayName = "No.36_異常系：不正値の確認（object型）_\"tags\"がオブジェクト以外")]
    public void Case036()
    {
        string target = "tags";
        // プロパティ作成(tagsがオブジェクト以外)
        var properties = JObject.Parse(@"
        {
            ""input"": ""TEST_INPUT"",
            ""measurement"": ""TEST_MEASUREMENT"",
            ""tags"": [],
            ""use_header"": true,
            ""use_data"": true,
            ""set_usertimestamp"": true,
            ""timestamp_index"": 1
        }
        ");

        var ex = Assert.ThrowsAny<Exception>(() => DBInfo.CreateInstance(properties));

        // 結果確認
        Assert.Contains(DesiredPropertiesHelper.ErrorRetrievingProperty(target, DesiredPropertiesHelper.DeserializedJsonType), ex.Message);
    }

    [Fact(DisplayName = "No.37_異常系：必須項目の確認_\"tag1\"が存在しない")]
    public void Case037()
    {
        // プロパティ作成(tag1が存在しない)
        var properties = JObject.Parse(@"
        {
            ""input"": ""TEST_INPUT"",
            ""measurement"": ""TEST_MEASUREMENT"",
            ""tags"": {},
            ""use_header"": true,
            ""use_data"": true,
            ""set_usertimestamp"": true,
            ""timestamp_index"": 1
        }
        ");

        var ex = Assert.ThrowsAny<Exception>(() => DBInfo.CreateInstance(properties));

        // 結果確認
        Assert.Contains("No tag value", ex.Message);
    }

    [Fact(DisplayName = "No.38_異常系：不正値の確認（string型）_\"tag1\"が存在せず、\"tag2\"が存在する")]
    public void Case038()
    {
        // プロパティ作成("tag1"が存在せず、"tag2"が存在する)
        var properties = JObject.Parse(@"
        {
            ""input"": ""TEST_INPUT"",
            ""measurement"": ""TEST_MEASUREMENT"",
            ""tags"": {
                ""tag2"": ""TEST_TAG2_STR""
            },
            ""use_header"": true,
            ""use_data"": true,
            ""set_usertimestamp"": true,
            ""timestamp_index"": 1
        }
        ");

        var ex = Assert.ThrowsAny<Exception>(() => DBInfo.CreateInstance(properties));

        // 結果確認
        Assert.Contains("No tag value", ex.Message);
    }

    [Fact(DisplayName = "No.40_正常系：正常値の確認（string型）_\"tag1\"が存在し、正常終了する")]
    public void Case040()
    {
        // プロパティ作成("tag1"が存在し、正常終了する)
        var properties = JObject.Parse(@"
        {
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
        ");

        DBInfo.CreateInstance(properties);
        
        // 結果確認
        AssertLogContains(LoggerHelper.EndMethod("CreateInstance"));
    }

    [Fact(DisplayName = "No.41_正常系：正常値の確認（string型）_\"tag1\"、\"tag2\"が存在し、正常終了する")]
    public void Case041()
    {
        var properties = JObject.Parse(@"
        {
            ""input"": ""TEST_INPUT"",
            ""measurement"": ""TEST_MEASUREMENT"",
            ""tags"": {
                ""tag1"": ""TEST_TAG1_STR"",
                ""tag2"": ""TEST_TAG2_STR""
            },
            ""use_header"": true,
            ""use_data"": true,
            ""set_usertimestamp"": true,
            ""timestamp_index"": 1
        }
        ");

        DBInfo.CreateInstance(properties);
        
        // 結果確認
        AssertLogContains(LoggerHelper.EndMethod("CreateInstance"));
    }

    [Fact(DisplayName = "No.42_正常系：正常値の確認（object型）_\"tag1\"が存在し、正常終了する")]
    public void Case042()
    {
        var properties = JObject.Parse(@"
        {
            ""input"": ""TEST_INPUT"",
            ""measurement"": ""TEST_MEASUREMENT"",
            ""tags"": {
                ""tag1"": {
                    ""tag_name"": ""TEST_TAG1_NAME"",
                    ""value_type"": ""MessageProperty""
                }
            },
            ""use_header"": true,
            ""use_data"": true,
            ""set_usertimestamp"": true,
            ""timestamp_index"": 1
        }
        ");

        DBInfo.CreateInstance(properties);
        
        // 結果確認
        AssertLogContains(LoggerHelper.EndMethod("CreateInstance"));
    }

    [Fact(DisplayName = "No.43_正常系：正常値の確認（object型）_\"tag1\"、\"tag2\"が存在し、正常終了する")]
    public void Case043()
    {
        var properties = JObject.Parse(@"
        {
            ""input"": ""TEST_INPUT"",
            ""measurement"": ""TEST_MEASUREMENT"",
            ""tags"": {
                ""tag1"": {
                    ""tag_name"": ""TEST_TAG1_NAME"",
                    ""value_type"": ""MessageProperty""
                },
                ""tag2"": {
                    ""tag_name"": ""TEST_TAG2_NAME"",
                    ""value_type"": ""MessagePropertyAndRowIndex""
                }
            },
            ""use_header"": true,
            ""use_data"": true,
            ""set_usertimestamp"": true,
            ""timestamp_index"": 1
        }
        ");

        DBInfo.CreateInstance(properties);
        
        // 結果確認
        AssertLogContains(LoggerHelper.EndMethod("CreateInstance"));
    }

    [Fact(DisplayName = "No.44_異常系：必須項目の確認_\"use_header\"が存在しない")]
    public void Case044()
    {
        string target = "use_header";
        var properties = JObject.Parse(@"
        {
            ""input"": ""TEST_INPUT"",
            ""measurement"": ""TEST_MEASUREMENT"",
            ""tags"": {
                ""tag1"": ""TEST_TAG1_STR""
            },
            ""use_data"": true,
            ""set_usertimestamp"": true,
            ""timestamp_index"": 1
        }
        ");

        var ex = Assert.ThrowsAny<Exception>(() => DBInfo.CreateInstance(properties));

        // 結果確認
        Assert.Contains(DesiredPropertiesHelper.RequiredPropertyNotSpecified(target), ex.Message);
    }

    [Fact(DisplayName = "No.45_異常系：不正値の確認（boolean型）_\"use_header\"が真偽値以外")]
    public void Case045()
    {
        string target = "use_header";
        var properties = JObject.Parse(@"
        {
            ""input"": ""TEST_INPUT"",
            ""measurement"": ""TEST_MEASUREMENT"",
            ""tags"": {
                ""tag1"": ""TEST_TAG1_STR""
            },
            ""use_header"": {},
            ""use_data"": true,
            ""set_usertimestamp"": true,
            ""timestamp_index"": 1
        }
        ");

        var ex = Assert.ThrowsAny<Exception>(() => DBInfo.CreateInstance(properties));

        // 結果確認
        Assert.Contains(DesiredPropertiesHelper.ErrorRetrievingProperty(target, DesiredPropertiesHelper.CannotConvert), ex.Message);
    }

    [Fact(DisplayName = "No.46_正常系：正常値の確認（boolean型）_\"use_header\"が真偽値")]
    public void Case046()
    {
        var properties = JObject.Parse(@"
        {
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
        ");

        var instance = DBInfo.CreateInstance(properties);

        // 結果確認
        Assert.True(instance.UseHeader);
        AssertLogContains(LoggerHelper.EndMethod("CreateInstance"));
    }

    [Fact(DisplayName = "No.47_異常系：必須項目の確認_\"use_data\"が存在しない")]
    public void Case047()
    {
        string target = "use_data";
        var properties = JObject.Parse(@"
        {
            ""input"": ""TEST_INPUT"",
            ""measurement"": ""TEST_MEASUREMENT"",
            ""tags"": {
                ""tag1"": ""TEST_TAG1_STR""
            },
            ""use_header"": true,
            ""set_usertimestamp"": true,
            ""timestamp_index"": 1
        }
        ");

        var ex = Assert.ThrowsAny<Exception>(() => DBInfo.CreateInstance(properties));

        // 結果確認
        Assert.Contains(DesiredPropertiesHelper.RequiredPropertyNotSpecified(target), ex.Message);
    }

    [Fact(DisplayName = "No.48_異常系：不正値の確認（boolean型）_\"use_data\"が真偽値以外")]
    public void Case048()
    {
        string target = "use_data";
        var properties = JObject.Parse(@"
        {
            ""input"": ""TEST_INPUT"",
            ""measurement"": ""TEST_MEASUREMENT"",
            ""tags"": {
                ""tag1"": ""TEST_TAG1_STR""
            },
            ""use_header"": true,
            ""use_data"": {},
            ""set_usertimestamp"": true,
            ""timestamp_index"": 1
        }
        ");

        var ex = Assert.ThrowsAny<Exception>(() => DBInfo.CreateInstance(properties));

        // 結果確認
        Assert.Contains(DesiredPropertiesHelper.ErrorRetrievingProperty(target, DesiredPropertiesHelper.CannotConvert), ex.Message);
    }
    [Fact(DisplayName = "No.49_正常系：正常値の確認（boolean型）_\"use_data\"が真偽値")]
    public void Case049()
    {
        var properties = JObject.Parse(@"
        {
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
        ");

        var instance = DBInfo.CreateInstance(properties);

        // 結果確認
        Assert.True(instance.UseData);
        AssertLogContains(LoggerHelper.EndMethod("CreateInstance"));
    }

    [Fact(DisplayName = "No.50_異常系：必須項目の確認_\"set_usertimestamp\"が存在しない")]
    public void Case050()
    {
        string target = "set_usertimestamp";
        var properties = JObject.Parse(@"
        {
            ""input"": ""TEST_INPUT"",
            ""measurement"": ""TEST_MEASUREMENT"",
            ""tags"": {
                ""tag1"": ""TEST_TAG1_STR""
            },
            ""use_header"": true,
            ""use_data"": true
        }
        ");

        var ex = Assert.ThrowsAny<Exception>(() => DBInfo.CreateInstance(properties));

        // 結果確認
        Assert.Contains(DesiredPropertiesHelper.RequiredPropertyNotSpecified(target), ex.Message);
    }

    [Fact(DisplayName = "No.51_異常系：不正値の確認（boolean型）_\"set_usertimestamp\"が真偽値以外")]
    public void Case051()
    {
        string target = "set_usertimestamp";
        var properties = JObject.Parse(@"
        {
            ""input"": ""TEST_INPUT"",
            ""measurement"": ""TEST_MEASUREMENT"",
            ""tags"": {
                ""tag1"": ""TEST_TAG1_STR""
            },
            ""use_header"": true,
            ""use_data"": true,
            ""set_usertimestamp"": {}
        }
        ");

        var ex = Assert.ThrowsAny<Exception>(() => DBInfo.CreateInstance(properties));

        // 結果確認
        Assert.Contains(DesiredPropertiesHelper.ErrorRetrievingProperty(target, DesiredPropertiesHelper.CannotConvert), ex.Message);
    }

    [Fact(DisplayName = "No.52_正常系：正常値の確認（boolean型）_\"set_usertimestamp\"が真偽値")]
    public void Case052()
    {
        var properties = JObject.Parse(@"
        {
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
        ");

        var instance = DBInfo.CreateInstance(properties);

        // 結果確認
        Assert.True(instance.SetUserTimestamp);
        AssertLogContains(LoggerHelper.EndMethod("CreateInstance"));
    }

    [Fact(DisplayName = "No.53_異常系：必須項目の確認_\"set_usertimestamp\"がtrue\"timestamp_index\"が存在しない")]
    public void Case053()
    {
        string target = "timestamp_index";
        var properties = JObject.Parse(@"
        {
            ""input"": ""TEST_INPUT"",
            ""measurement"": ""TEST_MEASUREMENT"",
            ""tags"": {
                ""tag1"": ""TEST_TAG1_STR""
            },
            ""use_header"": true,
            ""use_data"": true,
            ""set_usertimestamp"": true
        }
        ");

        var ex = Assert.ThrowsAny<Exception>(() => DBInfo.CreateInstance(properties));
        Assert.Contains(DesiredPropertiesHelper.RequiredPropertyNotSpecified(target), ex.Message);
    }

    [Fact(DisplayName = "No.54_異常系：不正値の確認（number型）_\"set_usertimestamp\"がtrue\"timestamp_index\"が数値以外")]
    public void Case054()
    {
        string target = "timestamp_index";
        string value = "INVALID_TYPE";
        
        var properties = JObject.Parse($@"
        {{
            ""input"": ""TEST_INPUT"",
            ""measurement"": ""TEST_MEASUREMENT"",
            ""tags"": {{
                ""tag1"": ""TEST_TAG1_STR""
            }},
            ""use_header"": true,
            ""use_data"": true,
            ""set_usertimestamp"": true,
            ""timestamp_index"": ""{value}""
        }}
        ");

        var ex = Assert.ThrowsAny<Exception>(() => DBInfo.CreateInstance(properties));

        // 結果確認
        Assert.Contains(DesiredPropertiesHelper.ErrorRetrievingProperty(target, DesiredPropertiesHelper.IncorrectFormat(value)), ex.Message);
    }

    [Fact(DisplayName = "No.55_異常系：不正値の確認（number型）_\"set_usertimestamp\"がtrue\"timestamp_index\"が1未満の数値")]
    public void Case055()
    {
        string target = "timestamp_index";
        var properties = JObject.Parse(@"
        {
            ""input"": ""TEST_INPUT"",
            ""measurement"": ""TEST_MEASUREMENT"",
            ""tags"": {
                ""tag1"": ""TEST_TAG1_STR""
            },
            ""use_header"": true,
            ""use_data"": true,
            ""set_usertimestamp"": true,
            ""timestamp_index"": 0
        }
        ");

        var ex = Assert.ThrowsAny<Exception>(() => DBInfo.CreateInstance(properties));

        // 結果確認
        Assert.Contains(DesiredPropertiesHelper.ErrorRetrievingProperty(target, DesiredPropertiesHelper.PropertyConditionNotMet(target)), ex.Message);
    }

    [Fact(DisplayName = "No.56_正常系：正常値の確認（number型）_\"set_usertimestamp\"がtrue\"timestamp_index\"が1以上の数値")]
    public void Case056()
    {
        var properties = JObject.Parse(@"
        {
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
        ");

        var instance = DBInfo.CreateInstance(properties);

        // 結果確認
        Assert.Equal(1, instance.TimestampIndex);
        AssertLogContains(LoggerHelper.EndMethod("CreateInstance"));
    }

    [Fact(DisplayName = "No.57_異常系：不正値の確認（string型）_\"field_data_type\"が\"automatic\"/\"string_all\"以外で\"test\"")]
    public void Case057()
    {
        string target = "field_data_type";
        var properties = JObject.Parse(@"
        {
            ""input"": ""TEST_INPUT"",
            ""measurement"": ""TEST_MEASUREMENT"",
            ""tags"": {
                ""tag1"": ""TEST_TAG1_STR""
            },
            ""use_header"": true,
            ""use_data"": true,
            ""set_usertimestamp"": true,
            ""timestamp_index"": 1,
            ""field_data_type"": ""test""
        }
        ");

        var ex = Assert.ThrowsAny<Exception>(() => DBInfo.CreateInstance(properties));

        // 結果確認
        Assert.Contains(DesiredPropertiesHelper.ErrorRetrievingProperty(target, DesiredPropertiesHelper.CouldNotConvert), ex.Message);
    }

    [Fact(DisplayName = "No.133_異常系：不正値の確認（string型）_\"field_data_type\"がオブジェクト")]
    public void Case133()
    {
        string target = "field_data_type";
        var properties = JObject.Parse(@"
        {
            ""input"": ""TEST_INPUT"",
            ""measurement"": ""TEST_MEASUREMENT"",
            ""tags"": {
                ""tag1"": ""TEST_TAG1_STR""
            },
            ""use_header"": true,
            ""use_data"": true,
            ""set_usertimestamp"": true,
            ""timestamp_index"": 1,
            ""field_data_type"": {}
        }
        ");

        var ex = Assert.ThrowsAny<Exception>(() => DBInfo.CreateInstance(properties));

        // 結果確認
        Assert.Contains(DesiredPropertiesHelper.ErrorRetrievingProperty(target, DesiredPropertiesHelper.CannotConvert), ex.Message);
    }

    [Fact(DisplayName = "No.58_正常系：デフォルト値の確認_\"field_data_type\"が存在しない")]
    public void Case058()
    {
        var properties = JObject.Parse(@"
        {
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
        ");

        var instance = DBInfo.CreateInstance(properties);

        // 結果確認
        Assert.Equal(DBInfo.FieldDataTypeMode.automatic, instance.FieldDataType);
        AssertLogContains(LoggerHelper.EndMethod("CreateInstance"));
    }

    [Fact(DisplayName = "No.59_正常系：正常値の確認（string型）_\"field_data_type\"が\"automatic\"")]
    public void Case059()
    {
        var properties = JObject.Parse(@"
        {
            ""input"": ""TEST_INPUT"",
            ""measurement"": ""TEST_MEASUREMENT"",
            ""tags"": {
                ""tag1"": ""TEST_TAG1_STR""
            },
            ""use_header"": true,
            ""use_data"": true,
            ""set_usertimestamp"": true,
            ""timestamp_index"": 1,
            ""field_data_type"": ""automatic""
        }
        ");

        var instance = DBInfo.CreateInstance(properties);

        // 結果確認
        Assert.Equal(DBInfo.FieldDataTypeMode.automatic, instance.FieldDataType);
        AssertLogContains(LoggerHelper.EndMethod("CreateInstance"));
    }

    [Fact(DisplayName = "No.60_正常系：正常値の確認（string型）_\"field_data_type\"が\"string_all\"")]
    public void Case060()
    {
        var properties = JObject.Parse(@"
        {
            ""input"": ""TEST_INPUT"",
            ""measurement"": ""TEST_MEASUREMENT"",
            ""tags"": {
                ""tag1"": ""TEST_TAG1_STR""
            },
            ""use_header"": true,
            ""use_data"": true,
            ""set_usertimestamp"": true,
            ""timestamp_index"": 1,
            ""field_data_type"": ""string_all""
        }
        ");

        var instance = DBInfo.CreateInstance(properties);

        // 結果確認
        Assert.Equal(DBInfo.FieldDataTypeMode.string_all, instance.FieldDataType);
        AssertLogContains(LoggerHelper.EndMethod("CreateInstance"));
    }

    [Fact(DisplayName = "No.134_正常系：正常値の確認（string型）_\"field_data_type\"が\"AUTOMATIC\"")]
    public void Case134()
    {
        var properties = JObject.Parse(@"
        {
            ""input"": ""TEST_INPUT"",
            ""measurement"": ""TEST_MEASUREMENT"",
            ""tags"": {
                ""tag1"": ""TEST_TAG1_STR""
            },
            ""use_header"": true,
            ""use_data"": true,
            ""set_usertimestamp"": true,
            ""timestamp_index"": 1,
            ""field_data_type"": ""AUTOMATIC""
        }
        ");

        var instance = DBInfo.CreateInstance(properties);

        // 結果確認
        Assert.Equal(DBInfo.FieldDataTypeMode.automatic, instance.FieldDataType);
        AssertLogContains(LoggerHelper.EndMethod("CreateInstance"));
    }

    [Fact(DisplayName = "No.135_正常系：正常値の確認（string型）_\"field_data_type\"が\"STRING_ALL\"")]
    public void Case135()
    {
        var properties = JObject.Parse(@"
        {
            ""input"": ""TEST_INPUT"",
            ""measurement"": ""TEST_MEASUREMENT"",
            ""tags"": {
                ""tag1"": ""TEST_TAG1_STR""
            },
            ""use_header"": true,
            ""use_data"": true,
            ""set_usertimestamp"": true,
            ""timestamp_index"": 1,
            ""field_data_type"": ""STRING_ALL""
        }
        ");

        var instance = DBInfo.CreateInstance(properties);

        // 結果確認
        Assert.Equal(DBInfo.FieldDataTypeMode.string_all, instance.FieldDataType);
        AssertLogContains(LoggerHelper.EndMethod("CreateInstance"));
    }
}
