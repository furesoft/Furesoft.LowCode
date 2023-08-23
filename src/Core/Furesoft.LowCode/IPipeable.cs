using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Furesoft.LowCode.Evaluation;

namespace Furesoft.LowCode;
public interface IPipeable<T>
{
    [Browsable(false)]
    public ICollection<T> PipeVariable { get; }
}
