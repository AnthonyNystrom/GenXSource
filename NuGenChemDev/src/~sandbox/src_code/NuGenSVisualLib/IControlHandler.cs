using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using NuGenSVisualLib.Recording;

namespace NuGenSVisualLib.Movement
{
    interface IControlHandler
    {
        bool Enabled
        {
            get;
            set;
        }

        ViewRecording.ViewRecorder Recorder
        {
            get;
            set;
        }
    
        bool OnMouseMove(MouseEventArgs e);
        bool OnMouseDown(MouseEventArgs e);
        bool OnMouseUp(MouseEventArgs e);
        bool OnKeyDown(KeyEventArgs e);
        bool OnKeyUp(KeyEventArgs e);
        bool OnMouseWheel(MouseEventArgs e);
    }
}
