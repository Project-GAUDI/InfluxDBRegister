using IotedgeV2InfluxDBRegister.Test.Stub;
using TICO.GAUDI.Commons;

namespace IotedgeV2InfluxDBRegister.Test;

[Collection(nameof(CollectionAttribute))]
public class MyApplicationMain_OnMessageReceivedAsync(ITestOutputHelper output): MetricsCollectorTesterBase(output)
{
    private readonly MyApplicationMain _app = new ();

    private static void SetPropertyValue<T>(T obj, string propName, object value, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
    {
        var type = typeof(T);
        var pi = type.GetProperty(propName, bindingFlags);
        pi!.SetValue(obj, value);
    }

    [Fact(DisplayName = "No.74 引数 message に null が指定される")]
    public async Task Case074()
    {
        var info = new DBInfo();

        var result = await _app.OnMessageReceivedAsync("input", null, info);
        Assert.False(result);
    }

    [Fact(DisplayName = "No.76 引数 userContext にDBInfo以外の型のオブジェクトが指定される")]
    public async Task Case076()
    {
        var message = new IotMessage();

        var result = await _app.OnMessageReceivedAsync("input", message, new object());
        Assert.False(result);
    }

    [Fact(DisplayName = "No.77 timestamp_indexで指定された値が有効な日時形式でないケース")]
    public async Task Case077()
    {
        var message = new IotMessage(@"
        {
            ""RecordList"": [
                {
                    ""RecordHeader"": [
                        ""time""
                    ],
                    ""RecordData"": [
                        ""INVALID_DATETIME_VALUE""
                    ]
                }
            ]
        }
        ");

        var info = new DBInfo();
        SetPropertyValue(info, nameof(DBInfo.TimestampType), (byte)1);
        SetPropertyValue(info, nameof(DBInfo.TimestampIndex), 1);

        var result = await _app.OnMessageReceivedAsync("input", message, info);
        Assert.False(result);
    }

    [Fact(DisplayName = "No.78,79 messageに指定された文字列から登録用メッセージへのパースに失敗するケース")]
    public async Task Case078()
    {
        const string MEASUREMENT = "TEST_MEASUREMENT";
        var message = new IotMessage(@"Invalid Format String");

        var info = new DBInfo();
        SetPropertyValue(info, nameof(DBInfo.Measurement), MEASUREMENT);

        var result = await _app.OnMessageReceivedAsync("input", message, info);
        Assert.True(result);

        _collector.ReviewCalledArguments(WriteMethodArguments.Parse(
            MEASUREMENT,
            new Dictionary<string, object>() { { "1", "\"\"" } },
            new Dictionary<string, string>(),
            null
        ));
    }

    [Fact(DisplayName = "No.80,81 messageにRecordListプロパティが存在せず、登録用メッセージへのパースに失敗するケース")]
    public async Task Case080()
    {
        const string MEASUREMENT = "TEST_MEASUREMENT";
        var message = new IotMessage(@"{}");

        var info = new DBInfo();
        SetPropertyValue(info, nameof(DBInfo.Measurement), MEASUREMENT);

        var result = await _app.OnMessageReceivedAsync("input", message, info);
        Assert.True(result);

        _collector.ReviewCalledArguments(WriteMethodArguments.Parse(
            MEASUREMENT,
            new Dictionary<string, object>() { { "1", "\"\"" } },
            new Dictionary<string, string>(),
            null
        ));
    }

    [Fact(DisplayName = "No.82,83 messageのRecordListプロパティが配列でなく、登録用メッセージへのパースに失敗するケース")]
    public async Task Case082()
    {
        const string MEASUREMENT = "TEST_MEASUREMENT";
        var message = new IotMessage(@"
        {
            ""RecordList"": {
                ""invalidFormat"": """"
            }
        }
        ");

        var info = new DBInfo();
        SetPropertyValue(info, nameof(DBInfo.Measurement), MEASUREMENT);

        var result = await _app.OnMessageReceivedAsync("input", message, info);
        Assert.True(result);

        _collector.ReviewCalledArguments(WriteMethodArguments.Parse(
            MEASUREMENT,
            new Dictionary<string, object>() { { "1", "\"\"" } },
            new Dictionary<string, string>(),
            null
        ));
    }

    [Fact(DisplayName = "No.84,85 messageのRecordList.RecordHeaderプロパティが存在せず、登録用メッセージへのパースに失敗するケース")]
    public async Task Case084()
    {
        const string MEASUREMENT = "TEST_MEASUREMENT";
        var message = new IotMessage(@"
        {
            ""RecordList"": [
                {
                    ""headerNotFound"": """"
                }
            ]
        }
        ");

        var info = new DBInfo();
        SetPropertyValue(info, nameof(DBInfo.Measurement), MEASUREMENT);

        var result = await _app.OnMessageReceivedAsync("input", message, info);
        Assert.True(result);

        _collector.ReviewCalledArguments(WriteMethodArguments.Parse(
            MEASUREMENT,
            new Dictionary<string, object>() { { "1", "\"\"" } },
            new Dictionary<string, string>(),
            null
        ));
    }

    [Fact(DisplayName = "No.86,87 messageのRecordList.RecordHeaderプロパティが配列でなく、登録用メッセージへのパースに失敗するケース")]
    public async Task Case086()
    {
        const string MEASUREMENT = "TEST_MEASUREMENT";
        var message = new IotMessage(@"
        {
            ""RecordList"": [
                {
                    ""RecordHeader"": ""INVALID_VALUE_FORMAT""
                }
            ]
        }
        ");

        var info = new DBInfo();
        SetPropertyValue(info, nameof(DBInfo.Measurement), MEASUREMENT);

        var result = await _app.OnMessageReceivedAsync("input", message, info);
        Assert.True(result);

        _collector.ReviewCalledArguments(WriteMethodArguments.Parse(
            MEASUREMENT,
            new Dictionary<string, object>() { { "1", "\"\"" } },
            new Dictionary<string, string>(),
            null
        ));
    }

    [Fact(DisplayName = "No.88,89 messageのRecordList.RecordHeaderプロパティに値型以外が指定されていて、登録用メッセージへのパースに失敗するケース")]
    public async Task Case088()
    {
        const string MEASUREMENT = "TEST_MEASUREMENT";
        var message = new IotMessage(@"
        {
            ""RecordList"": [
                {
                    ""RecordHeader"": [
                        {}
                    ]
                }
            ]
        }
        ");

        var info = new DBInfo();
        SetPropertyValue(info, nameof(DBInfo.Measurement), MEASUREMENT);

        var result = await _app.OnMessageReceivedAsync("input", message, info);
        Assert.True(result);

        _collector.ReviewCalledArguments(WriteMethodArguments.Parse(
            MEASUREMENT,
            new Dictionary<string, object>() { { "1", "\"\"" } },
            new Dictionary<string, string>(),
            null
        ));
    }

    [Fact(DisplayName = "No.90,91 messageのRecordList.RecordDataプロパティが存在せず、登録用メッセージへのパースに失敗するケース")]
    public async Task Case090()
    {
        const string MEASUREMENT = "TEST_MEASUREMENT";
        var message = new IotMessage(@"
        {
            ""RecordList"": [
                {
                    ""RecordHeader"": [],
                    ""dataNotFound"": """"
                }
            ]
        }
        ");

        var info = new DBInfo();
        SetPropertyValue(info, nameof(DBInfo.Measurement), MEASUREMENT);

        var result = await _app.OnMessageReceivedAsync("input", message, info);
        Assert.True(result);

        _collector.ReviewCalledArguments(WriteMethodArguments.Parse(
            MEASUREMENT,
            new Dictionary<string, object>() { { "1", "\"\"" } },
            new Dictionary<string, string>(),
            null
        ));
    }

    [Fact(DisplayName = "No.92,93 messageのRecordList.RecordDataプロパティが配列でなく、登録用メッセージへのパースに失敗するケース")]
    public async Task Case092()
    {
        const string MEASUREMENT = "TEST_MEASUREMENT";
        var message = new IotMessage(@"
        {
            ""RecordList"": [
                {
                    ""RecordHeader"": [],
                    ""RecordData"": ""INVALID_VALUE_FORMAT""
                }
            ]
        }
        ");

        var info = new DBInfo();
        SetPropertyValue(info, nameof(DBInfo.Measurement), MEASUREMENT);

        var result = await _app.OnMessageReceivedAsync("input", message, info);
        Assert.True(result);

        _collector.ReviewCalledArguments(WriteMethodArguments.Parse(
            MEASUREMENT,
            new Dictionary<string, object>() { { "1", "\"\"" } },
            new Dictionary<string, string>(),
            null
        ));
    }

    [Fact(DisplayName = "No.94,95 messageのRecordList.RecordDataプロパティに値型以外が指定されていて、登録用メッセージへのパースに失敗するケース")]
    public async Task Case094()
    {
        const string MEASUREMENT = "TEST_MEASUREMENT";
        var message = new IotMessage(@"
        {
            ""RecordList"": [
                {
                    ""RecordHeader"": [],
                    ""RecordData"": [
                        {}
                    ]
                }
            ]
        }
        ");

        var info = new DBInfo();
        SetPropertyValue(info, nameof(DBInfo.Measurement), MEASUREMENT);

        var result = await _app.OnMessageReceivedAsync("input", message, info);
        Assert.True(result);

        _collector.ReviewCalledArguments(WriteMethodArguments.Parse(
            MEASUREMENT,
            new Dictionary<string, object>() { { "1", "\"\"" } },
            new Dictionary<string, string>(),
            null
        ));
    }

    [Fact(DisplayName = "No.96,97 正常登録ケース")]
    public async Task Case096()
    {
        const string MEASUREMENT = "TEST_MEASUREMENT";
        var message = new IotMessage(@"
        {
            ""RecordList"": [
                {
                    ""RecordHeader"": [
                        ""head1-1""
                    ],
                    ""RecordData"": [
                        1
                    ]
                },
                {
                    ""RecordHeader"": [
                        ""head2-1""
                    ],
                    ""RecordData"": [
                        2
                    ]
                }
            ]
        }
        ");
        message.SetProperty("TEST_TAG1", "tag1");
        message.SetProperty("TEST_TAG3", "5");

        var info = new DBInfo();
        SetPropertyValue(info, nameof(DBInfo.Input), "TEST_INPUT1");
        SetPropertyValue(info, nameof(DBInfo.Measurement), MEASUREMENT);
        var tag = new TagInfo();
        SetPropertyValue(tag, nameof(TagInfo.TagName), "TEST_TAG1");
        SetPropertyValue(tag, "TagValueType", 0, BindingFlags.Instance | BindingFlags.NonPublic);
        info.TagList.Add(tag);
        tag = new TagInfo();
        SetPropertyValue(tag, nameof(TagInfo.TagName), "TEST_TAG2");
        SetPropertyValue(tag, "TagValueType", 0, BindingFlags.Instance | BindingFlags.NonPublic);
        info.TagList.Add(tag);
        tag = new TagInfo();
        SetPropertyValue(tag, nameof(TagInfo.TagName), "TEST_TAG3");
        SetPropertyValue(tag, "TagValueType", 1, BindingFlags.Instance | BindingFlags.NonPublic);
        info.TagList.Add(tag);
        tag = new TagInfo();
        SetPropertyValue(tag, nameof(TagInfo.TagName), "TEST_TAG4");
        SetPropertyValue(tag, "TagValueType", 1, BindingFlags.Instance | BindingFlags.NonPublic);
        info.TagList.Add(tag);
        SetPropertyValue(info, nameof(DBInfo.UseHeader), true);
        SetPropertyValue(info, nameof(DBInfo.UseData), true);
        SetPropertyValue(info, nameof(DBInfo.SetUserTimestamp), false);
        SetPropertyValue(info, nameof(DBInfo.TimestampType), (byte)0);
        SetPropertyValue(info, nameof(DBInfo.TimestampIndex), 0);
        SetPropertyValue(info, nameof(DBInfo.FieldDataType), (byte)0);

        var result = await _app.OnMessageReceivedAsync("input", message, info);
        Assert.True(result);

        _collector.ReviewCalledArguments([
            WriteMethodArguments.Parse(
                MEASUREMENT,
                new Dictionary<string, object>()
                {
                    { "1", "head1-1" },
                    { "2", 1 }
                },
                new Dictionary<string, string>()
                {
                    { "TEST_TAG1", "tag1" },
                    { "TEST_TAG2", "" },
                    { "TEST_TAG3", "5" },
                    { "TEST_TAG4", "" }
                },
                null
            ),
            WriteMethodArguments.Parse(
                MEASUREMENT,
                new Dictionary<string, object>()
                {
                    { "1", "head2-1" },
                    { "2", 2 }
                },
                new Dictionary<string, string>()
                {
                    { "TEST_TAG1", "tag1" },
                    { "TEST_TAG2", "" },
                    { "TEST_TAG3", "6" },
                    { "TEST_TAG4", "" }
                },
                null
            )
        ]);
    }

    [Fact(DisplayName = "No.98,99 正常登録ケース（useHeaderがfalse）")]
    public async Task Case098()
    {
        const string MEASUREMENT = "TEST_MEASUREMENT";
        var message = new IotMessage(@"
        {
            ""RecordList"": [
                {
                    ""RecordHeader"": [
                        ""head1-1""
                    ],
                    ""RecordData"": [
                        1
                    ]
                },
                {
                    ""RecordHeader"": [
                        ""head2-1""
                    ],
                    ""RecordData"": [
                        2
                    ]
                }
            ]
        }
        ");

        var info = new DBInfo();
        SetPropertyValue(info, nameof(DBInfo.Input), "TEST_INPUT1");
        SetPropertyValue(info, nameof(DBInfo.Measurement), MEASUREMENT);
        SetPropertyValue(info, nameof(DBInfo.UseHeader), false);
        SetPropertyValue(info, nameof(DBInfo.UseData), true);
        SetPropertyValue(info, nameof(DBInfo.SetUserTimestamp), false);
        SetPropertyValue(info, nameof(DBInfo.TimestampType), (byte)0);
        SetPropertyValue(info, nameof(DBInfo.TimestampIndex), 0);
        SetPropertyValue(info, nameof(DBInfo.FieldDataType), (byte)0);

        var result = await _app.OnMessageReceivedAsync("input", message, info);
        Assert.True(result);

        _collector.ReviewCalledArguments([
            WriteMethodArguments.Parse(
                MEASUREMENT,
                new Dictionary<string, object>()
                {
                    { "1", 1 }
                },
                new Dictionary<string, string>(),
                null
            ),
            WriteMethodArguments.Parse(
                MEASUREMENT,
                new Dictionary<string, object>()
                {
                    { "1", 2 }
                },
                new Dictionary<string, string>(),
                null
            )
        ]);
    }

    [Fact(DisplayName = "No.100,101 正常登録ケース（useDataがfalse）")]
    public async Task Case100()
    {
        const string MEASUREMENT = "TEST_MEASUREMENT";
        var message = new IotMessage(@"
        {
            ""RecordList"": [
                {
                    ""RecordHeader"": [
                        ""head1-1""
                    ],
                    ""RecordData"": [
                        1
                    ]
                },
                {
                    ""RecordHeader"": [
                        ""head2-1""
                    ],
                    ""RecordData"": [
                        2
                    ]
                }
            ]
        }
        ");

        var info = new DBInfo();
        SetPropertyValue(info, nameof(DBInfo.Input), "TEST_INPUT1");
        SetPropertyValue(info, nameof(DBInfo.Measurement), MEASUREMENT);
        SetPropertyValue(info, nameof(DBInfo.UseHeader), true);
        SetPropertyValue(info, nameof(DBInfo.UseData), false);
        SetPropertyValue(info, nameof(DBInfo.SetUserTimestamp), false);
        SetPropertyValue(info, nameof(DBInfo.TimestampType), (byte)0);
        SetPropertyValue(info, nameof(DBInfo.TimestampIndex), 0);
        SetPropertyValue(info, nameof(DBInfo.FieldDataType), (byte)0);

        var result = await _app.OnMessageReceivedAsync("input", message, info);
        Assert.True(result);

        _collector.ReviewCalledArguments([
            WriteMethodArguments.Parse(
                MEASUREMENT,
                new Dictionary<string, object>()
                {
                    { "1", "head1-1" }
                },
                new Dictionary<string, string>(),
                null
            ),
            WriteMethodArguments.Parse(
                MEASUREMENT,
                new Dictionary<string, object>()
                {
                    { "1", "head2-1" }
                },
                new Dictionary<string, string>(),
                null
            )
        ]);
    }

    [Fact(DisplayName = "No.102,103 正常登録ケース（fieldDataTypeがstring_all）")]
    public async Task Case102()
    {
        const string MEASUREMENT = "TEST_MEASUREMENT";
        var message = new IotMessage(@"
        {
            ""RecordList"": [
                {
                    ""RecordHeader"": [
                        ""head1-1""
                    ],
                    ""RecordData"": [
                        ""1""
                    ]
                },
                {
                    ""RecordHeader"": [
                        ""head2-1""
                    ],
                    ""RecordData"": [
                        2
                    ]
                }
            ]
        }
        ");

        var info = new DBInfo();
        SetPropertyValue(info, nameof(DBInfo.Input), "TEST_INPUT1");
        SetPropertyValue(info, nameof(DBInfo.Measurement), MEASUREMENT);
        SetPropertyValue(info, nameof(DBInfo.UseHeader), true);
        SetPropertyValue(info, nameof(DBInfo.UseData), true);
        SetPropertyValue(info, nameof(DBInfo.SetUserTimestamp), false);
        SetPropertyValue(info, nameof(DBInfo.TimestampType), (byte)0);
        SetPropertyValue(info, nameof(DBInfo.TimestampIndex), 0);
        SetPropertyValue(info, nameof(DBInfo.FieldDataType), (byte)1);

        var result = await _app.OnMessageReceivedAsync("input", message, info);
        Assert.True(result);

        _collector.ReviewCalledArguments([
            WriteMethodArguments.Parse(
                MEASUREMENT,
                new Dictionary<string, object>()
                {
                    { "1", "head1-1" },
                    { "2", "1" }
                },
                new Dictionary<string, string>(),
                null
            ),
            WriteMethodArguments.Parse(
                MEASUREMENT,
                new Dictionary<string, object>()
                {
                    { "1", "head2-1" },
                    { "2", "2" }
                },
                new Dictionary<string, string>(),
                null
            )
        ]);
    }

    [Fact(DisplayName = "No.104, 105 正常登録ケース（timestampTypeがtimestamp_index）")]
    public async Task Case104()
    {
        const string MEASUREMENT = "TEST_MEASUREMENT";
        var message = new IotMessage(@"
        {
            ""RecordList"": [
                {
                    ""RecordHeader"": [
                        ""datetime""
                    ],
                    ""RecordData"": [
                        ""2025/01/01 00:00:00""
                    ]
                }
            ]
        }
        ");

        var info = new DBInfo();
        SetPropertyValue(info, nameof(DBInfo.Input), "TEST_INPUT1");
        SetPropertyValue(info, nameof(DBInfo.Measurement), MEASUREMENT);
        SetPropertyValue(info, nameof(DBInfo.UseHeader), false);
        SetPropertyValue(info, nameof(DBInfo.UseData), true);
        SetPropertyValue(info, nameof(DBInfo.SetUserTimestamp), true);
        SetPropertyValue(info, nameof(DBInfo.TimestampType), (byte)1);
        SetPropertyValue(info, nameof(DBInfo.TimestampIndex), 1);
        SetPropertyValue(info, nameof(DBInfo.FieldDataType), (byte)0);

        var result = await _app.OnMessageReceivedAsync("input", message, info);
        Assert.True(result);

        _collector.ReviewCalledArguments(WriteMethodArguments.Parse(
            MEASUREMENT,
            new Dictionary<string, object>()
            {
                { "1", "2025/01/01 00:00:00" }
            },
            new Dictionary<string, string>(),
            DateTime.Parse("2025/01/01 00:00:00").ToUniversalTime()
        ));
    }
}
