namespace Furesoft.LowCode.Analyzing;

public class Message
{
    public Severity Severity { get; set; }
    public string Content { get; set; }
    public object[] Targets { get; set; }
}
