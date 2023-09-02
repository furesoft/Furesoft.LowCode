namespace Furesoft.LowCode;

public interface IProgressReporter
{
    void Report(byte percentProgress, string message);
}
