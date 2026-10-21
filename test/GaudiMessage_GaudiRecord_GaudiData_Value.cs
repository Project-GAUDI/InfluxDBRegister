namespace IotedgeV2InfluxDBRegister.Test;

[Collection(nameof(CollectionAttribute))]
public class GaudiMessage_GaudiRecord_GaudiData_Value
{
    [Fact(DisplayName = "No.107_正常系：正常動作の確認_GaudiDataに任意のJValueを入力")]
    public void Case107()
    {
        JValue inputValue = new JValue("ABC");
        GaudiMessage.GaudiRecord.GaudiData result = new GaudiMessage.GaudiRecord.GaudiData(inputValue);
        
        // 結果確認
        Assert.NotNull(result.Value);
        Assert.Equal("ABC", result.Value);
    }
}
