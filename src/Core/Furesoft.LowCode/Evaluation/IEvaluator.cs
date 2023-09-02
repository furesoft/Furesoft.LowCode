namespace Furesoft.LowCode.Evaluation;

public interface IEvaluator
{
    public IProgressReporter Progress { get; set; }
    public Task Execute(CancellationToken cancellationToken);
}
