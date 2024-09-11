using System;
using System.Collections.Generic;
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
        static CollectorConfiguration collector { get; set; } = new CollectorConfiguration();
        static MetricsCollector metricsCollector { get; set; }
        static ILogger MyLogger { get; } = LoggerFactory.GetLogger(typeof(Program));

        public SQLParser()
        {
            CollectorLog.RegisterErrorHandler((message, exception) =>
            {
                MyLogger.WriteLog(ILogger.LogLevel.ERROR, $"{message}: {exception}", true);
            });
        }

        public void InitParser(string userenv, string passenv, string dbenv, string urienv, int interval)
        {
            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"Start Method: InitParser");

            collector.Batch.AtInterval(TimeSpan.FromSeconds(interval));
            collector.WriteTo.InfluxDB(urienv, dbenv, userenv, passenv);
            metricsCollector = collector.CreateCollector();

            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"End Method: InitParser");
        }

        public static void InsertDB(string measurement, Dictionary<string, object> fields, Dictionary<string, string> tags = null, DateTime? timestamp = null)
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
                metricsCollector.Write(measurement, fields, tags, timestamp);
                MyLogger.WriteLog(ILogger.LogLevel.TRACE, "InfluxDB Insert success");
            }
            catch(Exception ex)
            {
                MyLogger.WriteLog(ILogger.LogLevel.ERROR, $"InfluxDB Insert failure. Exception:{ex}", true);
            }

            MyLogger.WriteLog(ILogger.LogLevel.TRACE, $"End Method: InsertDB");
        }
    }
}
