using IotedgeV2InfluxDBRegister.Test.Stub;

namespace IotedgeV2InfluxDBRegister.Test;

[Collection(nameof(CollectionAttribute))]
public class MyApplicationMain_InsertInfluxDB(ITestOutputHelper output): MetricsCollectorTesterBase(output)
{
    private readonly MyApplicationMain _app = new ();

    private static void SetPrivateProperty<T>(T obj, string propName, object value, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public)
    {
        var type = typeof(T);
        var pi = type.GetProperty(propName, bindingFlags);
        pi!.SetValue(obj, value);
    }

    private static void ExecuteInsertInfluxDb(DBInfo dbinfo, Dictionary<string, string> msgProps, GaudiMessage.GaudiRecord? record, int rowIndex)
    {
        var type = typeof(MyApplicationMain);
        var mi = type.GetMethod("InsertInfluxDB", BindingFlags.Static | BindingFlags.NonPublic);
        mi!.Invoke(null, [ dbinfo, msgProps, record, rowIndex ]);
    }

    [Fact(DisplayName = "No.106 timestamp_indexで指定された値が有効な日時形式でないケース")]
    public void Case106()
    {
        var info = new DBInfo();
        SetPrivateProperty(info, "TimestampType", (byte)1);
        SetPrivateProperty(info, "TimestampIndex", 1);

        var record = new GaudiMessage.GaudiRecord(JObject.Parse(@"
        {
            ""RecordHeader"": [
                ""time""
            ],
            ""RecordData"": [
                ""INVALID_DATETIME_VALUE""
            ]
        }"));

        Assert.ThrowsAny<Exception>(() => {
            try {
                ExecuteInsertInfluxDb(info, [], record, -1);
            }
            catch (TargetInvocationException tie)
            {
                throw tie.InnerException!;
            }
        });
    }

    [Fact(DisplayName = "No.107, 108 recordがnull")]
    public void Case107()
    {
        const string MEASUREMENT = "TEST_MEASUREMENT";

        var info = new DBInfo();
        SetPrivateProperty(info, nameof(DBInfo.Input), "TEST_INPUT1");
        SetPrivateProperty(info, nameof(DBInfo.Measurement), MEASUREMENT);
        SetPrivateProperty(info, "UseHeader", true);
        SetPrivateProperty(info, "UseData", true);
        SetPrivateProperty(info, "SetUserTimestamp", false);
        SetPrivateProperty(info, "TimestampType", (byte)0);
        SetPrivateProperty(info, "TimestampIndex", 0);
        SetPrivateProperty(info, "FieldDataType", (byte)1);

        ExecuteInsertInfluxDb(info, [], null, -1);

        _collector.ReviewCalledArguments(WriteMethodArguments.Parse(
            MEASUREMENT,
            new Dictionary<string, object>()
            {
                { "1", "\"\"" }
            },
            new Dictionary<string, string>(),
            null
        ));
    }

    [Fact(DisplayName = "No.109, 110 正常登録ケース")]
    public void Case109()
    {
        const string MEASUREMENT = "TEST_MEASUREMENT";

        var info = new DBInfo();
        SetPrivateProperty(info, nameof(DBInfo.Input), "TEST_INPUT1");
        SetPrivateProperty(info, nameof(DBInfo.Measurement), MEASUREMENT);
        var tag = new TagInfo();
        SetPrivateProperty(tag, nameof(TagInfo.TagName), "TEST_TAG1");
        SetPrivateProperty(tag, "TagValueType", 0, BindingFlags.Instance | BindingFlags.NonPublic);
        info.TagList.Add(tag);
        tag = new TagInfo();
        SetPrivateProperty(tag, nameof(TagInfo.TagName), "TEST_TAG2");
        SetPrivateProperty(tag, "TagValueType", 0, BindingFlags.Instance | BindingFlags.NonPublic);
        info.TagList.Add(tag);
        tag = new TagInfo();
        SetPrivateProperty(tag, nameof(TagInfo.TagName), "TEST_TAG3");
        SetPrivateProperty(tag, "TagValueType", 1, BindingFlags.Instance | BindingFlags.NonPublic);
        info.TagList.Add(tag);
        tag = new TagInfo();
        SetPrivateProperty(tag, nameof(TagInfo.TagName), "TEST_TAG4");
        SetPrivateProperty(tag, "TagValueType", 1, BindingFlags.Instance | BindingFlags.NonPublic);
        info.TagList.Add(tag);
        SetPrivateProperty(info, nameof(DBInfo.UseHeader), true);
        SetPrivateProperty(info, nameof(DBInfo.UseData), true);
        SetPrivateProperty(info, nameof(DBInfo.SetUserTimestamp), false);
        SetPrivateProperty(info, nameof(DBInfo.TimestampType), (byte)0);
        SetPrivateProperty(info, nameof(DBInfo.TimestampIndex), 0);
        SetPrivateProperty(info, nameof(DBInfo.FieldDataType), (byte)0);

        var props = new Dictionary<string, string>
        {
            { "TEST_TAG1", "tag1" },
            { "TEST_TAG3", "5" }
        };

        var record = new GaudiMessage.GaudiRecord(JObject.Parse(@"
        {
            ""RecordHeader"": [
                ""head1-1""
            ],
            ""RecordData"": [
                1
            ]
        }"));

        ExecuteInsertInfluxDb(info, props, record, 5);

        _collector.ReviewCalledArguments(WriteMethodArguments.Parse(
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
                { "TEST_TAG3", "10" },
                { "TEST_TAG4", "" }
            },
            null
        ));
    }

    [Fact(DisplayName = "No.111, 112 正常登録ケース（useHeaderがfalse）")]
    public void Case111()
    {
        const string MEASUREMENT = "TEST_MEASUREMENT";

        var info = new DBInfo();
        SetPrivateProperty(info, nameof(DBInfo.Input), "TEST_INPUT1");
        SetPrivateProperty(info, nameof(DBInfo.Measurement), MEASUREMENT);
        SetPrivateProperty(info, "UseHeader", false);
        SetPrivateProperty(info, "UseData", true);
        SetPrivateProperty(info, "SetUserTimestamp", false);
        SetPrivateProperty(info, "TimestampType", (byte)0);
        SetPrivateProperty(info, "TimestampIndex", 0);
        SetPrivateProperty(info, "FieldDataType", (byte)0);

        var record = new GaudiMessage.GaudiRecord(JObject.Parse(@"
        {
            ""RecordHeader"": [
                ""head1-1""
            ],
            ""RecordData"": [
                1
            ]
        }"));

        ExecuteInsertInfluxDb(info, [], record, 0);

        _collector.ReviewCalledArguments(WriteMethodArguments.Parse(
            MEASUREMENT,
            new Dictionary<string, object>()
            {
                { "1", 1 }
            },
            new Dictionary<string, string>(),
            null
        ));
    }

    [Fact(DisplayName = "No.113, 114 正常登録ケース（useDataがfalse）")]
    public void Case113()
    {
        const string MEASUREMENT = "TEST_MEASUREMENT";

        var info = new DBInfo();
        SetPrivateProperty(info, nameof(DBInfo.Input), "TEST_INPUT1");
        SetPrivateProperty(info, nameof(DBInfo.Measurement), MEASUREMENT);
        SetPrivateProperty(info, "UseHeader", true);
        SetPrivateProperty(info, "UseData", false);
        SetPrivateProperty(info, "SetUserTimestamp", false);
        SetPrivateProperty(info, "TimestampType", (byte)0);
        SetPrivateProperty(info, "TimestampIndex", 0);
        SetPrivateProperty(info, "FieldDataType", (byte)0);

        var record = new GaudiMessage.GaudiRecord(JObject.Parse(@"
        {
            ""RecordHeader"": [
                ""head1-1""
            ],
            ""RecordData"": [
                1
            ]
        }"));

        ExecuteInsertInfluxDb(info, [], record, 0);

        _collector.ReviewCalledArguments(WriteMethodArguments.Parse(
            MEASUREMENT,
            new Dictionary<string, object>()
            {
                { "1", "head1-1" }
            },
            new Dictionary<string, string>(),
            null
        ));
    }

    [Fact(DisplayName = "No.115, 116 正常登録ケース（fieldDataTypeがstring_all）")]
    public void Case115()
    {
        const string MEASUREMENT = "TEST_MEASUREMENT";

        var info = new DBInfo();
        SetPrivateProperty(info, nameof(DBInfo.Input), "TEST_INPUT1");
        SetPrivateProperty(info, nameof(DBInfo.Measurement), MEASUREMENT);
        SetPrivateProperty(info, "UseHeader", true);
        SetPrivateProperty(info, "UseData", true);
        SetPrivateProperty(info, "SetUserTimestamp", false);
        SetPrivateProperty(info, "TimestampType", (byte)0);
        SetPrivateProperty(info, "TimestampIndex", 0);
        SetPrivateProperty(info, "FieldDataType", (byte)1);

        var record = new GaudiMessage.GaudiRecord(JObject.Parse(@"
        {
            ""RecordHeader"": [
                ""head1-1""
            ],
            ""RecordData"": [
                20
            ]
        }"));

        ExecuteInsertInfluxDb(info, [], record, 0);

        _collector.ReviewCalledArguments(WriteMethodArguments.Parse(
            MEASUREMENT,
            new Dictionary<string, object>()
            {
                { "1", "head1-1" },
                { "2", "20" }
            },
            new Dictionary<string, string>(),
            null
        ));
    }

    [Fact(DisplayName = "No.117, 118 正常登録ケース（timestampTypeがtimestamp_index）")]
    public void Case117()
    {
        const string MEASUREMENT = "TEST_MEASUREMENT";

        var info = new DBInfo();
        SetPrivateProperty(info, nameof(DBInfo.Input), "TEST_INPUT1");
        SetPrivateProperty(info, nameof(DBInfo.Measurement), MEASUREMENT);
        SetPrivateProperty(info, "UseHeader", false);
        SetPrivateProperty(info, "UseData", true);
        SetPrivateProperty(info, "SetUserTimestamp", true);
        SetPrivateProperty(info, "TimestampType", (byte)1);
        SetPrivateProperty(info, "TimestampIndex", 1);
        SetPrivateProperty(info, "FieldDataType", (byte)0);

        var record = new GaudiMessage.GaudiRecord(JObject.Parse(@"
        {
            ""RecordHeader"": [
                ""datetime""
            ],
            ""RecordData"": [
                ""2025/01/01 00:00:00""
            ]
        }"));

        ExecuteInsertInfluxDb(info, [], record, 0);

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
