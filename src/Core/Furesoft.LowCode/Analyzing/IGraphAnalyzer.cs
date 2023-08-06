namespace Furesoft.LowCode.Analyzing;

public interface IGraphAnalyzer
{
    IList<Message> Messages { get; }
    public AnalyzerContext AnalyzerContext { get; set; }
    void Analyze(object node);
}
