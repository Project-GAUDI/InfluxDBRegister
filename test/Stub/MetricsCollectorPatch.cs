using HarmonyLib;
using InfluxDB.Collector;

namespace IotedgeV2InfluxDBRegister.Test.Stub;

/// <summary>
/// MetricsCollectorのWrite関数をスタブ化するためのパッチ
/// </summary>
/// <remarks>
/// 改修時は<see href="https://harmony.pardeike.net/articles/intro.html">ドキュメント</see>参照
/// </remarks>
[HarmonyPatch(typeof(MetricsCollector), nameof(MetricsCollector.Write))]
public class MetricsCollectorPatch
{
    /// <summary>
    /// MetricsCollector.Write が呼ばれる前に自動的に差し込まれ、呼び出した引数の保管を行う
    /// </summary>
    /// <returns>常にfalse (インスタンス本体のWrite関数を動作させないため)</returns>
    [HarmonyPrefix]
    public static bool WritePrefix(
        MetricsCollector __instance,
        string measurement,
        IReadOnlyDictionary<string, object> fields,
        IReadOnlyDictionary<string, string>? tags,
        DateTime? timestamp)
    {
        var instance = __instance as MockMetricsCollector;
        instance!.StackWriteArguments(measurement, fields, tags, timestamp);

        return false;
    }
}
