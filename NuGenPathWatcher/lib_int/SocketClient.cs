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

namespace Genetibase.Debug
{
	/// <summary>
	/// Summary description for SocketClient.
	/// </summary>
	/// 
	public delegate void ClientCallBack(string RxString);
//
//	public class SocketClient
//	{
//		private Socket SocClient;
//		private IPAddress Ip;
//		private int iPortNr;
//		private long AnzTxDat;
//		private long AnzRxDat;
//
//		private ClientCallBack pfClientReceivedData;
//		private ClientCallBack pfClientErrorHandler;
//
//		private AsyncCallback pfClientCallBack;
//		
//		IAsyncResult m_asynResult;
//		
//
//		public SocketClient()
//		{
//			AnzTxDat = 0;
//			AnzRxDat = 0;
//			iPortNr  = 8221;
//		}
//
//		public void StartClient()
//		{
//			try
//			{
//				//create the socket instance...
//				SocClient = new Socket (AddressFamily.InterNetwork,SocketType.Stream ,ProtocolType.Tcp );
//				//create the end point 
////				IPEndPoint ipEnd = new IPEndPoint (Ip.Address,iPortNr);
//				//connect to the remote host...
//				SocClient.Connect ( ipEnd );
//				//watch for data ( asynchronously )...
//				WaitForDataClientRx();
//			}
//			catch(Exception se)
//			{
//				CallErrorHandler(se.Message);
//			}
//		}
//
//		public void StopClient()
//		{
//			try
//			{
//				if ( SocClient != null )
//				{
//					SocClient.Close ();
//					SocClient = null;
//				}
//			}
//			catch(Exception se)
//			{
//				CallErrorHandler(se.Message );
//			}
//		}
//
//		public void SendData(string TxData)
//		{
//			try
//			{
//				Object objData = TxData;
//				byte[] byData = System.Text.Encoding.ASCII.GetBytes(objData.ToString ());
//				if(SocClient!=null)
//				{
//					AnzTxDat+= TxData.Length;
//					SocClient.Send (byData);
//				}
//			}
//			catch(Exception se)
//			{
//				CallErrorHandler(se.Message);
//			}
//		}
//
//
//		public long NoTxDat
//		{
//			get {return AnzTxDat;}	
//		}
//		public long NoRxDat
//		{
//		  get  { return AnzRxDat;}
//		}
//		public int PortNr 
//		{
//			get {return iPortNr;}
//			set {iPortNr=value;	}
//		}
//		public ClientCallBack ClientCallbackRxData
//		{
//			set {pfClientReceivedData=value;}
//		}
//
//		public ClientCallBack ServerCallbackError
//		{
//			set {pfClientErrorHandler=value;}
//		}
//
//		public IPAddress IP
//		{
//			set {Ip=value;}
//		}
//		private void CallErrorHandler(string Msg)
//		{
//			if (pfClientErrorHandler!=null)
//			{
//				pfClientErrorHandler(Msg);
//			}
//		}
//
//		
//		private void WaitForDataClientRx()
//		{
//			if(SocClient==null)
//			{
//				return;
//			}
//			try
//			{
//				if  ( pfClientCallBack == null ) 
//				{
//					pfClientCallBack = new AsyncCallback (OnDataReceivedClient);
//				}
//				CSocketPacket theSocPkt = new CSocketPacket ();
//				theSocPkt.thisSocket = SocClient;
//				// now start to listen for any data...
//				m_asynResult = SocClient.BeginReceive (theSocPkt.dataBuffer ,0,theSocPkt.dataBuffer.Length ,SocketFlags.None,pfClientCallBack,theSocPkt);
//			}
//			catch(SocketException se)
//			{
//				CallErrorHandler(se.Message);
//			}
//		}
//		
//		private  void OnDataReceivedClient(IAsyncResult asyn)
//		{
//			if(SocClient==null)
//			{
//				return;
//			}
//			try
//			{
//				CSocketPacket theSockId = (CSocketPacket)asyn.AsyncState ;
//				//end receive...
//				int iRx  = 0 ;
//				iRx = theSockId.thisSocket.EndReceive (asyn);
//				char[] chars = new char[iRx +  1];
//				System.Text.Decoder d = System.Text.Encoding.UTF8.GetDecoder();
//				int charLen = d.GetChars(theSockId.dataBuffer, 0, iRx, chars, 0);
//				System.String szData = new System.String(chars);
//				
//				if(pfClientReceivedData != null)
//				{
//					AnzRxDat+=szData.Length-1;
//					pfClientReceivedData(szData);
//				}
//
//				WaitForDataClientRx();
//			}
//			catch(Exception se)
//			{
//				CallErrorHandler(se.Message);
//			}
//		}
//
//		private class CSocketPacket
//		{
//			public System.Net.Sockets.Socket thisSocket;
//			public byte[] dataBuffer = new byte[1024];
//		}
//
//	}
}
