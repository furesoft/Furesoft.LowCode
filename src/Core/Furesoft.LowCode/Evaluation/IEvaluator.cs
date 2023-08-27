namespace Furesoft.LowCode.Evaluation;

public interface IEvaluator
{
    public Task Execute(CancellationToken cancellationToken);
}
