using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.NuGenTransform
{
    public interface NuGenImageListener
    {
        void UpdateImage(System.Drawing.Image img);
        void Clear();
    }

    public interface NuGenImageProvider
    {
        void RegisterListener(NuGenImageListener l);
        void UpdateListenersImage(System.Drawing.Image img);
    }
}
