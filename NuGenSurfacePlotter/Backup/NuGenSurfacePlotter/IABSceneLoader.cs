using System;
using System.Collections.Generic;
using System.Text;
using NuGenCRBase.AvalonBridge;

namespace NuGenCRBase.SceneFormats
{
    interface IABSceneLoader
    {
        ABScene3D LoadScene(string file);
    }
}