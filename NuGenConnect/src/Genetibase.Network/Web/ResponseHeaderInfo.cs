using System;

using Genetibase.Network.Sockets;
using Genetibase.Network.Sockets.Protocols;

namespace Genetibase.Network.Web {
	public class ResponseHeaderInfo: EntityHeaderInfo {
		protected string _Location = "";
		protected string _Server = "";
		protected string _ProxyConnection = "";
		protected HeaderList _ProxyAuthenticate;
		protected HeaderList _WWWAuthenticate;

		protected override void DoProcessHeaders() {
			base.DoProcessHeaders();
			_Location = _RawHeaders.Values("Location");
			_Server = _RawHeaders.Values("Server");
			_ProxyConnection = _RawHeaders.Values("Server");
			string RangeDecode = _RawHeaders.Values("Content-Range");
			if (RangeDecode != "") {
				Genetibase.Network.Sockets.Global.Fetch(ref RangeDecode);
				_ContentRangeStart = Int32.Parse(Genetibase.Network.Sockets.Global.Fetch(ref RangeDecode, "-"));
				_ContentRangeEnd = Int32.Parse(Genetibase.Network.Sockets.Global.Fetch(ref RangeDecode, "/"));
			}
			_ContentRangeStart = 0;
			_ContentRangeEnd = 0;
			_WWWAuthenticate.Clear();
			_RawHeaders.Extract("WWW-Authenticate", _WWWAuthenticate);
			_ProxyAuthenticate.Clear();
			_RawHeaders.Extract("Proxy-Authenticate", _ProxyAuthenticate);
		}

		public ResponseHeaderInfo() {
			_ProxyAuthenticate = new HeaderList();
			_WWWAuthenticate = new HeaderList();
		}

		~ResponseHeaderInfo() {
			_ProxyAuthenticate.Clear();
			_ProxyAuthenticate = null;
			_WWWAuthenticate.Clear();
			_WWWAuthenticate = null;
		}

		public override void Clear() {
			base.Clear();
		}

		public string Location {
			get {
				return _Location;
			}
			set {
				_Location = value;
			}
		}

		public string ProxyConnection {
			get {
				return _ProxyConnection;
			}
			set {
				_ProxyConnection = value;
			}
		}

		public HeaderList ProxyAuthenticate {
			get {
				return _ProxyAuthenticate;
			}
			set {
				_ProxyAuthenticate = value;
			}
		}

		public string Server {
			get {
				return _Server;
			}
			set {
				_Server = value;
			}
		}

		public HeaderList WWWAuthenticate {
			get {
				return _WWWAuthenticate;
			}
			set {
				_WWWAuthenticate = value;
			}
		}
	}
}
