using DiskQueue;

namespace Furesoft.LowCode.Nodes.Queue;

public class QueueManager : IDisposable
{
    public static readonly QueueManager Instance = new();
    private readonly Dictionary<string, IPersistentQueue> _queues = new();

    public QueueManager()
    {
        BasePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    }

    public string BasePath { get; set; }

    public void Dispose()
    {
        foreach (var queue in _queues)
        {
            queue.Value.Dispose();
        }

        _queues.Clear();
    }

    ~QueueManager()
    {
        Dispose();
    }

    public void Enqueue<T>(string queueName, T obj)
    {
        var queue = GetQueue<T>(queueName);
        using var session = queue.OpenSession();

        session.Enqueue(obj);
        session.Flush();
    }

    public T Dequeue<T>(string queueName)
    {
        var queue = GetQueue<T>(queueName);
        using var session = queue.OpenSession();

        var obj = session.Dequeue();
        session.Flush();

        return obj;
    }

    public int GetItemCountInQueue(string queueName)
    {
        var queue = GetQueue<object>(queueName);

        return queue.EstimatedCountOfItemsInQueue;
    }
    
    public void ClearQueue(string queueName)
    {
        var queue = GetQueue<object>(queueName);

        queue.HardDelete(true);
    }

    private IPersistentQueue<T> GetQueue<T>(string queueName)
    {
        if (!_queues.ContainsKey(queueName))
        {
            var queue = new PersistentQueue<T>(Path.Combine(BasePath, queueName));
            _queues.Add(queueName, queue);
        }

        return _queues[queueName] as PersistentQueue<T>;
    }
}
