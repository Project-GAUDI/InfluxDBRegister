namespace IotedgeV2InfluxDBRegister.Test;

public class DBInfo_CreateInstance
{
    [Fact(DisplayName = "No.48 引数 jobj に以下の空のJSONオブジェクトが設定される")]
    public void Case048()
    {
        var properties = JObject.Parse(@"{}");

        Assert.ThrowsAny<Exception>(() => DBInfo.CreateInstance(properties));
    }

    [Fact(DisplayName = "No.49 引数 jobj の input プロパティにオブジェクト（非文字列値）が設定される")]
    public void Case049()
    {
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

        Assert.ThrowsAny<Exception>(() => DBInfo.CreateInstance(properties));
    }

    [Fact(DisplayName = "No.50 引数 jobj の measurement プロパティにオブジェクト（非文字列）が設定される")]
    public void Case050()
    {
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

        Assert.ThrowsAny<Exception>(() => DBInfo.CreateInstance(properties));
    }

    [Fact(DisplayName = "No.51 引数 jobj の tags プロパティに空オブジェクトが設定される")]
    public void Case051()
    {
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

        Assert.ThrowsAny<Exception>(() => DBInfo.CreateInstance(properties));
    }

    [Fact(DisplayName = "No.52 引数 jobj の tags.tag1 プロパティにJSON配列（非文字列 or オブジェクト）が設定される")]
    public void Case052()
    {
        var properties = JObject.Parse(@"
        {
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
        ");

        Assert.ThrowsAny<Exception>(() => DBInfo.CreateInstance(properties));
    }

    [Fact(DisplayName = "No.53 引数 jobj の tags.tag1.tag_name プロパティにオブジェクト（非文字列）が設定される")]
    public void Case053()
    {
        var properties = JObject.Parse(@"
        {
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
        ");

        Assert.ThrowsAny<Exception>(() => DBInfo.CreateInstance(properties));
    }

    [Fact(DisplayName = "No.54 引数 jobj の tags.tag1.value_type プロパティにオブジェクト（非文字列）が設定される")]
    public void Case054()
    {
        var properties = JObject.Parse(@"
        {
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
        ");

        Assert.ThrowsAny<Exception>(() => DBInfo.CreateInstance(properties));
    }

    [Fact(DisplayName = "No.55 引数 jobj の tags.tag1.value_type プロパティに受け入れ不可能な文字列（\"MessagePropertyAndRowIndex\", \"MessageProperty\"以外の文字列）が設定される")]
    public void Case055()
    {
        var properties = JObject.Parse(@"
        {
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
        ");

        Assert.ThrowsAny<Exception>(() => DBInfo.CreateInstance(properties));
    }

    [Fact(DisplayName = "No.56 引数 jobj の use_header プロパティにオブジェクト（非真偽値）が設定される")]
    public void Case056()
    {
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

        Assert.ThrowsAny<Exception>(() => DBInfo.CreateInstance(properties));
    }

    [Fact(DisplayName = "No.57 引数 jobj の use_data プロパティにオブジェクト（非真偽値）が設定される")]
    public void Case057()
    {
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

        Assert.ThrowsAny<Exception>(() => DBInfo.CreateInstance(properties));
    }

    [Fact(DisplayName = "No.58 引数 jobj の set_usertimestamp プロパティにオブジェクト（非真偽値）が設定される")]
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
            ""set_usertimestamp"": {}
        }
        ");

        Assert.ThrowsAny<Exception>(() => DBInfo.CreateInstance(properties));
    }

    [Fact(DisplayName = "No.59 引数 jobj の set_usertimestamp プロパティに true が設定されている、かつ、timestamp_index プロパティにオブジェクト（非整数値）が設定される")]
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
            ""timestamp_index"": ""INVALID_TYPE""
        }
        ");

        Assert.ThrowsAny<Exception>(() => DBInfo.CreateInstance(properties));
    }

    [Fact(DisplayName = "No.60 引数 jobj の set_usertimestamp プロパティに true が設定されている、かつ、timestamp_index プロパティに0（0以下の値）が設定される")]
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
            ""timestamp_index"": 0
        }
        ");

        Assert.ThrowsAny<Exception>(() => DBInfo.CreateInstance(properties));
    }

    [Fact(DisplayName = "No.61 引数 jobj の field_data_type プロパティにオブジェクト（非文字列）が設定される")]
    public void Case061()
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
            ""field_data_type"": {}
        }
        ");

        Assert.ThrowsAny<Exception>(() => DBInfo.CreateInstance(properties));
    }

