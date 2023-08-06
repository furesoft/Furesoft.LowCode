namespace Furesoft.LowCode;

public sealed class GraphException : Exception
{
    private readonly EmptyNode _node;

    public GraphException(Exception innerException, EmptyNode node) : base(innerException.Message, innerException)
    {
        _node = node;
        Source = _node.Label;
    }

    public override string StackTrace => _node.GetCallStack();
}
