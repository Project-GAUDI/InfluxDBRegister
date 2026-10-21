using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using InfluxDB.Collector;
using InfluxDB.Collector.Diagnostics;
using TICO.GAUDI.Commons;

namespace IotedgeV2InfluxDBRegister
{

    /// <summary>
    /// ルート情報
    /// </summary>
    class SQLParser
    {
        static CollectorConfiguration Collector { get; set; } = new CollectorConfiguration();
        static MetricsCollector MetricsCollector { get; set; }
        static ILogger MyLogger { get; } = LoggerFactory.GetLogger(typeof(Program));
        
        // DB接続情報（存在チェック用）
        private static string _username;
        private static string _password;
        private static string _dbname;
        private static string _uri;
        private static readonly HttpClient _httpClient = new()
        {
            Timeout = TimeSpan.FromSeconds(30) // 30秒タイムアウト
        };

        public SQLParser()
        {
            CollectorLog.RegisterErrorHandler((message, exception) =>
            {
                MyLogger.WriteLog(ILogger.LogLevel.ERROR, $"{message}: {exception}", true);
            });
        }

        public static async Task InitParserAsync(string userenv, string passenv, string dbenv, string urienv, int interval)
        {
            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Start Method: InitParserAsync");

            // 接続情報を保存
            _username = userenv;
            _password = passenv;
            _dbname = dbenv;
            _uri = urienv;

            // DB存在チェックを実行
            await CheckDBExistsAsync().ConfigureAwait(false);

            Collector.Batch.AtInterval(TimeSpan.FromSeconds(interval));
            Collector.WriteTo.InfluxDB(urienv, dbenv, userenv, passenv);
            MetricsCollector = Collector.CreateCollector();

            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"End Method: InitParserAsync");
        }

        public static bool InsertDB(string measurement, Dictionary<string, object> fields, Dictionary<string, string> tags = null, DateTime? timestamp = null)
        {
            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Start Method: InsertDB");

            try
            {
                if (MyLogger.IsLogLevelToOutput(ILogger.LogLevel.TRACE))
                {
                    foreach (var val in fields)
                    {
                        MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Insert params field:Key=[{ val.Key }] Value=[{ val.Value }]");
                    }
                    foreach (var val in tags)
                    {
                        MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Insert params tag:Key=[{ val.Key }] Value=[{ val.Value }]");
                    }
                }
                MetricsCollector.Write(measurement, fields, tags, timestamp);
                MyLogger.WriteLog(ILogger.LogLevel.TRACE, "InfluxDB Insert success");
            }
            catch(Exception ex)
            {
                MyLogger.WriteLog(ILogger.LogLevel.ERROR, $"InfluxDB Insert failure. Exception:{ex}", true);
                return false;
            }

            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"End Method: InsertDB");
            return true;
        }

        /// <summary>
        /// InfluxDBのデータベース存在チェック
        /// </summary>
        public static async Task CheckDBExistsAsync()
        {
            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Start Method: CheckDBExistsAsync");
            try
            {
                // SHOW DATABASESクエリを実行
                string queryUrl = $"{_uri.TrimEnd('/')}/query";
                string query = "SHOW DATABASES";

                using var request = new HttpRequestMessage(HttpMethod.Get, $"{queryUrl}?q={Uri.EscapeDataString(query)}");
                // Basic認証の設定
                if (!string.IsNullOrEmpty(_username) && !string.IsNullOrEmpty(_password))
                {
                    var credentials = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{_username}:{_password}"));
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);
                }

                var response = await _httpClient.SendAsync(request).ConfigureAwait(false);
                string responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                // responseのステータスが成功かつデータベースが存在する場合
                if (response.IsSuccessStatusCode && responseContent.Contains($"\"{_dbname}\""))
                {
                    MyLogger.WriteLog(ILogger.LogLevel.INFO, $"Database '{_dbname}' exists.");
                }
                else
                {
                    throw new Exception($"Failed to check database existence. dbname: {_dbname}, Status: {response.StatusCode}, Response: {responseContent}");
                }
            }
            catch (Exception ex)
            {
                MyLogger.WriteLog(ILogger.LogLevel.ERROR, $"Error checking database existence: {ex.Message}");
                throw;
            }
            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"End Method: CheckDBExistsAsync");
        }

    }
}
