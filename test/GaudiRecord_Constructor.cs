using Newtonsoft.Json.Linq;
using System;
using Xunit;
using Xunit.Abstractions;
using System.Threading.Tasks;

namespace IotedgeV2InfluxDBRegister.Test
{
    public class GaudiRecord_Constructor
    {
        private readonly ITestOutputHelper _output;

        public GaudiRecord_Constructor(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact(DisplayName = "正常系: 任意のオブジェクトを入力 → GaudiRecordクラス生成")]
        public void SimpleValue_GaudiRecordCreated()
        {
            JObject inputRouteMsg = JObject.Parse("{\"RecordHeader\":[\"ABC\"],\"RecordData\":[\"あいう\"]}");
            GaudiMessage.GaudiRecord result = new GaudiMessage.GaudiRecord(inputRouteMsg);
            Assert.IsAssignableFrom<GaudiMessage.GaudiRecord>(result);
        }

        [Fact(DisplayName = "正常系: RecordHeader0件のオブジェクトを入力 → 出力インタンスが持つRecordHeaderの要素数が0")]
        public void RecordHeader0_OutputGaudiRecord()
        {
            JObject inputRouteMsg = JObject.Parse("{\"RecordHeader\":[],\"RecordData\":[\"あいう\"]}");
            GaudiMessage.GaudiRecord result = new GaudiMessage.GaudiRecord(inputRouteMsg);
            Assert.Empty(result.RecordHeader);
        }

        [Fact(DisplayName = "正常系: RecordHeader1件のオブジェクトを入力 → 出力インタンスが持つRecordHeaderの要素数が1")]

        public void RecordHeader1_OutputGaudiRecord()
        {
            JObject inputRouteMsg = JObject.Parse("{\"RecordHeader\":[\"ABC\"],\"RecordData\":[\"あいう\"]}");
            GaudiMessage.GaudiRecord result = new GaudiMessage.GaudiRecord(inputRouteMsg);
            Assert.Single(result.RecordHeader);
        }

        [Fact(DisplayName = "正常系: RecordHeader2件のオブジェクトを入力 → 出力インタンスが持つRecordHeaderの要素数が2")]
        public void RecordHeader2_OutputGaudiRecord()
        {
            JObject inputRouteMsg = JObject.Parse("{\"RecordHeader\":[\"ABC\",\"DEF\"],\"RecordData\":[\"あいう\"]}");
            GaudiMessage.GaudiRecord result = new GaudiMessage.GaudiRecord(inputRouteMsg);
            Assert.Equal(2, result.RecordHeader.Count);
        }

        [Fact(DisplayName = "正常系: RecordData0件のオブジェクトを入力 → 出力インタンスが持つRecordDataの要素数が0")]
        public void RecordData0_OutputGaudiRecord()
        {
            JObject inputRouteMsg = JObject.Parse("{\"RecordHeader\":[\"ABC\"],\"RecordData\":[]}");
            GaudiMessage.GaudiRecord result = new GaudiMessage.GaudiRecord(inputRouteMsg);
            Assert.Empty(result.RecordData);
        }

        [Fact(DisplayName = "正常系: RecordData1件のオブジェクトを入力 → 出力インタンスが持つRecordDataの要素数が1")]
        public void RecordData1_OutputGaudiRecord()
        {
            JObject inputRouteMsg = JObject.Parse("{\"RecordHeader\":[\"ABC\"],\"RecordData\":[\"あいう\"]}");
            GaudiMessage.GaudiRecord result = new GaudiMessage.GaudiRecord(inputRouteMsg);
            Assert.Single(result.RecordData);
        }

        [Fact(DisplayName = "正常系: RecordData2件のオブジェクトを入力 → 出力インタンスが持つRecordDataの要素数が2")]
        public void RecordData2_OutputGaudiRecord()
        {
            JObject inputRouteMsg = JObject.Parse("{\"RecordHeader\":[\"ABC\"],\"RecordData\":[\"あいう\",\"えお\"]}");
            GaudiMessage.GaudiRecord result = new GaudiMessage.GaudiRecord(inputRouteMsg);
            Assert.Equal(2, result.RecordData.Count);
        }

