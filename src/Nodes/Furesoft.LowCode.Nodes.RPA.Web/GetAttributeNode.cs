using System.Runtime.Serialization;
using Furesoft.LowCode.Designer.Core;

namespace Furesoft.LowCode.Nodes.RPA.Web;

public class GetAttributeNode : WebNode, IOutVariableProvider
{
    [DataMember(EmitDefaultValue = false)]
    public string Selector { get; set; } = string.Empty;
    
    [DataMember(EmitDefaultValue = false)]
    public string Attribute { get; set; } = string.Empty;
    
    [DataMember(EmitDefaultValue = false)]
    public string OutVariable { get; set; }

    public GetAttributeNode() : base("Get Attribute")
    {
    }

    protected override async Task Invoke(CancellationToken cancellationToken)
    {
        var page = GetPage();

        var element = await page.QuerySelectorAsync(Selector);
        SetOutVariable(OutVariable, element.GetPropertyAsync(Attribute));
    }
}
