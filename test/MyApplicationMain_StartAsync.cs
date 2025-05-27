namespace IotedgeV2InfluxDBRegister.Test;

[Collection(nameof(CollectionAttribute))]
public class MyApplicationMain_StartAsync
{
    private readonly MyApplicationMain _app = new ();

    [Fact(DisplayName = "No.21 \"DBInfos\"にnullが設定されていること。")]
    public async Task Case021()
    {
        var type = typeof(MyApplicationMain);
        type.GetProperty("DBInfos", BindingFlags.Static | BindingFlags.NonPublic)!.SetValue(null, null);

        await Assert.ThrowsAnyAsync<Exception>(_app.StartAsync);
    }

    [Fact(DisplayName = "No.22 \"DBInfos\"が初期化されており、0件のリストになっていること。")]
    public async Task Case022()
    {
        var type = typeof(MyApplicationMain);
        type.GetProperty("DBInfos", BindingFlags.Static | BindingFlags.NonPublic)!.SetValue(null, new List<DBInfo>());

        var result = await _app.StartAsync();
        Assert.True(result);
    }

    [Fact(DisplayName = "No.23 \"DBInfos\"が初期化されており、\"Input\"プロパティがnullの1件のデータが追加されていること。",
        Skip = "ModuleClientのインスタンス生成がCommonsによるライフサイクルで動作するため、現構成での実装は不可。自動テストから除外")]
    public void Case023()
    {
        throw new NotImplementedException();
    }

    [Fact(DisplayName = "No.24 \"DBInfos\"が初期化されており、\"Input\"プロパティが空文字の1件のデータが追加されていること。",
        Skip = "ModuleClientのインスタンス生成がCommonsによるライフサイクルで動作するため、現構成での実装は不可。自動テストから除外")]
    public void Case024()
    {
        throw new NotImplementedException();
    }

    [Fact(DisplayName = "No.25 \"DBInfos\"が初期化されており、\"Input\"プロパティに\"TEST-INPUT\"の文字列が入った1件のデータが追加されていること。",
        Skip = "ModuleClientのインスタンス生成がCommonsによるライフサイクルで動作するため、現構成での実装は不可。自動テストから除外")]
    public void Case025()
    {
        throw new NotImplementedException();
    }
}
