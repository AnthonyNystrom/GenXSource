using Genetibase.VisUI.UI;

namespace Genetibase.VisUI.Scripting
{
    public abstract class ScriptLayer
    {
	    internal ILayerScriptInterface lInterface;
        internal ILayer layer;

        internal void SetData(ILayerScriptInterface lInterface, ILayer layer)
        {
            this.lInterface = lInterface;
            this.layer = layer;
        }
	
	    public virtual void CloseLayer()
        {
            lInterface.CloseLayer(this);
        }
    }

    internal interface ILayerScriptInterface
    {
        void CloseLayer(ScriptLayer layer);
    }
}