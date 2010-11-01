using System;
using System.Collections.Generic;
using System.Text;
using Genetibase.UI.NuGenMeters;
using DevComponents.DotNetBar;

namespace NuGenRealTime
{
    static class ButtonMeterMap
    {
        static List<ButtonItem> buttons = new List<ButtonItem>();        
        static List<String> graphs = new List<String>();

        public static String GetGraph(ButtonItem button)
        {
            int i = 0;

            foreach (ButtonItem storedButton in buttons)
            {
                if (button == storedButton)
                {
                    return graphs[i];
                }

                i++;
            }

            return null;
        }

        public static void AssociateButton(ButtonItem button, String graph)
        {
            buttons.Add(button);
            graphs.Add(graph);
        }

    }
}
