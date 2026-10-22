using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace ToolsAreNecessary
{
    // the defmodextension. add this to your tools to make them compatible with this mod. use <can____>true</can____> to make the tool unlock that job.
    public class TANModExtension : DefModExtension
    {
        public bool canMine = false;
        public bool canChop = false;

        // might add more jobs later (like no tending without tools).
        // prolly not tho,
    }
}
