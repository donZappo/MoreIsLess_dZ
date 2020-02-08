using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoreIsLess_dZ
{
    public class ModSettings
    {
        public bool Debug = false;
        public string modDirectory;

        public int intPerXP = 8000;
        public float floatXPMulti = 0.9f;
        public bool XPCap = true;
        public int XPMax = 60000;

        public bool AlwaysOn = true;
    }
}
