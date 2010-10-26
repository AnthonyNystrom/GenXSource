using System.Windows.Forms;

namespace NuGenVisUI
{
    interface IViewInputController
    {
        bool Enabled
        {
            get;
            set;
        }

        //ViewRecording.ViewRecorder Recorder
        //{
        //    get;
        //    set;
        //}

        bool OnMouseMove(MouseEventArgs e);
        bool OnMouseDown(MouseEventArgs e);
        bool OnMouseUp(MouseEventArgs e);
        bool OnKeyDown(KeyEventArgs e);
        bool OnKeyUp(KeyEventArgs e);
        bool OnMouseWheel(MouseEventArgs e);

        void LockXAxis(bool _lock);
        void LockYAxis(bool _lock);
        void LockZAxis(bool _lock);
    }
}