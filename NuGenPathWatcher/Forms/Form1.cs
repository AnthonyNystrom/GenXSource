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
  public class Form1 : System.Windows.Forms.Form
  {
    #region Private Variables
    private FileSystemWatcher FileWatch;
//    private SocketServer SocServer;
////    private SocketClient SocClient;
//    private string ChoosedFileName="";
//    private bool bSocServerIsRunning=false;
//    private bool bSocClientIsRunning=false;
//    private string LastClipBoard;
    private Process myCmdProcess;
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
    private System.Windows.Forms.ComboBox comboBoxFwatchDrive;
      private System.Windows.Forms.ToolTip toolTip1;
      private DevComponents.DotNetBar.PanelEx panelEx2;
      private Janus.Windows.EditControls.UICheckBox uICheckBox1;
      private ToolStrip toolStrip1;
      private ToolStripButton toolStripButton1;
      private ToolStripButton toolStripButton2;
      private ToolStripButton toolStripButton3;
      private Janus.Windows.EditControls.UIButton uIButton1;
      private FolderBrowserDialog folderBrowserDialog1;
      private ToolStripButton toolStripButton4;
      private ToolStripLabel toolStripLabel1;
      private ToolStripDropDownButton toolStripDropDownButton1;
      private ToolStripMenuItem toolStripMenuItem2;
      private ToolStripMenuItem toolStripMenuItem3;
      private ToolStripMenuItem toolStripMenuItem4;
      private ToolStripMenuItem toolStripMenuItem5;
      private ToolStripMenuItem toolStripMenuItem6;
      private ToolStripMenuItem toolStripMenuItem7;
      private ToolStripMenuItem toolStripMenuItem8;
      private ToolStripMenuItem toolStripMenuItem9;
      private ToolStripMenuItem toolStripMenuItem10;
      private ToolStripMenuItem toolStripMenuItem11;
    private System.Windows.Forms.Label label8;
    #endregion
    
    #region Main Form Code
    public Form1()
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

      Application.Run(new Form1());
    }
    private void Form1_Load(object sender, System.EventArgs e)
    {
    
    }
//    private void uicheckBox1_CheckedChanged(object sender, System.EventArgs e)
//    {
//      this.TopMost = uicheckBox.Checked;
//    }

    private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      e.Cancel=true;
      this.Visible=false;
