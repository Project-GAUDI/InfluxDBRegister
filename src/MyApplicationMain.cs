using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TICO.GAUDI.Commons;

namespace IotedgeV2InfluxDBRegister
{
    /// <summary>
    /// Application Main class
    /// </summary>
    internal class MyApplicationMain : IApplicationMain
    {
        static ILogger MyLogger { get; } = LoggerFactory.GetLogger(typeof(MyApplicationMain));
        const string DB_INFO_KEY = "dbparam";
        static List<DBInfo> DBInfos { get; set; } = null;
        static SQLParser SQLparser { get; set; } = new SQLParser();

        public void Dispose()
        {
            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Start Method: Dispose");

            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"End Method: Dispose");
        }

        /// <summary>
        /// アプリケーション初期化					
        /// システム初期化前に呼び出される
        /// </summary>
        /// <returns></returns>
        public async Task<bool> InitializeAsync()
        {
            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Start Method: InitializeAsync");

            // ここでApplicationMainの初期化処理を行う。
            // 通信は未接続、DesiredPropertiesなども未取得の状態
            // ＝＝＝＝＝＝＝＝＝＝＝＝＝ここから＝＝＝＝＝＝＝＝＝＝＝＝＝
            bool retStatus = true;
            try
            {
                string userenv = Environment.GetEnvironmentVariable("UserName");
                string passenv = Environment.GetEnvironmentVariable("Password");
                string dbenv = Environment.GetEnvironmentVariable("DBName");
                string urienv = Environment.GetEnvironmentVariable("URI");
                Int32.TryParse(Environment.GetEnvironmentVariable("Interval"), out int interval);
                SQLparser.InitParser(userenv, passenv, dbenv, urienv, interval);
            }
            catch (Exception ex)
            {
                var errmsg = "Environment parameter error.";
                MyLogger.WriteLog(ILogger.LogLevel.WARN, $"{errmsg} Exception:{ex}");
                MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: InitializeAsync caused by {errmsg}");
                retStatus = false;
                return retStatus;
            }

            await Task.CompletedTask;
            // ＝＝＝＝＝＝＝＝＝＝＝＝＝ここまで＝＝＝＝＝＝＝＝＝＝＝＝＝

            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"End Method: InitializeAsync");
            return retStatus;
        }

        /// <summary>
        /// アプリケーション起動処理					
        /// システム初期化完了後に呼び出される
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public async Task<bool> StartAsync()
        {
            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Start Method: StartAsync");

            // ここでApplicationMainの起動処理を行う。
            // 通信は接続済み、DesiredProperties取得済みの状態
            // ＝＝＝＝＝＝＝＝＝＝＝＝＝ここから＝＝＝＝＝＝＝＝＝＝＝＝＝
            bool retStatus = true;

            IApplicationEngine appEngine = ApplicationEngineFactory.GetEngine();
            foreach (var info in DBInfos)
            {
                // メッセージ受信時のコールバック定義
                await appEngine.AddMessageInputHandlerAsync(info.Input, OnMessageReceivedAsync, info).ConfigureAwait(false);
            }

            // ＝＝＝＝＝＝＝＝＝＝＝＝＝ここまで＝＝＝＝＝＝＝＝＝＝＝＝＝

            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"End Method: StartAsync");
            return retStatus;
        }

        /// <summary>
        /// アプリケーション解放。					
        /// </summary>
        /// <returns></returns>
        public async Task<bool> TerminateAsync()
        {
            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Start Method: TerminateAsync");

            // ここでApplicationMainの終了処理を行う。
            // アプリケーション終了時や、
            // DesiredPropertiesの更新通知受信後、
            // 通信切断時の回復処理時などに呼ばれる。
            // ＝＝＝＝＝＝＝＝＝＝＝＝＝ここから＝＝＝＝＝＝＝＝＝＝＝＝＝
            bool retStatus = true;

            await Task.CompletedTask;
            // ＝＝＝＝＝＝＝＝＝＝＝＝＝ここまで＝＝＝＝＝＝＝＝＝＝＝＝＝

            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"End Method: TerminateAsync");
            return retStatus;
        }


        /// <summary>
        /// DesiredPropertis更新コールバック。					
        /// </summary>
        /// <param name="desiredProperties">DesiredPropertiesデータ。JSONのルートオブジェクトに相当。</param>
        /// <returns></returns>
        public async Task<bool> OnDesiredPropertiesReceivedAsync(JObject desiredProperties)
        {
            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Start Method: OnDesiredPropertiesReceivedAsync");

            // DesiredProperties更新時の反映処理を行う。
            // 必要に応じて、メンバ変数への格納等を実施。
            // ＝＝＝＝＝＝＝＝＝＝＝＝＝ここから＝＝＝＝＝＝＝＝＝＝＝＝＝
            bool retStatus = true;
            DBInfos = new List<DBInfo>();
            for (int i = 1; desiredProperties.ContainsKey(DB_INFO_KEY + i.ToString("D")); i++)
            {
                try
                {
                    JObject jobj = Util.GetRequiredValue<JObject>(desiredProperties, DB_INFO_KEY + i.ToString("D"));
                    DBInfos.Add(DBInfo.CreateInstance(jobj));
                }
                catch (Exception ex)
                {
                    var errmsg = $"Property {DB_INFO_KEY + i.ToString("D")} is invalid.";
                    MyLogger.WriteLog(ILogger.LogLevel.ERROR, $"{errmsg} {ex}", true);
                    MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: OnDesiredPropertiesReceivedAsync caused by {errmsg}");
                    retStatus = false;
                    return retStatus;
                }
            }
            try
            {
                if (DBInfos.Count == 0)
                {
                    var msg = "DesiredProperties is empty.";
                    throw new Exception(msg);
                }
            }
            catch (Exception ex)
            {
                MyLogger.WriteLog(ILogger.LogLevel.ERROR, $"{ex}", true);
                MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: OnDesiredPropertiesReceivedAsync caused by {ex}");
                retStatus = false;
                return retStatus;
            }

            await Task.CompletedTask;
            // ＝＝＝＝＝＝＝＝＝＝＝＝＝ここまで＝＝＝＝＝＝＝＝＝＝＝＝＝

            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"End Method: OnDesiredPropertiesReceivedAsync");

            return retStatus;
        }

        /// <summary>
        /// メッセージ受信コールバック。					
        /// </summary>
        /// <param name="inputName"></param>
        /// <param name="message"></param>
        /// <param name="userContext"></param>
        /// <returns>
        /// 受信処理成否
        ///     true : 処理成功。
        ///     false ： 処理失敗。edgeHubから再送を受ける。
        /// </returns>
        public async Task<bool> OnMessageReceivedAsync(string inputName, IotMessage message, object userContext)
        {
            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Start Method: OnMessageReceivedAsync");

            // メッセージ受信時のコールバック処理を行う。
            // ＝＝＝＝＝＝＝＝＝＝＝＝＝ここから＝＝＝＝＝＝＝＝＝＝＝＝＝
            bool retStatus = true;
            try
            {
                byte[] messageBytes = message.GetBytes();

                MyLogger.WriteLog(ILogger.LogLevel.INFO, "1 message received.");

                string messageString = Encoding.UTF8.GetString(messageBytes);
                MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Received Message. Body: [{messageString}]");

                DBInfo dbinfo = (DBInfo)userContext;
                MyLogger.WriteLog(ILogger.LogLevel.DEBUG, $"Received message.  input name='{dbinfo.Input}'");

                var msgProps = message.GetProperties();
                var input = GaudiMessage.CreateInstance(messageString);

                // InfluxDBへデータを追加
                int rowIndex = 0;
                if (input == null || input.RecordList == null || input.RecordList.Count <= 0)
                {
                    // RecordListが0件の場合、ブランクフィールドで1行追加
                    InsertInfluxDB(dbinfo, msgProps);
                    rowIndex++;
                }
                else
                {
                    // RecordListが1件以上ある場合、RecordListの件数分追加
                    foreach (var record in input.RecordList)
                    {
                        InsertInfluxDB(dbinfo, msgProps, record, rowIndex);
                        rowIndex++;
                    }
                }
                MyLogger.WriteLog(ILogger.LogLevel.DEBUG, $"Insert {rowIndex} records.  input name='{dbinfo.Input}'");

            }
            catch (Exception ex)
            {
                MyLogger.WriteLog(ILogger.LogLevel.ERROR, $"OnMessageReceivedAsync failed. {ex}", true);
                retStatus = false;
            }
            await Task.CompletedTask;
            // ＝＝＝＝＝＝＝＝＝＝＝＝＝ここまで＝＝＝＝＝＝＝＝＝＝＝＝＝

            MyLogger.WriteLog(ILogger.LogLevel.DEBUG, $"Return status : {retStatus}");
            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"End Method: OnMessageReceivedAsync");
            return retStatus;
        }

        /// <summary>
        /// InfluxDBへデータを登録する
        /// </summary>
        /// <param name="dbinfo"></param>
        /// <param name="msgProps"></param>
        /// <param name="record"></param>
        /// <param name="rowIndex"></param>
        private static void InsertInfluxDB(DBInfo dbinfo, IDictionary<string, string> msgProps, GaudiMessage.GaudiRecord record = null, int rowIndex = -1)
        {
            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Start Method: InsertInfluxDB");
            Dictionary<string, string> tags = new Dictionary<string, string>();
            Dictionary<string, object> fields = new Dictionary<string, object>();

            // タグ情報作成
            foreach (var tag in dbinfo.TagList)
            {
                var tagvalue = tag.getTagValue(msgProps, rowIndex);
                tags.Add(tag.TagName, tagvalue);
                if (MyLogger.IsLogLevelToOutput(ILogger.LogLevel.TRACE))
                    MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"taginfo add : Key [ {tag.TagName} ] Value [ {tagvalue} ]");
            }

            // フィールド情報作成
            int index = 1;
            if (record != null)
            {
                if (dbinfo.UseHeader && record.RecordHeader != null)
                {
                    foreach (var val in record.RecordHeader)
                    {
                        object fieldVal = val;
                        if (DBInfo.FieldDataTypeMode.string_all == dbinfo.FieldDataType)
                        {
                            fieldVal = val.ToString();
                        }
                        fields.Add(index.ToString("D"), fieldVal);
                        if (MyLogger.IsLogLevelToOutput(ILogger.LogLevel.TRACE))
                            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"fieldinfo add : Key [ {index} ] Value [ {val} ]");
                        index++;
                    }
                }
                if (dbinfo.UseData && record.RecordData != null)
                {
                    foreach (var val in record.RecordData)
                    {
                        object fieldVal = val.Value;
                        if (DBInfo.FieldDataTypeMode.string_all == dbinfo.FieldDataType)
                        {
                            fieldVal = val.ToString();
                        }
                        fields.Add(index.ToString("D"), fieldVal);
                        if (MyLogger.IsLogLevelToOutput(ILogger.LogLevel.TRACE))
                            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"fieldinfo add : Key [ {index} ] Value [ {val} ]");
                        index++;
                    }
                }
            }
            // フィールドが0件の場合、ブランク列を追加
            if (fields.Count <= 0)
            {
                string s = "\"\"";
                // フィールド情報作成
                fields.Add("1", s);
                if (MyLogger.IsLogLevelToOutput(ILogger.LogLevel.TRACE))
                    MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"fieldinfo add : Key [ 1 ] Value [ {s} ]");
            }

            DateTime? timestampValue = null;

            // InfluxDBインサート
            switch (dbinfo.TimestampType)
            {
                case DBInfo.UserTimestampType.timestamp_index:
                    int targetIndex = dbinfo.TimestampIndex - 1;

                    if (record.RecordData.Count <= targetIndex)
                    {
                        // throw new Exception($"Out of index on RecordData. RecordData Count: {record.RecordData.Count}, specified: {targetIndex}");
                        timestampValue = null;
                    }
                    else
                    {
                        try
                        {
                            timestampValue = DateTime.Parse(record.RecordData[targetIndex].ToString()).ToUniversalTime();
                        }
                        catch
                        {
                            var errmsg = $"Unexpected data type: {record.RecordData[targetIndex]}.";
                            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Exit Method: InsertInfluxDB caused by {errmsg}");
                            throw new Exception(errmsg);
                        }
                    }
                    break;
                // case DBInfo.UserTimestampType.usertimestamp:
                //     timestampValue = dbinfo.TimestampValue;
                //     break;
                default:
                    timestampValue = null;
                    break;
            }

            SQLParser.InsertDB(dbinfo.Measurement, fields, tags, timestampValue);
            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"End Method: InsertInfluxDB");
        }

    }
}