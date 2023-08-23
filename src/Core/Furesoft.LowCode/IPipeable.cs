using System.Collections;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Furesoft.LowCode;

public interface IPipeable
{
    [Browsable(false), JsonIgnore]
    IEnumerable PipeVariable { get; set; }
}
