using System.ComponentModel;
using Furesoft.LowCode.Attributes;
using Furesoft.LowCode.Nodes.Data.DataTable;
using Furesoft.LowCode.Nodes.Data.DataTable.Core;

namespace Furesoft.LowCode.Nodes.Data.XML;

[NodeCategory("Data/XML")]
[Description("Write DataTable To XML")]
[NodeIcon(
    "M11.89 0 13.85.4 10.11 18 8.15 17.6 11.89 0M18.59 9 15 5.41V2.58L21.42 9 15 15.41V12.58L18.59 9M.58 9 7 2.58V5.41L3.41 9 7 12.58V15.41L.58 9Z")]
public class ReadXmlNode() : DataTableNode(TableAction.Read, "Read XML")
{
    protected override Task Invoke(CancellationToken cancellationToken)
    {
        ScriptInitializer.ReadXml(Path, GetTable());

        return Task.CompletedTask;
    }
}
