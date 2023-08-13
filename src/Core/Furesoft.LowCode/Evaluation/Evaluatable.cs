namespace Furesoft.LowCode.Evaluation;

public class Evaluatable<T>
{
    public Evaluatable(string source)
    {
        Source = source;
    }

    internal string Source { get; set; }
}
