using System;
using System.Collections.Generic;
using System.Text;
using GenetiBase.MathX.NuGenStatistic;
using System.Windows.Forms;

namespace GenetiBase.MathX.NuGenStatistic
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new NuGenStatistic_Form());

        }
    }
}