        [Fact(DisplayName = "正常系: RecordHeader,RecordData0件のオブジェクトを入力 → 出力インタンスが持つRecordHeader,RecordDataの要素数が0")]
        public void RecordHeader0_RecordData0_OutputGaudiRecord()
        {
            JObject inputRouteMsg = JObject.Parse("{\"RecordHeader\":[],\"RecordData\":[]}");
            GaudiMessage.GaudiRecord result = new GaudiMessage.GaudiRecord(inputRouteMsg);
            Assert.Empty(result.RecordHeader);
            Assert.Empty(result.RecordData);
        }

        [Fact(DisplayName = "正常系: RecordHeader,RecordData1件のオブジェクトを入力 → 出力インタンスが持つRecordHeader,RecordDataの要素数が1")]
        public void RecordHeader1_RecordData1_OutputGaudiRecord()
        {
            JObject inputRouteMsg = JObject.Parse("{\"RecordHeader\":[\"ABC\"],\"RecordData\":[\"あいう\"]}");
            GaudiMessage.GaudiRecord result = new GaudiMessage.GaudiRecord(inputRouteMsg);
            Assert.Single(result.RecordHeader);
            Assert.Single(result.RecordData);
        }

        [Fact(DisplayName = "正常系: RecordHeader,RecordData2件のオブジェクトを入力 → 出力インタンスが持つRecordHeader,RecordDataの要素数が2")]
        public void RecordHeader2_RecordData2_OutputGaudiRecord()
        {
            JObject inputRouteMsg = JObject.Parse("{\"RecordHeader\":[\"ABC\",\"DEF\"],\"RecordData\":[\"あいう\",\"えお\"]}");
            GaudiMessage.GaudiRecord result = new GaudiMessage.GaudiRecord(inputRouteMsg);
            Assert.Equal(2, result.RecordHeader.Count);
            Assert.Equal(2, result.RecordData.Count);
        }

        [Fact(DisplayName = "異常系: RecordHeaderを持たないオブジェクトを入力")]
        public void NoRecordHeader_ExceptionThrown()
        {
            JObject inputRouteMsg = JObject.Parse("{\"RecordData\":[\"あいう\"]}");
            Assert.Throws<Exception>(() => new GaudiMessage.GaudiRecord(inputRouteMsg));
        }

        [Fact(DisplayName = "異常系: RecordDataを持たないオブジェクトを入力")]
        public void NoRecordData_ExceptionThrown()
        {
            JObject inputRouteMsg = JObject.Parse("{\"RecordHeader\":[\"ABC\"]}");
            Assert.Throws<Exception>(() => new GaudiMessage.GaudiRecord(inputRouteMsg));
        }

        [Fact(DisplayName = "異常系: RecordHeader,RecordDataを持たないオブジェクトを入力")]
        public void NoRecordData_NoRecordData_ExceptionThrown()
        {
            JObject inputRouteMsg = JObject.Parse("{}");
            Assert.Throws<Exception>(() => new GaudiMessage.GaudiRecord(inputRouteMsg));
        }


        [Fact(DisplayName = "異常系: RecordHeaderの値が配列")]
        public void RecordHeaderArray_ExceptionThrown()
        {
            JObject inputRouteMsg = JObject.Parse("{\"RecordHeader\":[[\"ABC\"]],\"RecordData\":[\"あいう\"]}");
            Assert.Throws<Exception>(() => new GaudiMessage.GaudiRecord(inputRouteMsg));
        }

        [Fact(DisplayName = "異常系: RecordDataの値が配列")]
        public void RecordDataArray_ExceptionThrown()
        {
            JObject inputRouteMsg = JObject.Parse("{\"RecordHeader\":[\"ABC\"],\"RecordData\":[[\"あいう\"]]}");
            Assert.Throws<Exception>(() => new GaudiMessage.GaudiRecord(inputRouteMsg));
        }
    }
}
