namespace UI
{
    using System;

    public class SelectionArgs : EventArgs
    {
        protected internal SelectionArgs(bool hasSelection)
        {
            hasSel_ = hasSelection;
        }

        public bool HasSelection
        {
            get
            {
                return hasSel_;
            }
        }
        
        private bool hasSel_;
    }
}

