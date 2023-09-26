using Furesoft.LowCode.Compilation;
using NiL.JS.Core;

namespace Furesoft.LowCode.Nodes.Queue;

public class ScriptInitializer : IScriptModuleInitalizer
{
    public void InitEngine(Context context)
    {
        context.Import("clearQueue", ClearQueue);
        context.Import("enqueue", Enqueue);
        context.Import("dequeue", Dequeue);
        context.Import("getItemCountInQueue", GetItemCountInQueue);
        context.Import("processQueue", ProcessQueue);
    }

    public void ClearQueue(string queueName)
    {
        QueueManager.Instance.ClearQueue(queueName);
    }

    public void Enqueue(string queueName, JSValue obj)
    {
        var value = obj.Value;

        QueueManager.Instance.Enqueue(queueName, value);
    }

    public object Dequeue(string queueName)
    {
        return Context.CurrentGlobalContext.WrapValue(QueueManager.Instance.Dequeue<object>(queueName));
    }

    public int GetItemCountInQueue(string queueName)
    {
        return QueueManager.Instance.GetItemCountInQueue(queueName);
    }

    public void ProcessQueue(string queue, ICallable callback)
    {
        while (QueueManager.Instance.GetItemCountInQueue(queue) > 0)
        {
            var args = new Arguments {Dequeue(queue)};

            callback.Call(null, args);
        }
    }
}
