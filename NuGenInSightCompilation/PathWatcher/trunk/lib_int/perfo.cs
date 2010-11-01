using System;
using System.Timers;
using System.Diagnostics;

namespace Genetibase.Debug
{
  public class PerfNetcardResult
  {
    public string BytesPsecTx;
    public string BytesPsecRx;
    public string BytesPsecTotal;
  }

  public delegate void NetPerfoResult(string rx, string tx, string total);

	public class perfo
	{
    public PerfNetcardResult   PerfResult;

    private const string NetCat       = "Network Interface";
    private const string NetTxName    = "Bytes Sent/sec"; 
    private const string NetRxName    = "Bytes Received/sec";
    private const string NetTotalName = "Bytes Total/sec";

//    private string AppNetTxName    = null;
//    private string AppNetRxName    = null;
//    private string AppNetTotalName = null;

    private string NetMachine;  
    private string NetInstance;

    private NetPerfoResult pAppCB;

    private PerformanceCounter  PerfNetTx;
    private PerformanceCounter  PerfNetRx;
    private PerformanceCounter  PerfNetTotal;
    private System.Timers.Timer PerfTimer;


		public perfo()
		{
      PerfResult  = new PerfNetcardResult();
      PerfTimer   = new System.Timers.Timer();
      PerfTimer.Enabled = false;
      NetMachine  = Environment.MachineName;
      NetInstance = GetNetFirstInstanceName();
		}

    public void SetPerfoCallback(NetPerfoResult pCB)
    {
      pAppCB = new NetPerfoResult(pCB);
    }

    #region getter setter
    public string GetMachineName
    {
      get { return  NetMachine;}
    }
    public string NetDevice
    {
      get { return  NetInstance;}
      set
      {
        NetInstance = value;
      }
    }
    public bool IsRunning
    {
      get
      {
        return PerfTimer.Enabled;
      }
    }
    #endregion
    public void Stop()
    {
      PerfTimer.Enabled =false;
      PerfResult.BytesPsecTx    = "0";
      PerfResult.BytesPsecRx    = "0";
      PerfResult.BytesPsecTotal = "0";
    }
    public void Start()
    {
      InitNetPerfoInstance(ref PerfNetTx,NetTxName);
      InitNetPerfoInstance(ref PerfNetRx,NetRxName);
      InitNetPerfoInstance(ref PerfNetTotal,NetTotalName);
      PerfTimerInit();
      PerfTimer.Enabled =true;
      PerfResult.BytesPsecTx    = "0";
      PerfResult.BytesPsecRx    = "0";
      PerfResult.BytesPsecTotal = "0";
    }
    private void PerfTimerInit()
    {
      PerfTimer          = new System.Timers.Timer();
      PerfTimer.Interval = 1000;
      PerfTimer.Elapsed += new System.Timers.ElapsedEventHandler(PerfTimerTimeout);
    }

    private void PerfTimerTimeout(object o , System.Timers.ElapsedEventArgs a)
    {
      PerfResult.BytesPsecTx    = PerfNetTx.NextValue().ToString();
      PerfResult.BytesPsecRx    = PerfNetRx.NextValue().ToString();
      PerfResult.BytesPsecTotal = PerfNetTotal.NextValue().ToString();

      if(pAppCB != null)
      {
        pAppCB(PerfResult.BytesPsecRx,PerfResult.BytesPsecTx ,PerfResult.BytesPsecTotal);
      }
    }

    private void InitNetPerfoInstance(ref PerformanceCounter PerfCnt, string CounterName)
    {
      PerfCnt               = new PerformanceCounter();
      PerfCnt.CategoryName  = NetCat;
      PerfCnt.MachineName   = NetMachine;
      PerfCnt.InstanceName  = NetInstance;
      PerfCnt.CounterName   = CounterName;
    }


    private string GetNetFirstInstanceName()
    {
      PerformanceCounterCategory mycat = new PerformanceCounterCategory("Network Interface");
      mycat.MachineName = NetMachine;
      string []s = mycat.GetInstanceNames();
      return s[1];
    }

    public string [] GetAllInstanceNames()
    {
      PerformanceCounterCategory mycat = new PerformanceCounterCategory("Network Interface");
      mycat.MachineName = NetMachine;
      string [] s = mycat.GetInstanceNames();
      return s;
    }

    #region helper
    public string GetNetCounters(string Category)
    {
      string result = "";

      PerformanceCounterCategory mycat = new PerformanceCounterCategory(Category);
      mycat.MachineName = NetMachine;
      // Retrieve the counters.
      PerformanceCounter[] mypc ;
      string []s = mycat.GetInstanceNames();
      foreach(string st in s)
      {
        mypc =  mycat.GetCounters(st); 
        result += "#####  " + st + "  ##### \r\n";
        // Add the retrieved counters to the list.
        for (int i = 0; i < mypc.Length; i++) 
        { 
          result += mypc[i].CounterName + "\r\n";
        }
      }
      return result;
    }
    public string GetNetCategories()
    {
      string result = "";
      PerformanceCounterCategory [] mycat;
      // Retrieve the counters.
      mycat = PerformanceCounterCategory.GetCategories();
      // Add the retrieved counters to the list.
      for (int i = 0; i < mycat.Length; i++) 
      { 
        result += mycat[i].CategoryName + "\r\n";
      }

      return result;
    }
    #endregion
	}
}
/*
 * #####  Intel[R] PRO_1000 CT Network Connection  ##### 

Packets/sec
Packets Received/sec
Packets Sent/sec

Current Bandwidth

Bytes Received/sec
Bytes Sent/sec
Bytes Total/sec

Packets Received Unicast/sec
Packets Received Non-Unicast/sec
Packets Received Discarded
Packets Received Errors
Packets Received Unknown

Packets Sent Unicast/sec
Packets Sent Non-Unicast/sec

Packets Outbound Discarded
Packets Outbound Errors

Output Queue Length
 * */

