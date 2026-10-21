namespace IotedgeV2InfluxDBRegister.Test;

[Collection(nameof(CollectionAttribute))]
public class GaudiMessage_GaudiRecord_GaudiData
{
    [Fact(DisplayName = "No.106_正常系：正常動作の確認_GaudiDataに任意のJValueを入力")]
    public void Case106()
    {
        JValue inputValue = new JValue("ABC");
        GaudiMessage.GaudiRecord.GaudiData result = new GaudiMessage.GaudiRecord.GaudiData(inputValue);
        
        // 結果確認
        Assert.IsAssignableFrom<GaudiMessage.GaudiRecord.GaudiData>(result);
        Assert.NotNull(result);
    }
}
