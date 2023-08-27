using Runly;

namespace RunlyRunner;

public class Program
{
    static async Task Main(string[] args)
    {
        await JobHost.CreateDefaultBuilder(args)
            .ConfigureServices((host, services) =>
            {

            })
            .Build()
            .RunJobAsync();
    }
}
