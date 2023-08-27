using Furesoft.LowCode.Evaluation;
using Runly;

namespace RunlyRunner;

public class GraphRunner : Job<GraphRunnerConfig>
{
    public GraphRunner(GraphRunnerConfig config) : base(config)
    {
    }

    public override async Task<Result> ProcessAsync()
    {
        var evaluator = new Evaluator(Config.Graph);

        await evaluator.Execute(CancellationToken);

        return Result.Success();
    }
}
