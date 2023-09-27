using System.ComponentModel;
using Furesoft.LowCode.Attributes;
using Furesoft.LowCode.Compilation;
using Furesoft.LowCode.Nodes.Data.DataTable;
using Furesoft.LowCode.Nodes.Data.DataTable.Core;

namespace Furesoft.LowCode.Nodes.Data.Office;

[NodeCategory("Data/Office")]
[Description("Reads Excel File to DataTable")]
[NodeIcon(
    "M907.094 467.103h-162.854v81.427h162.853v-81.427zM907.094 420.573h-162.853v-81.427h162.854v81.427zM907.094 804.442h-162.853v-81.427h162.854v81.427zM907.094 676.486h-162.853v-81.427h162.854v81.427zM907.094 292.617h-162.853v-81.427h162.854v81.427zM1019.579 132.09c-4.653-24.195-33.734-24.777-53.16-25.591h-361.767v-104.691h-72.237l-532.415 93.059v814.151l535.555 93.175h69.096v-92.711l349.321.001c19.659-.814 41.295.582 58.395-11.167 11.981-17.216 10.819-39.085 11.633-58.86l-.466-605.582c-.582-33.85 3.141-68.398-3.955-101.783zM426.56 314.602c-32.105 65.141-64.792 129.818-96.781 194.959 31.64 63.396 62.815 127.026 93.873 190.655-26.406-1.28-52.811-2.908-79.1-4.769-19.659-47.809-42.575-94.339-57.929-143.776-14.308 46.646-33.269 91.547-50.601 137.03-25.591-1.396-51.182-2.908-76.774-4.42 26.987-59.558 55.719-118.301 81.892-178.208-30.826-58.162-59.674-117.138-89.569-175.649 25.475-1.047 50.95-2.094 76.425-2.443 18.146 46.297 40.713 90.849 56.533 138.076 14.192-50.717 38.271-97.596 58.046-146.103 28.034-1.978 55.952-3.722 83.986-5.351zM968.52 854.236h-363.869v-49.794h93.059v-81.427h-93.059v-46.53h93.059v-81.427h-93.059v-46.53h93.059v-81.427h-93.059v-46.53h93.059v-81.427h-93.059v-46.53h93.059v-81.427h-93.059v-53.794h363.869v696.84z")]
public class WriteExcelNode : DataTableNode
{
    public WriteExcelNode() : base(TableAction.Write, "Write Excel File")
    {
    }

    protected override Task Invoke(CancellationToken cancellationToken)
    {
        ScriptInitializer.WriteExcel(Path, GetTable());

        return Task.CompletedTask;
    }

    public override void Compile(CodeWriter builder)
    {
        CompileWriteCall(builder, "XLS.write", Path, TableName.AsEvaluatable());
    }
}
