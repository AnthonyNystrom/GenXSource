using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.NuGenMediImage.UI.Controls
{
    public class ValueChangedEventArgs : EventArgs
    {
        private double oldValue = 0.0;

        public double OldValue
        {
            get { return oldValue; }
            set { oldValue = value; }
        }
        private double newValue = 0.0;

        public double NewValue
        {
            get { return newValue; }
            set { newValue = value; }
        }

        public ValueChangedEventArgs(double oldValue, double newValue)
        {
            this.oldValue = oldValue;
            this.newValue = newValue;
        }
    }
}
