using System.Text;

namespace IotedgeV2InfluxDBRegister.Test;
public class MultiTextWriter : TextWriter
{
    private readonly List<TextWriter> writers;

    public MultiTextWriter(params TextWriter[] writers)
    {
        this.writers = [.. writers];
    }

    public override Encoding Encoding => writers[0].Encoding;

    public override void Write(char value)
    {
        foreach (var w in writers)
            w.Write(value);
    }

    public override void Write(string? value)
    {
        foreach (var w in writers)
            w.Write(value);
    }

    public override void WriteLine(string? value)
    {
        foreach (var w in writers)
            w.WriteLine(value);
    }

    public override void Flush()
    {
        foreach (var w in writers)
            w.Flush();
    }

    /// <summary>
    /// writersの中にStringWriterがあれば、その内容を返す（最初に見つかったもの）
    /// </summary>
    public string GetStringWriterContent()
    {
        string retStr = "No String Writer.";
        foreach (var w in writers)
        {
            if (w is StringWriter sw)
            {
                retStr = sw.GetStringBuilder().ToString().Trim();
            }
        }
        return retStr;
    }

    /// <summary>
    /// writersの中にStringWriterがあれば、その内容をクリアする（最初に見つかったもの）
    /// </summary>
    public void ClearStringWriterContent()
    {
        foreach (var w in writers)
        {
            if (w is StringWriter sw)
            {
                sw.GetStringBuilder().Clear();
                break;
            }
        }
    }

}