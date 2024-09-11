using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using TICO.GAUDI.Commons;

namespace IotedgeV2InfluxDBRegister
{
    class TagInfo
    {
        private enum ValueType
        {
            MessageProperty = 0,
            MessagePropertyAndRowIndex = 1
        }

        static ILogger MyLogger { get; } = LoggerFactory.GetLogger(typeof(TagInfo));

        public string TagName { get; private set; } = null;

        private ValueType TagValueType { get; set; } = ValueType.MessageProperty;

        public static TagInfo CreateInstance(JToken jtok)
        {
            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Start Method: CreateInstance");

            var ret = new TagInfo();

            if (jtok is JValue)
            {
                ret.TagName = jtok.Value<string>();
                ret.TagValueType = ValueType.MessageProperty;
            }
            else
            {
                JObject jobj = (JObject)jtok;
                // tag_name
                try
                {
                    ret.TagName = Util.GetRequiredValue<string>(jobj, "tag_name");
                }
                catch (Exception)
                {
                    var errmsg = $"Property 'tag_name' does not exist.";
                    MyLogger.WriteLog(ILogger.LogLevel.ERROR, $"{errmsg}", true);
                    MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: CreateInstance caused by {errmsg}");
                    throw;
                }
                // value_type
                string typestr = null;
                try
                {
                    typestr = Util.GetRequiredValue<string>(jobj, "value_type").ToUpper();
                }
                catch (Exception)
                {
                    var errmsg = $"Property value_type does not exist.";
                    MyLogger.WriteLog(ILogger.LogLevel.ERROR, $"{errmsg}", true);
                    MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: CreateInstance caused by {errmsg}");
                    throw;
                }

                if (Enum.TryParse<ValueType>(typestr,true, out ValueType value))
                {
                    ret.TagValueType = value;
                }
                else
                {
                    var errmsg = $"Property 'value_type' is not an expected string.";
                    MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: CreateInstance caused by {errmsg}");
                    throw new Exception(errmsg);
                }
            }

            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"End Method: CreateInstance");

            return ret;
        }

        public string getTagValue(IDictionary<string, string> msgProps, int rowIndex = -1)
        {

            string ret = "";

            // MessagePropertyesから値を取得
            string propvalue = "";
            if (!msgProps.TryGetValue(this.TagName, out propvalue))
            {
                if (MyLogger.IsLogLevelToOutput(ILogger.LogLevel.TRACE))
                    MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"taginfo [{this.TagName}] : tagvalue is not exist.");
                ret = "";
            }
            else
            {
                // Typeにより編集
                if (this.TagValueType.Equals(ValueType.MessagePropertyAndRowIndex) && rowIndex >= 0)
                {
                    int num = 0;
                    int.TryParse(propvalue, out num);
                    num += rowIndex;
                    ret = num.ToString("D");
                }
                else
                {
                    ret = propvalue;
                }
            }


            return ret;
        }

    }
}
