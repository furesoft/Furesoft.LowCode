﻿namespace Furesoft.LowCode.Analyzing;

public abstract class GraphAnalyzerBase<T> : IGraphAnalyzer
{
    public IList<Message> Messages { get; } = new List<Message>();
    public AnalyzerContext AnalyzerContext { get; set; }

    public void Analyze(object node)
    {
        Analyze((T)node);
    }

    public abstract void Analyze(T node);

    protected void AddMessage(Severity severity, string message, params object[] targets)
    {
        var msg = new Message {Severity = severity, Content = message, Targets = targets};

        Messages.Add(msg);
    }

    protected void AddError(string message, params object[] targets)
    {
        AddMessage(Severity.Error, message, targets);
    }

    protected void AddWarning(string message, params object[] targets)
    {
        AddMessage(Severity.Warning, message, targets);
    }

    protected void AddInfo(string message, params object[] targets)
    {
        AddMessage(Severity.Info, message, targets);
    }
}
