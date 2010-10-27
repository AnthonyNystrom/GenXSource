using System;
using System.Text;
using System.Collections.Generic;
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

public enum RunTimeOS { J2ME = 0, Symbian = 1, WindowsMobile = 2, BlackJackII = 3, MotoQ = 4,BlackBerryCurve = 5, BlackBerryPearl = 6, BlackBerryCurveNoWifi = 7, BlackBerryPearlNoWifi = 8 , BlackBerryBold = 9};
/// <summary>
/// Summary description for MobilePhones
/// </summary>
public class MobilePhone
{
    public int ID { get; set; }
    public string Manufacturer { get; set; }
    public string Model { get; set; }
    public RunTimeOS Runtime { get; set; }
    public static List<MobilePhone> MobilePhoneList;

    public MobilePhone()
    {
        PopulateMobilePhoneList();
    }

    public static MobilePhone GetMobilePhoneByID(int ID)
    {
        for (int i = 0; i < MobilePhoneList.Count; i++)
        {
            if (MobilePhoneList[i].ID == ID)
            {
                return MobilePhoneList[i];
            }
        }

        return null;
    }

    public static MobilePhone GetMobilePhoneFromUserAgent(string HTTP_USER_AGENT)
    {
        HTTP_USER_AGENT = HTTP_USER_AGENT.ToLower();
        MobilePhone MPhone = new MobilePhone();

        // loop through looking for the device Manufacturer
        for (int i = 0; i < MobilePhoneList.Count; i++)
        {
            if (HTTP_USER_AGENT.Contains(MobilePhoneList[i].Manufacturer.ToLower()))
            {
                MPhone.Manufacturer = MobilePhoneList[i].Manufacturer;
                break;
            }
        }

        // loop through looking for the device Model
        for (int i = 0; i < MobilePhoneList.Count; i++)
        {
            if (HTTP_USER_AGENT.Contains(MobilePhoneList[i].Model.ToLower()))
            {
                MPhone.Model = MobilePhoneList[i].Model;
                break;
            }
        }

        return MPhone;
    }

    public List<MobilePhone> FilterManufacturer(string Manufacturer)
    {
        List<MobilePhone> FilteredPhones = new List<MobilePhone>();

        for (int i = 0; i < MobilePhoneList.Count; i++)
        {
            if (MobilePhoneList[i].Manufacturer == Manufacturer)
            {
                FilteredPhones.Add(MobilePhoneList[i]);
            }
        }

        return FilteredPhones;
    }

    public string ToJavaScriptArray()
    {
        StringBuilder sbJSArray = new StringBuilder();

        sbJSArray.Append("var Devices = new Array(");

        for (int i = 0; i < MobilePhoneList.Count; i++)
		{
            string[] Parameters = new string[5];
            Parameters[0] = MobilePhoneList[i].Manufacturer;
            Parameters[1] = MobilePhoneList[i].Model;
            Parameters[2] = ((int)MobilePhoneList[i].Runtime).ToString();
            Parameters[3] = (i < MobilePhoneList.Count - 1) ? "," : string.Empty;
            Parameters[4] = MobilePhoneList[i].ID.ToString();

            sbJSArray.AppendFormat("new Array('{0}','{1}',{2},{4}){3}", Parameters);
		}

        sbJSArray.Append(");");

        return sbJSArray.ToString();
    }

    public MobilePhone(string Manufacturer, string Model, RunTimeOS Runtime, int ID)
    {
        this.Manufacturer = Manufacturer;
        this.Model = Model;
        this.Runtime = Runtime;
        this.ID = ID;
    }

