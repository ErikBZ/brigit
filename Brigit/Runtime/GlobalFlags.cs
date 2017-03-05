using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brigit.Runtime
{
    // mostly being  used as an abstraction and
    static class GlobalFlags
    {
        static public Dictionary<string, bool> flags { get; set; }
        // might add some helped functions later I don't know
    }
}
