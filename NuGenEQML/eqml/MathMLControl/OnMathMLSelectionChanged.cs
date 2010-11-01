namespace Genetibase.MathX
{
    using System;

    public class OnMathMLSelectionChanged : EventArgs
    {
        protected internal OnMathMLSelectionChanged(bool hasSelection)
        {
            this.hasSelection = hasSelection;
        }
        
        private bool hasSelection;
    }
}

