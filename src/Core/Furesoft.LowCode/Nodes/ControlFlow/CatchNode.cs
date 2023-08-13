using System.ComponentModel;
using System.Runtime.Serialization;
using Furesoft.LowCode.Attributes;
using Furesoft.LowCode.NodeViews;
using NiL.JS.Core;

namespace Furesoft.LowCode.Nodes.ControlFlow;

[NodeCategory("Control Flow")]
[Description("Catches an error")]
[NodeView(typeof(IconNodeView),
    "M50 4.75C50 1.9886 47.7614-.25 45-.25L30-.25C27.2386-.25 25 1.9886 25 4.75L25 9.75 5 9.75C2.2385 9.75 0 11.9886 0 14.75L0 29.75C0 34.194 1.9345 38.1867 5 40.9304L5 57.25C5 61.392 8.358 64.75 12.5 64.75L62.5 64.75C66.6422 64.75 70 61.392 70 57.25L70 40.9295C73.0652 38.1861 75 34.1942 75 29.75L75 14.75C75 11.9886 72.7614 9.75 70 9.75L50 9.75 50 4.75zM30 4.75 30 9.75 45 9.75 45 4.75 30 4.75zM70 14.75 50 14.75 47.5 14.75 27.5 14.75 25 14.75 5 14.75 5 29.75C5 33.02 6.567 35.9233 9.0015 37.752 10.6725 39.0072 12.746 39.75 15 39.75L35 39.75 35 37.25C35 35.8693 36.1193 34.75 37.5 34.75 38.8807 34.75 40 35.8693 40 37.25L40 39.75 60 39.75C62.2535 39.75 64.327 39.0067 65.9985 37.7511 68.4333 35.9223 70 33.0198 70 29.75L70 14.75zM40 44.75 60 44.75C61.752 44.75 63.4354 44.4486 65 43.8953L65 57.25C65 58.6305 63.8807 59.75 62.5 59.75L12.5 59.75C11.1195 59.75 10 58.6305 10 57.25L10 43.8957C11.5645 44.4489 13.248 44.75 15 44.75L35 44.75 35 47.25C35 48.6307 36.1193 49.75 37.5 49.75 38.8807 49.75 40 48.6307 40 47.25L40 44.75z")]
public class CatchNode : InputNode
{
    private string _errorName = "error";

    public CatchNode() : base("Catch Error")
    {
    }

    [Pin("On Error", PinAlignment.Right)] public IOutputPin OnErrorPin { get; set; }

    [Pin("Do", PinAlignment.Bottom)] public IOutputPin DoPin { get; set; }

    [Description("Give the error a name. If its not set the name will be 'error'")]
    [DataMember(EmitDefaultValue = false)]
    public string ErrorName
    {
        get => _errorName;
        set => SetProperty(ref _errorName, value);
    }

    public override async Task Execute(CancellationToken cancellationToken)
    {
        try
        {
            await ContinueWith(DoPin, cancellationToken);
        }
        catch (Exception ex)
        {
            var subContext = new Context(Context);
            DefineConstant(ErrorName, ex, subContext);

            await ContinueWith(OnErrorPin, cancellationToken, subContext);
        }
    }
}
