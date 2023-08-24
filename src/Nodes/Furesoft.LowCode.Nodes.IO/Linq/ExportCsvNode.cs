using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Furesoft.LowCode.Attributes;
using Furesoft.LowCode.Evaluation;

namespace Furesoft.LowCode.Nodes.IO.Linq;
[Description("Exports a Piped variable to a CSV")]
[NodeCategory("Linq")]
internal class ExportCsvNode : InputOutputNode
{
    public Evaluatable<string> TargetPath { get; set; }
    public Evaluatable<string> Delimiter { get; set; }
    public Evaluatable<bool> WithHeader { get; set; }
    public ExportCsvNode() : base("Export CSV")
    {
    }

    public IEnumerable ConvertPreviousToPipeable()
    {
        var node = GetPreviousNode<InputOutputNode>();
        if (node is IPipeable pipe)
        {
            return pipe.PipeVariable;
        }
        else if (node is IOutVariableProvider outVariableProvider)
        {
            var pip = Evaluate(new Evaluatable<object>(outVariableProvider.OutVariable));

            if (pip is IEnumerable pipes)
            {
                return pipes;
            }
            else
            {
                return new List<object>() { pip };
            }
        }
        return Enumerable.Empty<object>();
    }

    public override Task Execute(CancellationToken cancellationToken)
    {
        var pipeData = ConvertPreviousToPipeable();
        using (StreamWriter writer = new StreamWriter(TargetPath))
        {
            if (pipeData.Cast<object>().Any())
            {
                // Get the type of the first item in the enumerable
                Type dataType = pipeData.Cast<object>().First().GetType();

                // Write the CSV header based on property names
                var properties = dataType.GetProperties();
                writer.WriteLine(string.Join(Delimiter, properties.Select(p => p.Name)));

                // Write each data item as a CSV line
                foreach (var item in pipeData)
                {
                    var prop = item.GetType().GetProperties();
                    string line = string.Join(Delimiter, prop.Select(p => p.GetValue(item)));
                    writer.WriteLine(line);
                }
            }
        }
        return ContinueWith(OutputPin, cancellationToken);
    }
}
