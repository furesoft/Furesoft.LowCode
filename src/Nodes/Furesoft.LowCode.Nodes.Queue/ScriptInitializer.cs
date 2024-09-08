using Furesoft.LowCode.Evaluation;
using NiL.JS.Core;
using NiL.JS.Core.Interop;

namespace Furesoft.LowCode.Nodes.Queue;

public class ScriptInitializer : IScriptModuleInitalizer
{
    public void InitEngine(Context context)
    {
        context.ImportAsObject<Nodes.ScriptInitializer>("Queue");
    }

    [JavaScriptName("clear")]
    public void ClearQueue(string queueName)
    {
        QueueManager.Instance.ClearQueue(queueName);
    }

    [JavaScriptName("enqueue")]
    public void Enqueue(string queueName, JSValue obj)
    {
        var value = obj.Value;

        QueueManager.Instance.Enqueue(queueName, value);
    }

    [JavaScriptName("dequeue")]
    public object Dequeue(string queueName)
    {
        return Context.CurrentGlobalContext.WrapValue(QueueManager.Instance.Dequeue<object>(queueName));
    }

    [JavaScriptName("count")]
    public int GetItemCountInQueue(string queueName)
    {
        return QueueManager.Instance.GetItemCountInQueue(queueName);
    }

    [JavaScriptName("process")]
    public void ProcessQueue(string queue, ICallable callback)
    {
        while (QueueManager.Instance.GetItemCountInQueue(queue) > 0)
        {
            var args = new Arguments {Dequeue(queue)};

            callback.Call(null, args);
        }
    }
}
