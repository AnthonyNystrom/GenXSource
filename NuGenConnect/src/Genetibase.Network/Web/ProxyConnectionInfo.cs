using System;
using System.ComponentModel;

using Genetibase.Network.Sockets;
using Genetibase.Network.Sockets.Protocols;
using Genetibase.Network.Sockets.Protocols.Authentication;

namespace Genetibase.Network.Web {
	public class ProxyConnectionInfo: IDisposable {
		void IDisposable.Dispose() {
		}
		protected AuthenticationBase _Authentication;
		protected string _Password = "";
		protected int _Port = 0;
		protected string _Server = "";
		protected string _UserName = "";
		protected bool _BasicByDefault = true;

		public ProxyConnectionInfo() {
			Clear();
		}

		public void Clear() {
			_Password = "";
			_UserName = "";
			_Port = 0;
			_Server = "";
		}

		public void SetHeaders(HeaderList AHeaders) {
			string S = "";
			if (_Authentication != null) {
				S = _Authentication.Authentication();
				if (S.Length > 0) {
					AHeaders.Values("Proxy-Authentication", S);
				}
			} else {
				if (_BasicByDefault) {
					_Authentication = new BasicAuthentication();
					_Authentication.UserName = _UserName;
					_Authentication.Password = _Password;
					S = _Authentication.Authentication();
					if (S.Length > 0) {
						AHeaders.Values("Proxy-Authentication", S);
					}
				}
			}
		}

		public AuthenticationBase Authentication {
			get {
				return _Authentication;
			}
			set {
				_Authentication = value;
			}
		}

		[Browsable(false)]
		public bool BasicAuthentication {
			get {
				return _BasicByDefault;
			}
			set {
				_BasicByDefault = value;
			}
		}

		public string ProxyPassword {
			get {
				return _Password;
			}
			set {
				_Password = value;
			}
		}

		public int ProxyPort {
			get {
				return _Port;
			}
			set {
				_Port = value;
			}
		}

		public string ProxyServer {
			get {
				return _Server;
			}
			set {
				_Server = value;
			}
		}

		public string ProxyUserName {
			get {
				return _UserName;
			}
			set {
				_UserName = value;
			}
		}
	}
}
