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

/*
 public void SocketServerCallBackData(string Data)
		{
			AddDataRemoteClipBoard(Data);
		}

		public void SocketServerCallBackError(string ErrorMsg)
		{
			TexBoxRemoteClip.AppendText(ErrorMsg+"\r\n");
		}

		public  void StartSocketServer()
		{
			SocServer = new SocketServer();
			SocServer.PortNr=8221;
			SocServer.ServerCallback=new ServerCallBack(SocketServerCallBackData);
			SocServer.ServerCallbackError=new ServerCallBack(SocketServerCallBackError);
			SocServer.StartServer();
		}

		public void StopSocketServer()
		{
			SocServer.StopServer();
		}
 * 
 * */
namespace Genetibase.Debug
{
	/// <summary>
	/// Summary description for SocketServer.
	/// </summary>
	///
	
	public delegate void ServerCallBack(string RxString);
	
	public class SocketServer
	{
		private Socket SocketListener;
		private Socket SocketWorker;
		private int iPortNr;
		private AsyncCallback pfnWorkerCallBack;
		private long AnzTxDat;
		private long AnzRxDat;

		
		private ServerCallBack pfServerReceivedData;
		private ServerCallBack pfServerErrorHandler;

		public SocketServer(/*int PortNr,ServerCallBack RxServerCB*/)
		{
			iPortNr =8221;// PortNr;
			pfServerReceivedData=null;
			AnzTxDat = 0;
			AnzRxDat = 0;
		}
		public void StartServer()
		{
			try
			{
				//create the listening socket...
				SocketListener = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);		
				IPEndPoint ipLocal = new IPEndPoint ( IPAddress.Any ,iPortNr);
				//bind to local IP Address...
				SocketListener.Bind( ipLocal );
				//start listening...
				SocketListener.Listen (100);
				// create the call back for any client connections...
				SocketListener.BeginAccept(new AsyncCallback ( OnClientConnect ),null);
			}
			catch(Exception se)
			{
				CallErrorHandler ( se.Message );
			}
		}
		public void StopServer()
		{
			try
			{
				if ( SocketListener != null )
				{
					SocketListener.Close ();
					SocketListener = null;
				}

				if ( SocketWorker != null )
				{
					SocketWorker.Close();
					SocketWorker = null;
				}
			}
			catch(Exception se)
			{
				CallErrorHandler(se.Message );
			}
		}

		public void SendData(string TxData)
		{
			try
			{
				Object objData = TxData;
				byte[] byData = System.Text.Encoding.ASCII.GetBytes(objData.ToString ());
				if(SocketWorker!=null)
				{
					AnzTxDat+= TxData.Length;
					SocketWorker.Send (byData);
				}
			}
			catch(Exception se)
			{
				CallErrorHandler(se.Message);
			}
		}

		public int PortNr 
		{
			get {return iPortNr;}
			set {iPortNr=value;}
		}

		public long NoTxDat
		{
			get {return AnzTxDat;}
		}
		
		public long NoRxDat
		{
			get {return AnzRxDat;}
		}

		public ServerCallBack ServerCallback 
		{
			set {pfServerReceivedData=value;}
		}

		public ServerCallBack ServerCallbackError
		{
			set {pfServerErrorHandler=value;}
		}

		private void CallErrorHandler(string Msg)
		{
			if (pfServerErrorHandler!=null)
			{
				pfServerErrorHandler(Msg);
			}
		}

		private void OnClientConnect(IAsyncResult asyn)
		{
			if(SocketListener == null )
			{
				return;
			}
			try
			{
				SocketWorker = SocketListener.EndAccept (asyn);
				WaitForDataServerRx(SocketWorker);
			}
			catch(Exception se)
			{
				CallErrorHandler(se.Message);
			}
		}

		private void WaitForDataServerRx(System.Net.Sockets.Socket soc)
		{
			try
			{
				if  ( pfnWorkerCallBack == null ) 
				{
					pfnWorkerCallBack = new AsyncCallback (OnDataReceivedServer);
				}
				CSocketPacket theSocPkt = new CSocketPacket ();
				theSocPkt.thisSocket = soc;
				// now start to listen for any data...
				soc .BeginReceive (theSocPkt.dataBuffer ,0,theSocPkt.dataBuffer.Length ,SocketFlags.None,pfnWorkerCallBack,theSocPkt);
			}
			catch(Exception se)
			{
				CallErrorHandler(se.Message);
			}
		}
		private  void OnDataReceivedServer(IAsyncResult asyn)
		{
			if(SocketListener == null)
			{
				return;
			}
			try
			{
				CSocketPacket theSockId = (CSocketPacket)asyn.AsyncState ;
				//end receive...
				int iRx  = 0 ;
				iRx = theSockId.thisSocket.EndReceive (asyn);
				char[] chars = new char[iRx +  1];
				System.Text.Decoder d = System.Text.Encoding.UTF8.GetDecoder();
				int charLen = d.GetChars(theSockId.dataBuffer, 0, iRx, chars, 0);
				System.String szData = new System.String(chars);

				if(pfServerReceivedData != null)
				{
                    AnzRxDat+=szData.Length-1;
					pfServerReceivedData(szData);
				}
	
				WaitForDataServerRx(SocketWorker );
			}
			catch(Exception se)
			{
				CallErrorHandler(se.Message);
			}
		}
		
		private class CSocketPacket
		{
			public System.Net.Sockets.Socket thisSocket;
			public byte[] dataBuffer = new byte[1024];
		}
	}
}

