namespace Furesoft.LowCode;

public class Evaluatable<T>
{
    public Evaluatable(string source)
    {
        Source = source;
    }

    internal string Source { get; set; }
}
