﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brigit.Attributes
{
    // mostly being  used as an abstraction and
    static class GlobalFlags
    {
        static public Dictionary<string, Flag> Globals { get; set; }
        // might add some helped functions later I don't know
    }
}
