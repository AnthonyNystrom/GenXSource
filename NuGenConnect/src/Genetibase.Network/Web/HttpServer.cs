using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using Genetibase.Network.Sockets;

namespace Genetibase.Network.Web {
	public sealed class HttpServer: CustomHttpServer {

        private List<IWebRequestHandler> mRequestHandlers = new List<IWebRequestHandler>();
		private ReaderWriterLock mRequestHandlersLock = new ReaderWriterLock();
		private HttpCommandEventHandler mOnRequestNotHandled;

		public override void Open() {
			base.Open();
			SessionList.OnSessionStart += new SessionEventHandler(SessionList_OnSessionStart);
			SessionList.OnSessionEnd += new SessionEventHandler(SessionList_OnSessionEnd);
		}

		void SessionList_OnSessionEnd(HttpSession Sender) {
			RequestHandlersLock.AcquireReaderLock(-1);
			try {
				for (int i = 0; i < mRequestHandlers.Count; i++) {
					mRequestHandlers[i].SessionEnd(Sender);
				}
			} finally {
				RequestHandlersLock.ReleaseReaderLock();
			}
		}

		void SessionList_OnSessionStart(HttpSession Sender) {
			RequestHandlersLock.AcquireReaderLock(-1);
			try {
				for (int i = 0; i < mRequestHandlers.Count; i++) {
                    mRequestHandlers[i].SessionStart(Sender);
				}
			} finally {
				RequestHandlersLock.ReleaseReaderLock();
			}
		}

		protected override void DoRequestNotHandled(ContextRFC AContext, HttpRequestInfo ARequestInfo, HttpResponseInfo AResponseInfo) {
			bool LHandled = false;
			if (mOnRequestNotHandled != null) {
				mOnRequestNotHandled(AContext, ARequestInfo, AResponseInfo, ref LHandled);
			}
			if (!LHandled) {
				AResponseInfo.ContentText = "<html><body><h3>The server didn't handle your request.</h3></body></html>\r\n";
				AResponseInfo.ContentType = "text/html";
			}
		}

		protected override void DoCommand(ContextRFC context, HttpRequestInfo request, HttpResponseInfo response, ref bool handled) {
            // first raise events is user want to handle request by event
            switch (request.CommandType)
            {
                case HttpCommandEnum.Connect:
                    if (OnCommandConnect != null) OnCommandConnect(context, request, response, ref handled);
                    break;
                case HttpCommandEnum.Delete:
                    if (OnCommandDelete != null) OnCommandDelete(context, request, response, ref handled);
                    break;
                case HttpCommandEnum.Get:
                    if (OnCommandGet != null) OnCommandGet(context, request, response, ref handled);
                    break;
                case HttpCommandEnum.Head:
                    if (OnCommandHead != null) OnCommandHead(context, request, response, ref handled);
                    break;
                case HttpCommandEnum.Options:
                    if (OnCommandOptions != null) OnCommandOptions(context, request, response, ref handled);
                    break;
                case HttpCommandEnum.Post:
                    if (OnCommandPost != null) OnCommandPost(context, request, response, ref handled);
                    break;
                case HttpCommandEnum.Put:
                    if (OnCommandPut != null) OnCommandPut(context, request, response, ref handled);
                    break;
                case HttpCommandEnum.Trace:
                    if (OnCommandTrace != null) OnCommandTrace(context, request, response, ref handled);
                    break;
                default:
                    if (OnCommandOther != null) OnCommandOther(context, request, response, ref handled);
                    break;
            }
            // if request handled from an event handler skip request handlers
            if (handled) return;

            // try to handle request by request handlers collection
            RequestHandlersLock.AcquireReaderLock(-1);
			try {
				for (int i = 0; i < mRequestHandlers.Count && !handled; i++) {
                    mRequestHandlers[i].HandleCommand(this, request, response, ref handled);
				}
			} finally {
				RequestHandlersLock.ReleaseReaderLock();
			}
		}
		public List<IWebRequestHandler> RequestHandlers {
			get {
				return mRequestHandlers;
			}
		}

		public ReaderWriterLock RequestHandlersLock {
			get {
				return mRequestHandlersLock;
			}
		}

		public event HttpCommandEventHandler OnRequestNotHandled {
			add {
				mOnRequestNotHandled += value;
			}
			remove {
				mOnRequestNotHandled -= value;
			}
		}

        public event SessionEventHandler OnSessionStart
        {
            add
            {
                _SessionList.OnSessionStart += value;
            }
            remove
            {
                _SessionList.OnSessionStart -= value;
            }
        }

        public event SessionEventHandler OnSessionEnd
        {
            add
            {
                _SessionList.OnSessionEnd += value;
            }
            remove
            {
                _SessionList.OnSessionEnd -= value;
            }
        }

        public event HttpCommandEventHandler OnCommandGet;
        public event HttpCommandEventHandler OnCommandConnect;
        public event HttpCommandEventHandler OnCommandDelete;
        public event HttpCommandEventHandler OnCommandHead;
        public event HttpCommandEventHandler OnCommandOptions;
        public event HttpCommandEventHandler OnCommandPost;
        public event HttpCommandEventHandler OnCommandPut;
        public event HttpCommandEventHandler OnCommandTrace;
        public event HttpCommandEventHandler OnCommandOther;
       
	}
}
