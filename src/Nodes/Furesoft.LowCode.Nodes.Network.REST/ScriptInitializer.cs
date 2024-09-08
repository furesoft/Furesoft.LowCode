using Furesoft.LowCode.Evaluation;
using NiL.JS.BaseLibrary;
using NiL.JS.Core;
using NiL.JS.Core.Interop;

namespace Furesoft.LowCode.Nodes.Network.REST;

public class ScriptInitializer : IScriptModuleInitalizer
{
    public void InitEngine(Context context)
    {
        context.DefineConstructor(typeof(HttpMethod));

        context.ImportAsSubObject<ScriptInitializer>("Network", "Rest");
    }

    [JavaScriptName("sendRequest")]
    public static Result SendRequest(HttpMethod method, string url, IEnumerable<string> headers = null,
        object content = null)
    {
        var client = new HttpClient();

        if (headers != null)
        {
            ApplyHeaders(client, headers);
        }

        client.BaseAddress = new(url);

        var request = new HttpRequestMessage();
        request.Method = method;

        if (content != null)
        {
            request.Content = new StringContent(JSON.stringify(Context.CurrentGlobalContext.WrapValue(content)));
        }

        var response = client.Send(request);

        if (response.IsSuccessStatusCode)
        {
            var obj = JSON.parse(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());

            return Result.Ok(obj);
        }

        return Result.Failure(new Exception($"{response.StatusCode}: {response.ReasonPhrase}"));
    }

    private static void ApplyHeaders(HttpClient client, IEnumerable<string> headers)
    {
        foreach (var header in headers)
        {
            var spl = header
                .Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            client.DefaultRequestHeaders.Add(spl[0], spl[1]);
        }
    }
}
