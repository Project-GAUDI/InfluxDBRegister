namespace IotedgeV2InfluxDBRegister.Test;

[Collection(nameof(CollectionAttribute))]
public class MyApplicationMain_TerminateAsync
{
    private readonly MyApplicationMain _app = new ();

    [Fact(DisplayName = "No.26 特になし")]
    public async Task Case026()
    {
        var result = await _app.TerminateAsync();
        Assert.True(result);
    }
}
