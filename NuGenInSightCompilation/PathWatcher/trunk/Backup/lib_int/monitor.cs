using System;
using System.Text;
using System.Management;

namespace Genetibase.Debug
{
	/// <summary>
	/// Summary description for monitor.
	/// </summary>
	/// 
  public class Processor
  {
    public string Name;
    public string Description;
    public string SpeedCurrent;
    public string SpeedMax;
  }
  public class Netcard
  {
    public string Manufactor;
    public string Type;
    public string MacAdress;
  
  }
  public class Os
  {
    public string Name;
    public string Version;
  }
  
	public class monitor
	{

    public Processor DatCpu;
    public Os        DatOs;
    public Netcard   DatNetCard;

    public enum WMICLASS
    {
      Os         = 0,
      Processor,
      Netcard
    };

    
    private string[] WmiClass=
    {
       "Win32_OperatingSystem", "Win32_Processor" , "Win32_NetworkAdapter"
    };

		public monitor()
		{
      DatCpu     = new Processor();
      DatOs      = new Os();
      DatNetCard = new Netcard();
      
      FillDataProcessor();
      FillDataOs();
      FillDataNetCard();
		}

    public string [] GetWmiOs()        { return GetWmiClass(WMICLASS.Os);        }
    public string [] GetWmiProcessor() { return GetWmiClass(WMICLASS.Processor); }
    public string [] GetWmiNetcard()   { return GetWmiClass(WMICLASS.Netcard);   }



    private void FillDataNetCard()
    {
      ManagementObjectSearcher query1 = new ManagementObjectSearcher("SELECT * FROM " + WmiClass[(int)WMICLASS.Netcard]+" WHERE DeviceID='" + "1" + "'"); 
      foreach (ManagementObject card in query1.Get())
      {
        DatNetCard.Manufactor  =  card["Manufacturer"].ToString().Trim() ;
        DatNetCard.Type        =  card["Name"].ToString().Trim() ;
        DatNetCard.MacAdress   =  card["MACAddress"].ToString().Trim() ;

        DatNetCard.Type = DatNetCard.Type.Replace("(","[");
        DatNetCard.Type = DatNetCard.Type.Replace(")","]");
        DatNetCard.Type = DatNetCard.Type.Replace("/","_");
      }
    }
    private void FillDataOs()
    {
      ManagementObjectSearcher query1 = new ManagementObjectSearcher("SELECT * FROM " + WmiClass[(int)WMICLASS.Os]);
      foreach (ManagementObject processor in  query1.Get())
      {
        DatOs.Name     =  processor["Caption"].ToString().Trim() ;
        DatOs.Version  =  processor["Version"].ToString().Trim() ;
      }
    }
    private void FillDataProcessor()
    {
      ManagementObjectSearcher query1 = new ManagementObjectSearcher("SELECT * FROM " + WmiClass[(int)WMICLASS.Processor]); 
      foreach (ManagementObject processor in query1.Get())
      {
        DatCpu.Name         =  processor["Name"].ToString().Trim() ;
        DatCpu.Description  =  processor["Description"].ToString().Trim() ;
        DatCpu.SpeedCurrent =  processor["CurrentClockSpeed"].ToString().Trim() ;
        DatCpu.SpeedMax     =  processor["MaxClockSpeed"].ToString().Trim() ;
      }
    }

    private string [] GetWmiClass(WMICLASS wmi)
    {
      string [] result;

      int Index=0;
      result = new String[1024*2];

      try
      {
        ManagementObjectSearcher query1 = new  ManagementObjectSearcher("SELECT * FROM " + WmiClass[(int)wmi]); 
        foreach( ManagementObject mo in query1.Get() ) 
        {
          foreach (PropertyData property in mo.Properties)
          {
            // write the propert name and the property value
            result[Index++]=(property.Name + " = " + property.Value);
          }
        }   
      }
      catch(Exception ex)
      {
        result[0]= ex.Message ;
      }
      return result;
    }
	}
}