    [Fact(DisplayName = "No.62 引数 jobj のfield_data_type プロパティに受け入れ不可能な文字列（\"automatic\", \"string_all\"以外の文字列）が設定される")]
    public void Case062()
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
            ""timestamp_index"": 0,
            ""field_data_type"": ""INVALID_VALUE""
        }
        ");

        Assert.ThrowsAny<Exception>(() => DBInfo.CreateInstance(properties));
    }

    [Fact(DisplayName = "No.63 引数 jobj に 受け入れ可能な形のJSONオブジェクトが設定される（\"set_usertimestamp\"プロパティがfalse, \"field_data_type\"プロパティの指定がないパターン）")]
    public void Case063()
    {
        var properties = JObject.Parse(@"
        {
            ""input"": ""TEST_INPUT1"",
            ""measurement"": ""TEST_MEASUREMENT"",
            ""tags"": {
                ""tag1"": ""TEST_TAG1_STR""
            },
            ""use_header"": true,
            ""use_data"": true,
            ""set_usertimestamp"": false
        }
        ");

        var instance = DBInfo.CreateInstance(properties);
        Assert.NotNull(instance);
        Assert.IsType<DBInfo>(instance);

        var actualJson = JsonConvert.SerializeObject(instance, Formatting.None);
        var expectedJson = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(@"
        {
            ""Input"": ""TEST_INPUT1"",
            ""Measurement"": ""TEST_MEASUREMENT"",
            ""TagList"": [
                {
                    ""TagName"": ""TEST_TAG1_STR""
                }
            ],
            ""UseHeader"": true,
            ""UseData"": true,
            ""SetUserTimestamp"": false,
            ""TimestampType"": 0,
            ""TimestampIndex"": 0,
            ""FieldDataType"": 0
        }
        "), Formatting.None);
        Assert.Equal(expectedJson, actualJson);
    }

    [Fact(DisplayName = "No.64 引数 jobj に 受け入れ可能な形のJSONオブジェクトが設定される（\"set_usertimestamp\"プロパティがtrue, \"field_data_type\"プロパティの指定がないパターン）")]
    public void Case064()
    {
        var properties = JObject.Parse(@"
        {
            ""input"": ""TEST_INPUT1"",
            ""measurement"": ""TEST_MEASUREMENT"",
            ""tags"": {
                ""tag1"": ""TEST_TAG1_STR""
            },
            ""use_header"": true,
            ""use_data"": true,
            ""set_usertimestamp"": true,
            ""timestamp_index"": 2
        }
        ");

        var instance = DBInfo.CreateInstance(properties);
        Assert.NotNull(instance);
        Assert.IsType<DBInfo>(instance);

        var actualJson = JsonConvert.SerializeObject(instance, Formatting.None);
        var expectedJson = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(@"
        {
            ""Input"": ""TEST_INPUT1"",
            ""Measurement"": ""TEST_MEASUREMENT"",
            ""TagList"": [
                {
                    ""TagName"": ""TEST_TAG1_STR""
                }
            ],
            ""UseHeader"": true,
            ""UseData"": true,
            ""SetUserTimestamp"": true,
            ""TimestampType"": 1,
            ""TimestampIndex"": 2,
            ""FieldDataType"": 0
        }
        "), Formatting.None);
        Assert.Equal(expectedJson, actualJson);
    }

    [Fact(DisplayName = "No.65 引数 jobj に 受け入れ可能な形のJSONオブジェクトが設定される（\"field_data_type\"プロパティに\"automatic\"が指定されるパターン）")]
    public void Case065()
    {
        var properties = JObject.Parse(@"
        {
            ""input"": ""TEST_INPUT1"",
            ""measurement"": ""TEST_MEASUREMENT"",
            ""tags"": {
                ""tag1"": ""TEST_TAG1_STR""
            },
            ""use_header"": true,
            ""use_data"": true,
            ""set_usertimestamp"": true,
            ""timestamp_index"": 2,
            ""field_data_type"": ""automatic""
        }
        ");

        var instance = DBInfo.CreateInstance(properties);
        Assert.NotNull(instance);
        Assert.IsType<DBInfo>(instance);

        var actualJson = JsonConvert.SerializeObject(instance, Formatting.None);
        var expectedJson = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(@"
        {
            ""Input"": ""TEST_INPUT1"",
            ""Measurement"": ""TEST_MEASUREMENT"",
            ""TagList"": [
                {
                    ""TagName"": ""TEST_TAG1_STR""
                }
            ],
            ""UseHeader"": true,
            ""UseData"": true,
            ""SetUserTimestamp"": true,
            ""TimestampType"": 1,
            ""TimestampIndex"": 2,
            ""FieldDataType"": 0
        }
        "), Formatting.None);
        Assert.Equal(expectedJson, actualJson);
    }

    [Fact(DisplayName = "No.66 引数 jobj に 受け入れ可能な形のJSONオブジェクトが設定される（\"field_data_type\"プロパティに\"string_all\"が指定されるパターン）")]
    public void Case066()
    {
        var properties = JObject.Parse(@"
        {
            ""input"": ""TEST_INPUT1"",
            ""measurement"": ""TEST_MEASUREMENT"",
            ""tags"": {
                ""tag1"": ""TEST_TAG1_STR""
            },
            ""use_header"": true,
            ""use_data"": true,
            ""set_usertimestamp"": true,
            ""timestamp_index"": 2,
            ""field_data_type"": ""string_all""
        }
        ");

        var instance = DBInfo.CreateInstance(properties);
        Assert.NotNull(instance);
        Assert.IsType<DBInfo>(instance);

        var actualJson = JsonConvert.SerializeObject(instance, Formatting.None);
        var expectedJson = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(@"
        {
            ""Input"": ""TEST_INPUT1"",
            ""Measurement"": ""TEST_MEASUREMENT"",
            ""TagList"": [
                {
                    ""TagName"": ""TEST_TAG1_STR""
                }
            ],
            ""UseHeader"": true,
            ""UseData"": true,
            ""SetUserTimestamp"": true,
            ""TimestampType"": 1,
            ""TimestampIndex"": 2,
            ""FieldDataType"": 1
        }
        "), Formatting.None);
        Assert.Equal(expectedJson, actualJson);
    }

    #region 単体テスト仕様書外の既存テスト
    [Fact(DisplayName = "正常系:DBInfoインスタンス生成")]
    public void SimpleValue_DBInfoCreated()
    {
        //set_usertimestampがある:正常
        JObject inputRouteMsg = JObject.Parse("{\"input\": \"cfrOut1\",\"measurement\": \"measurement1\",\"use_header\": false,\"use_data\": true,\"set_usertimestamp\": true,\"timestamp_index\": 1,\"usertimestamp\": \"2022/02/22 10:00:00\",\"tags\": {\"tag1\": \"col1\",\"tag2\": {\"tag_name\": \"col2\",\"value_type\": \"MessageProperty\"}},\"field_data_type\": \"automatic\"}");
        DBInfo result = DBInfo.CreateInstance(inputRouteMsg);
        Assert.IsAssignableFrom<DBInfo>(result);
    }

    [Fact(DisplayName = "異常系:set_usertimestampがない → 例外")]
    public void set_usertimestamp_notexist_ExceptionThrown()
    {
        JObject inputRouteMsg = JObject.Parse("{\"input\": \"cfrOut1\",\"measurement\": \"measurement1\",\"use_header\": false,\"use_data\": true,\"timestamp_index\": 1,\"usertimestamp\": \"2022/02/22 10:00:00\",\"tags\": {\"tag1\": \"col1\",\"tag2\": {\"tag_name\": \"col2\",\"value_type\": \"MessageProperty\"}},\"field_data_type\": \"automatic\"}");
        Assert.Throws<Exception>(() => DBInfo.CreateInstance(inputRouteMsg));
    }

    [Fact(DisplayName = "正常系:set_usertimestampがtrue")]
    public void set_usertimestamp_exist_True()
    {
        JObject inputRouteMsg = JObject.Parse("{\"input\": \"cfrOut1\",\"measurement\": \"measurement1\",\"use_header\": false,\"use_data\": true,\"set_usertimestamp\": true,\"timestamp_index\": 1,\"usertimestamp\": \"2022/02/22 10:00:00\",\"tags\": {\"tag1\": \"col1\",\"tag2\": {\"tag_name\": \"col2\",\"value_type\": \"MessageProperty\"}},\"field_data_type\": \"automatic\"}");
        DBInfo result = DBInfo.CreateInstance(inputRouteMsg);
        Assert.True(result.SetUserTimestamp);
    }

    [Fact(DisplayName = "正常系:set_usertimestampがfalse")]
    public void set_usertimestamp_exist_False()
    {
        JObject inputRouteMsg = JObject.Parse("{\"input\": \"cfrOut1\",\"measurement\": \"measurement1\",\"use_header\": false,\"use_data\": true,\"set_usertimestamp\": false,\"timestamp_index\": 1,\"usertimestamp\": \"2022/02/22 10:00:00\",\"tags\": {\"tag1\": \"col1\",\"tag2\": {\"tag_name\": \"col2\",\"value_type\": \"MessageProperty\"}},\"field_data_type\": \"automatic\"}");
        DBInfo result = DBInfo.CreateInstance(inputRouteMsg);
        Assert.False(result.SetUserTimestamp);
    }

    [Fact(DisplayName = "正常系:set_usertimestampがtrue,timestamp_indexがある,usertimestampがない → timestamp_indexが反映")]
    public void timestamp_index_exist_TypeIndex()
    {
        JObject inputRouteMsg = JObject.Parse("{\"input\": \"cfrOut1\",\"measurement\": \"measurement1\",\"use_header\": false,\"use_data\": true,\"set_usertimestamp\": true,\"timestamp_index\": 1,\"tags\": {\"tag1\": \"col1\",\"tag2\": {\"tag_name\": \"col2\",\"value_type\": \"MessageProperty\"}},\"field_data_type\": \"automatic\"}");
        DBInfo result = DBInfo.CreateInstance(inputRouteMsg);
        Assert.Equal(1, result.TimestampIndex);
        Assert.Equal(DBInfo.UserTimestampType.timestamp_index, result.TimestampType);
    }

    [Fact(DisplayName = "正常系:set_usertimestampがtrue,timestamp_indexがある,usertimestampがある → timestamp_indexが反映")]
    public void eachtimestamp_exist_TypeIndex()
    {
        JObject inputRouteMsg = JObject.Parse("{\"input\": \"cfrOut1\",\"measurement\": \"measurement1\",\"use_header\": false,\"use_data\": true,\"set_usertimestamp\": true,\"timestamp_index\": 1,\"usertimestamp\": \"2022/02/22 10:00:00\",\"tags\": {\"tag1\": \"col1\",\"tag2\": {\"tag_name\": \"col2\",\"value_type\": \"MessageProperty\"}},\"field_data_type\": \"automatic\"}");
        DBInfo result = DBInfo.CreateInstance(inputRouteMsg);
        Assert.Equal(1, result.TimestampIndex);
        Assert.Equal(DBInfo.UserTimestampType.timestamp_index, result.TimestampType);
    }

    [Fact(DisplayName = "異常系:set_usertimestampがtrue,timestamp_indexがない,usertimestampがない → 例外")]
    public void neithertimestamp_exist_ExceptionThrown()
    {
        JObject inputRouteMsg = JObject.Parse("{\"input\": \"cfrOut1\",\"measurement\": \"measurement1\",\"use_header\": false,\"use_data\": true,\"set_usertimestamp\": true,\"tags\": {\"tag1\": \"col1\",\"tag2\": {\"tag_name\": \"col2\",\"value_type\": \"MessageProperty\"}},\"field_data_type\": \"automatic\"}");
        Assert.Throws<Exception>(() => DBInfo.CreateInstance(inputRouteMsg));
    }

    //set_usertimestampがtrue,timestamp_indexが1:正常
    [Fact(DisplayName = "正常系:set_usertimestampがtrue,timestamp_indexが1")]
    public void timestamp_index_Value1()
    {
        JObject inputRouteMsg = JObject.Parse("{\"input\": \"cfrOut1\",\"measurement\": \"measurement1\",\"use_header\": false,\"use_data\": true,\"set_usertimestamp\": true,\"timestamp_index\": 1,\"usertimestamp\": \"2022/02/22 10:00:00\",\"tags\": {\"tag1\": \"col1\",\"tag2\": {\"tag_name\": \"col2\",\"value_type\": \"MessageProperty\"}},\"field_data_type\": \"automatic\"}");
        DBInfo result = DBInfo.CreateInstance(inputRouteMsg);
        Assert.Equal(1, result.TimestampIndex);
    }

    //set_usertimestampがtrue,timestamp_indexが0:異常
    [Fact(DisplayName = "異常系:set_usertimestampがtrue,timestamp_indexが0")]
    public void timestamp_index_Value0_ExceptionThrown()
    {
        JObject inputRouteMsg = JObject.Parse("{\"input\": \"cfrOut1\",\"measurement\": \"measurement1\",\"use_header\": false,\"use_data\": true,\"set_usertimestamp\": true,\"timestamp_index\": 0,\"usertimestamp\": \"2022/02/22 10:00:00\",\"tags\": {\"tag1\": \"col1\",\"tag2\": {\"tag_name\": \"col2\",\"value_type\": \"MessageProperty\"}},\"field_data_type\": \"automatic\"}");
        Assert.Throws<Exception>(() => DBInfo.CreateInstance(inputRouteMsg));
    }

    [Fact(DisplayName = "異常系:set_usertimestampがtrue,timestamp_indexがABC")]
    public void timestamp_index_ValueAny_ExceptionThrown()
    {
        JObject inputRouteMsg = JObject.Parse("{\"input\": \"cfrOut1\",\"measurement\": \"measurement1\",\"use_header\": false,\"use_data\": true,\"set_usertimestamp\": true,\"timestamp_index\": \"ABC\",\"usertimestamp\": \"2022/02/22 10:00:00\",\"tags\": {\"tag1\": \"col1\",\"tag2\": {\"tag_name\": \"col2\",\"value_type\": \"MessageProperty\"}},\"field_data_type\": \"automatic\"}");
        Assert.Throws<FormatException>(() => DBInfo.CreateInstance(inputRouteMsg));
    }

    [Fact(DisplayName = "正常系:field_data_typeがない")]
    public void field_data_type_notexist_DefaultVal()
    {
        JObject inputRouteMsg = JObject.Parse("{\"input\": \"cfrOut1\",\"measurement\": \"measurement1\",\"use_header\": false,\"use_data\": true,\"set_usertimestamp\": true,\"timestamp_index\": 1,\"usertimestamp\": \"2022/02/22 10:00:00\",\"tags\": {\"tag1\": \"col1\",\"tag2\": {\"tag_name\": \"col2\",\"value_type\": \"MessageProperty\"}}}");
        DBInfo result = DBInfo.CreateInstance(inputRouteMsg);
        Assert.Equal(DBInfo.FieldDataTypeMode.automatic, result.FieldDataType);
    }

    [Fact(DisplayName = "正常系:field_data_typeがstring_all")]
    public void field_data_type_exist_string_all()
    {
        JObject inputRouteMsg = JObject.Parse("{\"input\": \"cfrOut1\",\"measurement\": \"measurement1\",\"use_header\": false,\"use_data\": true,\"set_usertimestamp\": true,\"timestamp_index\": 1,\"usertimestamp\": \"2022/02/22 10:00:00\",\"tags\": {\"tag1\": \"col1\",\"tag2\": {\"tag_name\": \"col2\",\"value_type\": \"MessageProperty\"}},\"field_data_type\": \"string_all\"}");
        DBInfo result = DBInfo.CreateInstance(inputRouteMsg);
        Assert.Equal(DBInfo.FieldDataTypeMode.string_all, result.FieldDataType);
    }

    [Fact(DisplayName = "正常系:field_data_typeがautomatic")]
    public void field_data_type_exist_automatic()
    {
        JObject inputRouteMsg = JObject.Parse("{\"input\": \"cfrOut1\",\"measurement\": \"measurement1\",\"use_header\": false,\"use_data\": true,\"set_usertimestamp\": true,\"timestamp_index\": 1,\"usertimestamp\": \"2022/02/22 10:00:00\",\"tags\": {\"tag1\": \"col1\",\"tag2\": {\"tag_name\": \"col2\",\"value_type\": \"MessageProperty\"}},\"field_data_type\": \"automatic\"}");
        DBInfo result = DBInfo.CreateInstance(inputRouteMsg);
        Assert.Equal(DBInfo.FieldDataTypeMode.automatic, result.FieldDataType);
    }

    [Fact(DisplayName = "異常系:field_data_typeがABC")]
    public void field_data_type_anystring_ExceptionTrown()
    {
        JObject inputRouteMsg = JObject.Parse("{\"input\": \"cfrOut1\",\"measurement\": \"measurement1\",\"use_header\": false,\"use_data\": true,\"set_usertimestamp\": true,\"timestamp_index\": 1,\"usertimestamp\": \"2022/02/22 10:00:00\",\"tags\": {\"tag1\": \"col1\",\"tag2\": {\"tag_name\": \"col2\",\"value_type\": \"MessageProperty\"}},\"field_data_type\": \"ABC\"}");
        Assert.Throws<Exception>(() => DBInfo.CreateInstance(inputRouteMsg));
    }

    //field_data_typeが0:異常
    [Fact(DisplayName = "異常系:field_data_typeが0")]
    public void field_data_type_anynumber_ExceptionTrown()
    {
        JObject inputRouteMsg = JObject.Parse("{\"input\": \"cfrOut1\",\"measurement\": \"measurement1\",\"use_header\": false,\"use_data\": true,\"set_usertimestamp\": true,\"timestamp_index\": 1,\"usertimestamp\": \"2022/02/22 10:00:00\",\"tags\": {\"tag1\": \"col1\",\"tag2\": {\"tag_name\": \"col2\",\"value_type\": \"MessageProperty\"}},\"field_data_type\": 0}");
        Assert.Throws<Exception>(() => DBInfo.CreateInstance(inputRouteMsg));
    }
    #endregion
}
