using Newtonsoft.Json.Linq;
using System;
using Xunit;
using Xunit.Abstractions;
using System.Threading.Tasks;

namespace IotedgeV2InfluxDBRegister.Test
{
    public class DBInfo_CreateInstance
    {
        private readonly ITestOutputHelper _output;

        public DBInfo_CreateInstance(ITestOutputHelper output)
        {
            _output = output;
        }

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

        // [Fact(DisplayName = "正常系:set_usertimestampがtrue,timestamp_indexがない,usertimestampがある → usertimestampが反映")]
        // public void usertimestamp_exist_TypeUser()
        // {
        //     JObject inputRouteMsg = JObject.Parse("{\"input\": \"cfrOut1\",\"measurement\": \"measurement1\",\"use_header\": false,\"use_data\": true,\"set_usertimestamp\": true,\"usertimestamp\": \"2022/02/22 10:00:00\",\"tags\": {\"tag1\": \"col1\",\"tag2\": {\"tag_name\": \"col2\",\"value_type\": \"MessageProperty\"}},\"field_data_type\": \"automatic\"}");
        //     DBInfo result = DBInfo.CreateInstance(inputRouteMsg);
        //     Assert.Equal(DateTime.Parse("2022/02/22 10:00:00"), result.TimestampValue);
        //     Assert.Equal(DBInfo.UserTimestampType.usertimestamp, result.TimestampType);
        // }

        [Fact(DisplayName = "異常系:set_usertimestampがtrue,timestamp_indexがない,usertimestampがない → 例外")]
        public void neithertimestamp_exist_ExceptionThrown()
        {
            JObject inputRouteMsg = JObject.Parse("{\"input\": \"cfrOut1\",\"measurement\": \"measurement1\",\"use_header\": false,\"use_data\": true,\"set_usertimestamp\": true,\"tags\": {\"tag1\": \"col1\",\"tag2\": {\"tag_name\": \"col2\",\"value_type\": \"MessageProperty\"}},\"field_data_type\": \"automatic\"}");
            Assert.Throws<Exception>(() => DBInfo.CreateInstance(inputRouteMsg));
        }

        // [Fact(DisplayName = "正常系:set_usertimestampがtrue,timestamp_indexがない,usertimestampがUTC形式")]
        // public void usertimestamp_UTC_LocalTimeSet()
        // {
        //     JObject inputRouteMsg = JObject.Parse("{\"input\": \"cfrOut1\",\"measurement\": \"measurement1\",\"use_header\": false,\"use_data\": true,\"set_usertimestamp\": true,\"usertimestamp\": \"2022/02/22T10:00:00Z\",\"tags\": {\"tag1\": \"col1\",\"tag2\": {\"tag_name\": \"col2\",\"value_type\": \"MessageProperty\"}},\"field_data_type\": \"automatic\"}");
        //     DBInfo result = DBInfo.CreateInstance(inputRouteMsg);
        //     DateTime dt1 = new DateTime(2022, 2, 22, 10, 0, 0);
        //     DateTime localTime1 = System.TimeZoneInfo.ConvertTimeFromUtc(dt1, System.TimeZoneInfo.Local);
        //     Assert.Equal(localTime1, result.TimestampValue);
        // }

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
    }
}
