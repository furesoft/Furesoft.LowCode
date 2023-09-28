namespace Furesoft.LowCode.Compilation;

public class GraphCompiler
{
    public string Compile(EmptyNode entry)
    {
        var sb = new CodeWriter();

        entry.Compile(sb);

        return sb.ToString();
    }

    public string Compile(IDrawingNode drawing)
    {
        var writer = new CodeWriter();
        var signals = drawing.GetNodes<SignalNode>();

        foreach (var signal in signals.Select(_ => _.DefiningNode).OfType<SignalNode>())
        {
            signal.Drawing = drawing;
            writer.Append(Compile(signal));
        }

        writer.AppendLine();

        var entryNode = drawing.GetNodes<EntryNode>().First().DefiningNode;

        var entry = Compile(entryNode);
        writer.AppendLine(entry);

        return writer.ToString();
    }
}
