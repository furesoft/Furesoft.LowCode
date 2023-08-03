using System.Reflection;
using System.Text;
using Avalonia.Controls;
using Furesoft.LowCode.Designer.Core;
using Furesoft.LowCode.Editor.Model;
using Furesoft.LowCode.Editor.MVVM;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Furesoft.LowCode.Designer.Services;

internal class NodeSerializer : INodeSerializer
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
            InitNodeView<T>(node);
        }
        
        if (obj is DrawingNodeEditor.Clipboard clp)
        {
            foreach (var n in clp.SelectedNodes)
            {
                InitNodeView<T>((CustomNodeViewModel)n);
            }
        }

        if (obj is DrawingNodeViewModel dvm)
        {
            foreach (var n in dvm.Nodes)
            {
                InitNodeView<T>((CustomNodeViewModel)n);
            }
        }

        return obj;
    }

    private static void InitNodeView<T>(CustomNodeViewModel node)
    {
        double w = 60;
        double h = 60;

        var view = node.DefiningNode.GetNodeView(ref w, ref h);

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

    private class ListContractResolver : DefaultContractResolver
    {
        private readonly Type _listType;

        public ListContractResolver(Type listType)
        {
            _listType = listType;
        }

        public override JsonContract ResolveContract(Type type)
        {
            if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(IList<>))
            {
                return base.ResolveContract(_listType.MakeGenericType(type.GenericTypeArguments[0]));
            }

            return base.ResolveContract(type);
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            return base.CreateProperties(type, memberSerialization).Where(p => p.Writable).ToList();
        }
    }
}

internal class ControlConverter : JsonConverter<UserControl>
{
    public override void WriteJson(JsonWriter writer, UserControl value, JsonSerializer serializer)
    {
        writer.WriteNull();
    }

    public override UserControl ReadJson(JsonReader reader, Type objectType, UserControl existingValue,
        bool hasExistingValue,
        JsonSerializer serializer)
    {
        return (UserControl)Activator.CreateInstance(objectType);
    }
}
