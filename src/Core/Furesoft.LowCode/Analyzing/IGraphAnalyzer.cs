namespace Furesoft.LowCode.Analyzing;

public interface IGraphAnalyzer
{
    public IList<Message> Messages { get; }
    void Analyze(object node);
}
