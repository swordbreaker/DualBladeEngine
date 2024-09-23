using Gum.Wireframe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DualBlade.GumUi;
internal interface IGumCursor : ICursor
{
    void Activity(double seconds);
}
