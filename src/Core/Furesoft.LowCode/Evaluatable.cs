namespace Furesoft.LowCode;

public class Evaluatable
{
    public Evaluatable(string source)
    {
        Source = source;
    }

    internal string Source { get; set; }
}
