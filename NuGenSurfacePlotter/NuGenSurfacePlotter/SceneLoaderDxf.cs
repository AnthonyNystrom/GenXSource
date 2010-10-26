using System;
using System.Collections.Generic;
using System.Text;
using NuGenCRBase.AvalonBridge;
using D2XLib;
using System.Windows.Media.Media3D;

namespace NuGenCRBase.SceneFormats.ThreeDS
{
    class SceneLoaderDxf : IABSceneLoader
    {
        #region IABSceneLoader Members

        public ABScene3D LoadScene(string file)
        {
            return ABScene3D.FromAvalonObj(DWGReader3D.GetModel(file));
        }

        #endregion
    }
}
