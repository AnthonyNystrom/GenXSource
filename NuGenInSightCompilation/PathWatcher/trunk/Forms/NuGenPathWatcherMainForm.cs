using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Net;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Net.Sockets;
using System.Management;
using System.DirectoryServices;
using JH.CommBase;
using Janus.Windows.EditControls.Design;


namespace Genetibase.Debug
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
  public class NuGenPathWatcherMainForm : System.Windows.Forms.Form
  {
    #region Private Variables
    private FileSystemWatcher FileWatch;
//    private SocketServer SocServer;
////    private SocketClient SocClient;
//    private string ChoosedFileName="";
//    private bool bSocServerIsRunning=false;
//    private bool bSocClientIsRunning=false;
//    private string LastClipBoard;
//    private perfo PerfNet;
//    private miniterminal MiniT;
//    private Process NetCmdProcess;
//    private Process NetShowShares;
    #endregion

    #region Main Form Controls
    
//    private System.Windows.Forms.ContextMenu contextMenu1;
//    private System.Windows.Forms.NotifyIcon notifyIcon1;
//    private System.Windows.Forms.MenuItem menuItem1;
    private System.Windows.Forms.Timer timer1;
    private System.ComponentModel.IContainer components;
    private System.Windows.Forms.OpenFileDialog openFileDialog1;
    private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    private System.Windows.Forms.ListBox listBoxFiles;
    private Janus.Windows.EditControls.UIComboBox comboBoxFwatchDrive;
      private System.Windows.Forms.ToolTip toolTip1;
      private Janus.Windows.EditControls.UICheckBox uICheckBox1;
      private Janus.Windows.EditControls.UIButton uIButton1;
      private FolderBrowserDialog folderBrowserDialog1;
      private Janus.Windows.UI.Tab.UITab uiTab1;
      private Janus.Windows.UI.Tab.UITabPage uiTabPage1;
      private Janus.Windows.UI.CommandBars.UICommandManager uiCommandManager1;
      private Janus.Windows.UI.CommandBars.UIRebar BottomRebar1;
      private Janus.Windows.UI.CommandBars.UICommandBar uiCommandBar1;
      private Janus.Windows.UI.CommandBars.UIRebar LeftRebar1;
      private Janus.Windows.UI.CommandBars.UIRebar RightRebar1;
      private Janus.Windows.UI.CommandBars.UIRebar TopRebar1;        
        private Janus.Windows.UI.CommandBars.UICommand Command01;
        private Janus.Windows.UI.CommandBars.UICommand Command11;
        private Janus.Windows.UI.CommandBars.UICommand Command21;
        private Janus.Windows.UI.CommandBars.UICommand Command31;
        private System.Windows.Forms.ImageList imageList1;
        private Janus.Windows.UI.CommandBars.UICommand Command0;
        private Janus.Windows.UI.CommandBars.UICommand Command1;
        private Janus.Windows.UI.CommandBars.UICommand Command2;
        private Janus.Windows.UI.CommandBars.UICommand Command3;
        private Janus.Windows.UI.CommandBars.UICommand Command4;
        private Janus.Windows.UI.CommandBars.UICommand Command41;
        private Janus.Windows.UI.CommandBars.UICommand Command42;
        private Janus.Windows.UI.CommandBars.UICommand Command43;
        private Janus.Windows.UI.CommandBars.UICommand Command44;
        private Janus.Windows.UI.CommandBars.UICommand Command45;
        private Janus.Windows.UI.CommandBars.UICommand Command46;
        private Janus.Windows.UI.CommandBars.UICommand Command47;
        private Janus.Windows.UI.CommandBars.UICommand Command48;
        private Janus.Windows.UI.CommandBars.UICommand Command49;
      private Janus.Windows.UI.CommandBars.UICommand Command410;
        private Janus.Windows.UI.CommandBars.UICommand toolStripDropDownButton1;
        private Janus.Windows.UI.CommandBars.UICommand Command51;
    private System.Windows.Forms.Label label8;
    #endregion
    
    #region Main Form Code
    public NuGenPathWatcherMainForm()
    {
      InitializeComponent();
    
      // Commands
//      CommandsInit();
//      // Net Performance
//      NetPerformanceInit();
//      // Mini Terminal
//      MiniTermInit();
//      // Tray Icon
//      InitTrayIcon();
//      // Remote Clipboard
//      RemoteClipBoardInit();
      // FileWatcher
      FileWatcherInit();
    }

    protected override void Dispose( bool disposing )
    {
      if( disposing )
      {
        if (components != null) 
        {
          components.Dispose();
        }
      }
      base.Dispose( disposing );
    }
   
    [STAThread]
    static void Main() 
    {

      Application.Run(new NuGenPathWatcherMainForm());
    }
    private void Form1_Load(object sender, System.EventArgs e)
    {
    
    }
//    private void uicheckBox1_CheckedChanged(object sender, System.EventArgs e)
//    {
//      this.TopMost = uicheckBox.Checked;
//    }
//
    #endregion

//    #region TrayIcon
//
//    private void menuItem1_Click(object Sender, EventArgs e) 
//    {
//      // Close the form, which closes the application.
//      this.notifyIcon1.Dispose();
//      Application.Exit();
//    }
//
//    private void InitTrayIcon()
//    {
//      // Create the NotifyIcon.
//      this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
//      this.contextMenu1 = new System.Windows.Forms.ContextMenu();
//      this.menuItem1 = new System.Windows.Forms.MenuItem();
//
//      // Initialize contextMenu1
//      this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {this.menuItem1});
//
//      // Initialize menuItem1
//      this.menuItem1.Index = 0;
//      this.menuItem1.Text = "E&xit";
//      this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
//
//      notifyIcon1.ContextMenu = this.contextMenu1;
//      notifyIcon1.Icon = new Icon("icon2.ico");//  this.Icon.Handle;//("App2.ico");
//
//      notifyIcon1.Text = "Genetibase.Debug";
//      notifyIcon1.Visible = false;
//      notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
//    
//    }
//    private void notifyIcon1_DoubleClick(object Sender, EventArgs e) 
//    {
//      this.Visible=true;
//      this.Activate();
//      this.WindowState=FormWindowState.Normal;
//    }
//
//    #endregion

//    #region Net
//
////    private void buttonGetIp_Click(object sender, System.EventArgs e)
////    {
////      textBoxStatus.Text		 = "Trying....";
////      textBoxStatus.Update();
////      try 
////      {
////        IPHostEntry hostInfo = Dns.GetHostByName(textBoxHostName.Text);
////        textBoxIpAdress.Text = hostInfo.AddressList[0].ToString();
////        textBoxStatus.Text	 = "ok";
////      }
////      catch (Exception excpt)
////      {
////        textBoxStatus.Text = excpt.Message;
////        textBoxIpAdress.Text = "";
////      }
////    }
////
////    private void buttonGetName_Click(object sender, System.EventArgs e)
////    {
////
////      textBoxStatus.Text		 = "Trying....";
////      textBoxStatus.Update();
////
////      try 
////      {
////        IPAddress ipaddr     = IPAddress.Parse(textBoxIpAdress.Text);
////        IPHostEntry hostInfo = Dns.GetHostByAddress(ipaddr);
////        textBoxHostName.Text = hostInfo.HostName;
////        textBoxStatus.Text	 = "ok"; //149.246.122.194
////      }
////      catch (Exception excpt)
////      {
////        textBoxStatus.Text    = excpt.Message; 
////        textBoxHostName.Text = "";
////      }		
////    }
//    #endregion
//    #region Net Shares
//    private void NetCommand()
//    {
//      System.EventHandler s = new System.EventHandler(NetCmdProcess_exited);
//      StartNetProc(ref NetCmdProcess, "/C  net view > pc.txt" , ref s);
//    }
//    private void NetCmdProcess_exited(object sender, System.EventArgs e)
//    {
//      try
//      {
//        Debug.WriteLine("NetCmdProcess_exited");
//        System.IO.StreamReader myFile =	new System.IO.StreamReader("pc.txt");
//        
//        int i=0;
//        string pc = "x";
//
////        listViewPCs.Items.Clear();
//        do
//        {
//          pc = myFile.ReadLine();
//
//          if(pc.StartsWith("\\"))
//          {
//            i++;
//            string pcname = pc.Substring(2,pc.IndexOf(" "));
//            string ip     = GetIP(pcname);
//            string desc   = pc.Substring(pc.IndexOf(" "));
//            AddPcToList(pcname,ip,desc.Trim());
//          }
//        }while (!pc.StartsWith("The command"));
//
//        myFile.Close();
////        textBoxAnzPCs.Text = i.ToString() +" Computer found!";
//      }
////      catch(Exception excpt)
//      {
////        textBoxCmdOutput.Text    = excpt.Message; 
//      }
//    }
//
//    private void buttonGetPCs_Click_1(object sender, System.EventArgs e)
//    {
//      NetCommand();
//    }
//    string GetIP(string PcName)
//    {
//      IPHostEntry hostInfo = Dns.GetHostByName(PcName);
//      return hostInfo.AddressList[0].ToString();
//    }
//
//    void AddPcToList(string PC, string IP, string Beschreibung)    
//    {
//      ListViewItem item1 = new ListViewItem(PC,0);
//      item1.SubItems.Add(IP);
//      item1.SubItems.Add(Beschreibung);
////      listViewPCs.Items.Add(item1);
//    }
//
//    private void StartShareSearch()
//    {
////      int anzMarked=listViewPCs.SelectedItems.Count;
//      if(anzMarked > 1 || anzMarked == 0)
//      {
//        return;
//      }
//      string pc="";
////      foreach (ListViewItem m in listViewPCs.SelectedItems)
//      {
//        pc = m.SubItems[0].Text;
//      }
//      System.EventHandler s = new System.EventHandler(NetShowShare_exited);
//      StartNetProc(ref NetShowShares, "/C  net view " + pc + "> pc.txt" , ref s);
//    }
//    private void listViewPCs_SelectedIndexChanged_1(object sender, System.EventArgs e)
//    {
//      StartShareSearch();
//    }
//
//
//    private void StartNetProc(ref Process NetCmdProc, string strCommand, ref System.EventHandler EvH)
//    {
//      if(NetCmdProc != null)
//      {
//        NetCmdProc.WaitForExit();
//      }
//      NetCmdProc = new Process();
//      NetCmdProc.Exited += EvH;
//      NetCmdProc.StartInfo.FileName               = "cmd";
//      //myCmdProcess.StartInfo.WorkingDirectory       = (string)ComboBoxDrive.SelectedItem+"\\";//"t:\\";
//      NetCmdProc.StartInfo.Arguments              = strCommand;
//      NetCmdProc.StartInfo.RedirectStandardOutput = true;
//      NetCmdProc.StartInfo.UseShellExecute        = false;
//      NetCmdProc.StartInfo.CreateNoWindow         = true;
//      NetCmdProc.EnableRaisingEvents              = true;
//      Debug.WriteLine("StartNetProc -> " + NetCmdProc.StartInfo.FileName + " " + NetCmdProc.StartInfo.Arguments);
//      NetCmdProc.Start();
//    }
//
//    private void NetShowShare_exited(object sender, System.EventArgs e)
//    {
//      Debug.WriteLine("NetShowShare_exited");
//      System.IO.StreamReader myFile =	new System.IO.StreamReader("pc.txt");
//      string shares = myFile.ReadToEnd();
//      myFile.Close();
////      textBoxShares.Text=shares;
//    }
//    
//    #endregion

    #region Commands
//
//    private void CommandsInit()
//    {
//      myCmdProcess = new Process();
//      myCmdProcess.Exited += new System.EventHandler(myCmdProcess_exited);
//    }

//    private void buttonShutdown_Click(object sender, System.EventArgs e)
//    {
//      string AddCmd="";
//
////      if (radioButtonLogOff.Checked)
////      {
////        AddCmd="-l";
////      }
////      else if (radioButtonRestart.Checked)
////      {
////        AddCmd="-f -r -t 5";
////      }			
////      else if (radioButtonShut.Checked)
////      {
////        AddCmd="-f -s -t 5";
////      }
//      else
//      {
//        return;
//      }				
		    
//      ProcessStartInfo proc = new ProcessStartInfo();
//      proc.FileName = "cmd";
//      proc.Arguments = "/C shutdown "+AddCmd; // c- terminate K - remains
//      Process.Start(proc);
//    }
//    private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
//    {
//      //mail to ajit
//      //System.Diagnostics.Process.Start("mailTo:ajit_mungale@hotmail.com");
//      System.Diagnostics.Process.Start("http:\\\\www.google.de");
//    }

   

    private void buttonDosBox_Click(object sender, System.EventArgs e)
    {
      ProcessStartInfo proc = new ProcessStartInfo();
      proc.FileName = "cmd";
      proc.Arguments = "/K dir";
      Process.Start(proc);
    }

//    private void myCmdProcess_exited(object sender, System.EventArgs e)
//    {
//				
//      try
//      {
//        System.IO.StreamReader myFile =	new System.IO.StreamReader("test.txt");
//        string myString = myFile.ReadToEnd();
//        myFile.Close();
//        textBoxCmdOutput.AppendText(myString);
//      }
//      catch(Exception excpt)
//      {
//        textBoxCmdOutput.Text    = excpt.Message; 
//      }
//			
//
//    }
////    private void buttonCmdClearOutput_Click(object sender, System.EventArgs e)
////    {
////      textBoxCmdOutput.Text = "";
////    }
//
//
//
//    private void buttonCmdCancel_Click(object sender, System.EventArgs e)
//    {
////      try
////      {
////        myCmdProcess.Kill();
////        textBoxCmdOutput.Text="Abbruch...\r\n";
////      }
////      catch(Exception excpt)
////      {
////        textBoxCmdOutput.Text=excpt.Message;
////      }
//    }


//    private void buttonCmdNetView_Click(object sender, System.EventArgs e)
//    {
//      myCmdProcess.StartInfo.FileName               = "cmd";
//      //myCmdProcess.StartInfo.WorkingDirectory       = (string)ComboBoxDrive.SelectedItem+"\\";//"t:\\";
//      myCmdProcess.StartInfo.Arguments              = "/C  net view >test.txt";
//      myCmdProcess.StartInfo.RedirectStandardOutput = true;
//      myCmdProcess.StartInfo.UseShellExecute        = false;
//      myCmdProcess.StartInfo.CreateNoWindow         = true;
//      myCmdProcess.EnableRaisingEvents               =true;
//
//      try 
//      {
//        textBoxCmdOutput.Text="Show PCs... \r\n";
//        textBoxCmdOutput.Update();
//        myCmdProcess.Start();
//				
//      }
//      catch(Exception excpt)
//      {
//        textBoxCmdOutput.Text    = excpt.Message; 
//      }
//    }
//
//    private void buttonCmdNetConf_Click(object sender, System.EventArgs e)
//    {
//      myCmdProcess.StartInfo.FileName               = "cmd";
//      //myCmdProcess.StartInfo.WorkingDirectory       = (string)ComboBoxDrive.SelectedItem+"\\";//"t:\\";
//      myCmdProcess.StartInfo.Arguments              = "/C  ipconfig >test.txt";
//      myCmdProcess.StartInfo.RedirectStandardOutput = true;
//      myCmdProcess.StartInfo.UseShellExecute        = false;
//      myCmdProcess.StartInfo.CreateNoWindow         = true;
//      myCmdProcess.EnableRaisingEvents               =true;
//
//      try 
//      {
//        textBoxCmdOutput.Text="Show Net Config... \r\n";
//        textBoxCmdOutput.Update();
//        myCmdProcess.Start();
//				
//      }
//      catch(Exception excpt)
//      {
//        textBoxCmdOutput.Text    = excpt.Message; 
//      }
//    }
    #endregion

    #region Remote Server
    //#########################################################################
    // Remote Server
    //#########################################################################

//    public void SocketServerCallBackData(string Data)
//    {
//      AddDataRemoteClipBoard(Data);
//      textBoxRemoteClipBUsedBuffer.Text=Convert.ToString(SocServer.NoRxDat);
//    }
//
//    public void SocketServerCallBackError(string ErrorMsg)
//    {
//      TexBoxRemoteClip.AppendText(ErrorMsg+"\r\n");
//      //StopSocketServer();
//    }
//
//    public  void StartSocketServer()
//    {
//      SocServer = new SocketServer();
//      SocServer.ServerCallbackError=new ServerCallBack(SocketServerCallBackError);
//      SocServer.ServerCallback=new ServerCallBack(SocketServerCallBackData);
//      try
//      {
//        SocServer.PortNr=Convert.ToInt16(textBoxPortNr.Text);
//      }
//      catch(Exception se)
//      {
//        SocketServerCallBackError(se.Message);
//      }
//      bSocServerIsRunning=true;
//      SocServer.StartServer();
//      if(bSocServerIsRunning)
//      {
//        TexBoxRemoteClip.AppendText("Server started"+"\r\n");
//        EnableCommands(false);
//      }
//      else
//      {
//        TexBoxRemoteClip.AppendText("Server couldn`t started"+"\r\n");
//      }
//    }
//
//    public void StopSocketServer()
//    {
//      if(SocServer!=null)
//      {
//        SocServer.StopServer();
//        bSocServerIsRunning=false;
//        TexBoxRemoteClip.AppendText("Server stopped"+"\r\n");
//        EnableCommands(true);
//      }
//    }
//
//    public void SendDataToClient(string TxData)
//    {
//      SocServer.SendData(TxData);
//      textBoxRemoteClipBUsedBufferTX.Text=Convert.ToString(SocServer.NoTxDat);
//    }
//		
//    private void EnableCommands( bool abEnableConnect ) 
//    {
//      buttonStopRemoteService.Enabled = !abEnableConnect;
//      buttonStartRemoteService.Enabled = abEnableConnect;
//    }
    //#########################################################################
    // Remote Server END
    //#########################################################################
    #endregion

    #region Remote Client
    //#########################################################################
    // Remote Client
    //#########################################################################

//    public void SocketClientCallBackData(string Data)
//    {
//      AddDataRemoteClipBoard(Data);
//      textBoxRemoteClipBUsedBuffer.Text=Convert.ToString(SocClient.NoRxDat);
//    }
//
//    public void SocketClientCallBackError(string ErrorMsg)
//    {
//      TexBoxRemoteClip.AppendText(ErrorMsg+"\r\n");
//      StopSocketClient();
//    }
//
//    public  void StartSocketClient()
//    {
//      SocClient = new SocketClient();
//      SocClient.ClientCallbackRxData=new ClientCallBack(SocketClientCallBackData);
//      SocClient.ServerCallbackError=new ClientCallBack(SocketClientCallBackError);
//      try
//      {
//        SocClient.PortNr= Convert.ToInt16(textBoxPortNr.Text);
//        SocClient.IP=IPAddress.Parse (textBoxServerIp.Text);
//      }
//      catch(Exception se)
//      {
//        SocketClientCallBackError(se.Message);
//      }
//
//
//      bSocClientIsRunning=true;
//      SocClient.StartClient();
//
//      if(bSocClientIsRunning)
//      {
//        TexBoxRemoteClip.AppendText("Client started"+"\r\n");
//        EnableCommands(false);
//      }
//      else
//      {
//        TexBoxRemoteClip.AppendText("Client couldn`t started"+"\r\n");
//      }
//			
//    }
//
//    public void StopSocketClient()
//    {
//      if(SocClient!=null)
//      {
//        SocClient.StopClient();
//        bSocClientIsRunning=false;
//        TexBoxRemoteClip.AppendText("Client stopped"+"\r\n");
//        EnableCommands(true);
//      }
//    }
//    public void SendDataToServer(string TxData)
//    {
//      SocClient.SendData(TxData);
//      textBoxRemoteClipBUsedBufferTX.Text=Convert.ToString(SocClient.NoTxDat);
//    }

    //#########################################################################
    // Remote Client END
    //#########################################################################
    #endregion

    #region Remote ClipBoard
//    private void RemoteClipBoardInit()
//    {
//      WriteLocalIp();
//      EnableCommands(true);
//      textBoxServerIp.ReadOnly=true;
//      textBoxRemoteClipBUsedBuffer.Text="0";
//      textBoxRemoteClipBUsedBufferTX.Text="0";
//      buttonSaveAs.Enabled=false;
//      buttonShowFile.Enabled=false;
//    }
//
//    private void SendDataToRemorClip(string TxData)
//    {
//      if(!buttonStartRemoteService.Enabled)
//      {
//        if(radioButtonClient.Checked)
//        {
//          SendDataToServer(TxData);
//        }
//        if(radioButtonServer.Checked)
//        {
//          SendDataToClient(TxData);
//        }
//      }
//    }
//    private void buttonRemoteOutClear_Click(object sender, System.EventArgs e)
//    {
//      TexBoxRemoteClip.Text="";
//      textBoxRemoteClipBUsedBufferTX.Text="0";
//      textBoxRemoteClipBUsedBuffer.Text="0";
//    }
//
//    private void buttonStartRemoteService_Click(object sender, System.EventArgs e)
//    {
//      if(radioButtonServer.Checked)
//      {
//        StartSocketServer();
//      }
//
//      if(radioButtonClient.Checked)
//      {
//        StartSocketClient();
//      }
//    }
//    private void WriteLocalIp()
//    {
//      IPHostEntry hostInfo = Dns.GetHostByName("");
//      textBoxServerIp.Text = hostInfo.AddressList[0].ToString();
//    }
//
//    private void buttonRempteClipBPing_Click(object sender, System.EventArgs e)
//    {
//      SendDataToRemorClip("MSG_PING_REQ");
//    }
//
//    private void RestartService()
//    {
//			
//      if(radioButtonServer.Checked)
//      {
//        StopSocketServer();
//      }
//
//      if(radioButtonClient.Checked)
//      {
//        StopSocketClient();
//      }
//    }
//
//	
//    private void AddDataRemoteClipBoard(string str)
//    {
//      if(String.Compare(str,"MSG_PING_REQ")==0)
//      {
//        SendDataToRemorClip("MSG_PING_REQ_ACK");
//      }
//      else if (String.Compare(str,"MSG_PING_REQ_ACK")==0)
//      {
//        TexBoxRemoteClip.Text += "Remote Ok";
//        TexBoxRemoteClip.Text += "\r\n";
//      }
//      else
//      {
//        if((TexBoxRemoteClip.TextLength+str.Length)>=TexBoxRemoteClip.MaxLength)
//        {
//          TexBoxRemoteClip.Text="";
//        }
//        else
//        {
//          TexBoxRemoteClip.Text += str;
//          TexBoxRemoteClip.Text += "\r\n";
//        }
//      }
//    }
//     
//
//    private void ButtonStopRemoteService_Click(object sender, System.EventArgs e)
//    {
//      if(radioButtonServer.Checked)
//      {
//        StopSocketServer();
//      }
//
//      if(radioButtonClient.Checked)
//      {
//        StopSocketClient();
//      }
//    }
    #endregion

//    #region Local ClipBoard
//
//    private void textBoxClipBoard_LinkClicked(object sender, System.Windows.Forms.LinkClickedEventArgs e)
//    {
//      System.Diagnostics.Process.Start(e.LinkText);
//    }
//
//    private void buttonRemoveTab_Click(object sender, System.EventArgs e)
//    {
//      // Tab to Spaces
//      textBoxClipBoard.Text = textBoxClipBoard.Text.Replace("\t"," ");
//    }
//
//    private void textBoxClipBoard_TextChanged(object sender, System.EventArgs e)
//    {
//      textBoxClipBSize.Text=Convert.ToString(Convert.ToInt64(textBoxClipBSize.Text)+1);
//    }
//
//    private void timer1_Tick(object sender, System.EventArgs e)
//    {
//      string actual;
//      long size;
//      statusBar1.Panels[0].Text=Convert.ToString(DateTime.Now);
//      IDataObject iData = Clipboard.GetDataObject();          
//      //Determine whether the data is in a format you can use.
//      if(iData.GetDataPresent(DataFormats.Text)) 
//      {
//        actual = (String)iData.GetData(DataFormats.Text);
//        if( LastClipBoard != actual)
//        {
//          LastClipBoard = (String)iData.GetData(DataFormats.Text);
//          size = LastClipBoard.Length;
//          textBoxClipBoard.AppendText(actual);
//          textBoxClipBoard.AppendText("\r\n");
//          textBoxClipBSize.Text=Convert.ToString(Convert.ToInt64(textBoxClipBSize.Text)+actual.Length);
//          SendDataToRemorClip(actual);
//        }
//      }
//    }
//
//    private void buttonClipBoardClear_Click(object sender, System.EventArgs e)
//    {
//      textBoxClipBoard.Text = "";
//      textBoxClipBSize.Text = "0";
//    }
//
//    private void buttonSave_Click(object sender, System.EventArgs e)
//    {
//      System.IO.StreamWriter myAppendFile ;
//      System.IO.StreamWriter myFile;
//			
//      try
//      {
//        //	alles in neue Datei schreiben
//        if(!checkBoxSaveSelected.Checked && !checkBoxAppendFile.Checked)
//        {
//          myFile = new System.IO.StreamWriter(ChoosedFileName);
//          myFile.Write(textBoxClipBoard.Text.Replace("\n","\r\n")+"\r\n");
//          myFile.Close();
//        }
//
//        // alles an Datei anhängen
//        if(!checkBoxSaveSelected.Checked && checkBoxAppendFile.Checked)
//        {
//          myAppendFile=File.AppendText(ChoosedFileName);
//          myAppendFile.Write(textBoxClipBoard.Text.Replace("\n","\r\n")+"\r\n");
//          myAppendFile.Close();
//				
//        }
//        // nur markiertes in neue Datei schreiben
//        if(checkBoxSaveSelected.Checked && !checkBoxAppendFile.Checked)
//        {
//          myFile = new System.IO.StreamWriter(ChoosedFileName);
//          myFile.Write(textBoxClipBoard.SelectedText.Replace("\n","\r\n")+"\r\n");
//          myFile.Close();
//        }
//				
//        //nur markiertes an Datei anhängen
//        if(checkBoxSaveSelected.Checked && checkBoxAppendFile.Checked)
//        {
//          myAppendFile= File.AppendText(ChoosedFileName);
//          myAppendFile.Write(textBoxClipBoard.SelectedText.Replace("\n","\r\n")+"\r\n");
//          myAppendFile.Close();
//        }
//      }
//      catch(Exception excpt)
//      {
//        textBoxClipBoard.Text += excpt.Message; 
//        buttonSaveAs.Enabled   = false;
//        buttonShowFile.Enabled = false;
//      }
//    }
//
//    private void buttonChooseFile_Click(object sender, System.EventArgs e)
//    {
//
//      saveFileDialog1.InitialDirectory="c:\\" ;
//      saveFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*" ;
//      saveFileDialog1.FilterIndex = 1 ;
//      saveFileDialog1.RestoreDirectory = true ;
//      saveFileDialog1.ShowDialog();
//
//      if(saveFileDialog1.FileName != "")
//      {
//        ChoosedFileName=saveFileDialog1.FileName;
//        textBoxChooseFile.Text=ChoosedFileName;
//        buttonSaveAs.Enabled=true;
//        buttonShowFile.Enabled=true;
//      }
//
//    }
//
//    private void buttonShowFile_Click(object sender, System.EventArgs e)
//    {
//			
//      System.IO.StreamReader myFile =	new System.IO.StreamReader(ChoosedFileName);
//      string myString = myFile.ReadToEnd();
//      myFile.Close();
//      Form2 f = new Form2();
//      f.Text=ChoosedFileName;
//      f.Show();
//      f.WriteTextFeld(myString);
//    }
//
//		
//    private void checkBoxEditMode_CheckedChanged(object sender, System.EventArgs e)
//    {
//      if(checkBoxEditMode.Checked)
//      {
//        textBoxClipBoard.ReadOnly=false;
//        textBoxClipBoard.BackColor=Color.White;
//      }
//      else
//      {
//        textBoxClipBoard.ReadOnly=true;
//        textBoxClipBoard.BackColor=Color.Wheat;
//      }
//    }
//    #endregion

    #region FileWatcher

    private void FileWatcherInit()
    {
      FileWatch = new FileSystemWatcher();
      FileWatch.Path="C:\\";
    }

    private void comboBoxFwatchDrive_Enter(object sender, System.EventArgs e)
    {
      comboBoxFwatchDrive.Items.Clear();
      string[] str = Directory.GetLogicalDrives(); //GetLogicalDrives();

      foreach (String drive in str)
      {
          comboBoxFwatchDrive.Items.Add(drive);
      }      
    }

//    private void buttonFwatchStart_Click(object sender, System.EventArgs e)
//    {
//      if (FileWatch.EnableRaisingEvents == true) 
//      {
//        return;
//      }
//
//      listBoxFiles.Items.Clear();
////      FileWatch.Filter = textBoxWatchFilter.Text;
//      FileWatch.NotifyFilter =  NotifyFilters.FileName    | NotifyFilters.Attributes |
//        NotifyFilters.LastAccess  | NotifyFilters.LastWrite  | 
//        NotifyFilters.Security    | NotifyFilters.Size;
//
//      FileWatch.Changed += new FileSystemEventHandler(OnFileEvent);
//      FileWatch.Created += new FileSystemEventHandler(OnFileEvent);  
//      FileWatch.Deleted += new FileSystemEventHandler(OnFileEvent);
//      FileWatch.Renamed += new RenamedEventHandler(OnRenameEvent);
//      FileWatch.IncludeSubdirectories = true;
//      FileWatch.EnableRaisingEvents   = true;
//    
//    }

      delegate void ListBoxUpdateDelegate();
      ListBoxUpdateDelegate lstboxDel;

    public void OnFileEvent(object source, FileSystemEventArgs fsea) 
    {
      DateTime dt = new DateTime();
      dt = System.DateTime.UtcNow;
      FileInfo f = new FileInfo(fsea.FullPath);
      try
      {
          lstboxDel = delegate()
          {
              listBoxFiles.Items.Add(listBoxFiles.Items.Count.ToString() + " " + dt.ToLocalTime() + " " + fsea.ChangeType.ToString() + " " + fsea.FullPath + " " + f.Attributes.ToString());
              listBoxFiles.SelectedIndex = listBoxFiles.Items.Count - 1;
          };

          listBoxFiles.Invoke(lstboxDel);
      }
      catch (Exception) { }
    }

    public  void OnRenameEvent(Object source, RenamedEventArgs rea) 
    {
      DateTime dt = new DateTime();
      dt = System.DateTime.UtcNow;
      listBoxFiles.Items.Add(listBoxFiles.Items.Count.ToString() + " " +dt.ToLocalTime() + " " + rea.ChangeType.ToString() + rea.OldFullPath+ "  to " +" " + rea.FullPath);
      listBoxFiles.SelectedIndex = listBoxFiles.Items.Count - 1;
    }

//    private void buttonFwatchStop_Click(object sender, System.EventArgs e)
//    {
//      FileWatch.EnableRaisingEvents = false;		//Stop looking
//    }

    private void button2_Click(object sender, System.EventArgs e)
    {
      if (FileWatch.EnableRaisingEvents == true) 
      {
        return;
      }
    }

    private void comboBoxFwatchDrive_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        FileWatch.Path = comboBoxFwatchDrive.Text;
      //toolStripStatusLabel1.Text = "Watching " + FileWatch.Path;
    }
    #endregion
