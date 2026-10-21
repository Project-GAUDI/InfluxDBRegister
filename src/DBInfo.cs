using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using TICO.GAUDI.Commons;

namespace IotedgeV2InfluxDBRegister
{

    /// <summary>
    /// ルート情報
    /// </summary>
    class DBInfo
    {
        public enum UserTimestampType : byte
        {
            none,
            timestamp_index,
            // usertimestamp
        }

        public enum FieldDataTypeMode : byte
        {
            automatic,
            string_all
        }

        const string TAG_INFO_KEY = "tag";
        public string Input { get; private set; }

        //public string Output { get; private set; }

        public string Measurement { get; private set; }

        public List<TagInfo> TagList { get; private set; } = new List<TagInfo>();
        public bool UseHeader { get; private set; }
        public bool UseData { get; private set; }
        public bool SetUserTimestamp { get; private set; }
        public UserTimestampType TimestampType { get; private set; } = UserTimestampType.none;
        public int TimestampIndex { get; private set; }
        // public DateTime TimestampValue { get; private set; }
        public FieldDataTypeMode FieldDataType { get; private set; } = FieldDataTypeMode.automatic;
        static ILogger MyLogger { get; } = LoggerFactory.GetLogger(typeof(DBInfo));

        public static DBInfo CreateInstance(JObject jobj)
        {
            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Start Method: CreateInstance");

            var ret = new DBInfo();
            ret.Input = Util.GetDesiredProperty<string>(jobj, "input", true);
            ret.Measurement = Util.GetDesiredProperty<string>(jobj, "measurement", true);
            var Tags = Util.GetDesiredProperty<JObject>(jobj, "tags", true);
            for (int i = 1; Tags.TryGetValue(TAG_INFO_KEY + i.ToString("D"), out JToken tag); i++)
            {
                ret.TagList.Add(TagInfo.CreateInstance(tag));
            }
            if(ret.TagList.Count == 0)
            {
                var errmsg = $"No tag value.";
                MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: CreateInstance caused by {errmsg}");
                throw new Exception(errmsg);
            }
            
            ret.UseHeader = Util.GetDesiredProperty<bool>(jobj, "use_header", true);
            
            ret.UseData = Util.GetDesiredProperty<bool>(jobj, "use_data", true);

            ret.SetUserTimestamp = Util.GetDesiredProperty<bool>(jobj, "set_usertimestamp", true);

            if (ret.SetUserTimestamp)
            {
                ret.TimestampIndex=Util.GetDesiredProperty<int>(jobj, "timestamp_index", true,1,condition: x => x >= 1);
                ret.TimestampType = UserTimestampType.timestamp_index;
            }
            ret.FieldDataType = Util.GetDesiredProperty<FieldDataTypeMode>(jobj, "field_data_type", false,FieldDataTypeMode.automatic);
            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"End Method: CreateInstance");

            return ret;
        }
    }
}
