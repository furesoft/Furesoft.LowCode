using System.Reflection;
using System.Text;
using Furesoft.LowCode.ProjectSystem.Items;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Furesoft.LowCode.Designer.Services.Serializing;

public class NodeSerializer : INodeSerializer
{
    private readonly JsonSerializerSettings _settings;

    public NodeSerializer(Type listType)
    {
        _settings = new()
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.Objects,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
            ContractResolver = new ListContractResolver(listType),
            NullValueHandling = NullValueHandling.Ignore
        };
        _settings.Converters.Add(new ControlConverter());
    }

    public string Serialize<T>(T value)
    {
        return JsonConvert.SerializeObject(value, _settings);
    }

    public T Deserialize<T>(string text)
    {
        var obj = JsonConvert.DeserializeObject<T>(text, _settings);

        if (obj is CustomNodeViewModel node)
        {
            InitNodeView(node);
        }

        if (obj is DrawingNodeEditor.Clipboard clp)
        {
            foreach (var n in clp.SelectedNodes)
            {
                InitNodeView((CustomNodeViewModel)n);
            }
        }

        if (obj is GraphItem {Drawing: DrawingNodeViewModel dvm})
        {
            foreach (var n in dvm.Nodes)
            {
                InitNodeView((CustomNodeViewModel)n);
            }
        }

        return obj;
    }

    private static void InitNodeView(CustomNodeViewModel node)
    {
        double w = 60;
        double h = 60;

        var view = node.DefiningNode.GetView(ref w, ref h);

        if (view is null)
        {
            return;
        }

        view.DataContext = node.DefiningNode;

        node.Content = view;
    }

    public T Load<T>(string path)
    {
        using var stream = File.OpenRead(path);
        using var streamReader = new StreamReader(stream, Encoding.UTF8);
        var text = streamReader.ReadToEnd();
        return Deserialize<T>(text);
    }

    public void Save<T>(string path, T value)
    {
        var text = Serialize(value);
        if (string.IsNullOrWhiteSpace(text))
        {
            return;
        }

        using var stream = File.Create(path);
        using var streamWriter = new StreamWriter(stream, Encoding.UTF8);
        streamWriter.Write(text);
    }

    private class ListContractResolver(Type listType) : DefaultContractResolver
    {
        public override JsonContract ResolveContract(Type type)
        {
            if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(IList<>))
            {
                return base.ResolveContract(listType.MakeGenericType(type.GenericTypeArguments[0]));
            }

            return base.ResolveContract(type);
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            return base.CreateProperties(type, memberSerialization).Where(p => p.Writable).ToList();
        }
    }
}
