namespace IotedgeV2InfluxDBRegister.Test;

[Collection(nameof(CollectionAttribute))]
public class MyApplicationMain_OnDesiredPropertiesReceivedAsync
{
    private readonly MyApplicationMain _app = new ();

    [Fact(DisplayName = "No.27 desiredProperties が空で設定される")]
    public async Task Case027()
    {
        var properties = JObject.Parse("{}");

        var result = await _app.OnDesiredPropertiesReceivedAsync(properties);
        Assert.False(result);
    }

    [Fact(DisplayName = "No.28 desiredProperties の dbparams1 に文字列（非オブジェクト）が設定される")]
    public async Task Case028()
    {
        var properties = JObject.Parse("{ \"dbparam1\": \"\" }");

        var result = await _app.OnDesiredPropertiesReceivedAsync(properties);
        Assert.False(result);
    }

    [Fact(DisplayName = "No.29 desiredProperties の dbparams1 に空オブジェクトが設定される")]
    public async Task Case029()
    {
        var properties = JObject.Parse("{ \"dbparam1\": { } }");

        var result = await _app.OnDesiredPropertiesReceivedAsync(properties);
        Assert.False(result);
    }

    [Fact(DisplayName = "No.30 desiredProperties の dbparams1.input にオブジェクト（非文字列）が設定される")]
    public async Task Case030()
    {
        var properties = JObject.Parse(@"
        {
            ""dbparam1"": {
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
        }
        ");

        var result = await _app.OnDesiredPropertiesReceivedAsync(properties);
        Assert.False(result);
    }

    [Fact(DisplayName = "No.31 desiredProperties の dbparams1.measurement にオブジェクト（非文字列）が設定される")]
    public async Task Case031()
    {
        var properties = JObject.Parse(@"
        {
            ""dbparam1"": {
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
        }
        ");

        var result = await _app.OnDesiredPropertiesReceivedAsync(properties);
        Assert.False(result);
    }

    [Fact(DisplayName = "No.32 desiredProperties の dbparams1.tags に空オブジェクトが設定される")]
    public async Task Case032()
    {
        var properties = JObject.Parse(@"
        {
            ""dbparam1"": {
                ""input"": ""TEST_INPUT"",
                ""measurement"": ""TEST_MEASUREMENT"",
                ""tags"": {},
                ""use_header"": true,
                ""use_data"": true,
                ""set_usertimestamp"": true,
                ""timestamp_index"": 1
            }
        }
        ");

        var result = await _app.OnDesiredPropertiesReceivedAsync(properties);
        Assert.False(result);
    }

    [Fact(DisplayName = "No.33 desiredProperties の dbparams1.tags.tag1 にJSON配列（非文字列 or オブジェクト）が設定される")]
    public async Task Case033()
    {
        var properties = JObject.Parse(@"
        {
            ""dbparam1"": {
                ""input"": ""TEST_INPUT"",
                ""measurement"": ""TEST_MEASUREMENT"",
                ""tags"": {
                    ""tag1"": []
                },
                ""use_header"": true,
                ""use_data"": true,
                ""set_usertimestamp"": true,
                ""timestamp_index"": 1
            }
        }
        ");

        var result = await _app.OnDesiredPropertiesReceivedAsync(properties);
        Assert.False(result);
    }

    [Fact(DisplayName = "No.34 desiredProperties の dbparams1.tags.tag1.tag_name にオブジェクト（非文字列）が設定される")]
    public async Task Case034()
    {
        var properties = JObject.Parse(@"
        {
            ""dbparam1"": {
                ""input"": ""TEST_INPUT"",
                ""measurement"": ""TEST_MEASUREMENT"",
                ""tags"": {
                    ""tag1"": {
                        ""tag_name"": {},
                        ""value_type"": ""MessageProperty""
                    }
                },
                ""use_header"": true,
                ""use_data"": true,
                ""set_usertimestamp"": true,
                ""timestamp_index"": 1
            }
        }
        ");

        var result = await _app.OnDesiredPropertiesReceivedAsync(properties);
        Assert.False(result);
    }

    [Fact(DisplayName = "No.35 desiredProperties の dbparams1.tags.tag1.value_type にオブジェクト（非文字列）が設定される")]
    public async Task Case035()
    {
        var properties = JObject.Parse(@"
        {
            ""dbparam1"": {
                ""input"": ""TEST_INPUT"",
                ""measurement"": ""TEST_MEASUREMENT"",
                ""tags"": {
                    ""tag1"": {
                        ""tag_name"": ""TEST_TAG1_NAME"",
                        ""value_type"": {}
                    }
                },
                ""use_header"": true,
                ""use_data"": true,
                ""set_usertimestamp"": true,
                ""timestamp_index"": 1
            }
        }
        ");

        var result = await _app.OnDesiredPropertiesReceivedAsync(properties);
        Assert.False(result);
    }

    [Fact(DisplayName = "No.36 desiredProperties の dbparams1.tags.tag1.value_type に受け入れ不可能な文字列（\"MessagePropertyAndRowIndex\", \"MessageProperty\"以外の文字列）が設定される")]
    public async Task Case036()
    {
        var properties = JObject.Parse(@"
        {
            ""dbparam1"": {
                ""input"": ""TEST_INPUT"",
                ""measurement"": ""TEST_MEASUREMENT"",
                ""tags"": {
                    ""tag1"": {
                        ""tag_name"": ""TEST_TAG1_NAME"",
                        ""value_type"": ""INVALID_VALUE""
                    }
                },
                ""use_header"": true,
                ""use_data"": true,
                ""set_usertimestamp"": true,
                ""timestamp_index"": 1
            }
        }
        ");

        var result = await _app.OnDesiredPropertiesReceivedAsync(properties);
        Assert.False(result);
    }

    [Fact(DisplayName = "No.37 desiredProperties の dbparams1.use_header にオブジェクト（非真偽値）が設定される")]
    public async Task Case037()
    {
        var properties = JObject.Parse(@"
        {
            ""dbparam1"": {
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
        }
        ");

        var result = await _app.OnDesiredPropertiesReceivedAsync(properties);
        Assert.False(result);
    }

    [Fact(DisplayName = "No.38 desiredProperties の dbparams1.use_data にオブジェクト（非真偽値）が設定される")]
    public async Task Case038()
    {
        var properties = JObject.Parse(@"
        {
            ""dbparam1"": {
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
        }
        ");

        var result = await _app.OnDesiredPropertiesReceivedAsync(properties);
        Assert.False(result);
    }

    [Fact(DisplayName = "No.39 desiredProperties の dbparams1.set_usertimestamp にオブジェクト（非真偽値）が設定される")]
    public async Task Case039()
    {
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
                ""set_usertimestamp"": {}
            }
        }
        ");

        var result = await _app.OnDesiredPropertiesReceivedAsync(properties);
        Assert.False(result);
    }

    [Fact(DisplayName = "No.40 desiredProperties の dbparams1.set_usertimestamp に true が設定されている、かつ、dbparams1.timestamp_index に文字列（非整数値）が設定される")]
    public async Task Case040()
    {
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
                ""timestamp_index"": ""INVALID_TYPE""
            }
        }
        ");

        var result = await _app.OnDesiredPropertiesReceivedAsync(properties);
        Assert.False(result);
    }

    [Fact(DisplayName = "No.41 desiredProperties の dbparams1.set_usertimestamp に true が設定されている、かつ、dbparams1.timestamp_index に0（0以下の値）が設定される")]
    public async Task Case041()
    {
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
                ""timestamp_index"": 0
            }
        }
        ");

        var result = await _app.OnDesiredPropertiesReceivedAsync(properties);
        Assert.False(result);
    }

    [Fact(DisplayName = "No.42 desiredProperties の dbparams1.field_data_type にオブジェクト（非文字列）が設定される")]
    public async Task Case042()
    {
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
                ""timestamp_index"": 1,
                ""field_data_type"": {}
            }
        }
        ");

        var result = await _app.OnDesiredPropertiesReceivedAsync(properties);
        Assert.False(result);
    }

    [Fact(DisplayName = "No.43 desiredProperties の dbparams1.field_data_type に受け入れ不可能な文字列（\"automatic\", \"string_all\"以外の文字列）が設定される")]
    public async Task Case043()
    {
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
                ""timestamp_index"": 0,
                ""field_data_type"": ""INVALID_VALUE""
            }
        }
        ");

        var result = await _app.OnDesiredPropertiesReceivedAsync(properties);
        Assert.False(result);
    }

    [Fact(DisplayName = "No.44,45 desiredProperties に dbparamsx が 1件 受け入れ可能な形で設定される")]
    public async Task Case044()
    {
        var properties = JObject.Parse(@"
        {
            ""dbparam1"": {
                ""input"": ""TEST_INPUT1"",
                ""measurement"": ""TEST_MEASUREMENT"",
                ""tags"": {
                    ""tag1"": ""TEST_TAG1_STR""
                },
                ""use_header"": true,
                ""use_data"": true,
                ""set_usertimestamp"": true,
                ""timestamp_index"": 5
            }
        }
        ");

        var result = await _app.OnDesiredPropertiesReceivedAsync(properties);
        Assert.True(result);

        var settings = typeof(MyApplicationMain).GetProperty("DBInfos", BindingFlags.Static | BindingFlags.NonPublic)!.GetValue(null) as List<DBInfo>;
        Assert.Equal(1, settings?.Count);
    }

    [Fact(DisplayName = "No.46,47 desiredProperties に dbparamsx が 3件 受け入れ可能な形で設定される")]
    public async Task Case046()
    {
        var properties = JObject.Parse(@"
        {
            ""dbparam1"": {
                ""input"": ""TEST_INPUT1"",
                ""measurement"": ""TEST_MEASUREMENT"",
                ""tags"": {
                    ""tag1"": ""TEST_TAG1_STR""
                },
                ""use_header"": true,
                ""use_data"": true,
                ""set_usertimestamp"": true,
                ""timestamp_index"": 5
            },
            ""dbparam2"": {
                ""input"": ""TEST_INPUT2"",
                ""measurement"": ""TEST_MEASUREMENT"",
                ""tags"": {
                    ""tag1"": ""TEST_TAG1_STR""
                },
                ""use_header"": true,
                ""use_data"": true,
                ""set_usertimestamp"": true,
                ""timestamp_index"": 5
            },
            ""dbparam3"": {
                ""input"": ""TEST_INPUT3"",
                ""measurement"": ""TEST_MEASUREMENT"",
                ""tags"": {
                    ""tag1"": ""TEST_TAG1_STR""
                },
                ""use_header"": true,
                ""use_data"": true,
                ""set_usertimestamp"": true,
                ""timestamp_index"": 5
            }
        }
        ");

        var result = await _app.OnDesiredPropertiesReceivedAsync(properties);
        Assert.True(result);

        var settings = typeof(MyApplicationMain).GetProperty("DBInfos", BindingFlags.Static | BindingFlags.NonPublic)!.GetValue(null) as List<DBInfo>;
        Assert.Equal(3, settings?.Count);
    }
}