//
//    #region Net Performance
//
//    void NetPerformanceInit()
//    {
//      PerfNet= new perfo();
//    }
//    void NetPerfoResultCB(string rx, string tx, string total)
//    {
//      statusBar1.Panels[1].Text = tx + " TX";
//      statusBar1.Panels[2].Text = rx + " RX";
//      textBoxNetRx.Text  =  rx ;
//      textBoxNetTx.Text  =  tx ;
//    }
//
//    private string SelectNetCard()
//    {
//      NetDevice n = new NetDevice(ref PerfNet);
//      n.ShowDialog();
//      if(n.DialogResult == DialogResult.OK)
//      {
//        return n.comboBox1.Text;
//      }
//      return null;
//    }
//
//    private void button4_Click(object sender, System.EventArgs e)
//    {
//      if(!PerfNet.IsRunning)
//      {
//        NetPerfoResult p = new NetPerfoResult(NetPerfoResultCB);
//        PerfNet.SetPerfoCallback(p);
//        string Card = SelectNetCard();
//        if(Card != null)
//        {
//          PerfNet.NetDevice = Card;
//          PerfNet.Start();
//          button4.BackColor = Color.Red;
//          button4.Text = "STOP";
//        }
//      }
//      else
//      {
//        button4.Text = "START";
//        button4.BackColor = Color.PaleGreen;
//        PerfNet.Stop();
//        statusBar1.Panels[1].Text = "0";
//        statusBar1.Panels[2].Text = "0";
//        textBoxNetRx.Text  =  "0";
//        textBoxNetTx.Text  =  "0";
//      }
//    }
//    #endregion

