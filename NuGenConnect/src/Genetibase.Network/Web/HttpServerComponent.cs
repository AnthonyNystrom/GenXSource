using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Genetibase.Network.Sockets.Protocols;
using Genetibase.Network.Sockets;

namespace Genetibase.Network.Web
{
    public partial class HttpServerComponent : Component
    {
        private HttpServer _server = new HttpServer();

        #region Properties.Public

        public ServerSocket ServerSocket
        {
            get { return _server.ServerSocket; }
        }

        public bool Active
        {
            get { return _server.Active; }
            set 
            {                
                if (DesignMode || Active == value) return;
                if (Active && !value)
                {
                    _server.Close();
                }
                else
                {
                    _server.Open();   
                }
            }
        }

        [Browsable(false)]
        public List<IWebRequestHandler> RequestHandlers
        {
            get
            {
                return _server.RequestHandlers;
            }
        }

        [Browsable(false)]
        public CustomSessionList SessionList
        {
            get
            {
                return _server.SessionList;
            }
        }

        [Browsable(false)]
        public MimeTable MimeTable
        {
            get
            {
                return _server.MimeTable;
            }
        }
       
        public int DefaultPort
        {
            get
            {
                return _server.DefaultPort;
            }
            set
            {
                if (DefaultPort == value) return;
                _server.DefaultPort = value;
            }
        }

        public bool KeepAlive
        {
            get
            {
                return _server.KeepAlive;
            }
            set
            {
                if (KeepAlive == value) return;
                _server.KeepAlive = value;
            }
        }

        public string ServerSoftware
        {
            get
            {
                return _server.ServerSoftware;
            }
            set
            {
                if (ServerSoftware == value) return;
                _server.ServerSoftware = value;
            }
        }

        public int SessionTimeOut
        {
            get
            {
                return _server.SessionTimeOut;
            }
            set
            {
                if (SessionTimeOut == value) return;
                _server.SessionTimeOut = value;
            }
        }

        public bool AutoStartSession
        {
            get
            {
                return _server.AutoStartSession;
            }
            set
            {
                if (AutoStartSession == value) return;
                _server.AutoStartSession = value;
            }
        }

        public UseTLSEnum UseTLS
        {
            get
            {
                return _server.UseTLS;
            }
            set
            {
                if (UseTLS == value) return;
                if (value != UseTLSEnum.NoTLSSupport)
                {
                    if (!(_server.ServerSocket is ServerSocketTLS))
                    _server.ServerSocket = new Sockets.ServerSocketSimulator();                    
                }
                else
                {
                    _server.ServerSocket = new Sockets.ServerSocketTcp();
                }
                _server.UseTLS = value;
            }
        }

        #endregion

        #region Events.Public
        
        public event HttpCommandEventHandler OnCommandConnect
        {
            add
            {
                _server.OnCommandConnect += value;
            }
            remove
            {
                _server.OnCommandConnect -= value;
            }
        }

        public event HttpCommandEventHandler OnCommandDelete
        {
            add
            {
                _server.OnCommandDelete += value;
            }
            remove
            {
                _server.OnCommandDelete -= value;
            }
        }

        public event HttpCommandEventHandler OnCommandGet
        {
            add
            {
                _server.OnCommandGet += value;
            }
            remove
            {
                _server.OnCommandGet -= value;
            }
        }

        public event HttpCommandEventHandler OnCommandHead
        {
            add
            {
                _server.OnCommandHead += value;
            }
            remove
            {
                _server.OnCommandHead -= value;
            }
        }

        public event HttpCommandEventHandler OnCommandOptions
        {
            add
            {
                _server.OnCommandOptions += value;
            }
            remove
            {
                _server.OnCommandOptions -= value;
            }
        }

        public event HttpCommandEventHandler OnCommandPost
        {
            add
            {
                _server.OnCommandPost += value;
            }
            remove
            {
                _server.OnCommandPost -= value;
            }
        }

        public event HttpCommandEventHandler OnCommandPut
        {
            add
            {
                _server.OnCommandPut += value;
            }
            remove
            {
                _server.OnCommandPut -= value;
            }
        }

        public event HttpCommandEventHandler OnCommandTrace
        {
            add
            {
                _server.OnCommandTrace += value;
            }
            remove
            {
                _server.OnCommandTrace -= value;
            }
        }

        public event HttpCommandEventHandler OnCommandOther
        {
            add
            {
                _server.OnCommandOther += value;
            }
            remove
            {
                _server.OnCommandOther -= value;
            }
        }

        public event HttpCommandEventHandler OnRequestNotHandled
        {
            add
            {
                _server.OnRequestNotHandled += value;
            }
            remove
            {
                _server.OnRequestNotHandled -= value;
            }
        }

        public event SessionEventHandler OnSessionStart
        {
            add
            {
                _server.OnSessionStart += value;
            }
            remove
            {
                _server.OnSessionStart -= value;
            }
        }

        public event SessionEventHandler OnSessionEnd
        {
            add
            {
                _server.OnSessionEnd += value;
            }
            remove
            {
                _server.OnSessionEnd -= value;
            }
        }

        #endregion

        #region Constructors

        public HttpServerComponent()
        {
            InitializeComponent();
        }

        public HttpServerComponent(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        #endregion


    }
}
