namespace Furesoft.LowCode.Nodes.RPA.Web.Core;

public class BrowserNotStartedException : Exception
{
    public BrowserNotStartedException() : base("Browser is not startet or connected")
    {
        
    }
}