//    #region Mini Terminal
//
//    private void MiniTermInit()
//    {
//      UpdateRxString pCB = new UpdateRxString(UpdateMiniTermRxWindow);
//      MiniT = new miniterminal(pCB);
//    
//    }
//    private void UpdateMiniTermRxWindow(string str)
//    {
//      textBoxRx.AppendText(str);
//    }
//
//
//    private void buttonComOpen_Click(object sender, System.EventArgs e)
//    {
//      if( buttonComOpen.Text.Equals("Close"))
//      {
//        MiniT.Close();
//        buttonComOpen.Text="Open";
//        textBoxTx.Enabled=false;
//        return;
//      }
//      MiniT.ComSettings.port     = comboBoxPort.Text;
//      MiniT.ComSettings.baudRate = int.Parse(comboBoxBaud.Text); 
//      if (!MiniT.Open())
//      {
//        MessageBox.Show("Port kann nicht geöffnet werden !", "Terminal", MessageBoxButtons.OK);
//      }
//      buttonComOpen.Text="Close";
//      textBoxTx.Enabled=true;
//    }
//
//    private void textBoxTx_TextChanged(object sender, System.EventArgs e)
//    {
//      string act = textBoxTx.Text;
//      string tmp = act.Substring(act.Length-1,1);
//
//      Encoding enc = Encoding.ASCII;
//      byte []  ch = new Byte[1];
//      ch  =  enc.GetBytes(tmp);
//
//      MiniT.SendByte(ch[0]);
//    }
//
//    private void checkBoxHex_CheckedChanged(object sender, System.EventArgs e)
//    {
//      MiniT.ShowRxHexValues = checkBoxHex.Checked; 
//    }
//
//    private void textBoxCR_TextChanged(object sender, System.EventArgs e)
//    {
//      if (textBoxCR.Text.Length > 2 || textBoxCR.Text == "")
//      {
//        textBoxCR.Text="";
//        return;
//      }
//      
//      try
//      {
//        // Convert hex string to unsigned integer
//        MiniT.SetCrHexValue = System.Convert.ToUInt32(textBoxCR.Text, 16);
//      }
//      catch (Exception exception) 
//      {
//        MiniT.SetCrHexValue=0xFFFF;
//        return;
//      }
//    }
//    #endregion

    #region misc
