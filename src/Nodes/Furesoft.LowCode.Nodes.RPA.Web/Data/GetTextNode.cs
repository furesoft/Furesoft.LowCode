﻿using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Furesoft.LowCode.Nodes.RPA.Web.Core;

namespace Furesoft.LowCode.Nodes.RPA.Web.Data;

public class GetTextNode : WebNode, IOutVariableProvider
{
    public GetTextNode() : base("Get Text")
    {
    }

    [DataMember(EmitDefaultValue = false)]
    [Required]
    public string Selector { get; set; } = string.Empty;

    [DataMember(EmitDefaultValue = false)]
    [Required]
    public string OutVariable { get; set; }

    protected override async Task Invoke(CancellationToken cancellationToken)
    {
        var page = GetPage();

        var element = await page.QuerySelectorAsync(Selector);
        SetOutVariable(OutVariable, element.GetPropertyAsync("innerText"));
    }
}
