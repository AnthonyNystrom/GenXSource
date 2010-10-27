using System;
using System.IO;
using System.Net;									// Endpoint
using System.Net.Sockets;							// Socket namespace
using System.Text;									// Text encoders
using System.Runtime.InteropServices;
using SharpFFmpeg;
//using Tao.OpenAl;
using System.Threading;
//using Yeti.MMedia.Mp3;
//using WaveLib;
using N2FProxy;
using MMedia;
using System.Drawing;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;

public delegate void LogError(string sNewMessage);
public delegate void InfoMsg(string sNewMessage);
public delegate void Red5Connected();

/// <summary>
/// Summary description for Class1
/// </summary>
public class RTMP
{
    [DllImport("kernel32.dll", SetLastError = true)]
    static extern int GetTickCount();

    int iVideoFrameAvar = 0;
    int iNSampleAvar = 0;
    bool bJpgSave;
    private MMedia.MMedia MM = new MMedia.MMedia();
    private FileStream fAMR = null;
    private FileStream fPCM = null;
    private FileStream fPCMRes = null;
    private FileStream fFlv = null;
    private FileStream fVideoSource = null;
    private bool bNoRed5Connection = false; // used for debuging no red5 connection

    //private Mp3WriterConfig m_Config = null;
    //private Mp3Writer m_writer = null;
    //private Mp3Writer m_writer2 = null;
    private bool bInitMP3 = false;
    private IntPtr pContextResample;
    private Socket m_sock;						// Server connection
    private Buffering m_Buffering = new Buffering();
    private byte[] m_byBuff = new byte[30000];	// Recieved data buffer
    private byte[] m_byFrameBuff = new byte[30000];	// Recieved audio data buffer 
    private short[] m_iFrameBuff = new short[30000];	// Recieved audio data buffer 
    private byte[] m_bySendBuffAudio = new byte[300000];
    private byte[] m_byAudioBuff = new byte[100000];
    private byte[] m_byBufHeader = new byte[20];
    private byte[] m_byAudioDummy = new byte[1024];
    private char[] m_test = new char[100000];
    private int m_iBuffHeader;
    private Byte[] AMR_block_size={ 12, 13, 15, 17, 19, 20, 26, 31, 5, 0, 0, 0, 0, 0, 0, 0 };
    private bool bIsJpg;

    private byte[] m_byBuffTmp = new byte[5000];
    private int m_iIdxBuffTmp;
    private int m_iSizeHeader;
    private bool m_bIsVideo;
    private int m_iFrameSize;
    public int m_iAudioType;

    const int N2F_CODEC_AMR = 0;
    const int N2F_CODEC_PCM = 1;
    const int N2F_CODEC_FLV = 2;
    const int N2F_CODEC_H263 = 3;
    const int N2F_CODEC_MPEG1 = 4;
    const int N2F_CODEC_MPEG4 = 5;
    const int N2F_CODEC_JPG = 6;



    private int iVideoFPS, iAudioFPS, iTickCount;

    private int iSizeAudioBuff;
    private int m_iRxBufFrame = 0;
    private int iStatMachine = 1;
    private int iRx = 0;
    private bool bFirstHeaderSent = false;
    private bool bFirstHeaderSentAudio = false;
    private String m_sUserName;
    private String m_sStreamName;
    private String m_sServerName;
    private String m_sPortHttp;
    private String m_sFileName;
    private String m_sParams;
    string m_sFBmp;
    private int m_iCodecIn;
    private int iLenStream;
    private int iFirstByte = 1;
    private bool bWaitHeader = true;
    private bool m_bLogInfo = true;
    private int m_TickStartStream;
    private bool m_bSendAudio = true;

    private byte[] byHeader;
    private byte[] byConnection2 = {
			0x43, 0x00, 0x01, 0x61, 0x00, 0x00, 0x15, 0x14, 
			0x02, 0x00, 0x07, 0x5f, 0x72, 0x65, 0x73, 0x75, 
			0x6c, 0x74, 0x00, 0x40, 0x00, 0x00, 0x00, 0x00, 
			0x00, 0x00, 0x00, 0x05, 0x06 };
    private byte[] byStartStream = {
			0x43, 0x00, 0x1b, 0xdb, 0x00, 0x00, 0x19, 0x14, 
			0x02, 0x00, 0x0c, 0x63, 0x72, 0x65, 0x61, 0x74, 
			0x65, 0x53, 0x74, 0x72, 0x65, 0x61, 0x6d, 0x00, 
			0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
			0x05 };
    private byte[] byCloseStream = {
			0x48, 0x00, 0x08, 0xf3, 0x00, 0x00, 0x18, 0x14, 
			0x02, 0x00, 0x0b, 0x63, 0x6c, 0x6f, 0x73, 0x65, 
			0x53, 0x74, 0x72, 0x65, 0x61, 0x6d, 0x00, 0x00, 
			0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05};



