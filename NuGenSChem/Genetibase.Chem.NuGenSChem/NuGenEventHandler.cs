using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Printing;

namespace Genetibase.Chem.NuGenSChem
{
    interface NuGenEventHandler
    {
        void Refresh();

        void New();
        void Save();
        void SaveAs();
        void ExportMOL();
        void ExportCML();
        void Open();
        void ZoomFull();
        void ZoomIn();
        void ZoomOut();
        void ShowElements();
        void ShowAllElements();
        void ShowIndices();
        void ShowRingID();
        void ShowCIPPriority();
        void ExportBMP();

        PrintDocument Document();

        Templates GetTemplates();
        ITemplSelectListener GetTemplListener();
    }
}
