using IotedgeV2InfluxDBRegister;
using System.Reflection;
using TICO.GAUDI.Commons;

namespace IotedgeV2InfluxDBRegister.Test;

[Collection(nameof(CollectionAttribute))]
public class LoggerHelper
{
    internal static (ILogger logger, MultiTextWriter writer) CreateLoggerWithMultiTextWriter(Func<MyApplicationMain, ILogger> getLogger, MyApplicationMain app, ILogger.LogLevel logLevel)
    {
        var logger = getLogger(app);
        var stringWriter = new StringWriter();
        var consoleWriter = Console.Out;
        var multiWriter = new MultiTextWriter(consoleWriter,stringWriter);
        logger.SetOutputLogLevel(logLevel.ToString());
        logger.SetOutputTextWriter(multiWriter); //StringWriterだけを渡すとGetString~を実行しない限りコンソールにログが出なくなってしまうため、MultiTextWriterを渡すこと
        return (logger, multiWriter);
    }

    /// <summary>
    /// MyApplicationMainのMyLoggerをキャプチャ設定する
    /// </summary>
    internal static MultiTextWriter SetupMyApplicationMainLogger(ILogger.LogLevel logLevel = ILogger.LogLevel.TRACE)
    {
        var logger = (ILogger)typeof(MyApplicationMain).GetProperty("MyLogger", BindingFlags.Static | BindingFlags.NonPublic)!.GetValue(null)!;
        var stringWriter = new StringWriter();
        var consoleWriter = Console.Out;
        var multiWriter = new MultiTextWriter(consoleWriter, stringWriter);
        logger.SetOutputLogLevel(logLevel.ToString());
        logger.SetOutputTextWriter(multiWriter);
        return multiWriter;
    }

    internal static string GetStringWriterContent(MultiTextWriter writer,ITestOutputHelper output){
        var consoleOutput = writer.GetStringWriterContent();
        output.WriteLine("-- StringWriter Output Log Start --");
        output.WriteLine(consoleOutput);
        output.WriteLine("-- StringWriter Output Log End --");
        return consoleOutput;
    }

    /// <summary>
    /// ログに指定した文字列が含まれていることを検証する
    /// </summary>
    internal static void AssertLogContains(MultiTextWriter writer, ITestOutputHelper output, string expected)
    {
        var log = GetStringWriterContent(writer, output);
        Assert.Contains(expected, log);
    }

    #region Commons出力ログメッセージ
    /// <summary>
    /// 必須環境変数が未指定の場合のエラーメッセージ
    /// </summary>
    internal static string RequiredEnvVarNotSpecified(string name)
        => $"Required environment variable '{name}' is not specified.";

    /// <summary>
    /// 環境変数の取得エラーメッセージ
    /// </summary>
    internal static string ErrorRetrievingEnvVar(string name)
        => $"Error retrieving environment variable '{name}'";

    /// <summary>
    /// 環境変数が条件を満たさない場合のエラーメッセージ
    /// </summary>
    internal static string EnvVarConditionNotMet(string name)
        => $"Environment variable '{name}' did not meet the specified condition.";

    /// <summary>
    /// メソッド終了ログメッセージ
    /// </summary>
    internal static string EndMethod(string methodName)
        => $"End Method: {methodName}";

    /// <summary>
    /// メソッド異常終了ログメッセージ
    /// </summary>
    internal static string ExitMethod(string methodName)
        => $"Exit Method: {methodName}";

    /// <summary>
    /// メソッド異常終了ログメッセージ（原因付き）
    /// </summary>
    internal static string ExitMethodCausedBy(string methodName)
        => $"Exit Method: {methodName} caused by";

    /// <summary>
    /// Exit Methodログが含まれていることを検証する
    /// </summary>
    internal static void AssertExitMethodLog(MultiTextWriter writer, ITestOutputHelper output, string methodName)
    {
        AssertLogContains(writer, output, ExitMethod(methodName));
    }

    /// <summary>
    /// Exit Method (caused by)ログが含まれていることを検証する
    /// </summary>
    internal static void AssertExitMethodCausedByLog(MultiTextWriter writer, ITestOutputHelper output, string methodName)
    {
        AssertLogContains(writer, output, ExitMethodCausedBy(methodName));
    }
    #endregion
}