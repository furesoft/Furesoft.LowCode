using Newtonsoft.Json;

namespace Furesoft.LowCode.Designer.Services.Serializing;

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
