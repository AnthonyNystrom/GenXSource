using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

/// <summary>
/// Summary description for ProfileScheme
/// </summary>
public class ProfileScheme
{ 
    public string BackgroundColor { get; set; }
    public string BorderColor { get; set; }

    //private static string[] BackgroundColorArr = new string[]{
    //                                                        "#ecf0f3",
    //                                                        "#f7f2fd","#f1fedd","#fefce3","#ecf3fa","#eeeffb",
    //                                                        "#fcf2f6","#fbf6f1","#f3f1f1","#fdecf4","#efe8ef",
    //                                                        "#D6E7F7","#F7E7D6","#F7F7D6","#E7F7D6","#999999"};


    //private static string[] BorderColorArr = new string[]{
    //                                                        "#cccccc",
    //                                                        "#e5c9e6","#d3e2bd","#f6dabb","#bbd2db","#c4c2d5",
    //                                                        "#e8aec7","#fbf6f1","#d7d7d7","#eac3da","#c5bcc5",
    //                                                        "#99C2EA","#EBC299","#EBEB99","#C2EA99","#333333"};

    private static string[] BackgroundColorArr = new string[]{
                                                            "#ecf0f3",
                                                            "#f7f2fd","#f1fedd","#fefce3","#ecf3fa","#eeeffb",
                                                            "#fcf2f6","#fbf6f1","#f3f1f1","#fdecf4","#FFFFFF",
                                                            "#D6E7F7","#F7E7D6","#EBEB99","#C2EA99","#999999"};


    private static string[] BorderColorArr = new string[]{
                                                            "#cccccc",
                                                            "#e5c9e6","#d3e2bd","#f6dabb","#bbd2db","#c4c2d5",
                                                            "#e8aec7","#fbf6f1","#d7d7d7","#eac3da","#CCCCCC",
                                                            "#99C2EA","#EBC299","#F7E7D6","#F7E7D6","#333333"};

    public static ProfileScheme GetScheme(int Index)
    {
        ProfileScheme scheme = new ProfileScheme();

        if (Index>-1 && Index < BackgroundColorArr.Length)
        {
            scheme.BackgroundColor = BackgroundColorArr[Index];
            scheme.BorderColor = BorderColorArr[Index];
        }

        return scheme;
    }

    /// <summary>
    /// Is the index within the allowed values
    /// </summary>
    /// <param name="Index"></param>
    /// <returns></returns>
    public static bool IsValidSchemeValue(int Index)
    {
        bool IsValue = false;

        if (Index > -1 && Index < BackgroundColorArr.Length)
        {
            IsValue = true;
        }

        return IsValue;
    }

    public ProfileScheme()
    {

    }
}
