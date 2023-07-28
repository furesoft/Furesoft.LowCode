using System.ComponentModel;
using System.Runtime.Serialization;
using Furesoft.LowCode.Designer.Core;
using Furesoft.LowCode.Designer.Core.NodeBuilding;
using Furesoft.LowCode.Editor.Model;

namespace Furesoft.LowCode.Nodes.IO;

[Description("Create a directory")]
[NodeCategory("IO")]
public class MakeDirNode : VisualNode
{
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public string Path { get; set; }
    public MakeDirNode() : base("Make Directory")
    {
    }
    
    [Pin("Flow Output", PinAlignment.Bottom)]
    public IOutputPin OutputPin { get; set; }
    
    [Pin("Flow Input", PinAlignment.Top)]
    public IInputPin InputPin { get; set; }

    public override Task Execute()
    {
        Directory.CreateDirectory(Evaluate<string>(Path));

        return ContinueWith(OutputPin);
    }
}
