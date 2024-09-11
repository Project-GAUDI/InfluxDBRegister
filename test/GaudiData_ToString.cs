using Newtonsoft.Json.Linq;
using System;
using Xunit;
using Xunit.Abstractions;
using System.Threading.Tasks;

namespace IotedgeV2InfluxDBRegister.Test
{
    public class GaudiData_ToString
    {
        private readonly ITestOutputHelper _output;

        public GaudiData_ToString(ITestOutputHelper output)
        {
            _output = output;
        }

        // ToString
        // 
        // 正常系: 整数データを入力 → 入力データがstring型で出力されること
        // 正常系: 整数文字列データを入力 → 入力データがstring型で出力されること
        // 正常系: 実数データを入力 → 入力データがstring型で出力されること
        // 正常系: 実数文字列データを入力 → 入力データがstring型で出力されること
        // 正常系: 真偽値データを入力 → 入力データがstring型で出力されること
        // 正常系: 真偽値文字列データを入力 → 入力データがstring型で出力されること
        // 正常系: 日付文字列データを入力 → 入力データがstring型で出力されること
        // 正常系: 空文字を入力 → 入力データがstring型で出力されること
        // 正常系: nullを入力 → 入力データがstring型で出力されること

        [Fact(DisplayName = "正常系: 文字列データを入力 → 入力データがstring型で出力されること")]
        public void InputString_TypeString()
        {
            JValue inputRouteMsg = new JValue("ABC");
            GaudiMessage.GaudiRecord.GaudiData result = new GaudiMessage.GaudiRecord.GaudiData(inputRouteMsg);
            Assert.IsAssignableFrom<string>(result.ToString());
            Assert.Equal("ABC",result.ToString());
        }

        [Fact(DisplayName = "正常系: 整数データを入力 → 入力データがstring型で出力されること")]
        public void InputInteger_TypeString()
        {
            JValue inputRouteMsg = new JValue(1);
            GaudiMessage.GaudiRecord.GaudiData result = new GaudiMessage.GaudiRecord.GaudiData(inputRouteMsg);
            Assert.IsAssignableFrom<string>(result.ToString());
            Assert.Equal("1",result.ToString());
        }

        [Fact(DisplayName = "正常系: 整数文字列データを入力 → 入力データがstring型で出力されること")]
        public void InputIntegerString_TypeString()
        {
            JValue inputRouteMsg = new JValue("1");
            GaudiMessage.GaudiRecord.GaudiData result = new GaudiMessage.GaudiRecord.GaudiData(inputRouteMsg);
            Assert.IsAssignableFrom<string>(result.ToString());
            Assert.Equal("1",result.ToString());
        }

        [Fact(DisplayName = "正常系: 実数データを入力 → 入力データがstring型で出力されること")]
        public void InputFloat_TypeString()
        {
            JValue inputRouteMsg = new JValue(3.14);
            GaudiMessage.GaudiRecord.GaudiData result = new GaudiMessage.GaudiRecord.GaudiData(inputRouteMsg);
            Assert.IsAssignableFrom<string>(result.ToString());
            Assert.Equal("3.14",result.ToString());
        }

        [Fact(DisplayName = "正常系: 実数文字列データを入力 → 入力データがstring型で出力されること")]
        public void InputFloatString_TypeString()
        {
            JValue inputRouteMsg = new JValue("3.14");
            GaudiMessage.GaudiRecord.GaudiData result = new GaudiMessage.GaudiRecord.GaudiData(inputRouteMsg);
            Assert.IsAssignableFrom<string>(result.ToString());
            Assert.Equal("3.14",result.ToString());
        }

        [Fact(DisplayName = "正常系: 真偽値データを入力 → 入力データがstring型で出力されること")]
        public void InputBoolean_TypeString()
        {
            JValue inputRouteMsg = new JValue(true);
            GaudiMessage.GaudiRecord.GaudiData result = new GaudiMessage.GaudiRecord.GaudiData(inputRouteMsg);
            Assert.IsAssignableFrom<string>(result.ToString());
            Assert.Equal("True",result.ToString());
        }

        [Fact(DisplayName = "正常系: 真偽値文字列データを入力 → 入力データがstring型で出力されること")]
        public void InputBooleanString_TypeString()
        {
            JValue inputRouteMsg = new JValue("true");
            GaudiMessage.GaudiRecord.GaudiData result = new GaudiMessage.GaudiRecord.GaudiData(inputRouteMsg);
            Assert.IsAssignableFrom<string>(result.ToString());
            Assert.Equal("true",result.ToString());
        }

        [Fact(DisplayName = "正常系: 日付文字列データを入力 → 入力データがstring型で出力されること")]
        public void InputDateTimeString_TypeString()
        {
            JValue inputRouteMsg = new JValue("2022/7/11 10:00:00");
            GaudiMessage.GaudiRecord.GaudiData result = new GaudiMessage.GaudiRecord.GaudiData(inputRouteMsg);
            Assert.IsAssignableFrom<string>(result.ToString());
            Assert.Equal("2022/7/11 10:00:00",result.ToString());
        }

        [Fact(DisplayName = "正常系: 空文字を入力 → 入力データがstring型で出力されること")]
        public void InputEmptyString_TypeString()
        {
            JValue inputRouteMsg = new JValue("");
            GaudiMessage.GaudiRecord.GaudiData result = new GaudiMessage.GaudiRecord.GaudiData(inputRouteMsg);
            Assert.IsAssignableFrom<string>(result.ToString());
            Assert.Equal("",result.ToString());
        }

        [Fact(DisplayName = "正常系: nullを入力 → 入力データがstring型で出力されること")]
        public void InputNull_ValueString()
        {
            JValue inputRouteMsg = new JValue((object)null);
            GaudiMessage.GaudiRecord.GaudiData result = new GaudiMessage.GaudiRecord.GaudiData(inputRouteMsg);
            //Assert.IsAssignableFrom<object>(result.ToString());
            Assert.Equal("null",result.ToString());
        }
    }
}
