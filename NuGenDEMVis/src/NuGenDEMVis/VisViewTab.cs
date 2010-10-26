using Microsoft.DirectX.Direct3D;

namespace Genetibase.NuGenDEMVis.Controls
{
    partial class VisViewTab : VisUI.Controls.ViewTab
    {
        public VisViewTab(UI3DVisControl control)
            : base(control)
        {
            InitializeComponent();
        }

        public UI3DVisControl VisViewControl
        {
            get { return (UI3DVisControl)viewControl; }
        }

        public void ChangeRenderingFillMode(FillMode fillMode)
        {
            ((UI3DVisControl)viewControl).SetDEMFillMode(fillMode);
        }

        public void ChangeDiffuseSource(int index)
        {
            ((UI3DVisControl)viewControl).SetDiffuseSource(index);
        }

        public void ToggleAxis(UI3DVisControl.Axis axis, bool on)
        {
            ((UI3DVisControl)viewControl).ToggleAxis(axis, on);
        }
    }
}