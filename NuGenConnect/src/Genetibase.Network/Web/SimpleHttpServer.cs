using System;

using Genetibase.Network.Sockets;

namespace Genetibase.Network.Web {
	public class SimpleHttpServer: CustomHttpServer {
		private HttpCommandEventHandler mOnRequestNotHandled;
		private HttpCommandEventHandler mOnCommandGet;
		private HttpCommandEventHandler mOnCommandOther;
		private HttpInvalidSessionEventHandler _OnInvalidSession;
		private CreatePostStreamEventHandler _OnCreatePostStream;
		private CreateSessionEventHandler _OnCreateSession;

		protected override void DoRequestNotHandled(ContextRFC AContext, HttpRequestInfo ARequestInfo, HttpResponseInfo AResponseInfo) {
			bool LHandled = false;
			if (mOnRequestNotHandled != null) {
				mOnRequestNotHandled(AContext, ARequestInfo, AResponseInfo, ref LHandled);
			}
			if (!LHandled) {
				AResponseInfo.ContentText = "<html><body><h3>The server didn't handle your request.</h3></body></html>";
			}
		}

		protected override void DoCommand(ContextRFC context, HttpRequestInfo request, HttpResponseInfo response, ref bool handled) {
			switch (request.CommandType) {
				case HttpCommandEnum.Get: {
						if (mOnCommandGet != null) {
							mOnCommandGet(context, request, response, ref handled);
						}
						break;
					}
				default: {
						if (mOnCommandOther != null) {
							mOnCommandOther(context, request, response, ref handled);
						}
						break;
					}
			}
		}

		public event HttpCommandEventHandler OnCommandGet {
			add {
				mOnCommandGet += value;
			}
			remove {
				mOnCommandGet -= value;
			}
		}

		public event HttpCommandEventHandler OnCommandOther {
			add {
				mOnCommandOther += value;
			}
			remove {
				mOnCommandOther -= value;
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

		public event CreatePostStreamEventHandler OnCreatePostStream {
			add {
				_OnCreatePostStream += value;
			}
			remove {
				_OnCreatePostStream -= value;
			}
		}

		public event HttpInvalidSessionEventHandler OnInvalidSession {
			add {
				_OnInvalidSession += value;
			}
			remove {
				_OnInvalidSession -= value;
			}
		}

		public event SessionEventHandler OnSessionStart {
			add {
				_SessionList.OnSessionStart += value;
			}
			remove {
				_SessionList.OnSessionStart -= value;
			}
		}

		public event SessionEventHandler OnSessionEnd {
			add {
				_SessionList.OnSessionEnd += value;
			}
			remove {
				_SessionList.OnSessionEnd -= value;
			}
		}

		public event CreateSessionEventHandler OnCreateSession {
			add {
				_OnCreateSession += value;
			}
			remove {
				_OnCreateSession -= value;
			}
		}
	}
}