//    private void button3_Click(object sender, System.EventArgs e)
//    {
//      Form2 fEnv = new Form2();
//      string s="";
//      IDictionary d =  Environment.GetEnvironmentVariables();
//      foreach (DictionaryEntry de in d)
//      {
//        s += de.Key + " --- " + de.Value +"\r\n";
//      }
//      s +=  "Os Version "  + "   ---   " + Environment.OSVersion.ToString()  + "\r\n";
//      s +=  "CLR Version " + "   ---   " + Environment.Version.ToString()    + "\r\n";
//      s +=  "Res. Mem "    + "   ---   " + Environment.WorkingSet.ToString() + "\r\n";
//      
//      fEnv.WriteTextFeld(s);
//      fEnv.Show();
//    }

   

   
    /*
      private void button9_Click(object sender, System.EventArgs e)
      {
        monitor m = new monitor();

        textBoxTest.Lines = m.GetWmiOs();

        textBoxTest.Text += "--- CPU ---"         + "\r\n";
        textBoxTest.Text += m.DatCpu.Name         + "\r\n";
        textBoxTest.Text += m.DatCpu.Description  + "\r\n";;
        textBoxTest.Text += m.DatCpu.SpeedCurrent + "\r\n";;
        textBoxTest.Text += m.DatCpu.SpeedMax     + "\r\n";;

        textBoxTest.Text += "--- Manufactor ---"    + "\r\n";
        textBoxTest.Text += m.DatNetCard.Manufactor + "\r\n";
        textBoxTest.Text += m.DatNetCard.Type       + "\r\n";
        textBoxTest.Text += m.DatNetCard.MacAdress  + "\r\n";

        textBoxTest.Text += "--- OS ---"    + "\r\n";
        textBoxTest.Text += m.DatOs.Name    + "\r\n";
        textBoxTest.Text += m.DatOs.Version + "\r\n";
      }
      */
    #endregion

    #region Windows Form Designer generated code
    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NuGenPathWatcherMainForm));
        this.timer1 = new System.Windows.Forms.Timer(this.components);
        this.label8 = new System.Windows.Forms.Label();
        this.comboBoxFwatchDrive = new Janus.Windows.EditControls.UIComboBox();
        this.listBoxFiles = new System.Windows.Forms.ListBox();
        this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
        this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
        this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
        this.uIButton1 = new Janus.Windows.EditControls.UIButton();
        this.uICheckBox1 = new Janus.Windows.EditControls.UICheckBox();
        this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
        this.uiTab1 = new Janus.Windows.UI.Tab.UITab();
        this.uiTabPage1 = new Janus.Windows.UI.Tab.UITabPage();
        this.uiCommandManager1 = new Janus.Windows.UI.CommandBars.UICommandManager(this.components);
        this.BottomRebar1 = new Janus.Windows.UI.CommandBars.UIRebar();
        this.uiCommandBar1 = new Janus.Windows.UI.CommandBars.UICommandBar();
        this.Command01 = new Janus.Windows.UI.CommandBars.UICommand("Command0");
        this.Command11 = new Janus.Windows.UI.CommandBars.UICommand("Command1");
        this.Command21 = new Janus.Windows.UI.CommandBars.UICommand("Command2");
        this.Command31 = new Janus.Windows.UI.CommandBars.UICommand("Command3");
        this.Command51 = new Janus.Windows.UI.CommandBars.UICommand("Command5");
        this.imageList1 = new System.Windows.Forms.ImageList(this.components);
        this.Command0 = new Janus.Windows.UI.CommandBars.UICommand("Command0");
        this.Command1 = new Janus.Windows.UI.CommandBars.UICommand("Command1");
        this.Command2 = new Janus.Windows.UI.CommandBars.UICommand("Command2");
        this.Command3 = new Janus.Windows.UI.CommandBars.UICommand("Command3");
        this.Command41 = new Janus.Windows.UI.CommandBars.UICommand("Command4");
        this.Command42 = new Janus.Windows.UI.CommandBars.UICommand("Command4");
        this.Command43 = new Janus.Windows.UI.CommandBars.UICommand("Command4");
        this.Command44 = new Janus.Windows.UI.CommandBars.UICommand("Command4");
        this.Command45 = new Janus.Windows.UI.CommandBars.UICommand("Command4");
        this.Command46 = new Janus.Windows.UI.CommandBars.UICommand("Command4");
        this.Command47 = new Janus.Windows.UI.CommandBars.UICommand("Command4");
        this.Command48 = new Janus.Windows.UI.CommandBars.UICommand("Command4");
        this.Command49 = new Janus.Windows.UI.CommandBars.UICommand("Command4");
        this.Command410 = new Janus.Windows.UI.CommandBars.UICommand("Command4");
        this.Command4 = new Janus.Windows.UI.CommandBars.UICommand("Command4");
        this.toolStripDropDownButton1 = new Janus.Windows.UI.CommandBars.UICommand("Command5");
        this.LeftRebar1 = new Janus.Windows.UI.CommandBars.UIRebar();
        this.RightRebar1 = new Janus.Windows.UI.CommandBars.UIRebar();
        this.TopRebar1 = new Janus.Windows.UI.CommandBars.UIRebar();
        ((System.ComponentModel.ISupportInitialize)(this.uiTab1)).BeginInit();
        this.uiTab1.SuspendLayout();
        this.uiTabPage1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.uiCommandManager1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.BottomRebar1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.uiCommandBar1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.LeftRebar1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.RightRebar1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.TopRebar1)).BeginInit();
        this.TopRebar1.SuspendLayout();
        this.SuspendLayout();
        // 
        // timer1
        // 
        this.timer1.Enabled = true;
        this.timer1.Interval = 1000;
        // 
        // label8
        // 
        this.label8.BackColor = System.Drawing.Color.Transparent;
        this.label8.Location = new System.Drawing.Point(116, 5);
        this.label8.Name = "label8";
        this.label8.Size = new System.Drawing.Size(37, 21);
        this.label8.TabIndex = 6;
        this.label8.Text = "Path:";
        this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
        // 
        // comboBoxFwatchDrive
        // 
        this.comboBoxFwatchDrive.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.comboBoxFwatchDrive.BackColor = System.Drawing.Color.White;
        this.comboBoxFwatchDrive.Location = new System.Drawing.Point(159, 6);
        this.comboBoxFwatchDrive.Name = "comboBoxFwatchDrive";
        this.comboBoxFwatchDrive.Size = new System.Drawing.Size(210, 20);
        this.comboBoxFwatchDrive.TabIndex = 5;
        this.comboBoxFwatchDrive.Text = "C:\\";
        this.comboBoxFwatchDrive.Enter += new System.EventHandler(this.comboBoxFwatchDrive_Enter);
        this.comboBoxFwatchDrive.SelectedIndexChanged += new System.EventHandler(this.comboBoxFwatchDrive_SelectedIndexChanged);
        // 
        // listBoxFiles
        // 
        this.listBoxFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                    | System.Windows.Forms.AnchorStyles.Left)
                    | System.Windows.Forms.AnchorStyles.Right)));
        this.listBoxFiles.BackColor = System.Drawing.Color.White;
        this.listBoxFiles.BorderStyle = System.Windows.Forms.BorderStyle.None;
        this.listBoxFiles.HorizontalScrollbar = true;
        this.listBoxFiles.Location = new System.Drawing.Point(11, 43);
        this.listBoxFiles.Name = "listBoxFiles";
        this.listBoxFiles.Size = new System.Drawing.Size(405, 234);
        this.listBoxFiles.TabIndex = 3;
        // 
        // saveFileDialog1
        // 
        this.saveFileDialog1.OverwritePrompt = false;
        this.saveFileDialog1.Title = "Choose a File ...";
        // 
        // uIButton1
        // 
        this.uIButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this.uIButton1.ButtonStyle = Janus.Windows.EditControls.ButtonStyle.Ellipsis;
        this.uIButton1.Location = new System.Drawing.Point(381, 5);
        this.uIButton1.Name = "uIButton1";
        this.uIButton1.Size = new System.Drawing.Size(41, 21);
        this.uIButton1.TabIndex = 11;
        this.uIButton1.UseThemes = false;
        this.uIButton1.VisualStyle = Janus.Windows.UI.VisualStyle.VS2005;
        this.uIButton1.Click += new System.EventHandler(this.uIButton1_Click_1);
        // 
        // uICheckBox1
        // 
        this.uICheckBox1.BackColor = System.Drawing.Color.Transparent;
        this.uICheckBox1.Location = new System.Drawing.Point(7, 6);
        this.uICheckBox1.Name = "uICheckBox1";
        this.uICheckBox1.Size = new System.Drawing.Size(103, 20);
        this.uICheckBox1.TabIndex = 10;
        this.uICheckBox1.Text = "Always On Top";
        this.uICheckBox1.UseThemes = false;
        this.uICheckBox1.VisualStyle = Janus.Windows.UI.VisualStyle.VS2005;
        this.uICheckBox1.CheckedChanged += new System.EventHandler(this.uICheckBox1_CheckedChanged);
        // 
        // uiTab1
        // 
        this.uiTab1.BackColor = System.Drawing.Color.Transparent;
        this.uiTab1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.uiTab1.Location = new System.Drawing.Point(0, 28);
        this.uiTab1.Name = "uiTab1";
        this.uiTab1.ShowTabs = false;
        this.uiTab1.Size = new System.Drawing.Size(427, 296);
        this.uiTab1.TabDisplay = Janus.Windows.UI.Tab.TabDisplay.Text;
        this.uiTab1.TabIndex = 12;
        this.uiTab1.TabPages.AddRange(new Janus.Windows.UI.Tab.UITabPage[] {
            this.uiTabPage1});
        this.uiTab1.VisualStyle = Janus.Windows.UI.Tab.TabVisualStyle.Office2007;
        // 
        // uiTabPage1
        // 
        this.uiTabPage1.Controls.Add(this.listBoxFiles);
        this.uiTabPage1.Controls.Add(this.uIButton1);
        this.uiTabPage1.Controls.Add(this.uICheckBox1);
        this.uiTabPage1.Controls.Add(this.comboBoxFwatchDrive);
        this.uiTabPage1.Controls.Add(this.label8);
        this.uiTabPage1.Location = new System.Drawing.Point(1, 1);
        this.uiTabPage1.Name = "uiTabPage1";
        this.uiTabPage1.Size = new System.Drawing.Size(425, 294);
        this.uiTabPage1.TabStop = true;
        this.uiTabPage1.Text = "New Tab";
        // 
        // uiCommandManager1
        // 
        this.uiCommandManager1.BottomRebar = this.BottomRebar1;
        this.uiCommandManager1.CommandBars.AddRange(new Janus.Windows.UI.CommandBars.UICommandBar[] {
            this.uiCommandBar1});
        this.uiCommandManager1.Commands.AddRange(new Janus.Windows.UI.CommandBars.UICommand[] {
            this.Command0,
            this.Command1,
            this.Command2,
            this.Command3,
            this.Command4,
            this.toolStripDropDownButton1});
        this.uiCommandManager1.ContainerControl = this;
        this.uiCommandManager1.Id = new System.Guid("c2663708-0a3b-4d29-8fdd-f47b1751c5cd");
        this.uiCommandManager1.LeftRebar = this.LeftRebar1;
        this.uiCommandManager1.RightRebar = this.RightRebar1;
        this.uiCommandManager1.TopRebar = this.TopRebar1;
        this.uiCommandManager1.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
        // 
        // BottomRebar1
        // 
        this.BottomRebar1.CommandManager = this.uiCommandManager1;
        this.BottomRebar1.Dock = System.Windows.Forms.DockStyle.Bottom;
        this.BottomRebar1.Location = new System.Drawing.Point(0, 324);
        this.BottomRebar1.Name = "BottomRebar1";
        this.BottomRebar1.Size = new System.Drawing.Size(427, 0);
        // 
        // uiCommandBar1
        // 
        this.uiCommandBar1.AllowClose = Janus.Windows.UI.InheritableBoolean.False;
        this.uiCommandBar1.AllowCustomize = Janus.Windows.UI.InheritableBoolean.False;
        this.uiCommandBar1.CommandManager = this.uiCommandManager1;
        this.uiCommandBar1.Commands.AddRange(new Janus.Windows.UI.CommandBars.UICommand[] {
            this.Command01,
            this.Command11,
            this.Command21,
            this.Command31,
            this.Command51});
        this.uiCommandBar1.FullRow = true;
        this.uiCommandBar1.ImageList = this.imageList1;
        this.uiCommandBar1.Key = "CommandBar1";
        this.uiCommandBar1.Location = new System.Drawing.Point(0, 0);
        this.uiCommandBar1.Name = "uiCommandBar1";
        this.uiCommandBar1.RowIndex = 0;
        this.uiCommandBar1.ShowAddRemoveButton = Janus.Windows.UI.InheritableBoolean.False;
        this.uiCommandBar1.ShowCustomizeButton = Janus.Windows.UI.InheritableBoolean.False;
        this.uiCommandBar1.ShowToolTips = Janus.Windows.UI.InheritableBoolean.False;
        this.uiCommandBar1.Size = new System.Drawing.Size(427, 28);
        this.uiCommandBar1.Text = "CommandBar1";
        this.uiCommandBar1.CommandClick += new Janus.Windows.UI.CommandBars.CommandEventHandler(this.uiCommandBar1_CommandClick);
        // 
        // Command01
        // 
        this.Command01.ImageIndex = 0;
        this.Command01.Key = "Command0";
        this.Command01.Name = "Command01";
        // 
        // Command11
        // 
        this.Command11.ImageIndex = 1;
        this.Command11.Key = "Command1";
        this.Command11.Name = "Command11";
        // 
        // Command21
        // 
        this.Command21.ImageIndex = 2;
        this.Command21.Key = "Command2";
        this.Command21.Name = "Command21";
        // 
        // Command31
        // 
        this.Command31.Key = "Command3";
        this.Command31.Name = "Command31";
        this.Command31.ShowTextInContainers = Janus.Windows.UI.InheritableBoolean.True;
        // 
        // Command51
        // 
        this.Command51.Key = "Command5";
        this.Command51.Name = "Command51";
        // 
        // imageList1
        // 
        this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
        this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
        this.imageList1.Images.SetKeyName(0, "NavigateForward.png");
        this.imageList1.Images.SetKeyName(1, "NoAccess.png");
        this.imageList1.Images.SetKeyName(2, "Delete.png");
        // 
        // Command0
        // 
        this.Command0.Key = "Command0";
        this.Command0.Name = "Command0";
        this.Command0.Text = "Start";
        // 
        // Command1
        // 
        this.Command1.Key = "Command1";
        this.Command1.Name = "Command1";
        this.Command1.Text = "Stop";
        // 
        // Command2
        // 
        this.Command2.Key = "Command2";
        this.Command2.Name = "Command2";
        this.Command2.Text = "Clear Files";
        // 
        // Command3
        // 
        this.Command3.Commands.AddRange(new Janus.Windows.UI.CommandBars.UICommand[] {
            this.Command41,
            this.Command42,
            this.Command43,
            this.Command44,
            this.Command45,
            this.Command46,
            this.Command47,
            this.Command48,
            this.Command49,
            this.Command410});
        this.Command3.IsEditableControl = Janus.Windows.UI.InheritableBoolean.True;
        this.Command3.Key = "Command3";
        this.Command3.Name = "Command3";
        this.Command3.Text = "Opacity";
        // 
        // Command41
        // 
        this.Command41.Key = "Command4";
        this.Command41.Name = "Command41";
        // 
        // Command42
        // 
        this.Command42.Key = "Command4";
        this.Command42.Name = "Command42";
        this.Command42.Text = "20%";
        // 
        // Command43
        // 
        this.Command43.Key = "Command4";
        this.Command43.Name = "Command43";
        this.Command43.Text = "30%";
        // 
        // Command44
        // 
        this.Command44.Key = "Command4";
        this.Command44.Name = "Command44";
        this.Command44.Text = "40%";
        // 
        // Command45
        // 
        this.Command45.Key = "Command4";
        this.Command45.Name = "Command45";
        this.Command45.Text = "50%";
        // 
        // Command46
        // 
        this.Command46.Key = "Command4";
        this.Command46.Name = "Command46";
        this.Command46.Text = "60%";
        // 
        // Command47
        // 
        this.Command47.Key = "Command4";
        this.Command47.Name = "Command47";
        this.Command47.Text = "70%";
        // 
        // Command48
        // 
        this.Command48.Key = "Command4";
        this.Command48.Name = "Command48";
        this.Command48.Text = "80%";
        // 
        // Command49
        // 
        this.Command49.Key = "Command4";
        this.Command49.Name = "Command49";
        this.Command49.Text = "90%";
        // 
        // Command410
        // 
        this.Command410.Key = "Command4";
        this.Command410.Name = "Command410";
        this.Command410.Text = "100%";
        // 
        // Command4
        // 
        this.Command4.Key = "Command4";
        this.Command4.Name = "Command4";
        this.Command4.Text = "10%";
        // 
        // toolStripDropDownButton1
        // 
        this.toolStripDropDownButton1.CommandType = Janus.Windows.UI.CommandBars.CommandType.Label;
        this.toolStripDropDownButton1.Enabled = Janus.Windows.UI.InheritableBoolean.False;
        this.toolStripDropDownButton1.Key = "Command5";
        this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
        this.toolStripDropDownButton1.Text = "Ready";
        // 
        // LeftRebar1
        // 
        this.LeftRebar1.CommandManager = this.uiCommandManager1;
        this.LeftRebar1.Dock = System.Windows.Forms.DockStyle.Left;
        this.LeftRebar1.Location = new System.Drawing.Point(0, 28);
        this.LeftRebar1.Name = "LeftRebar1";
        this.LeftRebar1.Size = new System.Drawing.Size(0, 296);
        // 
        // RightRebar1
        // 
        this.RightRebar1.CommandManager = this.uiCommandManager1;
        this.RightRebar1.Dock = System.Windows.Forms.DockStyle.Right;
        this.RightRebar1.Location = new System.Drawing.Point(427, 28);
        this.RightRebar1.Name = "RightRebar1";
        this.RightRebar1.Size = new System.Drawing.Size(0, 296);
        // 
        // TopRebar1
        // 
        this.TopRebar1.CommandBars.AddRange(new Janus.Windows.UI.CommandBars.UICommandBar[] {
            this.uiCommandBar1});
        this.TopRebar1.CommandManager = this.uiCommandManager1;
        this.TopRebar1.Controls.Add(this.uiCommandBar1);
        this.TopRebar1.Dock = System.Windows.Forms.DockStyle.Top;
        this.TopRebar1.Location = new System.Drawing.Point(0, 0);
        this.TopRebar1.Name = "TopRebar1";
        this.TopRebar1.Size = new System.Drawing.Size(427, 28);
        // 
        // NuGenPathWatcherMainForm
        // 
        this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
        this.ClientSize = new System.Drawing.Size(427, 324);
        this.Controls.Add(this.uiTab1);
        this.Controls.Add(this.TopRebar1);
        this.Cursor = System.Windows.Forms.Cursors.Default;
        this.DoubleBuffered = true;
        this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
        this.MaximizeBox = false;
        this.Name = "NuGenPathWatcherMainForm";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "Path Watcher";
        this.Load += new System.EventHandler(this.Form1_Load);
        ((System.ComponentModel.ISupportInitialize)(this.uiTab1)).EndInit();
        this.uiTab1.ResumeLayout(false);
        this.uiTabPage1.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)(this.uiCommandManager1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.BottomRebar1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.uiCommandBar1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.LeftRebar1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.RightRebar1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.TopRebar1)).EndInit();
        this.TopRebar1.ResumeLayout(false);
        this.ResumeLayout(false);

	}
		#endregion

  	private void uIButton1_Click(object sender, System.EventArgs e)
	  {
		if (FileWatch.EnableRaisingEvents == true) 
		{
			return;
		}

		listBoxFiles.Items.Clear();
		//      FileWatch.Filter = textBoxWatchFilter.Text;
		FileWatch.NotifyFilter =  NotifyFilters.FileName    | NotifyFilters.Attributes |
			NotifyFilters.LastAccess  | NotifyFilters.LastWrite  | 
			NotifyFilters.Security    | NotifyFilters.Size;

		FileWatch.Changed += new FileSystemEventHandler(OnFileEvent);
		FileWatch.Created += new FileSystemEventHandler(OnFileEvent);  
		FileWatch.Deleted += new FileSystemEventHandler(OnFileEvent);
		FileWatch.Renamed += new RenamedEventHandler(OnRenameEvent);
		FileWatch.IncludeSubdirectories = true;
		FileWatch.EnableRaisingEvents   = true;
	  }

  	private void uIButton2_Click(object sender, System.EventArgs e)

		{
			FileWatch.EnableRaisingEvents = false;		//Stop looking
		}

  	private void uICheckBox1_CheckedChanged(object sender, System.EventArgs e)
	  {
		this.TopMost = this.uICheckBox1.Checked;
	  }

  	private void uIButton3_Click(object sender, System.EventArgs e)
	  {
		this.Close();
		Application.Exit();
	  }

      private void uiButton3_Click_1(object sender, EventArgs e)
      {
          this.Close();
          Application.Exit();
      }

      private void toolStripButton1_Click(object sender, EventArgs e)
      {

    
          if (FileWatch.EnableRaisingEvents == true)
          {
              return;
          }
          
          listBoxFiles.Items.Clear();
          //      FileWatch.Filter = textBoxWatchFilter.Text;
          FileWatch.NotifyFilter = NotifyFilters.FileName | NotifyFilters.Attributes |
              NotifyFilters.LastAccess | NotifyFilters.LastWrite |
              NotifyFilters.Security | NotifyFilters.Size;
          FileWatch.IncludeSubdirectories = true;
          
          FileWatch.Changed += new FileSystemEventHandler(OnFileEvent);
          FileWatch.Created += new FileSystemEventHandler(OnFileEvent);
          FileWatch.Deleted += new FileSystemEventHandler(OnFileEvent);
          FileWatch.Renamed += new RenamedEventHandler(OnRenameEvent);
          FileWatch.IncludeSubdirectories = true;
          FileWatch.EnableRaisingEvents = true;
          uiCommandBar1.Text = "Watching " + FileWatch.Path;
          comboBoxFwatchDrive.Enabled = false;
          uIButton1.Enabled = false;
      }

      private void toolStripButton2_Click(object sender, EventArgs e)
      {
          FileWatch.EnableRaisingEvents = false;		//Stop looking
          uiCommandBar1.Text = "Ready";
          comboBoxFwatchDrive.Enabled = true;
          uIButton1.Enabled = true;
      }

      private void toolStripButton3_Click(object sender, EventArgs e)
      {
          this.Close();
      }

      private void uIButton1_Click_1(object sender, EventArgs e)
      {
          string flname;
          if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
          {
              flname = this.folderBrowserDialog1.SelectedPath;
              this.comboBoxFwatchDrive.Text = flname;
              flname = this.folderBrowserDialog1.SelectedPath;
              FileWatch.Path = flname;              
          }
          
      }

      private void toolStripButton4_Click(object sender, EventArgs e)
      {
          listBoxFiles.Items.Clear();
      }

      private void toolStripMenuItem2_Click(object sender, EventArgs e)
      {
          this.Opacity = 1;
          Application.DoEvents();
      }

      private void toolStripMenuItem3_Click(object sender, EventArgs e)
      {
          this.Opacity = .9;
          Application.DoEvents();
      }

      private void toolStripMenuItem4_Click(object sender, EventArgs e)
      {
          this.Opacity = .8;
          Application.DoEvents();
      }

      private void toolStripMenuItem5_Click(object sender, EventArgs e)
      {
          this.Opacity = .7;
          Application.DoEvents();
      }

      private void toolStripMenuItem6_Click(object sender, EventArgs e)
      {
          this.Opacity = .6;
          Application.DoEvents();
      }

      private void toolStripMenuItem7_Click(object sender, EventArgs e)
      {
          this.Opacity = .5;
          Application.DoEvents();
      }

      private void toolStripMenuItem8_Click(object sender, EventArgs e)
      {
          this.Opacity = .4;
          Application.DoEvents();
      }

      private void toolStripMenuItem9_Click(object sender, EventArgs e)
      {
          this.Opacity = .3;
          Application.DoEvents();
      }

      private void toolStripMenuItem10_Click(object sender, EventArgs e)
      {
          this.Opacity = .2;
          Application.DoEvents();
      }

      private void toolStripMenuItem11_Click(object sender, EventArgs e)
      {
          this.Opacity = .1;
          Application.DoEvents();
      }

      private void uiCommandBar1_CommandClick(object sender, Janus.Windows.UI.CommandBars.CommandEventArgs e)
      {
          switch (e.Command.Text)
          {
              case "Start":
                  toolStripButton1_Click(sender, null); break;
              case "Stop":
                  toolStripButton2_Click(sender, null); break;
              case "Clear Files":
                  toolStripButton4_Click(sender, null); break;
              default:
                  try
                  {
                      this.Opacity = .01 * Int32.Parse(e.Command.Text.Replace("%", ""));
                  }
                  catch (Exception) { } break;

          }
      }
  }          
}





