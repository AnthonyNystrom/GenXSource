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
  	private DevComponents.DotNetBar.PanelEx panelEx1;
  	private DevComponents.DotNetBar.PanelEx panelEx2;
  	private Janus.Windows.EditControls.UIButton uIButton1;
  	private Janus.Windows.EditControls.UIButton uIButton2;
  	private Janus.Windows.EditControls.UICheckBox uICheckBox1;
  	private Janus.Windows.EditControls.UIButton uIButton3;
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
      string[] str=Directory.GetLogicalDrives();
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
    }

    public  void OnRenameEvent(Object source, RenamedEventArgs rea) 
    {
      DateTime dt = new DateTime();
      dt = System.DateTime.UtcNow;
      listBoxFiles.Items.Add(listBoxFiles.Items.Count.ToString() + " " +dt.ToLocalTime() + " " + rea.ChangeType.ToString() + rea.OldFullPath+ "  to " +" " + rea.FullPath);
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
      FileWatch.Path=comboBoxFwatchDrive.Text;
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
		this.timer1 = new System.Windows.Forms.Timer(this.components);
		this.label8 = new System.Windows.Forms.Label();
		this.comboBoxFwatchDrive = new System.Windows.Forms.ComboBox();
		this.listBoxFiles = new System.Windows.Forms.ListBox();
		this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
		this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
		this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
		this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
		this.panelEx2 = new DevComponents.DotNetBar.PanelEx();
		this.uICheckBox1 = new Janus.Windows.EditControls.UICheckBox();
		this.uIButton1 = new Janus.Windows.EditControls.UIButton();
		this.uIButton2 = new Janus.Windows.EditControls.UIButton();
		this.uIButton3 = new Janus.Windows.EditControls.UIButton();
		this.panelEx1.SuspendLayout();
		this.panelEx2.SuspendLayout();
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
		this.label8.Location = new System.Drawing.Point(8, 32);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(36, 24);
		this.label8.TabIndex = 6;
		this.label8.Text = "Drive:";
		this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		// 
		// comboBoxFwatchDrive
		// 
		this.comboBoxFwatchDrive.BackColor = System.Drawing.Color.White;
		this.comboBoxFwatchDrive.Location = new System.Drawing.Point(48, 32);
		this.comboBoxFwatchDrive.Name = "comboBoxFwatchDrive";
		this.comboBoxFwatchDrive.Size = new System.Drawing.Size(64, 21);
		this.comboBoxFwatchDrive.TabIndex = 5;
		this.comboBoxFwatchDrive.Text = "C:\\";
		this.comboBoxFwatchDrive.SelectedIndexChanged += new System.EventHandler(this.comboBoxFwatchDrive_SelectedIndexChanged);
		this.comboBoxFwatchDrive.Enter += new System.EventHandler(this.comboBoxFwatchDrive_Enter);
		// 
		// listBoxFiles
		// 
		this.listBoxFiles.BackColor = System.Drawing.Color.White;
		this.listBoxFiles.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.listBoxFiles.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.listBoxFiles.HorizontalScrollbar = true;
		this.listBoxFiles.Location = new System.Drawing.Point(0, 69);
		this.listBoxFiles.Name = "listBoxFiles";
		this.listBoxFiles.Size = new System.Drawing.Size(632, 377);
		this.listBoxFiles.TabIndex = 3;
		// 
		// saveFileDialog1
		// 
		this.saveFileDialog1.OverwritePrompt = false;
		this.saveFileDialog1.Title = "Choose a File ...";
		// 
		// panelEx1
		// 
		this.panelEx1.Controls.Add(this.panelEx2);
		this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panelEx1.Location = new System.Drawing.Point(0, 0);
		this.panelEx1.Name = "panelEx1";
		this.panelEx1.Size = new System.Drawing.Size(632, 446);
		this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
		this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
		this.panelEx1.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
		this.panelEx1.Style.BackgroundImagePosition = DevComponents.DotNetBar.eBackgroundImagePosition.Tile;
		this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
		this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
		this.panelEx1.Style.GradientAngle = 90;
		this.panelEx1.TabIndex = 13;
		// 
		// panelEx2
		// 
		this.panelEx2.Controls.Add(this.label8);
		this.panelEx2.Controls.Add(this.comboBoxFwatchDrive);
		this.panelEx2.Controls.Add(this.uICheckBox1);
		this.panelEx2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panelEx2.Location = new System.Drawing.Point(0, 0);
		this.panelEx2.Name = "panelEx2";
		this.panelEx2.Size = new System.Drawing.Size(632, 68);
		this.panelEx2.Style.Alignment = System.Drawing.StringAlignment.Center;
		this.panelEx2.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground;
		this.panelEx2.Style.BackColor2.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarBackground2;
		this.panelEx2.Style.BackgroundImagePosition = DevComponents.DotNetBar.eBackgroundImagePosition.Tile;
		this.panelEx2.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.BarDockedBorder;
		this.panelEx2.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.ItemText;
		this.panelEx2.Style.GradientAngle = 90;
		this.panelEx2.TabIndex = 4;
		// 
		// uICheckBox1
		// 
		this.uICheckBox1.BackColor = System.Drawing.Color.Transparent;
		this.uICheckBox1.Location = new System.Drawing.Point(4, 8);
		this.uICheckBox1.Name = "uICheckBox1";
		this.uICheckBox1.Size = new System.Drawing.Size(108, 20);
		this.uICheckBox1.TabIndex = 10;
		this.uICheckBox1.Text = "Always On Top";
		this.uICheckBox1.CheckedChanged += new System.EventHandler(this.uICheckBox1_CheckedChanged);
		// 
		// uIButton1
		// 
		this.uIButton1.BackColor = System.Drawing.SystemColors.Control;
		this.uIButton1.Location = new System.Drawing.Point(116, 4);
		this.uIButton1.Name = "uIButton1";
		this.uIButton1.Size = new System.Drawing.Size(76, 28);
		this.uIButton1.TabIndex = 8;
		this.uIButton1.Text = "Start";
		this.uIButton1.Click += new System.EventHandler(this.uIButton1_Click);
		// 
		// uIButton2
		// 
		this.uIButton2.BackColor = System.Drawing.SystemColors.Control;
		this.uIButton2.Location = new System.Drawing.Point(116, 36);
		this.uIButton2.Name = "uIButton2";
		this.uIButton2.Size = new System.Drawing.Size(76, 28);
		this.uIButton2.TabIndex = 9;
		this.uIButton2.Text = "Stop";
		this.uIButton2.Click += new System.EventHandler(this.uIButton2_Click);
		// 
		// uIButton3
		// 
		this.uIButton3.BackColor = System.Drawing.SystemColors.Control;
		this.uIButton3.Location = new System.Drawing.Point(552, 36);
		this.uIButton3.Name = "uIButton3";
		this.uIButton3.Size = new System.Drawing.Size(76, 28);
		this.uIButton3.TabIndex = 11;
		this.uIButton3.Text = "Close";
		this.uIButton3.Click += new System.EventHandler(this.uIButton3_Click);
		// 
		// Form1
		// 
		this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
		this.ClientSize = new System.Drawing.Size(632, 446);
		this.ControlBox = false;
		this.Controls.Add(this.uIButton3);
		this.Controls.Add(this.listBoxFiles);
		this.Controls.Add(this.uIButton2);
		this.Controls.Add(this.uIButton1);
		this.Controls.Add(this.panelEx1);
		this.Cursor = System.Windows.Forms.Cursors.Hand;
		this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
		this.MaximizeBox = false;
		this.Name = "Form1";
		this.Text = "NuGenPathWatcher";
		this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
		this.Load += new System.EventHandler(this.Form1_Load);
		this.panelEx1.ResumeLayout(false);
		this.panelEx2.ResumeLayout(false);
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
	

     

  }          
}





