using System.ComponentModel;

namespace Furesoft.LowCode.Nodes.Queue;

[Description("Clear the queue")]
[NodeIcon(
    "M14.998 3.18 11.844.755C10.972.084 9.709.249 9.037 1.12L.637 12.017C-.034 12.888.13 14.149 1.002 14.82L4.155 17.245C5.027 17.916 6.29 17.751 6.962 16.88L15.363 5.983C16.034 5.112 15.87 3.851 14.998 3.18ZM6.548 15.467 6.011 16.148C5.859 16.345 5.626 16.458 5.372 16.458 5.248 16.458 5.063 16.429 4.887 16.294L1.734 13.869C1.566 13.74 1.459 13.552 1.431 13.339 1.403 13.126 1.459 12.917 1.588 12.75L2.125 12.069C2.277 11.872 2.51 11.758 2.765 11.758 2.889 11.758 3.074 11.787 3.25 11.922L6.404 14.347C6.572 14.476 6.679 14.664 6.707 14.877 6.733 15.09 6.677 15.299 6.548 15.467Z")]
public class ClearNode() : QueueBaseNode("Clear Queue")
{
    public override Task Invoke(CancellationToken cancellationToken)
    {
        QueueManager.Instance.ClearQueue(Queue);

        return Task.CompletedTask;
    }
}
