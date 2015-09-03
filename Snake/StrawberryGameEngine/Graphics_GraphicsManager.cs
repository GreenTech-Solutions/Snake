using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrawberryGameEngine
{
    namespace Video
    {
        internal abstract class GraphicsManager
        {
            protected bool Loaded { get; set; }

            private int CurrentFrame;

            public abstract void Initialize();

            public abstract void ShutDown();
        }
    }
}
