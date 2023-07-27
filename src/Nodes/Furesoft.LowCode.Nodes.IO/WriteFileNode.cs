using System.ComponentModel;
using System.Runtime.Serialization;
using Furesoft.LowCode.Designer.Core;
using Furesoft.LowCode.Designer.Core.Components.Views;
using Furesoft.LowCode.Designer.Core.NodeBuilding;
using Furesoft.LowCode.Editor.Model;

namespace Furesoft.LowCode.Nodes.IO;

[Description("Read a file")]
[NodeCategory("IO/Files")]
[NodeView(typeof(IconNodeView), "M122.375 84.375A9.375 9.375 0 00103.625 84.375V155.4938L82.1375 133.9875A9.375 9.375 0 1068.8625 147.2625L106.3625 184.7625A9.375 9.375 0 00119.6375 184.7625L157.1375 147.2625A9.375 9.375 0 00143.8625 133.9875L122.375 155.4938V84.375zM225.5 37.5V215.625L141.125 300H38A37.5 37.5 0 01.5 262.5V37.5A37.5 37.5 0 0138 0H188A37.5 37.5 0 01225.5 37.5zM141.125 243.75A28.125 28.125 0 01169.25 215.625H206.75V37.5A18.75 18.75 0 00188 18.75H38A18.75 18.75 0 0019.25 37.5V262.5A18.75 18.75 0 0038 281.25H141.125V243.75z")]
public class WriteFileNode : VisualNode
{
    [Description("Destination Filename")]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public string Filename { get; set; }
    
    [Description("The content to save")]
    [DataMember(IsRequired = false, EmitDefaultValue = false)]
    public string Content { get; set; }

    [Pin("Flow Input", PinAlignment.Top)]
    public IInputPin InputPin { get; set; }

    [Pin("Flow Output", PinAlignment.Bottom)]
    public IOutputPin OutputPin { get; set; }
    
    public WriteFileNode() : base("Save File")
    {
    }

    public override Task Execute()
    {
        var filename = Evaluate<string>(Filename);
        var content = Evaluate<string>(Content);
       
        File.WriteAllText(filename, content);

        return ContinueWith(OutputPin);
    }
}
