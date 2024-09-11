using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Runtime.CompilerServices;
using TICO.GAUDI.Commons;

[assembly: InternalsVisibleTo("InfluxDBRegister.Test")]

namespace IotedgeV2InfluxDBRegister
{
    public class GaudiMessage
    {
        static ILogger MyLogger { get; } = LoggerFactory.GetLogger(typeof(GaudiMessage));

        public class GaudiRecord
        {
            public class GaudiData
            {
                JValue jtkData = null;

                public object Value
                {
                    get
                    {
                        // 型を指定して返す
                        return jtkData.ToObject<object>();
                    }
                }
                public GaudiData(JValue data)
                {
                    jtkData = data;
                }

                public override string ToString()
                {
                    string retVal = "null";

                    if (null != jtkData && null != Value)
                    {
                        // 文字列型として返す
                        retVal = jtkData.ToObject<string>();
                    }

                    return retVal;
                }
            }

            public List<string> RecordHeader { get; private set; } = null;
            public List<GaudiData> RecordData { get; private set; } = null;

            /// <summary>
            /// RecordHeader,RecordDataそれぞれの中の要素をGaudiDataクラスの要素にして返す
            /// </summary>
            /// <param name="jObj">メッセージオブジェクト</param>
            public GaudiRecord(JObject jObj)
            {

                JToken recordHeaders = null;
                if (jObj.TryGetValue("RecordHeader", out recordHeaders))
                {
                    List<string> gaudiHeaders = new List<string>();

                    JArray recordHeaderArray = recordHeaders as JArray;

                    foreach (var header in recordHeaderArray)
                    {
                        if (header is JValue)
                        {
                            JValue jvHeader = header as JValue;
                            string data = jvHeader.Value<string>();
                            gaudiHeaders.Add(data);
                        }
                        else
                        {
                            var errmsg = "Invalid RecordHeader value's datatype";
                            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: GaudiRecord caused by {errmsg}");
                            throw new Exception(errmsg);
                        }
                    }

                    RecordHeader = gaudiHeaders;
                }
                else
                {
                    var errmsg = "RecordHeader Required.";
                    MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: GaudiRecord caused by {errmsg}");
                    throw new Exception(errmsg);
                }

                JToken recordDatas = null;
                if (jObj.TryGetValue("RecordData", out recordDatas))
                {
                    List<GaudiData> gaudiDatas = new List<GaudiData>();

                    JArray recordDataArray = recordDatas as JArray;

                    foreach (var data in recordDataArray)
                    {
                        if (data is JValue)
                        {
                            GaudiData gaudiData = new GaudiData(data as JValue);
                            gaudiDatas.Add(gaudiData);
                        }
                        else
                        {
                            var errmsg = "Invalid RecordData value's datatype";
                            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: GaudiRecord caused by {errmsg}");
                            throw new Exception(errmsg);
                        }
                    }

                    RecordData = gaudiDatas;
                }
                else
                {
                    var errmsg = "RecordData Required.";
                    MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: GaudiRecord caused by {errmsg}");
                    throw new Exception(errmsg);
                }
            }
        }

        public List<GaudiRecord> RecordList { get; private set; } = null;

        public static GaudiMessage CreateInstance(string JsonString)
        {
            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Start Method: CreateInstance");

            GaudiMessage retGaudiMessage = null;
            try
            {
                retGaudiMessage = new GaudiMessage(JsonString);
            }
            catch
            {
                retGaudiMessage = null;
            }

            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"End Method: CreateInstance");

            return retGaudiMessage;
        }

        /// <summary>
        /// 引数がGAUDI標準形式のメッセージの場合、中の各要素をGaudiDataクラスの要素にして返す
        /// </summary>
        /// <param name="JsonString">GAUDI標準形式メッセージ文字列</param>
        private GaudiMessage(string JsonString)
        {
            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Start Method: GaudiMessage");

            var msg_root = JObject.Parse(JsonString);

            JToken recordList = null;
            if (msg_root.TryGetValue("RecordList", out recordList))
            {
                List<GaudiRecord> gaudiRecords = new List<GaudiRecord>();

                if (recordList is JArray)
                {
                    JArray recordListArray = recordList as JArray;

                    foreach (var record in recordListArray)
                    {
                        GaudiRecord gaudiRecord = new GaudiRecord(record as JObject);

                        gaudiRecords.Add(gaudiRecord);
                    }

                }
                else
                {
                    var errmsg = "Invalid RecordList datatype";
                    MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: GaudiMessage caused by {errmsg}");
                    throw new Exception(errmsg);
                }

                RecordList = gaudiRecords;

            }
            else
            {
                var errmsg = "RecordList Required.";
                MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: GaudiMessage caused by {errmsg}");
                throw new Exception(errmsg);
            }

            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"End Method: GaudiMessage");
        }
    }
}
