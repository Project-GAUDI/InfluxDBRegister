namespace IotedgeV2InfluxDBRegister.Test;

public class GaudiMessage_CreateInstance
{
    [Fact(DisplayName = "No.119 JSON配列が指定されていて、登録用メッセージへのパースに失敗するケース")]
    public void Case119()
    {
        var result = GaudiMessage.CreateInstance("[]");
        Assert.Null(result);
    }

    [Fact(DisplayName = "No.120 RecordListプロパティが存在せず、登録用メッセージへのパースに失敗するケース")]
    public void Case120()
    {
        var result = GaudiMessage.CreateInstance("{}");
        Assert.Null(result);
    }

    [Fact(DisplayName = "No.121 RecordListプロパティが配列でなく、登録用メッセージへのパースに失敗するケース")]
    public void Case121()
    {
        var json = @"
        {
            ""RecordList"": {
                ""invalidFormat"": """"
            }
        }";
        var result = GaudiMessage.CreateInstance(json);
        Assert.Null(result);
    }

    [Fact(DisplayName = "No.122 RecordList.RecordHeaderプロパティが存在せず、登録用メッセージへのパースに失敗するケース")]
    public void Case122()
    {
        var json = @"
        {
            ""RecordList"": [
                {
                    ""headerNotFound"": """"
                }
            ]
        }";
        var result = GaudiMessage.CreateInstance(json);
        Assert.Null(result);
    }

    [Fact(DisplayName = "No.123 RecordList.RecordHeaderプロパティが配列でなく、登録用メッセージへのパースに失敗するケース")]
    public void Case123()
    {
        var json = @"
        {
            ""RecordList"": [
                {
                    ""RecordHeader"": ""INVALID_VALUE_FORMAT""
                }
            ]
        }";
        var result = GaudiMessage.CreateInstance(json);
        Assert.Null(result);
    }

    [Fact(DisplayName = "No.124 RecordList.RecordHeaderプロパティに値型以外が指定されていて、登録用メッセージへのパースに失敗するケース")]
    public void Case124()
    {
        var json = @"
        {
            ""RecordList"": [
                {
                    ""RecordHeader"": [
                        {}
                    ]
                }
            ]
        }";
        var result = GaudiMessage.CreateInstance(json);
        Assert.Null(result);
    }

    [Fact(DisplayName = "No.125 RecordList.RecordDataプロパティが存在せず、登録用メッセージへのパースに失敗するケース")]
    public void Case125()
    {
        var json = @"
        {
            ""RecordList"": [
                {
                    ""RecordHeader"": [],
                    ""dataNotFound"": """"
                }
            ]
        }";
        var result = GaudiMessage.CreateInstance(json);
        Assert.Null(result);
    }

    [Fact(DisplayName = "No.126 RecordList.RecordDataプロパティが配列でなく、登録用メッセージへのパースに失敗するケース")]
    public void Case126()
    {
        var json = @"
        {
            ""RecordList"": [
                {
                    ""RecordHeader"": [],
                    ""RecordData"": ""INVALID_VALUE_FORMAT""
                }
            ]
        }";
        var result = GaudiMessage.CreateInstance(json);
        Assert.Null(result);
    }

    [Fact(DisplayName = "No.127 RecordList.RecordDataプロパティに値型以外が指定されていて、登録用メッセージへのパースに失敗するケース")]
    public void Case127()
    {
        var json = @"
        {
            ""RecordList"": [
                {
                    ""RecordHeader"": [],
                    ""RecordData"": [
                        {}
                    ]
                }
            ]
        }";
        var result = GaudiMessage.CreateInstance(json);
        Assert.Null(result);
    }

    [Fact(DisplayName = "No.128 正常終了ケース")]
    public void Case128()
    {
        List<string> headers = [ "head1-1-int", "head1-2-str", "head1-3-null" ];
        // Newtonsoft.Jsonは数値型を既定でlongとして扱うため、期待値の整数値は予めlongとする
        // 参照元: https://stackoverflow.com/questions/8237748/c-sharp-newtonsoft-json-linq-jvalue-always-returning-int64
        List<object> data = [ (long)1, "str-2", null ];
        var json = $@"
        {{
            ""RecordList"": [
                {{
                    ""RecordHeader"": { JsonConvert.SerializeObject(headers) },
                    ""RecordData"": { JsonConvert.SerializeObject(data) }
                }}
            ]
        }}";
        var result = GaudiMessage.CreateInstance(json);

        Assert.Single(result!.RecordList!);
        var record = result.RecordList[0];
        for (var i = 0; i < headers.Count; ++i)
        {
            Assert.Equal(headers[i], record.RecordHeader[i]);
        }
        for (var i = 0; i < data.Count; ++i)
        {
            if (data[i] == null)
            {
                Assert.Null(record.RecordData[i].Value);
                Assert.Equal("null", record.RecordData[i].ToString());
            }
            else
            {
                Assert.Equal(data[i], record.RecordData[i].Value);
                Assert.IsType(data[i].GetType(), record.RecordData[i].Value);
                Assert.Equal(data[i].ToString(), record.RecordData[i].ToString());
            }
        }
    }

    #region 単体テスト仕様書外の既存テスト
    [Fact(DisplayName = "正常系: GAUDI標準形式文字列を入力 → GaudiMessageクラス生成")]
    public void SimpleValue_GaudiMessageCreated()
    {
        string inputRouteMsg = "{\"RecordList\":[{\"RecordHeader\":[\"ABC\"],\"RecordData\":[\"あいう\"]}]}";
        GaudiMessage result = GaudiMessage.CreateInstance(inputRouteMsg);
        Assert.IsAssignableFrom<GaudiMessage>(result);
    }

    [Fact(DisplayName = "正常系: RecordList2件のGAUDI標準形式文字列を入力 → 出力インスタンスが持つRecordListの要素数が2")]
    public void RecordList2_OutputGaudiMessage()
    {
        string inputRouteMsg = "{\"RecordList\":[{\"RecordHeader\":[\"ABC\"],\"RecordData\":[\"あいう\"]},{\"RecordHeader\":[\"DEF\"],\"RecordData\":[\"えお\"]}]}";
        GaudiMessage result = GaudiMessage.CreateInstance(inputRouteMsg);
        Assert.Equal(2, result.RecordList.Count);
    }

    [Fact(DisplayName = "正常系: RecordList0件のGAUDI標準形式文字列を入力 → 出力インスタンスが持つRecordListの要素数が0")]
    public void RecordList0_OutputGaudiMessage()
    {
        string inputRouteMsg = "{\"RecordList\":[]}";
        GaudiMessage result = GaudiMessage.CreateInstance(inputRouteMsg);
        Assert.Empty(result.RecordList);
    }

    [Fact(DisplayName = "異常系: RecordListを持たないJSON形式の文字列を入力")]
    public void NoRecordList_ReturnedNull()
    {
        string inputRouteMsg = "{\"ABC\":[{\"RecordHeader\":[\"ABC\"],\"RecordData\":[\"あいう\"]}]}";
        GaudiMessage result = GaudiMessage.CreateInstance(inputRouteMsg);
        Assert.Null(result);
    }

    [Fact(DisplayName = "異常系: GaudiRecordが生成できない形式の文字列を入力")]
    public void IllegalRecord_ReturnedNull()
    {
        string inputRouteMsg = "{\"RecordList\":[{\"RecordHeader\":[\"ABC\"],\"RecordData\":[\"あいう\"]},{\"a\":[\"DEF\"],\"b\":[\"えお\"]}]}";
        GaudiMessage result = GaudiMessage.CreateInstance(inputRouteMsg);
        Assert.Null(result);
    }

    [Fact(DisplayName = "異常系: RecordListが配列でない")]
    public void RecordListNotArray_ReturnedNull()
    {
        string inputRouteMsg = "{\"RecordList\":1}";
        GaudiMessage result = GaudiMessage.CreateInstance(inputRouteMsg);
        Assert.Null(result);
    }

    [Fact(DisplayName = "異常系: メッセージが空")]
    public void MsgIsEmpty_ReturnedNull()
    {
        string inputRouteMsg = "";
        GaudiMessage result = GaudiMessage.CreateInstance(inputRouteMsg);
        Assert.Null(result);
    }
    #endregion
}
