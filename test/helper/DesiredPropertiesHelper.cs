namespace IotedgeV2InfluxDBRegister.Test;

/// <summary>
/// DesiredPropertiesでthrowされる例外メッセージのヘルパー
/// </summary>
public static class DesiredPropertiesHelper
{
    /// <summary>
    /// 必須プロパティが未指定の場合のエラーメッセージ
    /// </summary>
    internal static string RequiredPropertyNotSpecified(string name)
        => $"Required property '{name}' is not specified.";

    /// <summary>
    /// プロパティの取得エラーメッセージ
    /// </summary>
    internal static string ErrorRetrievingProperty(string name, string detail)
        => $"Error retrieving property '{name}': {detail}";

    /// <summary>
    /// プロパティがnullまたは空の場合のエラー詳細
    /// </summary>
    internal static string PropertyNullOrEmpty(string name)
        => $"Required property '{name}' cannot be null or empty.";

    /// <summary>
    /// プロパティが条件を満たさない場合のエラー詳細
    /// </summary>
    internal static string PropertyConditionNotMet(string name)
        => $"Property '{name}' did not meet the specified condition.";

    /// <summary>
    /// 不正なフォーマットの場合のエラー詳細
    /// </summary>
    internal static string IncorrectFormat(string value)
        => $"The input string '{value}' was not in a correct format.";

    /// <summary>
    /// 変換できない場合のエラー詳細（部分一致用）
    /// </summary>
    internal const string CannotConvert = "Can not convert";

    /// <summary>
    /// 変換できない場合のエラー詳細（部分一致用）
    /// </summary>
    internal const string CouldNotConvert = "Could not convert";

    /// <summary>
    /// JSONデシリアライズ型エラー（部分一致用）
    /// </summary>
    internal const string DeserializedJsonType = "Deserialized JSON type";
}