    private void PopulateMobilePhoneList()
    {
        MobilePhoneList = new List<MobilePhone>();
        MobilePhoneList.Add(new MobilePhone("Ben","Q C81",RunTimeOS.J2ME,1));
        MobilePhoneList.Add(new MobilePhone("Ben","Q CX75",RunTimeOS.J2ME,2));
        MobilePhoneList.Add(new MobilePhone("Ben","Q EF81",RunTimeOS.J2ME,3));
        MobilePhoneList.Add(new MobilePhone("Ben","Q EF91",RunTimeOS.J2ME,4));
        MobilePhoneList.Add(new MobilePhone("Ben","Q M75",RunTimeOS.J2ME,5));
        MobilePhoneList.Add(new MobilePhone("Ben","Q S65",RunTimeOS.J2ME,6));
        MobilePhoneList.Add(new MobilePhone("Ben","Q S68",RunTimeOS.J2ME,7));
        MobilePhoneList.Add(new MobilePhone("Ben","Q S75",RunTimeOS.J2ME,8));
        MobilePhoneList.Add(new MobilePhone("Ben","Q S81",RunTimeOS.J2ME,9));
        MobilePhoneList.Add(new MobilePhone("Ben","Q SK65",RunTimeOS.J2ME,10));
        MobilePhoneList.Add(new MobilePhone("Ben","Q SL75",RunTimeOS.J2ME,11));
        MobilePhoneList.Add(new MobilePhone("Ben","Q SXG75",RunTimeOS.J2ME,12));
        MobilePhoneList.Add(new MobilePhone("Motorola","i580",RunTimeOS.J2ME,13));
        MobilePhoneList.Add(new MobilePhone("Motorola","i605",RunTimeOS.J2ME,14));
        MobilePhoneList.Add(new MobilePhone("Motorola","i870",RunTimeOS.J2ME,15));
        MobilePhoneList.Add(new MobilePhone("Motorola","i875",RunTimeOS.J2ME,16));
        MobilePhoneList.Add(new MobilePhone("Motorola","i880 / i885",RunTimeOS.J2ME,17));
        MobilePhoneList.Add(new MobilePhone("Motorola","ic902",RunTimeOS.J2ME,152));
        MobilePhoneList.Add(new MobilePhone("Motorola","MOTOSLVR L9 / L72",RunTimeOS.J2ME,18));
        MobilePhoneList.Add(new MobilePhone("Motorola","RAZR V3x",RunTimeOS.J2ME,19));
        MobilePhoneList.Add(new MobilePhone("Motorola","V176",RunTimeOS.J2ME,20));
        MobilePhoneList.Add(new MobilePhone("Nokia","3109 Classic",RunTimeOS.J2ME,21));
        MobilePhoneList.Add(new MobilePhone("Nokia","3250",RunTimeOS.J2ME,22));
        MobilePhoneList.Add(new MobilePhone("Nokia","5200",RunTimeOS.J2ME,23));
        MobilePhoneList.Add(new MobilePhone("Nokia","5300",RunTimeOS.J2ME,24));
        MobilePhoneList.Add(new MobilePhone("Nokia","6021",RunTimeOS.J2ME,25));
        MobilePhoneList.Add(new MobilePhone("Nokia","6085",RunTimeOS.J2ME,26));
        MobilePhoneList.Add(new MobilePhone("Nokia","6086",RunTimeOS.J2ME,27));
        MobilePhoneList.Add(new MobilePhone("Nokia","6125",RunTimeOS.J2ME,28));
        MobilePhoneList.Add(new MobilePhone("Nokia","6126",RunTimeOS.J2ME,28));
        MobilePhoneList.Add(new MobilePhone("Nokia","6131",RunTimeOS.J2ME,30));
        MobilePhoneList.Add(new MobilePhone("Nokia","6131 NFC",RunTimeOS.J2ME,31));
        MobilePhoneList.Add(new MobilePhone("Nokia","6136",RunTimeOS.J2ME,32));
        MobilePhoneList.Add(new MobilePhone("Nokia","6151",RunTimeOS.J2ME,33));
        MobilePhoneList.Add(new MobilePhone("Nokia","6230",RunTimeOS.J2ME,34));
        MobilePhoneList.Add(new MobilePhone("Nokia","6230i",RunTimeOS.J2ME,35));
        MobilePhoneList.Add(new MobilePhone("Nokia","6233",RunTimeOS.J2ME,36));
        MobilePhoneList.Add(new MobilePhone("Nokia","6234",RunTimeOS.J2ME,37));
        MobilePhoneList.Add(new MobilePhone("Nokia","6255",RunTimeOS.J2ME,38));
        MobilePhoneList.Add(new MobilePhone("Nokia","6265",RunTimeOS.J2ME,39));
        MobilePhoneList.Add(new MobilePhone("Nokia","6270",RunTimeOS.J2ME,40));
        MobilePhoneList.Add(new MobilePhone("Nokia","6275",RunTimeOS.J2ME,41));
        MobilePhoneList.Add(new MobilePhone("Nokia","6280",RunTimeOS.J2ME,42));
        MobilePhoneList.Add(new MobilePhone("Nokia","6282",RunTimeOS.J2ME,42));
        MobilePhoneList.Add(new MobilePhone("Nokia","6300",RunTimeOS.J2ME,43));
        MobilePhoneList.Add(new MobilePhone("Nokia","6630",RunTimeOS.J2ME,44));
        MobilePhoneList.Add(new MobilePhone("Nokia","6680",RunTimeOS.J2ME,45));
        MobilePhoneList.Add(new MobilePhone("Nokia","6681",RunTimeOS.J2ME,46));
        MobilePhoneList.Add(new MobilePhone("Nokia","6682",RunTimeOS.J2ME,47));
        MobilePhoneList.Add(new MobilePhone("Nokia","7370",RunTimeOS.J2ME,48));
        MobilePhoneList.Add(new MobilePhone("Nokia","7373",RunTimeOS.J2ME,49));
        MobilePhoneList.Add(new MobilePhone("Nokia","7390",RunTimeOS.J2ME,50));
        MobilePhoneList.Add(new MobilePhone("Nokia","7610",RunTimeOS.J2ME,51));
        MobilePhoneList.Add(new MobilePhone("Nokia","8800",RunTimeOS.J2ME,52));
        MobilePhoneList.Add(new MobilePhone("Nokia","8800 Sirocco Edition",RunTimeOS.J2ME,53));
        MobilePhoneList.Add(new MobilePhone("Nokia","8801",RunTimeOS.J2ME,54));
        MobilePhoneList.Add(new MobilePhone("Nokia","9300i",RunTimeOS.J2ME,55));
        MobilePhoneList.Add(new MobilePhone("Nokia", "9500", RunTimeOS.J2ME, 56));
        MobilePhoneList.Add(new MobilePhone("SavaJe","GSPDA Jasper S20",RunTimeOS.J2ME,57));
        MobilePhoneList.Add(new MobilePhone("Siemens","CF75",RunTimeOS.J2ME,58));
        MobilePhoneList.Add(new MobilePhone("Siemens","CX75",RunTimeOS.J2ME,59));
        MobilePhoneList.Add(new MobilePhone("Siemens","S65",RunTimeOS.J2ME,60));
        MobilePhoneList.Add(new MobilePhone("Siemens","S66",RunTimeOS.J2ME,61));
        MobilePhoneList.Add(new MobilePhone("Siemens","S6C",RunTimeOS.J2ME,62));
        MobilePhoneList.Add(new MobilePhone("Siemens","S6V",RunTimeOS.J2ME,63));
        MobilePhoneList.Add(new MobilePhone("Siemens","Sk65",RunTimeOS.J2ME,64));
        MobilePhoneList.Add(new MobilePhone("Siemens","SP65",RunTimeOS.J2ME,65));
        MobilePhoneList.Add(new MobilePhone("Sony Ericcson","K510",RunTimeOS.J2ME,66));
        MobilePhoneList.Add(new MobilePhone("Sony Ericcson","K550",RunTimeOS.J2ME,67));
        MobilePhoneList.Add(new MobilePhone("Sony Ericcson","K600",RunTimeOS.J2ME,68));
        MobilePhoneList.Add(new MobilePhone("Sony Ericcson","K608",RunTimeOS.J2ME,69));
        MobilePhoneList.Add(new MobilePhone("Sony Ericcson","K610",RunTimeOS.J2ME,70));
        MobilePhoneList.Add(new MobilePhone("Sony Ericcson","K618",RunTimeOS.J2ME,71));
        MobilePhoneList.Add(new MobilePhone("Sony Ericcson","K630i",RunTimeOS.J2ME,72));
        MobilePhoneList.Add(new MobilePhone("Sony Ericcson","K660i",RunTimeOS.J2ME,73));
        MobilePhoneList.Add(new MobilePhone("Sony Ericcson","K750",RunTimeOS.J2ME,74));
        MobilePhoneList.Add(new MobilePhone("Sony Ericcson","K790",RunTimeOS.J2ME,75));
        MobilePhoneList.Add(new MobilePhone("Sony Ericcson","K800",RunTimeOS.J2ME,76));
        MobilePhoneList.Add(new MobilePhone("Sony Ericcson","K850i / K858c",RunTimeOS.J2ME,77));
        MobilePhoneList.Add(new MobilePhone("Sony Ericcson","M600",RunTimeOS.J2ME,78));
        MobilePhoneList.Add(new MobilePhone("Sony Ericcson","P1i / P1c",RunTimeOS.J2ME,79));
        MobilePhoneList.Add(new MobilePhone("Sony Ericcson","P990",RunTimeOS.J2ME,80));
        MobilePhoneList.Add(new MobilePhone("Sony Ericcson","V600",RunTimeOS.J2ME,81));
        MobilePhoneList.Add(new MobilePhone("Sony Ericcson","V640i",RunTimeOS.J2ME,82));
        MobilePhoneList.Add(new MobilePhone("Sony Ericcson","W300",RunTimeOS.J2ME,83));
        MobilePhoneList.Add(new MobilePhone("Sony Ericcson","W600",RunTimeOS.J2ME,84));
        MobilePhoneList.Add(new MobilePhone("Sony Ericcson","W700",RunTimeOS.J2ME,85));
        MobilePhoneList.Add(new MobilePhone("Sony Ericcson","W710",RunTimeOS.J2ME,86));
        MobilePhoneList.Add(new MobilePhone("Sony Ericcson","W800",RunTimeOS.J2ME,87));
        MobilePhoneList.Add(new MobilePhone("Sony Ericcson","W850",RunTimeOS.J2ME,88));
        MobilePhoneList.Add(new MobilePhone("Sony Ericcson","W880i / W888c",RunTimeOS.J2ME,98));
        MobilePhoneList.Add(new MobilePhone("Sony Ericcson","W890i / W890c",RunTimeOS.J2ME,90));
        MobilePhoneList.Add(new MobilePhone("Sony Ericcson","W900",RunTimeOS.J2ME,91));
        MobilePhoneList.Add(new MobilePhone("Sony Ericcson","W910i / W908c",RunTimeOS.J2ME,92));
        MobilePhoneList.Add(new MobilePhone("Sony Ericcson","W950",RunTimeOS.J2ME,93));
        MobilePhoneList.Add(new MobilePhone("Sony Ericcson","Z500",RunTimeOS.J2ME,94));
        MobilePhoneList.Add(new MobilePhone("Sony Ericcson","Z520",RunTimeOS.J2ME,95));
        MobilePhoneList.Add(new MobilePhone("Sony Ericcson","Z525",RunTimeOS.J2ME,96));
        MobilePhoneList.Add(new MobilePhone("Sony Ericcson","Z530",RunTimeOS.J2ME,97));
        MobilePhoneList.Add(new MobilePhone("Sony Ericcson","Z550",RunTimeOS.J2ME,98));
        MobilePhoneList.Add(new MobilePhone("Sony Ericcson","Z610",RunTimeOS.J2ME,99));
        MobilePhoneList.Add(new MobilePhone("Sony Ericcson","Z750",RunTimeOS.J2ME,100));
        MobilePhoneList.Add(new MobilePhone("Sony Ericcson","D750i",RunTimeOS.J2ME,101));
        MobilePhoneList.Add(new MobilePhone("LG","KS10",RunTimeOS.Symbian,102));
        MobilePhoneList.Add(new MobilePhone("LG","KT610",RunTimeOS.Symbian,103));
        MobilePhoneList.Add(new MobilePhone("Nokia","3250",RunTimeOS.Symbian,104));
        MobilePhoneList.Add(new MobilePhone("Nokia","5500 Sport",RunTimeOS.Symbian,105));
        MobilePhoneList.Add(new MobilePhone("Nokia","5700",RunTimeOS.Symbian,106));
        MobilePhoneList.Add(new MobilePhone("Nokia","6110 Navigator",RunTimeOS.Symbian,107));
        MobilePhoneList.Add(new MobilePhone("Nokia","6121",RunTimeOS.Symbian,108));
        MobilePhoneList.Add(new MobilePhone("Nokia","6210 Navigator",RunTimeOS.Symbian,109));
        MobilePhoneList.Add(new MobilePhone("Nokia","6220",RunTimeOS.Symbian,110));
        MobilePhoneList.Add(new MobilePhone("Nokia","6290",RunTimeOS.Symbian,120));
        MobilePhoneList.Add(new MobilePhone("Nokia","E50",RunTimeOS.Symbian,121));
        MobilePhoneList.Add(new MobilePhone("Nokia","E51",RunTimeOS.Symbian,122));
        MobilePhoneList.Add(new MobilePhone("Nokia","E60",RunTimeOS.Symbian,123));
        MobilePhoneList.Add(new MobilePhone("Nokia","E61",RunTimeOS.Symbian,124));
        MobilePhoneList.Add(new MobilePhone("Nokia","E61i",RunTimeOS.Symbian,125));
        MobilePhoneList.Add(new MobilePhone("Nokia","E62",RunTimeOS.Symbian,126));
        MobilePhoneList.Add(new MobilePhone("Nokia","E65",RunTimeOS.Symbian,127));
        MobilePhoneList.Add(new MobilePhone("Nokia","E70", RunTimeOS.Symbian, 128));
        MobilePhoneList.Add(new MobilePhone("Nokia","E71", RunTimeOS.Symbian, 152));
        MobilePhoneList.Add(new MobilePhone("Nokia","E90",RunTimeOS.Symbian,129));
        MobilePhoneList.Add(new MobilePhone("Nokia","N71",RunTimeOS.Symbian,130));
        MobilePhoneList.Add(new MobilePhone("Nokia","N73",RunTimeOS.Symbian,131));
        MobilePhoneList.Add(new MobilePhone("Nokia","N75",RunTimeOS.Symbian,132));
        MobilePhoneList.Add(new MobilePhone("Nokia","N76",RunTimeOS.Symbian,133));
        MobilePhoneList.Add(new MobilePhone("Nokia","N77",RunTimeOS.Symbian,134));
        MobilePhoneList.Add(new MobilePhone("Nokia","N78",RunTimeOS.Symbian,135));
        MobilePhoneList.Add(new MobilePhone("Nokia","N80",RunTimeOS.Symbian,136));
        MobilePhoneList.Add(new MobilePhone("Nokia","N81",RunTimeOS.Symbian,137));
        MobilePhoneList.Add(new MobilePhone("Nokia","N81 8GB",RunTimeOS.Symbian,138));
        MobilePhoneList.Add(new MobilePhone("Nokia","N82",RunTimeOS.Symbian,139));
        MobilePhoneList.Add(new MobilePhone("Nokia","N91",RunTimeOS.Symbian,140));
        MobilePhoneList.Add(new MobilePhone("Nokia","N92",RunTimeOS.Symbian,141));
        MobilePhoneList.Add(new MobilePhone("Nokia","N93",RunTimeOS.Symbian,142));
        MobilePhoneList.Add(new MobilePhone("Nokia","N93i",RunTimeOS.Symbian,143));
        MobilePhoneList.Add(new MobilePhone("Nokia","N95",RunTimeOS.Symbian,144));
        MobilePhoneList.Add(new MobilePhone("Nokia","N95 8GB",RunTimeOS.Symbian,145));
        MobilePhoneList.Add(new MobilePhone("Nokia","N96",RunTimeOS.Symbian,146));
        MobilePhoneList.Add(new MobilePhone("Samsung","SGH-G810",RunTimeOS.Symbian,147));
        MobilePhoneList.Add(new MobilePhone("Samsung","SGH-i400",RunTimeOS.Symbian,148));
        MobilePhoneList.Add(new MobilePhone("Samsung","SGH-i450",RunTimeOS.Symbian,149));
        MobilePhoneList.Add(new MobilePhone("Samsung","SGH-i520",RunTimeOS.Symbian,150));
        MobilePhoneList.Add(new MobilePhone("Samsung","SGH-i550",RunTimeOS.Symbian,151));
        MobilePhoneList.Add(new MobilePhone("Samsung", "SGH-i560", RunTimeOS.Symbian, 153));
        MobilePhoneList.Add(new MobilePhone("Samsung", "BlackJack II", RunTimeOS.BlackJackII, 153001));
        MobilePhoneList.Add(new MobilePhone("Motorola", "Moto Q", RunTimeOS.MotoQ, 154));

        // Wednesday october
        MobilePhoneList.Add(new MobilePhone("BlackBerry", "Curve", RunTimeOS.BlackBerryCurve, 155));
        MobilePhoneList.Add(new MobilePhone("BlackBerry", "Pearl", RunTimeOS.BlackBerryPearl, 156));
    }
}
