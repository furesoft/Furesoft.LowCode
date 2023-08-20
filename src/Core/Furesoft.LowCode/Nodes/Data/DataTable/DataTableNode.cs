using NiL.JS.Extensions;

namespace Furesoft.LowCode.Nodes.Data.DataTable;

public abstract class DataTableNode : InputOutputNode
{
    public string TableName { get; set; }
    public DataTableNode(string label) : base(label)
    {
    }

    public override async Task Execute(CancellationToken cancellationToken)
    {
        await Invoke(cancellationToken);

        await ContinueWith(OutputPin, cancellationToken);
    }

    protected System.Data.DataTable GetTable()
    {
       return Context.GetVariable(TableName).As<System.Data.DataTable>();
    }

    protected abstract Task Invoke(CancellationToken cancellationToken);
}
