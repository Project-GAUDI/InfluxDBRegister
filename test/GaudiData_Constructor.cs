using Newtonsoft.Json.Linq;
using System;
using Xunit;
using Xunit.Abstractions;
using System.Threading.Tasks;

namespace IotedgeV2InfluxDBRegister.Test
{
    public class GaudiData_Constructor
    {
        private readonly ITestOutputHelper _output;

        public GaudiData_Constructor(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact(DisplayName = "正常系: 任意のJValueを入力 → GaudiDataクラス生成")]
        public void SimpleValue_GaudiDataCreated()
        {
            JValue inputRouteMsg = new JValue("ABC");
            GaudiMessage.GaudiRecord.GaudiData result = new GaudiMessage.GaudiRecord.GaudiData(inputRouteMsg);
            Assert.IsAssignableFrom<GaudiMessage.GaudiRecord.GaudiData>(result);
        }

        [Fact(DisplayName = "正常系: 文字列データを入力 → 入力データがセットできること")]
        public void InputString_GaudiDataCreated()
        {
            JValue inputRouteMsg = new JValue("ABC");
            GaudiMessage.GaudiRecord.GaudiData result = new GaudiMessage.GaudiRecord.GaudiData(inputRouteMsg);
            Assert.IsAssignableFrom<GaudiMessage.GaudiRecord.GaudiData>(result);
        }

        [Fact(DisplayName = "正常系: 整数データを入力 → 入力データがセットできること")]
        public void InputInteger_GaudiDataCreated()
        {
            JValue inputRouteMsg = new JValue(1);
            GaudiMessage.GaudiRecord.GaudiData result = new GaudiMessage.GaudiRecord.GaudiData(inputRouteMsg);
            Assert.IsAssignableFrom<GaudiMessage.GaudiRecord.GaudiData>(result);
        }

        [Fact(DisplayName = "正常系: 整数文字列データを入力 → 入力データがセットできること")]
        public void InputIntegerString_GaudiDataCreated()
        {
            JValue inputRouteMsg = new JValue("1");
            GaudiMessage.GaudiRecord.GaudiData result = new GaudiMessage.GaudiRecord.GaudiData(inputRouteMsg);
            Assert.IsAssignableFrom<GaudiMessage.GaudiRecord.GaudiData>(result);
        }

        [Fact(DisplayName = "正常系: 実数データを入力 → 入力データがセットできること")]
        public void InputFloat_GaudiDataCreated()
        {
            JValue inputRouteMsg = new JValue(3.14);
            GaudiMessage.GaudiRecord.GaudiData result = new GaudiMessage.GaudiRecord.GaudiData(inputRouteMsg);
            Assert.IsAssignableFrom<GaudiMessage.GaudiRecord.GaudiData>(result);
        }

        [Fact(DisplayName = "正常系: 実数文字列データを入力 → 入力データがセットできること")]
        public void InputFloatString_GaudiDataCreated()
        {
            JValue inputRouteMsg = new JValue("3.14");
            GaudiMessage.GaudiRecord.GaudiData result = new GaudiMessage.GaudiRecord.GaudiData(inputRouteMsg);
            Assert.IsAssignableFrom<GaudiMessage.GaudiRecord.GaudiData>(result);
        }

        [Fact(DisplayName = "正常系: 真偽値データを入力 → 入力データがセットできること")]
        public void InputBoolean_GaudiDataCreated()
        {
            JValue inputRouteMsg = new JValue(true);
            GaudiMessage.GaudiRecord.GaudiData result = new GaudiMessage.GaudiRecord.GaudiData(inputRouteMsg);
            Assert.IsAssignableFrom<GaudiMessage.GaudiRecord.GaudiData>(result);
        }

        [Fact(DisplayName = "正常系: 真偽値文字列データを入力 → 入力データがセットできること")]
        public void InputBooleanString_GaudiDataCreated()
        {
            JValue inputRouteMsg = new JValue("true");
            GaudiMessage.GaudiRecord.GaudiData result = new GaudiMessage.GaudiRecord.GaudiData(inputRouteMsg);
            Assert.IsAssignableFrom<GaudiMessage.GaudiRecord.GaudiData>(result);
        }

        [Fact(DisplayName = "正常系: 日付文字列データを入力 → 入力データがセットできること")]
        public void InputDateTimeString_GaudiDataCreated()
        {
            JValue inputRouteMsg = new JValue("2022/7/11 13:53:24");
            GaudiMessage.GaudiRecord.GaudiData result = new GaudiMessage.GaudiRecord.GaudiData(inputRouteMsg);
            Assert.IsAssignableFrom<GaudiMessage.GaudiRecord.GaudiData>(result);
        }

        [Fact(DisplayName = "正常系: 空文字を入力 → 入力データがセットできること")]
        public void InputEmptyString_GaudiDataCreated()
        {
            JValue inputRouteMsg = new JValue("");
            GaudiMessage.GaudiRecord.GaudiData result = new GaudiMessage.GaudiRecord.GaudiData(inputRouteMsg);
            Assert.IsAssignableFrom<GaudiMessage.GaudiRecord.GaudiData>(result);
        }

        [Fact(DisplayName = "正常系: nullを入力 → 入力データがセットできること")]
        public void InputNull_GaudiDataCreated()
        {
            JValue inputRouteMsg = new JValue((Object)null);
            GaudiMessage.GaudiRecord.GaudiData result = new GaudiMessage.GaudiRecord.GaudiData(inputRouteMsg);
            Assert.IsAssignableFrom<GaudiMessage.GaudiRecord.GaudiData>(result);
        }
    }
}
