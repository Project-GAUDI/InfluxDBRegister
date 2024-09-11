using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
            try
            {
                ret.Input = Util.GetRequiredValue<string>(jobj, "input");
            }
            catch (Exception)
            {
                var errmsg = $"Property input does not exist.";
                MyLogger.WriteLog(ILogger.LogLevel.ERROR, $"{errmsg}", true);
                MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: CreateInstance caused by {errmsg}");
                throw;
            }
            //try
            //{
            //    ret.Output = Util.GetRequiredValue<string>(jobj, "output");
            //}
            //catch (Exception)
            //{
            //    var errmsg = $"Property output does not exist.";
            //    MyLogger.WriteLog(ILogger.LogLevel.ERROR, $"{errmsg}", true);
            //    MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: CreateInstance caused by {errmsg}");
            //    throw;
            //}
            try
            {
                ret.Measurement = Util.GetRequiredValue<string>(jobj, "measurement");
            }
            catch (Exception)
            {
                var errmsg = $"Property measurement does not exist.";
                MyLogger.WriteLog(ILogger.LogLevel.ERROR, $"{errmsg}", true);
                MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: CreateInstance caused by {errmsg}");
                throw;
            }
            if (jobj.TryGetValue("tags", out JToken jtoken))
            {
                var Tags = (JObject)jtoken;
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
            }
            try
            {
                ret.UseHeader = Util.GetRequiredValue<bool>(jobj, "use_header");
            }
            catch (Exception)
            {
                var errmsg = $"Property use_header does not exist.";
                MyLogger.WriteLog(ILogger.LogLevel.ERROR, $"{errmsg}", true);
                MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: CreateInstance caused by {errmsg}");
                throw;
            }
            try
            {
                ret.UseData = Util.GetRequiredValue<bool>(jobj, "use_data");
            }
            catch (Exception)
            {
                var errmsg = $"Property use_data does not exist.";
                MyLogger.WriteLog(ILogger.LogLevel.ERROR, $"{errmsg}", true);
                MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: CreateInstance caused by {errmsg}");
                throw;
            }
            try
            {
                ret.SetUserTimestamp = Util.GetRequiredValue<bool>(jobj, "set_usertimestamp");
            }
            catch (Exception)
            {
                var errmsg = $"Property set_usertimestamp does not exist.";
                MyLogger.WriteLog(ILogger.LogLevel.ERROR, $"{errmsg}", true);
                MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: CreateInstance caused by {errmsg}");
                throw;
            }
            if (ret.SetUserTimestamp)
            {
                if (jobj.TryGetValue("timestamp_index", out JToken jtk_timestamp_index))
                {
                    int index = jtk_timestamp_index.Value<int>();
                    if (index < 1)
                    {
                        var errmsg = $"Index value less than 1.";
                        MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: CreateInstance caused by {errmsg}");
                        throw new Exception(errmsg);
                    }
                    ret.TimestampIndex = index;
                    ret.TimestampType = UserTimestampType.timestamp_index;
                }
                // else if (jobj.TryGetValue("usertimestamp", out JToken jtk_usertimestamp))
                // {
                //     ret.TimestampValue = jtk_usertimestamp.Value<DateTime>();
                //     ret.TimestampType = UserTimestampType.usertimestamp;
                // }
                else
                {
                    var errmsg = $"No timestamp value.";
                    MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: CreateInstance caused by {errmsg}");
                    throw new Exception(errmsg);
                }
            }

            if (jobj.TryGetValue("field_data_type", out JToken jtk_field_data_type))
            {
                string fieldDataTypeStr = jtk_field_data_type.Value<string>();
                if (Enum.IsDefined(typeof(FieldDataTypeMode), fieldDataTypeStr))
                {
                    Enum.TryParse(fieldDataTypeStr, out FieldDataTypeMode fieldDataTypeEnum);
                    ret.FieldDataType = fieldDataTypeEnum;
                }
                else 
                {
                    var errmsg = $"Not supported field_data_type: {fieldDataTypeStr}.";
                    MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: CreateInstance caused by {errmsg}");
                    throw new Exception(errmsg);
                }
            }

            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"End Method: CreateInstance");

            return ret;
        }
    }
}
