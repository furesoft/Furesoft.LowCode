namespace Furesoft.LowCode;

public static class StringExtensions
{
    public static Evaluatable<string> AsEvaluatable(this string s)
    {
        return new(s);
    }
}
