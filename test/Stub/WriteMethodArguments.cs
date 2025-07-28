namespace IotedgeV2InfluxDBRegister.Test.Stub;

public class WriteMethodArguments
{
    public string? Measurement { get; set; }

    public IReadOnlyDictionary<string, object>? Fields { get; set; }

    public IReadOnlyDictionary<string, string>? Tags { get; set; }

    public DateTime? Timestamp { get; set; }

    public static WriteMethodArguments Parse(string measurement, IReadOnlyDictionary<string, object> fields, IReadOnlyDictionary<string, string>? tags, DateTime? timestamp) {
        var instance = new WriteMethodArguments()
        {
            Measurement = measurement,
            Fields = fields,
            Tags = tags,
            Timestamp = timestamp
        };
        return instance;
    }
}
