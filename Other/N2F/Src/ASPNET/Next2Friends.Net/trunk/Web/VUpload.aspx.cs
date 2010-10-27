using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Telerik.WebControls;

public partial class VUpload : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            //Do not display SelectedFilesCount progress indicator.
            RadProgressArea1.ProgressIndicators &= ~ProgressIndicators.SelectedFilesCount;

            RadProgressArea1.Localization["UploadedFiles"] = "Processed ";
            RadProgressArea1.Localization["TotalFiles"] = "";
            RadProgressArea1.Localization["CurrentFileName"] = "File: ";

            RadProgressContext progress = RadProgressContext.Current;
            //Prevent the secondary progress from appearing when the file is uploaded (FileCount etc.)
            progress["SecondaryTotal"] = "0";
            progress["SecondaryValue"] = "0";
            progress["SecondaryPercent"] = "0";
        }
    }

    protected void buttonSubmit_Click(object sender, System.EventArgs e)
    {
        UploadedFile file = RadUploadContext.Current.UploadedFiles[inputFile.UniqueID];

        if (file != null)
        {
            LooongMethodWhichUpdatesTheProgressContext(file);
        }

        

        int i = 10;
    }

    private void LooongMethodWhichUpdatesTheProgressContext(UploadedFile file)
    {
        const int total = 100;

        RadProgressContext progress = RadProgressContext.Current;

        for (int i = 0; i < total; i++)
        {
            progress["SecondaryTotal"] = total.ToString();
            progress["SecondaryValue"] = i.ToString();
            progress["SecondaryPercent"] = i.ToString();
            progress["CurrentOperationText"] = file.GetName() + " is being processed...";

            if (!Response.IsClientConnected)
            {
                //Cancel button was clicked or the browser was closed, so stop processing
                break;
            }

            //Stall the current thread for 0.1 seconds
            System.Threading.Thread.Sleep(100);
        }
    }
}
