using Furesoft.LowCode.Evaluation;

namespace TestProject;

[TestFixture]
public class Tests
{
    [Test]
    [TestCase("csv.json", "testDB")]
    public async Task TestByJsVariable(string filename, string resultingJSVar)
    {
        var evaluator = new Evaluator(File.OpenRead($"Testfiles/{filename}"));
        var cts = new CancellationTokenSource();

        await evaluator.Execute(cts.Token);

        await Verify(evaluator.Context.GetVariable(resultingJSVar).Value);
    }
}
