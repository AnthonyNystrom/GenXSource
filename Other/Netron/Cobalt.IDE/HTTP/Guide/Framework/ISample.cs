using System;
using System.Collections.Generic;
using System.Text;
using Netron.Diagramming.Win;
namespace Netron.Cobalt
{
    public interface ISample
    {

        DiagramControl DiagramControl { get;set;}

        /// <summary>
        /// This initializes the context and/or canvas before the actual sample is ran.
        /// </summary>
        void Init();
        /// <summary>
        /// Runs the sample code.
        /// </summary>
        void Run();
    }
}
