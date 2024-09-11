using Newtonsoft.Json.Linq;
using System;
using Xunit;
using Xunit.Abstractions;
using System.Threading.Tasks;

namespace IotedgeV2InfluxDBRegister.Test
{
    public class GaudiMessage_Constructor
    {
        private readonly ITestOutputHelper _output;

        public GaudiMessage_Constructor(ITestOutputHelper output)
        {
            _output = output;
        }

        // private化したためテストの対象外とする

        // [Fact(DisplayName = "正常系: GAUDI標準形式文字列を入力 → GaudiMessageクラス生成")]
        // public void SimpleValue_GaudiMessageCreated()
        // {
        //     string inputRouteMsg = "{\"RecordList\":[{\"RecordHeader\":[\"ABC\"],\"RecordData\":[\"あいう\"]}]}";
        //     GaudiMessage result = new GaudiMessage(inputRouteMsg);
        //     Assert.IsAssignableFrom<GaudiMessage>(result);
        // }

        // [Fact(DisplayName = "正常系: RecordList2件のGAUDI標準形式文字列を入力 → 出力インスタンスが持つRecordListの要素数が2")]
        // public void RecordList2_OutputGaudiMessage()
        // {
        //     string inputRouteMsg = "{\"RecordList\":[{\"RecordHeader\":[\"ABC\"],\"RecordData\":[\"あいう\"]},{\"RecordHeader\":[\"DEF\"],\"RecordData\":[\"えお\"]}]}";
        //     GaudiMessage result = new GaudiMessage(inputRouteMsg);
        //     Assert.Equal(2, result.RecordList.Count);
        // }

        // [Fact(DisplayName = "正常系: RecordList0件のGAUDI標準形式文字列を入力 → 出力インスタンスが持つRecordListの要素数が0")]
        // public void RecordList0_OutputGaudiMessage()
        // {
        //     string inputRouteMsg = "{\"RecordList\":[]}";
        //     GaudiMessage result = new GaudiMessage(inputRouteMsg);
        //     Assert.Empty(result.RecordList);
        // }

        // [Fact(DisplayName = "異常系: RecordListを持たないJSON形式の文字列を入力")]
        // public void NoRecordList_ThrownException()
        // {
        //     string inputRouteMsg = "{\"ABC\":[{\"RecordHeader\":[\"ABC\"],\"RecordData\":[\"あいう\"]}]}";
        //     Assert.Throws<Exception>(() => new GaudiMessage(inputRouteMsg));
        // }

        // [Fact(DisplayName = "異常系: GaudiRecordが生成できない形式の文字列を入力")]
        // public void IllegalRecord_ThrownException()
        // {
        //     string inputRouteMsg = "{\"RecordList\":[{\"RecordHeader\":[\"ABC\"],\"RecordData\":[\"あいう\"]},{\"a\":[\"DEF\"],\"b\":[\"えお\"]}]}";
        //     Assert.Throws<Exception>(() => new GaudiMessage(inputRouteMsg));
        // }

        // [Fact(DisplayName = "異常系: RecordListが配列でない")]
        // public void RecordListNotArray_ThrownException()
        // {
        //     string inputRouteMsg = "{\"RecordList\":1}";
        //     Assert.Throws<Exception>(() => new GaudiMessage(inputRouteMsg));
        // }
    }
}
