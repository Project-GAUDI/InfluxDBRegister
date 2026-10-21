namespace IotedgeV2InfluxDBRegister.Test;

[Collection(nameof(CollectionAttribute))]
public class GaudiMessage_GaudiRecord_GaudiData_ToString
{
    [Fact(DisplayName = "No.108_正常系：正常動作の確認_GaudiDataに任意のJValueを入力")]
    public void Case108()
    {
        JValue inputValue = new JValue("ABC");
        GaudiMessage.GaudiRecord.GaudiData result = new GaudiMessage.GaudiRecord.GaudiData(inputValue);

        // 結果確認
        Assert.IsAssignableFrom<string>(result.ToString());
        Assert.Equal("ABC", result.ToString());
    }

    [Fact(DisplayName = "No.138_正常系：正常動作の確認_GaudiDataにnullを入力")]
    public void Case138()
    {
        JValue inputValue = new JValue((string?)null);
        GaudiMessage.GaudiRecord.GaudiData result = new GaudiMessage.GaudiRecord.GaudiData(inputValue);
        
        // 結果確認
        Assert.Equal("null", result.ToString());
    }
}