//      notifyIcon1.Visible=true;
    }
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

   
    private void buttonSysInfo_Click(object sender, System.EventArgs e)
    {
      myCmdProcess.StartInfo.FileName               = "cmd";
      //myCmdProcess.StartInfo.WorkingDirectory       = (string)ComboBoxDrive.SelectedItem+"\\";//"t:\\";
      myCmdProcess.StartInfo.Arguments              = "/C  systeminfo> test.txt";
      myCmdProcess.StartInfo.RedirectStandardOutput = true;
      myCmdProcess.StartInfo.UseShellExecute        = false;
      myCmdProcess.StartInfo.CreateNoWindow         = true;
      myCmdProcess.EnableRaisingEvents               =true;

//      try 
//      {
//        textBoxCmdOutput.Text="Show SysInfo... \r\n";
//        textBoxCmdOutput.Update();
//        myCmdProcess.Start();
//				
//      }
//      catch(Exception excpt)
//      {
//        textBoxCmdOutput.Text    = excpt.Message; 
//      }
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
      comboBoxFwatchDrive.Items.AddRange(str);
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

    public void OnFileEvent(object source, FileSystemEventArgs fsea) 
    {
      DateTime dt = new DateTime();
      dt = System.DateTime.UtcNow;
      FileInfo f = new FileInfo(fsea.FullPath);
      listBoxFiles.Items.Add(listBoxFiles.Items.Count.ToString() + " " +dt.ToLocalTime() + " " + fsea.ChangeType.ToString() + " " + fsea.FullPath + " " + f.Attributes.ToString());
      listBoxFiles.SelectedIndex = listBoxFiles.Items.Count - 1;
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
        this.timer1 = new System.Windows.Forms.Timer(this.components);
        this.label8 = new System.Windows.Forms.Label();
        this.comboBoxFwatchDrive = new System.Windows.Forms.ComboBox();
        this.listBoxFiles = new System.Windows.Forms.ListBox();
        this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
        this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
        this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
        this.panelEx2 = new DevComponents.DotNetBar.PanelEx();
        this.uIButton1 = new Janus.Windows.EditControls.UIButton();
        this.uICheckBox1 = new Janus.Windows.EditControls.UICheckBox();
        this.toolStrip1 = new System.Windows.Forms.ToolStrip();
        this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
        this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
        this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
        this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
        this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
        this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
        this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
        this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
        this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
        this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
        this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
        this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
        this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
        this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
        this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripMenuItem();
        this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripMenuItem();
        this.toolStripMenuItem11 = new System.Windows.Forms.ToolStripMenuItem();
        this.panelEx2.SuspendLayout();
        this.toolStrip1.SuspendLayout();
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
        this.label8.Location = new System.Drawing.Point(103, 7);
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
        this.comboBoxFwatchDrive.Location = new System.Drawing.Point(146, 7);
        this.comboBoxFwatchDrive.Name = "comboBoxFwatchDrive";
        this.comboBoxFwatchDrive.Size = new System.Drawing.Size(614, 21);
        this.comboBoxFwatchDrive.TabIndex = 5;
        this.comboBoxFwatchDrive.Text = "C:\\";
        this.comboBoxFwatchDrive.Enter += new System.EventHandler(this.comboBoxFwatchDrive_Enter);
        this.comboBoxFwatchDrive.SelectedIndexChanged += new System.EventHandler(this.comboBoxFwatchDrive_SelectedIndexChanged);
        // 
        // listBoxFiles
        // 
        this.listBoxFiles.BackColor = System.Drawing.Color.White;
        this.listBoxFiles.BorderStyle = System.Windows.Forms.BorderStyle.None;
        this.listBoxFiles.Dock = System.Windows.Forms.DockStyle.Fill;
        this.listBoxFiles.HorizontalScrollbar = true;
        this.listBoxFiles.Location = new System.Drawing.Point(0, 59);
        this.listBoxFiles.Name = "listBoxFiles";
        this.listBoxFiles.Size = new System.Drawing.Size(792, 533);
        this.listBoxFiles.TabIndex = 3;
        // 
        // saveFileDialog1
        // 
        this.saveFileDialog1.OverwritePrompt = false;
        this.saveFileDialog1.Title = "Choose a File ...";
        // 
        // panelEx2
        // 
        this.panelEx2.ColorScheme.BarBackground = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
        this.panelEx2.ColorScheme.BarBackground2 = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(206)))), ((int)(((byte)(254)))));
        this.panelEx2.ColorScheme.BarCaptionBackground = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
        this.panelEx2.ColorScheme.BarCaptionBackground2 = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(100)))), ((int)(((byte)(160)))));
        this.panelEx2.ColorScheme.BarCaptionInactiveBackground = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
        this.panelEx2.ColorScheme.BarCaptionInactiveBackground2 = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(210)))), ((int)(((byte)(255)))));
        this.panelEx2.ColorScheme.BarCaptionInactiveText = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(55)))), ((int)(((byte)(114)))));
        this.panelEx2.ColorScheme.BarCaptionText = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
        this.panelEx2.ColorScheme.BarDockedBorder = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(157)))), ((int)(((byte)(217)))));
        this.panelEx2.ColorScheme.BarFloatingBorder = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(100)))), ((int)(((byte)(160)))));
        this.panelEx2.ColorScheme.BarPopupBackground = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
        this.panelEx2.ColorScheme.BarPopupBorder = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
        this.panelEx2.ColorScheme.BarStripeColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(157)))), ((int)(((byte)(217)))));
        this.panelEx2.ColorScheme.CustomizeBackground = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(232)))), ((int)(((byte)(255)))));
        this.panelEx2.ColorScheme.CustomizeBackground2 = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(157)))), ((int)(((byte)(217)))));
        this.panelEx2.ColorScheme.DockSiteBackColorGradientAngle = 0;
        this.panelEx2.ColorScheme.ItemCheckedBackground = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(213)))), ((int)(((byte)(120)))));
        this.panelEx2.ColorScheme.ItemCheckedBackground2 = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(200)))), ((int)(((byte)(79)))));
        this.panelEx2.ColorScheme.ItemCheckedBorder = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(85)))), ((int)(((byte)(3)))));
        this.panelEx2.ColorScheme.ItemDesignTimeBorder = System.Drawing.Color.Black;
        this.panelEx2.ColorScheme.ItemDisabledText = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
        this.panelEx2.ColorScheme.ItemExpandedBackground = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(239)))), ((int)(((byte)(254)))));
        this.panelEx2.ColorScheme.ItemExpandedBorder = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
        this.panelEx2.ColorScheme.ItemHotBackground = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(245)))), ((int)(((byte)(204)))));
        this.panelEx2.ColorScheme.ItemHotBackground2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(219)))), ((int)(((byte)(117)))));
        this.panelEx2.ColorScheme.ItemHotBorder = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(189)))), ((int)(((byte)(105)))));
        this.panelEx2.ColorScheme.ItemPressedBackground = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(151)))), ((int)(((byte)(61)))));
        this.panelEx2.ColorScheme.ItemPressedBackground2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(184)))), ((int)(((byte)(94)))));
        this.panelEx2.ColorScheme.ItemPressedBorder = System.Drawing.Color.FromArgb(((int)(((byte)(251)))), ((int)(((byte)(140)))), ((int)(((byte)(60)))));
        this.panelEx2.ColorScheme.ItemSeparator = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(154)))), ((int)(((byte)(198)))), ((int)(((byte)(255)))));
        this.panelEx2.ColorScheme.ItemSeparatorShade = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
        this.panelEx2.ColorScheme.MenuBackground = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
        this.panelEx2.ColorScheme.MenuBarBackground = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
        this.panelEx2.ColorScheme.MenuBorder = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(147)))), ((int)(((byte)(207)))));
        this.panelEx2.ColorScheme.MenuSide = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
        this.panelEx2.ColorScheme.MenuUnusedBackground = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(246)))), ((int)(((byte)(246)))));
        this.panelEx2.ColorScheme.MenuUnusedSide = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(218)))), ((int)(((byte)(218)))));
        this.panelEx2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
        this.panelEx2.Controls.Add(this.uIButton1);
        this.panelEx2.Controls.Add(this.label8);
        this.panelEx2.Controls.Add(this.comboBoxFwatchDrive);
        this.panelEx2.Controls.Add(this.uICheckBox1);
        this.panelEx2.Dock = System.Windows.Forms.DockStyle.Top;
        this.panelEx2.Location = new System.Drawing.Point(0, 25);
        this.panelEx2.Name = "panelEx2";
        this.panelEx2.Size = new System.Drawing.Size(792, 34);
        this.panelEx2.Style.Alignment = System.Drawing.StringAlignment.Center;
        this.panelEx2.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
        this.panelEx2.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
        this.panelEx2.Style.BackgroundImagePosition = DevComponents.DotNetBar.eBackgroundImagePosition.Tile;
        this.panelEx2.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
        this.panelEx2.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
        this.panelEx2.Style.GradientAngle = 90;
        this.panelEx2.TabIndex = 4;
        // 
        // uIButton1
        // 
        this.uIButton1.ButtonStyle = Janus.Windows.EditControls.ButtonStyle.Ellipsis;
        this.uIButton1.Dock = System.Windows.Forms.DockStyle.Right;
        this.uIButton1.Location = new System.Drawing.Point(766, 0);
        this.uIButton1.Name = "uIButton1";
        this.uIButton1.Size = new System.Drawing.Size(26, 34);
        this.uIButton1.TabIndex = 11;
        this.uIButton1.UseThemes = false;
        this.uIButton1.VisualStyle = Janus.Windows.UI.VisualStyle.VS2005;
        this.uIButton1.Click += new System.EventHandler(this.uIButton1_Click_1);
        // 
        // uICheckBox1
        // 
        this.uICheckBox1.BackColor = System.Drawing.Color.Transparent;
        this.uICheckBox1.Location = new System.Drawing.Point(3, 7);
        this.uICheckBox1.Name = "uICheckBox1";
        this.uICheckBox1.Size = new System.Drawing.Size(103, 20);
        this.uICheckBox1.TabIndex = 10;
        this.uICheckBox1.Text = "Always On Top";
        this.uICheckBox1.UseThemes = false;
        this.uICheckBox1.VisualStyle = Janus.Windows.UI.VisualStyle.VS2005;
        this.uICheckBox1.CheckedChanged += new System.EventHandler(this.uICheckBox1_CheckedChanged);
        // 
        // toolStrip1
        // 
        this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButton4,
            this.toolStripDropDownButton1,
            this.toolStripButton3,
            this.toolStripLabel1});
        this.toolStrip1.Location = new System.Drawing.Point(0, 0);
        this.toolStrip1.Name = "toolStrip1";
        this.toolStrip1.Size = new System.Drawing.Size(792, 25);
        this.toolStrip1.TabIndex = 6;
        this.toolStrip1.Text = "toolStrip1";
        // 
        // toolStripButton1
        // 
        this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
        this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.toolStripButton1.Name = "toolStripButton1";
        this.toolStripButton1.Size = new System.Drawing.Size(35, 22);
        this.toolStripButton1.Text = "Start";
        this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
        // 
        // toolStripButton2
        // 
        this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
        this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.toolStripButton2.Name = "toolStripButton2";
        this.toolStripButton2.Size = new System.Drawing.Size(33, 22);
        this.toolStripButton2.Text = "Stop";
        this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
        // 
        // toolStripButton3
        // 
        this.toolStripButton3.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
        this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
        this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.toolStripButton3.Name = "toolStripButton3";
        this.toolStripButton3.Size = new System.Drawing.Size(29, 22);
        this.toolStripButton3.Text = "Exit";
        this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
        // 
        // toolStripButton4
        // 
        this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
        this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.toolStripButton4.Name = "toolStripButton4";
        this.toolStripButton4.Size = new System.Drawing.Size(60, 22);
        this.toolStripButton4.Text = "Clear Files";
        this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
        // 
        // toolStripLabel1
        // 
        this.toolStripLabel1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        this.toolStripLabel1.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
        this.toolStripLabel1.Name = "toolStripLabel1";
        this.toolStripLabel1.Size = new System.Drawing.Size(38, 22);
        this.toolStripLabel1.Text = "Ready";
        // 
        // toolStripDropDownButton1
        // 
        this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
        this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.toolStripMenuItem5,
            this.toolStripMenuItem6,
            this.toolStripMenuItem7,
            this.toolStripMenuItem8,
            this.toolStripMenuItem9,
            this.toolStripMenuItem10,
            this.toolStripMenuItem11});
        this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
        this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
        this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
        this.toolStripDropDownButton1.Size = new System.Drawing.Size(57, 22);
        this.toolStripDropDownButton1.Text = "Opacity";
        // 
        // toolStripMenuItem2
        // 
        this.toolStripMenuItem2.Name = "toolStripMenuItem2";
        this.toolStripMenuItem2.Size = new System.Drawing.Size(152, 22);
        this.toolStripMenuItem2.Text = "100%";
        this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
        // 
        // toolStripMenuItem3
        // 
        this.toolStripMenuItem3.Name = "toolStripMenuItem3";
        this.toolStripMenuItem3.Size = new System.Drawing.Size(152, 22);
        this.toolStripMenuItem3.Text = "90%";
        this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
        // 
        // toolStripMenuItem4
        // 
        this.toolStripMenuItem4.Name = "toolStripMenuItem4";
        this.toolStripMenuItem4.Size = new System.Drawing.Size(152, 22);
        this.toolStripMenuItem4.Text = "80%";
        this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
        // 
        // toolStripMenuItem5
        // 
        this.toolStripMenuItem5.Name = "toolStripMenuItem5";
        this.toolStripMenuItem5.Size = new System.Drawing.Size(152, 22);
        this.toolStripMenuItem5.Text = "70%";
        this.toolStripMenuItem5.Click += new System.EventHandler(this.toolStripMenuItem5_Click);
        // 
        // toolStripMenuItem6
        // 
        this.toolStripMenuItem6.Name = "toolStripMenuItem6";
        this.toolStripMenuItem6.Size = new System.Drawing.Size(152, 22);
        this.toolStripMenuItem6.Text = "60%";
        this.toolStripMenuItem6.Click += new System.EventHandler(this.toolStripMenuItem6_Click);
        // 
        // toolStripMenuItem7
        // 
        this.toolStripMenuItem7.Name = "toolStripMenuItem7";
        this.toolStripMenuItem7.Size = new System.Drawing.Size(152, 22);
        this.toolStripMenuItem7.Text = "50%";
        this.toolStripMenuItem7.Click += new System.EventHandler(this.toolStripMenuItem7_Click);
        // 
        // toolStripMenuItem8
        // 
        this.toolStripMenuItem8.Name = "toolStripMenuItem8";
        this.toolStripMenuItem8.Size = new System.Drawing.Size(152, 22);
        this.toolStripMenuItem8.Text = "40%";
        this.toolStripMenuItem8.Click += new System.EventHandler(this.toolStripMenuItem8_Click);
        // 
        // toolStripMenuItem9
        // 
        this.toolStripMenuItem9.Name = "toolStripMenuItem9";
        this.toolStripMenuItem9.Size = new System.Drawing.Size(152, 22);
        this.toolStripMenuItem9.Text = "30%";
        this.toolStripMenuItem9.Click += new System.EventHandler(this.toolStripMenuItem9_Click);
        // 
        // toolStripMenuItem10
        // 
        this.toolStripMenuItem10.Name = "toolStripMenuItem10";
        this.toolStripMenuItem10.Size = new System.Drawing.Size(152, 22);
        this.toolStripMenuItem10.Text = "20%";
        this.toolStripMenuItem10.Click += new System.EventHandler(this.toolStripMenuItem10_Click);
        // 
        // toolStripMenuItem11
        // 
        this.toolStripMenuItem11.Name = "toolStripMenuItem11";
        this.toolStripMenuItem11.Size = new System.Drawing.Size(152, 22);
        this.toolStripMenuItem11.Text = "10%";
        this.toolStripMenuItem11.Click += new System.EventHandler(this.toolStripMenuItem11_Click);
        // 
        // Form1
        // 
        this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
        this.ClientSize = new System.Drawing.Size(792, 592);
        this.ControlBox = false;
        this.Controls.Add(this.listBoxFiles);
        this.Controls.Add(this.panelEx2);
        this.Controls.Add(this.toolStrip1);
        this.Cursor = System.Windows.Forms.Cursors.Default;
        this.DoubleBuffered = true;
        this.MinimumSize = new System.Drawing.Size(800, 600);
        this.Name = "Form1";
        this.ShowIcon = false;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "NuGenPathWatcher";
        this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
        this.Load += new System.EventHandler(this.Form1_Load);
        this.panelEx2.ResumeLayout(false);
        this.toolStrip1.ResumeLayout(false);
        this.toolStrip1.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();

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
          toolStripLabel1.Text = "Watching " + FileWatch.Path;
          comboBoxFwatchDrive.Enabled = false;
          uIButton1.Enabled = false;
      }

      private void toolStripButton2_Click(object sender, EventArgs e)
      {
          FileWatch.EnableRaisingEvents = false;		//Stop looking
          toolStripLabel1.Text = "Ready";
          comboBoxFwatchDrive.Enabled = true;
          uIButton1.Enabled = true;
      }

      private void toolStripButton3_Click(object sender, EventArgs e)
      {
          this.Close();
          Application.Exit();
      }

      private void uIButton1_Click_1(object sender, EventArgs e)
      {
          string flname;
          this.folderBrowserDialog1.ShowDialog();
          flname = this.folderBrowserDialog1.SelectedPath;
          this.comboBoxFwatchDrive.Text = flname;
          flname = this.folderBrowserDialog1.SelectedPath;
          FileWatch.Path = flname;
          
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


	

     

  }          
}





