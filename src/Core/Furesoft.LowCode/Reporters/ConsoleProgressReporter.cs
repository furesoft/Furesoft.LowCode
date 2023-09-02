namespace Furesoft.LowCode.Reporters;

public class ConsoleProgressReporter : IProgressReporter
{
    private const char _block = '■';
    private const string _back = "\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b";
    private const string _twirl = "-\\|/";

    public void Report(byte percentProgress, string message)
    {
        WriteProgressBar(percentProgress, true);

        Console.Write($" {message}");
    }

    public static void WriteProgressBar(int percent, bool update = false)
    {
        if (update)
        {
            Console.Write(_back);
        }

        Console.Write("[");
        var p = (int)(percent / 10f + .5f);

        for (var i = 0; i < 10; ++i)
        {
            Console.Write(i >= p ? ' ' : _block);
        }

        Console.Write("] {0,3:##0}%", percent);
    }

    public static void WriteProgress(int progress, bool update = false)
    {
        if (update)
        {
            Console.Write("\b");
        }

        Console.Write(_twirl[progress % _twirl.Length]);
    }
}