    public event LogError m_Errors;				// Add Errors Event handler for Form
    public event InfoMsg m_InfoMsg;
    public event Red5Connected m_Red5Conn;
    public RTMP()
    {
        byHeader = new byte[12];
        InitDecode();
        iTickCount = GetTickCount();
    }
    public bool StartConnect(String sServerName, int iPortRed5, String sPortHttp, String sStreamName, String sParams, bool bLog, String sUserName, int iWidth, int iHeight, int iCodec)
    {
        m_bLogInfo = bLog;
        m_sUserName = sUserName;
        m_sServerName = sServerName;
        m_sStreamName = sStreamName;
        m_sPortHttp = sPortHttp;
        m_sParams = sParams;
        m_iCodecIn = iCodec;
        bJpgSave = false;
        m_sFileName = sStreamName.Replace('/', '&');

        // Init FF funcs
        bool bRetffInit = false ;
        if (iCodec != N2F_CODEC_FLV && iCodec < N2F_CODEC_JPG)
        {   unsafe
            {   string sInfoFromPhone = "Width:" + iWidth.ToString() + " Height:" + iHeight.ToString();
                m_InfoMsg(sInfoFromPhone);
                m_sFBmp = m_sFileName + ".bmp";
                System.Text.ASCIIEncoding  encoding=new System.Text.ASCIIEncoding();
                fixed (byte* pbyIn = encoding.GetBytes(m_sFBmp))
                    bRetffInit = MM.FFInit(iCodec, N2F_CODEC_FLV, iWidth, iHeight, 300000, pbyIn);
            }
        }
        else
            m_iCodecIn = N2F_CODEC_FLV;



        Console.WriteLine("RED5 start connection streamname:{0}", sStreamName);
        if (global::N2FProxy.Params.Default.LogInfo)
        {
            fAMR = new FileStream(m_sFileName + ".amr", FileMode.Create);
            fPCM = new FileStream(m_sFileName + ".pcm", FileMode.Create);
            fPCMRes = new FileStream(m_sFileName + "_Res.pcm", FileMode.Create);
            fFlv = new FileStream(m_sFileName + ".flv", FileMode.Create);
            fVideoSource = new FileStream(m_sFileName + ".263", FileMode.Create);
            byte[] byHeaderAMR = {
			0x23, 0x21, 0x041, 0x4d, 0x52, 0x0A};
            fAMR.Write(byHeaderAMR, 0, 6);
        }
        iLenStream = m_sStreamName.Length;
        try
        {
            if (bNoRed5Connection)
            {
                m_Red5Conn();
                return true ;
            }
            // Close the socket if it is still open
            if (m_sock != null && m_sock.Connected)
            {
                CloseConnection();
            }

            // Create the socket object
            m_sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Define the Server address and port
            IPEndPoint epServer = new IPEndPoint(IPAddress.Parse(m_sServerName), iPortRed5);

            // Connect to the server blocking method and setup callback for recieved data
            // m_sock.Connect( epServer );
            // SetupRecieveCallback( m_sock );

            // Connect to server non-Blocking method
            m_sock.Blocking = false;
            AsyncCallback onconnect = new AsyncCallback(OnConnect);
            m_sock.BeginConnect(epServer, onconnect, m_sock);
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }
    public void OnConnect(IAsyncResult ar)
    {
        // Socket was the passed in object
        Socket sock = (Socket)ar.AsyncState;

        // Check if we were sucessfull
        try
        {
            //sock.EndConnect( ar );
            if (sock.Connected)
            {
                SetupRecieveCallback(sock);
                m_byBuff[0] = 3;
                m_sock.Send(m_byBuff, 1537, 0);
            }
            else
                m_Errors("Unable to connect to remote machine");
        }
        catch (Exception ex)
        {
            m_Errors("Unusual error during Connect!:" + ex.ToString());
        }
    }

    void SendPublisherCmd(String sStreamName)
    {
        byte[] byb1 = {
			0x02, 0xe2, 0x0c, 0xff, 0x00, 0x00, 0x0a, 0x04, 
			0x00, 0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x00, 
			0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x08, 0x00, 
			0x1c, 0xb8, 0x00, 0x00, 0x31, 0x14, 0x01, 0x00, 
			0x00, 0x00, 0x02, 0x00, 0x07, 0x70, 0x75, 0x62, 
			0x6c, 0x69, 0x73, 0x68, 0x00, 0x00, 0x00, 0x00, 
			0x00, 0x00, 0x00, 0x00, 0x00, 0x05, 0x02, 0x00, 0x00};
        int iSize1 = 57;
        byte[] byb2 = {
			0x02, 0x00, 0x04, 0x6c, 0x69, 0x76, 0x65};
        int iSize2 = 7;

        int iSizePack = 0x17 + sStreamName.Length + iSize2;
        byb1[27] = (byte)((iSizePack >> 8) & 0xff);
        byb1[28] = (byte)(iSizePack & 0xff);
        byb1[55] = (byte)((sStreamName.Length >> 8) & 0xff);
        byb1[56] = (byte)(sStreamName.Length & 0xff);
        m_sock.Send(byb1, iSize1, 0);

        ASCIIEncoding enc = new ASCIIEncoding();
        byte[] byStream = enc.GetBytes(sStreamName);
        m_sock.Send(byStream, sStreamName.Length, 0);

        m_sock.Send(byb2, iSize2, 0);

    }
    void SetBufferIPConnection(String sParams)
    {
        byte[] byBuff = { 
                        0x03,0x00,0x00,0x00,0x00,0x01,0x0E,0x14,
                        0x00,0x00,0x00,0x00,0x02,0x00,0x07,0x63,0x6F,0x6E,0x6E,0x65,0x63,0x74,0x00,0x3F,
                        0xF0,0x00,0x00,0x00,0x00,0x00,0x00,0x03,0x00,0x03,0x61,0x70,0x70,0x02,0x00,0x0C,
                        0x44,0x65,0x76,0x69,0x63,0x65,0x53,0x74,0x72,0x65,0x61,0x6D,0x00,0x08,0x66,0x6C,
                        0x61,0x73,0x68,0x56,0x65,0x72,0x02,0x00,0x0C,0x57,0x49,0x4E,0x20,0x38,0x2C,0x30,
                        0x2C,0x32,0x32,0x2C,0x30,0x00,0x06,0x73,0x77,0x66,0x55,0x72,0x6C,0x02,0x00,0x2F,
                        0x66,0x69,0x6C,0x65,0x3A,0x2F,0x2F,0x2F,0x44,0x7C,0x2F,0x44,0x65,0x76,0x2F,0x46,
                        0x6C,0x61,0x73,0x68,0x53,0x6F,0x75,0x72,0x63,0x65,0x2F,0x44,0x65,0x76,0x69,0x63,
                        0x65,0x53,0x74,0x72,0x65,0x61,0x6D,0x54,0x65,0x73,0x74,0x2E,0x73,0x77,0x66,0x00,
                        0x05,0x74,0x63,0x55,0xC3,0x72,0x6C,0x02,0x00,0x20,0x72,0x74,0x6D,0x70,0x3A,0x2F,
                        0x2F,0x36,0x39,0x2E,0x32,0x31,0x2E,0x31,0x31,0x34,0x2E,0x39,0x39,0x2F,0x44,0x65,
                        0x76,0x69,0x63,0x65,0x53,0x74,0x72,0x65,0x61,0x6D,0x00,0x04,0x66,0x70,0x61,0x64,
                        0x01,0x00,0x00,0x0B,0x61,0x75,0x64,0x69,0x6F,0x43,0x6F,0x64,0x65,0x63,0x73,0x00,
                        0x40,0x83,0x38,0x00,0x00,0x00,0x00,0x00,0x00,0x0B,0x76,0x69,0x64,0x65,0x6F,0x43,
                        0x6F,0x64,0x65,0x63,0x73,0x00,0x40,0x53,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x07,
                        0x70,0x61,0x67,0x65,0x55,0x72,0x6C,0x06,0x00,0x00,0x09,0x02,0x00,0x25};/*0x33,0x01,
                        0x70,0x61,0x73,0x73,0x77,0x6F,0x72,0x64,0x01,0x56,0x49,0x44,0x36,0x31,0x31,0x34,
                        0x35,0x01,0x54,0x65,0x73,0xC3,0x74,0x20,0x76,0x69,0x64,0x65,0x6F,0x20,0x73,0x74,
                        0x72,0x65,0x61,0x6D };*/

        Byte[] data = Encoding.ASCII.GetBytes(sParams);
        int iSize = data.Length, i, ii;
        if (sParams.Length >= 24)
            iSize++;
        Byte[] Params = new Byte[iSize];

        for (i = 0, ii = 0; i < iSize; i++)
        {
            /*if (data[ii] == '|')
            {   Params[i] = 0x01;
                ii++;
            }
            else */
            if (i == 23)
                Params[i] = 0xC3;
            else
            {
                Params[i] = data[ii];
                ii++;
            }
        }
        int iSizePacket = (byBuff.Length - 13) + sParams.Length;
        byBuff[5] = (byte)((iSizePacket >> 8) & 0xff);
        byBuff[6] = (byte)(iSizePacket & 0xff);
        byBuff[byBuff.Length - 1] = (byte)sParams.Length;
        m_sock.Send(byBuff, byBuff.Length, 0);
        m_sock.Send(Params, Params.Length, 0);
    }

    void SetBufferIPConnection_OflaDemo()
    {
        byte[] byHeader = { 0x03, 0x00, 0x00, 0x0B, 0x00, 0x01, 0x38, 0x14, 0x00, 0x00, 0x00, 0x00 };
        byte[] byBuffer1 = {
                                 0x02, 0x00, 0x07, 0x63, //......8........c
                                 0x6F, 0x6E, 0x6E, 0x65, 0x63, 0x74, 0x00, 0x3F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x03, //onnect.?........
                                 0x00, 0x03, 0x61, 0x70, 0x70, 0x02, 0x00, 0x08, 0x6F, 0x66, 0x6C, 0x61, 0x44, 0x65, 0x6D, 0x6F, //..app...oflaDemo
                                 0x00, 0x08, 0x66, 0x6C, 0x61, 0x73, 0x68, 0x56, 0x65, 0x72, 0x02, 0x00, 0x0C, 0x57, 0x49, 0x4E, //..flashVer...WIN
                                 0x20, 0x39, 0x2C, 0x30, 0x2C, 0x34, 0x35, 0x2C, 0x30, 0x00, 0x06, 0x73, 0x77, 0x66, 0x55, 0x72, // 9,0,45,0..swfUr
                                 0x6C, 0x02                                                                                      //l.
                                }; int iSize1 = 70;
        byte[] byBuffer2 = {
                                0x00, 0x04, 0x66, 0x70, 0x61, 0x64, 0x01, 0x00, 0x00, 0x0B, 0x61, 0x75, //Demo..fpad....au
                                0x64, 0x69, 0x6F, 0x43, 0x6F, 0x64, 0x65, 0x63, 0x73, 0x00, 0x40, 0x83, 0x38, 0x00, 0x00, 0x00, //dioCodecs.@.8...
                                0x00, 0x00, 0x00, 0x0B, 0x76, 0x69, 0x64, 0x65, 0x6F, 0x43, 0x6F, 0x64, 0x65, 0x63, 0x73, 0x00, //....videoCodecs.
                                0x40, 0x5F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0D, 0x76, 0x69, 0x64, 0x65, 0x6F, 0x46, //@_........videoF
                                0x75, 0x6E, 0x63, 0x74, 0x69, 0x6F, 0x6E, 0x00, 0x3F, 0xF0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, //unction.?.......
                                0x00, 0x07, 0x70, 0x61, 0x67, 0x65, 0x55, 0x72, 0x6C,0x02                      //..pageUrl                                                                             //localhost                                                                 
	                           }; int iSize2 = 86;
        byte[] byBuffer3 = {
                                0x00, 0x0E, 0x6F, 0x62, 0x6A, 0x65, 0x63, 0x74, 0x45, 0x6E, 0x63, 0x6F, 0x64, 0x69, 0x6E, 0x67, 
                                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x09                                  							 //,http://
	                           }; int iSize3 = 28;
        byte[] byBuffer4 = {
                                0x00, 0x05, 0x74, 0x63, 0x55, 0x72, 0x6C, 0x02,0x00,0x00                                							 //,http://
	                           }; int iSize4 = 10;


        int ii, i, iSizeData, iSizeAddr;
        byte[] bufTmp = new byte[10];
        string sServer, sServerRTMP;
        //sServer = "http://" + m_sServerName + ":" + m_sPortHttp + "/demos/publisher.swf";
        sServer = "http://" + m_sServerName + ":" + m_sPortHttp + global::N2FProxy.Params.Default.live_swfFile;
        //sServerRTMP = "rtmp://" + m_sServerName + "/oflaDemo";
        sServerRTMP = "rtmp://" + m_sServerName + "/" + global::N2FProxy.Params.Default.live_AppName;
        iSizeAddr = sServer.Length;

        iSizeData = iSizeAddr * 2 + iSize1 + iSize2 + iSize3 + 5 + sServerRTMP.Length + iSize4;

        byHeader[5] = (byte)((iSizeData >> 8) & 0xff);
        byHeader[6] = (byte)(iSizeData & 0xff);
        byBuffer4[8] = (byte)((sServerRTMP.Length >> 8) & 0xff);
        byBuffer4[9] = (byte)(sServerRTMP.Length & 0xff);

        byte[] sChunk ={ 0xC3 };

        m_sock.Send(byHeader, 12, 0);
        m_sock.Send(byBuffer1, iSize1, 0);
        bufTmp[0] = (byte)((iSizeAddr >> 8) & 0xff);
        bufTmp[1] = (byte)(iSizeAddr & 0xff);
        m_sock.Send(bufTmp, 2, 0);

        ii = 0;
        ii += iSize1 + 2;
        int iStep = 128;

        ASCIIEncoding enc = new ASCIIEncoding();
        byte[] byIp = enc.GetBytes(sServer);

        for (i = 0; i < iSizeAddr; i++, ii++)
        {
            if ((ii % iStep) == 0 && ii != 0)
            {
                m_sock.Send(sChunk, 1, 0);
                i--;
                ii = -1;
            }
            else
            {
                Array.Copy(byIp, i, bufTmp, 0, 1);
                m_sock.Send(bufTmp, 1, 0);
            }
        }



        for (i = 0; i < iSize4; i++, ii++)
        {
            if ((ii % iStep) == 0 && ii != 0)
            {
                m_sock.Send(sChunk, 1, 0);
                i--;
                ii = -1;
            }
            else
            {
                Array.Copy(byBuffer4, i, bufTmp, 0, 1);
                m_sock.Send(bufTmp, 1, 0);
            }
        }
        byte[] byIpRtmp = enc.GetBytes(sServerRTMP);
        for (i = 0; i < sServerRTMP.Length; i++, ii++)
        {
            if ((ii % iStep) == 0 && ii != 0)
            {
                m_sock.Send(sChunk, 1, 0);
                i--;
                ii = -1;
            }
            else
            {
                Array.Copy(byIpRtmp, i, bufTmp, 0, 1);
                m_sock.Send(bufTmp, 1, 0);
            }
        }



        for (i = 0; i < iSize2; i++, ii++)
        {
            if ((ii % iStep) == 0 && ii != 0)
            {
                m_sock.Send(sChunk, 1, 0);
                i--;
                ii = -1;
            }
            else
            {
                Array.Copy(byBuffer2, i, bufTmp, 0, 1);
                m_sock.Send(bufTmp, 1, 0);
            }
        }
        //sServer = "http://" + m_sServerName + ":" + m_sPortHttp + "/demos/publisher.html";
        sServer = "http://" + m_sServerName + ":" + m_sPortHttp + global::N2FProxy.Params.Default.live_HtmlPage;
        iSizeAddr = sServer.Length;
        bufTmp[0] = (byte)((iSizeAddr >> 8) & 0xff);
        bufTmp[1] = (byte)(iSizeAddr & 0xff);

        if ((ii % iStep) == 0)
        {
            m_sock.Send(sChunk, 1, 0);
            ii = -1;
        }
        Array.Copy(bufTmp, 0, byIp, 0, 1);
        m_sock.Send(byIp, 1, 0);
        ii++;

        if ((ii % iStep) == 0)
        {
            m_sock.Send(sChunk, 1, 0);
            ii = -1;
        }
        Array.Copy(bufTmp, 1, byIp, 0, 1);
        m_sock.Send(byIp, 1, 0);
        ii++;


        byte[] byIp2 = enc.GetBytes(sServer);
        for (i = 0; i < iSizeAddr; i++, ii++)
        {
            if ((ii % iStep) == 0 && ii != 0)
            {
                m_sock.Send(sChunk, 1, 0);
                i--;
                ii = -1;
            }
            else
            {
                Array.Copy(byIp2, i, bufTmp, 0, 1);
                m_sock.Send(bufTmp, 1, 0);


            }
        }

        for (i = 0; i < iSize3; i++, ii++)
        {
            if ((ii % iStep) == 0 && ii != 0)
            {
                m_sock.Send(sChunk, 1, 0);
                i--;
                ii = -1;
            }
            else
            {
                Array.Copy(byBuffer3, i, bufTmp, 0, 1);
                m_sock.Send(bufTmp, 1, 0);

            }
        }

    }

    /// <summary>
    /// Get the new data and send it out to all other connections. 
    /// Note: If not data was recieved the connection has probably 
    /// died.
    /// </summary>
    /// <param name="ar"></param>
    public void OnRecievedData(IAsyncResult ar)
    {
        // Socket was the passed in object
        Socket sock = (Socket)ar.AsyncState;

        // Check if we got any data
        try
        {
            int nBytesRec = sock.EndReceive(ar);
            if (nBytesRec > 0)
            {
                // Wrote the data to the List
                //string sRecieved = Encoding.ASCII.GetString( m_byBuff, 0, nBytesRec );


                iRx += nBytesRec;
                switch (iStatMachine)
                {
                    case 1:
                        if (iRx >= 1536 * 2 + 1)
                        {
                            m_sock.Send(m_byBuff, 1536, 0);
                            if (global::N2FProxy.Params.Default.UseOflaDemoApp)
                                SetBufferIPConnection_OflaDemo();
                            else
                                SetBufferIPConnection(m_sParams);
                            iStatMachine++;
                            iRx = 0;
                        }
                        break;
                    case 2:
                        if (iRx >= 186)
                        {
                            m_InfoMsg("HandShake Completed ");
                            m_sock.Send(byConnection2, 29, 0);
                            m_sock.Send(byStartStream, 33, 0);
                            iStatMachine++;
                            iRx = 0;
                        }


                        break;
                    case 3:
                        if (iRx >= 37)
                        {
                            m_InfoMsg("Stream Command Completed ");
                            iStatMachine++;
                            iRx = 0;
                            SendPublisherCmd(m_sStreamName);
                        }
                        break;
                    case 4:
                        if (iRx >= 132 + m_sStreamName.Length)
                        {
                            m_InfoMsg("Publish Command Completed ");
                            iStatMachine++;
                            iRx = 0;

                            m_Red5Conn();
                            m_TickStartStream = GetTickCount();
                        }
                        break;
                }
                // If the connection is still usable restablish the callback
                SetupRecieveCallback(sock);
            }
            else
            {
                // If no data was recieved then the connection is probably dead
                m_Errors("Server disconnected:" + sock.RemoteEndPoint.ToString());
                CloseConnection();
            }
        }
        catch (Exception ex)
        {
            m_Errors("Unusual error druing Recieve!" + ex.ToString());
        }
    }

    public void SendDataStreamVideo(byte[] byBuffer, int iLen, byte bKeyFrame)
    {
        //Console.WriteLine("SendDataStreamVideo");

        if (fFlv != null)
            fFlv.Write(byBuffer, 0, iLen);

        if (bNoRed5Connection)
            return;
        int iSizeL = iLen + 1;
        byte[] byHeader1 = {
		            0x06, 0x00, 0x16, 0x2a, 0x00, (byte)((iSizeL >> 8) & 0xff), (byte)(iSizeL & 0xFF), 0x09, 
		            0x01, 0x00, 0x00, 0x00, 0x12};
        byte[] byHeader2 = {
		            0x46, 0x00, 0x00, 0xE4, 0x00, (byte)((iSizeL >> 8) & 0xff), (byte)(iSizeL & 0xFF), 0x09,0x22};

        byte[] byChunk ={ 0xc6 };
        if (bKeyFrame==2)
        {
            switch (iFirstByte)
            {
                case 1:
                    byHeader2[8] = 0x12;
                    break;
                case 2:
                    byHeader2[8] = 0x32;
                    break;
                case 3:
                    byHeader2[8] = 0x22;
                    break;
                case 4:
                    byHeader2[8] = 0x32;
                    break;
                case 5:
                    byHeader2[8] = 0x32;
                    break;
            }
            iFirstByte++;
            if (iFirstByte > 5)
                iFirstByte = 1;
        }
        else{
            if (bKeyFrame == 0 && iFirstByte!=1)
                byHeader2[8] = 0x22;
            else{
                byHeader2[8] = 0x12;
                iFirstByte++;
            }
        }
        

        int i;
        if (!bFirstHeaderSent)
        {
            bFirstHeaderSent = true;
            m_sock.Send(byHeader1, 13, 0);
        }
        else
        {
            m_sock.Send(byHeader2, 9, 0);
        }

        int iLastPacket;
        i = 0;
        int iChunck;
        bool bFirstChunk = true;
        while (i < iLen)
        {
            if (bFirstChunk)
            {
                iChunck = 127;
                bFirstChunk = false;
            }
            else
                iChunck = 128;

            if (i + iChunck < iLen)
            {
                byte[] buf = new byte[iChunck];
                Array.Copy(byBuffer, i, buf, 0, iChunck);
                m_sock.Send(buf, iChunck, 0);
                m_sock.Send(byChunk, 1, 0);

                i += iChunck;
            }
            else
            {
                iLastPacket = iLen - i;
                byte[] buf = new byte[iLastPacket];
                Array.Copy(byBuffer, i, buf, 0, iLastPacket);
                m_sock.Send(buf, iLastPacket, 0);
                i += iLastPacket;
            }
        }
    }

    public void SendDataStreamAudio(byte[] byBuffer, int iLen)
    {
        if (bNoRed5Connection)
            return;

        //Console.WriteLine("SendDataStreamAudio size frame:{0}", iLen);
        int iFixedByte = 1;
        int iSizeL = iLen + iFixedByte;
        byte[] byHeader1 = {
		            0x05, 0x00, 0x16, 0x2a, 0x00, (byte)((iSizeL >> 8) & 0xff), (byte)(iSizeL & 0xFF), 0x08, 
//		            0x01, 0x00, 0x00, 0x00, 0x2A};//22 mp3
//                    0x01, 0x00, 0x00, 0x00, 0x06}; // 22 pcm
		            0x01, 0x00, 0x00, 0x00, 0x0A};//11 pcm
//		            0x01, 0x00, 0x00, 0x00, 0x02};//5 pcm
        byte[] byHeader2 = {
		            0x45, 0x00, 0x00, 0xE4, 0x00, (byte)((iSizeL >> 8) & 0xff), (byte)(iSizeL & 0xFF), 0x08,0x02};

        int iSampleOut = Convert.ToInt32(global::N2FProxy.Params.Default.SampleRateOut);
        switch (iSampleOut)
            {
            case 5500:                
                byHeader1[12]=byHeader2[8] = 0x02;
                break;
            case 11025:
                byHeader1[12]=byHeader2[8] = 0x06;
                break;
            case 22050:
                byHeader1[12]=byHeader2[8] = 0x0A;
                break;
            case 44100:
                byHeader1[12]=byHeader2[8] = 0x0E;
                break;
            }


        byte[] byChunk ={ 0xc5 };

        int i;
        int iIdxDest = 0;
        if (!bFirstHeaderSentAudio)
        {
            bFirstHeaderSentAudio = true;
            //m_sock.Send(byHeader1, 13, 0);
            Array.Copy(byHeader1, 0, m_bySendBuffAudio, iIdxDest, byHeader1.Length); iIdxDest += byHeader1.Length;
        }
        else
        {
            //m_sock.Send(byHeader2, 9, 0);
            Array.Copy(byHeader2, 0, m_bySendBuffAudio, iIdxDest, byHeader2.Length); iIdxDest += byHeader2.Length;
        }

        int iLastPacket;
        i = 0;
        int iChunck;
        bool bFirstChunk = true;
        while (i < iLen)
        {
            if (bFirstChunk)
            {
                iChunck = 128 - iFixedByte;
                bFirstChunk = false;
            }
            else
                iChunck = 128;

            if (i + iChunck < iLen)
            {
                //byte[] buf = new byte[iChunck];
                //Array.Copy(byBuffer, i, buf, 0, iChunck);
                //m_sock.Send(buf, iChunck, 0);
                Array.Copy(byBuffer, i, m_bySendBuffAudio, iIdxDest, iChunck); iIdxDest += iChunck;
                //m_sock.Send(byChunk, 1, 0);
                m_bySendBuffAudio[iIdxDest] = byChunk[0]; iIdxDest++;

                i += iChunck;
            }
            else
            {
                iLastPacket = iLen - i;
                //byte[] buf = new byte[iLastPacket];
                //Array.Copy(byBuffer, i, buf, 0, iLastPacket);
                //m_sock.Send(buf, iLastPacket, 0);
                Array.Copy(byBuffer, i, m_bySendBuffAudio, iIdxDest, iLastPacket); iIdxDest += iLastPacket;

                i += iLastPacket;
            }
        }
        m_sock.Send(m_bySendBuffAudio, iIdxDest, SocketFlags.None);
    }



    /// <summary>
    /// Setup the callback for recieved data and loss of conneciton
    /// </summary>
    public void SetupRecieveCallback(Socket sock)
    {
        try
        {
            AsyncCallback recieveData = new AsyncCallback(OnRecievedData);
            sock.BeginReceive(m_byBuff, 0, m_byBuff.Length, SocketFlags.None, recieveData, sock);
        }
        catch (Exception ex)
        {
            m_Errors("Setup Recieve Callback failed!:" + ex.ToString());
        }
    }

    public void CloseConnection()
    {
        if (m_sock != null && m_sock.Connected)
        {/*
            if (m_writer2 != null)
                m_writer2.Close();*/
            if (fAMR != null)
                fAMR.Close();
            if (fPCM != null)
                fPCM.Close();
            if (fPCMRes != null)
                fPCMRes.Close();
            if (fFlv != null)
                fFlv.Close();
            if (fVideoSource != null)
                fVideoSource.Close();

            m_sock.Shutdown(SocketShutdown.Both);
            System.Threading.Thread.Sleep(10);
            m_sock.Close();
            FFmpeg.av_resample_close(pContextResample);

            if (!global::N2FProxy.Params.Default.UseOflaDemoApp)
            {
                SqlConnection conn = null;
                //SqlDataReader rdr = null;

                try
                {
                    //m_sTitle = null;
                    // create and open a connection object
                    conn = new SqlConnection("server=192.168.3.4;database=Next2Friends;uid=N2FDBLogin8745;password=59c42xMJH03t3fl83dk;Max Pool Size=200; Min Pool Size=20;");
                    conn.Open();
                    // the stored procedure
                    SqlCommand cmd = new SqlCommand("HG_EndLiveStream", conn);
                    // to execute a stored procedure
                    cmd.CommandType = CommandType.StoredProcedure;
                    //    will be passed to the stored procedure

                    //String sParameter = "@WebLiveBroadcastID", m_sStreamName;
                    cmd.Parameters.Add(new SqlParameter("@WebLiveBroadcastID", m_sStreamName));
                    cmd.Parameters.Add(new SqlParameter("@Nickname", m_sUserName));
                    //cmd.Parameters.Add(new SqlParameter("@Title", ??));
                    //cmd.Parameters.Add(new SqlParameter("@Description", ??));

                    // execute the command
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    LogInfo("Error Updating DB for Close Connection:" + ex.ToString());
                }
                finally
                {
                    if (conn != null)
                    {
                        conn.Close();
                    }
                }
            }
            m_Errors("Connection Stopped");
        }
    }

    private void LogInfo(String sRaw)
    {
        if (m_bLogInfo)
        {
            Console.WriteLine(m_sStreamName + " : " + sRaw);
        }
    }


    //byBuffer  : buffer input
    //iLen      : len of byBuffer
    //bHeaderGood: return true if data header are good
    //bIsVideo  : return true if a video frame or false for audio
    //frame size: return size follow bytes
    //iIdxData  : index of byBuffer for start data. 
    // Return true if header is complete
    private bool GetHeaderInfo(byte[] byBuffer, int iLen, ref bool bHeaderGood, ref bool bIsVideo, ref int iFrameSize, ref int iIdxData)
    {

        /*Console.WriteLine("Header:");
        int i;
        for (i = 0; i < 8; i++)
            Console.Write("0x{0:X} ", byBuffer[i]);
        Console.WriteLine("");
        */

        bool bRet = false;
        try
        {
            if (m_iIdxBuffTmp == 0)
            {
                m_iSizeHeader = (int)((byBuffer[0] & 0xF0) >> 6);
                switch (m_iSizeHeader)
                {
                    case 0:
                        m_iSizeHeader = 12;
                        break;
                    case 1:
                        m_iSizeHeader = 8;
                        break;
                    default:// bad header data
                        bHeaderGood = false;
                        return true;
                }
            }

            if (iLen + m_iIdxBuffTmp < m_iSizeHeader)
            {
                Array.Copy(byBuffer, 0, m_byBuffTmp, m_iIdxBuffTmp, iLen);
                m_iIdxBuffTmp += iLen;
            }
            else
            {
                Array.Copy(byBuffer, 0, m_byBuffTmp, m_iIdxBuffTmp, m_iSizeHeader - m_iIdxBuffTmp);
                m_iIdxBuffTmp = 0;
                iIdxData = m_iSizeHeader - m_iIdxBuffTmp; // index of start data

                // Header Complete. Now I check if a good header

                bool bBadHeader = true;
                if (m_iSizeHeader == 12 && (m_byBuffTmp[0] == 0x06 || m_byBuffTmp[0] == 0x05))
                    bBadHeader = false;
                else if (m_iSizeHeader == 8 && (m_byBuffTmp[0] == 0x46 || m_byBuffTmp[0] == 0x45))
                    bBadHeader = false;
                if (m_byBuffTmp[7] != 0x09 && m_byBuffTmp[7] != 0x08)
                    bBadHeader = true;
                if (!bBadHeader)
                {   // header data good
                    bHeaderGood = true;
                    m_iAudioType = m_byBuffTmp[1];
                    iFrameSize = (m_byBuffTmp[5] << 8) | m_byBuffTmp[6];

                    if (m_byBuffTmp[7] == 0x09)
                    {
                        m_bIsVideo = true;
                        if (m_byBuffTmp[1] == 0x01)
                            bIsJpg = true;
                        else
                            bIsJpg = false;
                    }
                    else if (m_byBuffTmp[7] == 0x08)
                    {
                        m_bIsVideo = false;
                    }
                }
                else
                    bHeaderGood = false;

                bRet = true;
            }
        }
        catch (Exception ex)
        {
            LogInfo("Error GetHeaderInfo:" + ex.ToString());
            m_Errors("Error GetHeaderInfo:" + ex.ToString());
            CloseConnection();
        }
        return bRet;
    }



    private void InitDecode()
    {
        // Init MMedia AMR Decoder
        MM.AmrInit();
        // Init resample
        int iInSam, iOutSam;
        iOutSam = Convert.ToInt32(global::N2FProxy.Params.Default.SampleRateOut);
        iInSam = Convert.ToInt32(global::N2FProxy.Params.Default.SampleRateIn);
        if (iOutSam!=iInSam)
            pContextResample = FFmpeg.audio_resample_init(1, 1, iOutSam, iInSam);
    }
    /*
    private void EncoderToMP3()
    {
        if (!bInitMP3)
        {
            WaveFormat Format = new WaveFormat(16000, 16, 1);
            Format.nAvgBytesPerSec = 88200;
            Format.nBlockAlign = 2;
            Format.nChannels = 1;
            Format.nSamplesPerSec = 16000;
            Format.wBitsPerSample = 16;
            Format.wFormatTag = 1;
            m_Config = new Mp3WriterConfig(Format);
            m_writer = new Mp3Writer(new MemoryStream(10000), m_Config);
            if (m_bLogInfo)
                m_writer2 = new Mp3Writer(new FileStream(m_sStreamName + ".mp3", FileMode.Create), m_Config);
            bInitMP3 = true;
        }
        if (bInitMP3)
        {
            m_writer.Write(m_byFrameBuff, 0, m_iRxBufFrame);
            if (m_bLogInfo)
                m_writer2.Write(m_byFrameBuff, 0, m_iRxBufFrame);
            if (m_writer.iDataEncoded > 0)
            {
                SendDataStreamAudio(m_writer.m_OutBuffer, (int)m_writer.iDataEncoded);
                Console.WriteLine("mp3 frame size:{0}", m_writer.iDataEncoded);
            }
        }
    }
    */
    private void DecoderAudio()
    {
        try
        {
            if (m_bSendAudio)
            {
                int iSize=0;
                if (m_iAudioType==N2F_CODEC_AMR)
                {   byte[] byPcm = new byte[1024];
                    if (m_bLogInfo)
                        fAMR.Write(m_byFrameBuff, 0, m_iRxBufFrame);

                    unsafe
                    {
                        fixed (byte* pbyIn = m_byFrameBuff, pbyOut = byPcm)
                        {
                            iSize = MM.AmrDecodeOneFrame(pbyIn, pbyOut);
                            Array.Copy(byPcm, 0, m_byAudioBuff, iSizeAudioBuff, iSize);
                            iSizeAudioBuff += iSize;
                            m_iRxBufFrame = iSize;
                        }
                    }
                    if (m_bLogInfo)
                        fPCM.Write(m_byAudioBuff, 0, iSizeAudioBuff);

                    m_Buffering.PushAudioFrame(m_byAudioBuff);
                    SendFramesBuffering();
                    iSizeAudioBuff = 0;


                    /*if (iSizeAudioBuff >= iSize)
                    {
                        if (m_bLogInfo)
                            fPCM.Write(m_byAudioBuff, 0, iSizeAudioBuff);

                        if (global::N2FProxy.Params.Default.SampleRateOut != global::N2FProxy.Params.Default.SampleRateIn)
                        {
                            IntPtr pin = Marshal.AllocHGlobal(iSizeAudioBuff);
                            IntPtr pout = Marshal.AllocHGlobal(iSizeAudioBuff * 10);
                            Marshal.Copy(m_byAudioBuff, 0, pin, iSizeAudioBuff);

                            int iSizeS = FFmpeg.audio_resample(pContextResample, pout, pin, iSizeAudioBuff);
                            Marshal.Copy(pout, m_byAudioBuff, 0, iSizeS);
                            m_iRxBufFrame = iSizeS;

                            //Console.WriteLine("size audio resampled:{0}", iSizeS);
                            if (m_bLogInfo)
                                fPCMRes.Write(m_byAudioBuff, 0, iSizeS);
                        }

                        SendDataStreamAudio(m_byAudioBuff, m_iRxBufFrame);
                        iSizeAudioBuff = 0;
                    }*/
                }
                else if (m_iAudioType == N2F_CODEC_PCM)
                {
                    if (global::N2FProxy.Params.Default.SampleRateOut != global::N2FProxy.Params.Default.SampleRateIn)
                    {
                        IntPtr pin = Marshal.AllocHGlobal(m_iRxBufFrame);
                        IntPtr pout = Marshal.AllocHGlobal(m_iRxBufFrame *20);
                        Marshal.Copy(m_byFrameBuff, 0, pin, m_iRxBufFrame);
                        iSize = FFmpeg.audio_resample(pContextResample, pout, pin, m_iRxBufFrame);
                        //Console.WriteLine("size audio resampled:{0}", iSize);
                        m_iRxBufFrame = iSize;
                        Marshal.Copy(pout, m_byFrameBuff, 0, iSize);
                    }
                    SendDataStreamAudio(m_byFrameBuff, m_iRxBufFrame);
                }
            }
        }
        catch (Exception ex)
        {
            LogInfo("Error Resample:" + ex.ToString());
            //m_Errors("Error Resample:" + ex.ToString());
        }
    }

    public void UpdateDBThumb()
    {
        if (!global::N2FProxy.Params.Default.UseOflaDemoApp)
        {
            SqlConnection conn = null;
            //SqlDataReader rdr = null;

            try
            {

                // create and open a connection object
                conn = new SqlConnection("server=192.168.3.4;database=Next2Friends;uid=N2FDBLogin8745;password=59c42xMJH03t3fl83dk;Max Pool Size=200; Min Pool Size=20;");
                conn.Open();
                // the stored procedure
                SqlCommand cmd = new SqlCommand("HG_SetLiveStreamThumbnail", conn);
                // to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;
                //    will be passed to the stored procedure

                //String sParameter = "@WebLiveBroadcastID", m_sStreamName;
                cmd.Parameters.Add(new SqlParameter("@WebLiveBroadcastID", m_sStreamName));

                // execute the command
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                LogInfo("Error UpdateDBThumb:" + ex.ToString());
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
                //if (rdr != null)
                //{
                //    rdr.Close();
                //}
            }

        }
    }

    void SendFramesBuffering()
    {   // Check for send frames to red5
        try
        {
            bool bAudio = true;
            byte bVideoKeyFrame = 1;
            while (m_Buffering.GetNextFrame2Send(m_byFrameBuff, ref m_iFrameSize, ref bAudio, ref bVideoKeyFrame))
            {
                if (!bAudio)
                    SendDataStreamVideo(m_byFrameBuff, m_iFrameSize, (byte)bVideoKeyFrame);
                else
                {
                    SendDataStreamAudio(m_byFrameBuff, m_iFrameSize);
                    if (m_bLogInfo)
                        fPCMRes.Write(m_byFrameBuff, 0, m_iFrameSize);
                }

            }
        } 
        catch (Exception ex)
        {
            LogInfo("Error SendFramesBuffering:" + ex.ToString());
        }
    }

    public void DataForward(byte[] byBuffer, int iLen)
    {
        int iIdxData = 0;
        int iSizeBlock = iLen;
        byte[] byData = new byte[iLen * 2];

        if (bWaitHeader)
        {
            bool bHeaderGood = false;
            if (iLen + m_iBuffHeader < 8)
            {
                Array.Copy(byBuffer, 0, m_byBufHeader, m_iBuffHeader, iLen);
                m_iBuffHeader += iLen;
                return;
            }
            if (m_iBuffHeader > 0)
            {
                Array.Copy(m_byBufHeader, byData, m_iBuffHeader);
                Array.Copy(byBuffer, 0, byData, m_iBuffHeader, iLen);
                iLen += m_iBuffHeader;
                m_iBuffHeader = 0;
            }
            else
                Array.Copy(byBuffer, byData, iLen);
            bool bRet = GetHeaderInfo(byData, iLen, ref bHeaderGood, ref m_bIsVideo, ref m_iFrameSize, ref iIdxData);
            if (bRet && !bHeaderGood) // bad data
            {
                LogInfo("BAD Header");
                m_Errors("BAD Header");
                CloseConnection();
                return;
            }
            else if (bRet)
            {// Header received all and data are good
                bWaitHeader = false;
                iSizeBlock = iLen - iIdxData;
            }
            //Console.WriteLine("W.Header");
        }
        else
        {
            Array.Copy(byBuffer, byData, iLen);
            //Console.WriteLine("Wo.Header");
        }
        if (!bWaitHeader)
        {
            int iSizeForFrame;
            int iIndexNewFrame = 0;

            if (m_iRxBufFrame + iSizeBlock > m_iFrameSize)
            {
                iSizeForFrame = m_iFrameSize - m_iRxBufFrame;
                iIndexNewFrame = iIdxData + iSizeForFrame;
            }
            else
                iSizeForFrame = iSizeBlock;


            /*Console.WriteLine("iSizeForFrame:{0} iIndexNewFrame:{1} m_iRxBufFrame:{2} iSizeBlock:{3} m_iFrameSize:{4} iIdxData:{5}, iLen:{6}",
                iSizeForFrame, iIndexNewFrame, m_iRxBufFrame, iSizeBlock, m_iFrameSize, iIdxData, iLen);*/

            Array.Copy(byData, iIdxData, m_byFrameBuff, m_iRxBufFrame, iSizeForFrame);


            m_iRxBufFrame += iSizeForFrame;

            // check if frame is completed
            if (m_iRxBufFrame == m_iFrameSize)
            {
                if (!m_bIsVideo)
                {   DecoderAudio();
                    iAudioFPS++;
                }
                else
                {   iVideoFPS++;
                    if (fVideoSource!=null)
                        fVideoSource.Write(m_byFrameBuff, 0, m_iFrameSize);
                    byte bKeyFrame;
                    if (!bIsJpg && m_iCodecIn != N2F_CODEC_FLV)
                    {
                        int iSizeNewCodec = 0, iSizeJpg=0;
                        byte[] byJpgBuffer = new byte[30000];

                        unsafe
                        {   byte *pNewCodec=null, pJpgFormat=null;
                            fixed (byte* pbyIn = m_byFrameBuff, pbyJ=byJpgBuffer)
                            {
                                iSizeNewCodec = MM.FFDecodeEncode(pbyIn, m_iFrameSize, &pNewCodec, &bKeyFrame);
                                // The following fixed statement pins the location of the src and dst objects
                                // in memory so that they will not be moved by garbage collection.
                                byte* ps = pNewCodec;
                                byte* pd = pbyIn;
                                for (int i = 0; i < iSizeNewCodec; i++)
                                    pd[i] = ps[i];
                            }
                        }
                        if (iSizeNewCodec>=0)
                        {
                            if (!bJpgSave && bKeyFrame==1) // Conversion from bmp to jpg
                            {
                                try{
                                    bJpgSave = true;
                                    Image img = Image.FromFile(m_sFBmp);
                                    String spath;
                                    if (!global::N2FProxy.Params.Default.UseOflaDemoApp)
                                        spath = global::N2FProxy.Params.Default.PathThumb + @"\" + m_sUserName + @"\vthmb\" + m_sStreamName + ".jpg";
                                    else
                                        spath = global::N2FProxy.Params.Default.PathThumb + "\\pippo.jpg";
                                    img.Save(spath);
                                    img.Dispose();
                                    File.Delete(@m_sFBmp);
                                    UpdateDBThumb();
                                }
                                catch (Exception ex)
                                {
                                }
                            }

                            m_Buffering.PushVideoFrame(m_byFrameBuff, iSizeNewCodec, bKeyFrame);
                            SendFramesBuffering();
                        }
                    }    
                    else if(m_iCodecIn==N2F_CODEC_FLV && !bIsJpg)
                        SendDataStreamVideo(m_byFrameBuff, m_iFrameSize, 1);
                    else
                    {
                        String spath;
                        if (!global::N2FProxy.Params.Default.UseOflaDemoApp)
                            spath = global::N2FProxy.Params.Default.PathThumb + @"\" + m_sUserName + @"\vthmb\" + m_sStreamName + ".jpg";
                        else
                            spath = global::N2FProxy.Params.Default.PathThumb + "\\pippo.jpg";
                        FileStream fjpg = new FileStream(spath, FileMode.Create);
                        fjpg.Write(m_byFrameBuff, 0,m_iFrameSize);
                        fjpg.Close();
                        
                        // update DB
                        UpdateDBThumb();
                    }
                }
                m_iRxBufFrame = 0;
                bWaitHeader = true;
                if (GetTickCount() - iTickCount > 1000)
                {
                    Console.WriteLine("Video FPS:{0} Audio FPS:{1}", iVideoFPS, iAudioFPS);
                    iAudioFPS = 0;
                    iVideoFPS = 0;
                    iTickCount = GetTickCount();
                }

                if (iIndexNewFrame != 0) // other data?
                {
                    byte[] byTmp = new byte[30000];
                    int iLen2 = iLen - iIndexNewFrame;
                    Array.Copy(byData, iIndexNewFrame, byTmp, 0, iLen2);
                    DataForward(byTmp, iLen2);
                }
            }
        }
    }
}
