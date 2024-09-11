
using Newtonsoft.Json.Linq;
using System;
using Xunit;
using Xunit.Abstractions;
using System.Threading.Tasks;

namespace IotedgeV2InfluxDBRegister.Test
{
    public class GaudiMessage_CreateInstance
    {
        private readonly ITestOutputHelper _output;

        public GaudiMessage_CreateInstance(ITestOutputHelper output)
        {
            _output = output;
        }

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
    }
}
