using System.ComponentModel;
using Newtonsoft.Json;

namespace Furesoft.LowCode;

public interface IPipeable
{
    [Browsable(false)] [JsonIgnore] object PipeVariable { get; set; }
}
