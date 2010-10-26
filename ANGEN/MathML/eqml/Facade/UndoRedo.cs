namespace Facade
{
    using System;
    using System.Collections.Generic;

    public class UndoRedoStack
    {
        public UndoRedoStack()
        {
            this.stack = new Stack<NodesInfo>();
        }

        public void Push(NodesInfo UndoRedoBuffer)
        {
            try
            {
                this.stack.Push(UndoRedoBuffer);
                this.callback(this, new EventArgs());
            }
            catch
            {
            }
        }

        public NodesInfo Pop()
        {
            NodesInfo info = null;
            try
            {
                info = stack.Pop();
                this.callback(this, new EventArgs());
            }
            catch
            {
            }
            return info;
        }

        public void Clear()
        {
            try
            {
                this.stack.Clear();
                this.callback(this, new EventArgs());
            }
            catch
            {
            }
        }

        public int Depth
        {
            get
            {
                if (this.stack != null)
                {
                    return this.stack.Count;
                }
                return 0;
            }
        }
        
        public EventHandler callback;
        private Stack<NodesInfo> stack;
    }
}

