using System.Collections;
using System.Collections.Generic;

namespace Genetibase.VisUI.UI
{
    interface ILayerStack : IEnumerable<ILayer>
    {
        uint LayerCount { get; }
        IEnumerator<ILayer> Layers { get; }
        void InsertLayer(ILayer layer, uint position);
        void RemoveLayer(uint position);
        void RemoveLayer(ILayer layer);
    }

    public class LayerStack : ILayerStack
    {
        readonly List<ILayer> layers;

        public LayerStack()
        {
            layers = new List<ILayer>();
        }

        #region ILayerStack Members

        public uint LayerCount
        {
            get { return (uint)layers.Count; }
        }

        public IEnumerator<ILayer> Layers
        {
            get { return layers.GetEnumerator(); }
        }

        public void InsertLayer(ILayer layer, uint level)
        {
            if (level == uint.MaxValue)
                layers.Insert(layers.Count, layer);
            else if (level == uint.MinValue)
                layers.Insert(0, layer);
            else
                layers.Insert((int)level, layer);
        }

        public void RemoveLayer(uint position)
        {
            layers.RemoveAt((int)position);
        }

        public void RemoveLayer(ILayer layer)
        {
            layers.Remove(layer);
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            layers.Clear();
        }
        #endregion

        #region IEnumerable<ILayer> Members

        public IEnumerator<ILayer> GetEnumerator()
        {
            return layers.GetEnumerator();
        }
        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return layers.GetEnumerator();
        }
        #endregion
    }
}