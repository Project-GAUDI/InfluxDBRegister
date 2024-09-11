using Newtonsoft.Json.Linq;
using System;
using Xunit;
using Xunit.Abstractions;
using System.Threading.Tasks;

namespace IotedgeV2InfluxDBRegister.Test
{
    public class GaudiData_Value
    {
        private readonly ITestOutputHelper _output;

        public GaudiData_Value(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact(DisplayName = "正常系: 文字列データを入力 → 出力インスタンスのValueがstring型になっていること")]
        public void InputString_TypeString()
        {
            JValue inputRouteMsg = new JValue("ABC");
            GaudiMessage.GaudiRecord.GaudiData result = new GaudiMessage.GaudiRecord.GaudiData(inputRouteMsg);
            Assert.IsAssignableFrom<string>(result.Value);
            Assert.Equal("ABC", result.Value);
        }

        [Fact(DisplayName = "正常系: 整数データを入力 → 出力インスタンスのValueがlong型になっていること")]
        public void InputInteger_TypeLong()
        {
            JValue inputRouteMsg = new JValue(1);
            GaudiMessage.GaudiRecord.GaudiData result = new GaudiMessage.GaudiRecord.GaudiData(inputRouteMsg);
            Assert.IsAssignableFrom<long>(result.Value);
            Assert.Equal(1L, result.Value);
        }

        [Fact(DisplayName = "正常系: 整数文字列データを入力 → 出力インスタンスのValueがstring型になっていること")]
        public void InputIntegerString_TypeString()
        {
            JValue inputRouteMsg = new JValue("1");
            GaudiMessage.GaudiRecord.GaudiData result = new GaudiMessage.GaudiRecord.GaudiData(inputRouteMsg);
            Assert.IsAssignableFrom<string>(result.Value);
            Assert.Equal("1", result.Value);
        }

        [Fact(DisplayName = "正常系: 実数データを入力 → 出力インスタンスのValueがdouble型になっていること")]
        public void InputFloat_TypeDouble()
        {
            JValue inputRouteMsg = new JValue(3.14);
            GaudiMessage.GaudiRecord.GaudiData result = new GaudiMessage.GaudiRecord.GaudiData(inputRouteMsg);
            Assert.IsAssignableFrom<double>(result.Value);
            Assert.Equal(3.14D, result.Value);
        }

        [Fact(DisplayName = "正常系: 実数文字列データを入力 → 出力インスタンスのValueがstring型になっていること")]
        public void InputFloatString_TypeString()
        {
            JValue inputRouteMsg = new JValue("3.14");
            GaudiMessage.GaudiRecord.GaudiData result = new GaudiMessage.GaudiRecord.GaudiData(inputRouteMsg);
            Assert.IsAssignableFrom<string>(result.Value);
            Assert.Equal("3.14", result.Value);
        }

        [Fact(DisplayName = "正常系: 真偽値データを入力 → 出力インスタンスのValueがboolean型になっていること")]
        public void InputBoolean_TypeBoolean()
        {
            JValue inputRouteMsg = new JValue(true);
            GaudiMessage.GaudiRecord.GaudiData result = new GaudiMessage.GaudiRecord.GaudiData(inputRouteMsg);
            Assert.IsAssignableFrom<bool>(result.Value);
            Assert.Equal(true, result.Value);
        }

        [Fact(DisplayName = "正常系: 真偽値文字列データを入力 → 出力インスタンスのValueがstring型になっていること")]
        public void InputBooleanString_TypeString()
        {
            JValue inputRouteMsg = new JValue("true");
            GaudiMessage.GaudiRecord.GaudiData result = new GaudiMessage.GaudiRecord.GaudiData(inputRouteMsg);
            Assert.IsAssignableFrom<string>(result.Value);
            Assert.Equal("true", result.Value);
        }

        [Fact(DisplayName = "正常系: 日付文字列データを入力 → 出力インスタンスのValueがstring型になっていること")]
        public void InputDateTimeString_TypeString()
        {
            JValue inputRouteMsg = new JValue("2022/7/11 10:00:00");
            GaudiMessage.GaudiRecord.GaudiData result = new GaudiMessage.GaudiRecord.GaudiData(inputRouteMsg);
            Assert.IsAssignableFrom<string>(result.Value);
            Assert.Equal("2022/7/11 10:00:00", result.Value);
        }

        [Fact(DisplayName = "正常系: 空文字を入力 → 出力インスタンスのValueがstring型になっていること")]
        public void InputEmptyString_TypeString()
        {
            JValue inputRouteMsg = new JValue("");
            GaudiMessage.GaudiRecord.GaudiData result = new GaudiMessage.GaudiRecord.GaudiData(inputRouteMsg);
            Assert.IsAssignableFrom<string>(result.Value);
            Assert.Equal("", result.Value);
        }

        [Fact(DisplayName = "正常系: nullを入力 → 出力インスタンスのValueがnullになっていること(型は未定義)")]
        public void InputNull_ValueNull()
        {
            JValue inputRouteMsg = new JValue((object)null);
            GaudiMessage.GaudiRecord.GaudiData result = new GaudiMessage.GaudiRecord.GaudiData(inputRouteMsg);
            //Assert.IsAssignableFrom<object>(result.Value);
            Assert.Null(result.Value);
        }


    }
}
