using System;

using Genetibase.Network.Sockets;
using Genetibase.Network.Sockets.Protocols;

namespace Genetibase.Network.Web {
	public class EntityHeaderInfo: IDisposable {
		protected string _CacheControl = "";
		protected HeaderList _RawHeaders;
		protected string _Connection = "";
		protected string _ContentEncoding = "";
		protected string _ContentLanguage = "";
		protected long _ContentLength;
		protected long _ContentRangeStart;
		protected long _ContentRangeEnd;
		protected string _ContentType = "";
		protected string _ContentVersion = "";
		protected HeaderList _CustomHeaders;
		protected DateTime _Date;
		protected DateTime _Expires;
		protected DateTime _LastModified;
		protected string _Pragma = "";
		protected bool _HasContentLength;

		void IDisposable.Dispose() {
			this.DoDispose();
		}

		protected virtual void DoDispose() {
		}

		protected virtual void DoProcessHeaders() {
			int LSecs;
			_Connection = _RawHeaders.Values("Connection");
			_ContentVersion = _RawHeaders.Values("Content-Version");
			_ContentEncoding = _RawHeaders.Values("Content-Encoding");
			_ContentLanguage = _RawHeaders.Values("Content-Language");
			_ContentType = _RawHeaders.Values("Content-Type");
			_ContentLength = Genetibase.Network.Sockets.Global.StrToInt32Def(_RawHeaders.Values("Content-Length").Trim(), -1);
			_HasContentLength = _ContentLength > 0;
			_Date = Http.GmtToLocalDateTime(_RawHeaders.Values("Date"));
			_LastModified = Http.GmtToLocalDateTime(_RawHeaders.Values("Last-Modified"));
            
			if (Genetibase.Network.Sockets.Global.StrToInt32Def(_RawHeaders.Values("Expires"), -1) != -1) {
				LSecs = Int32.Parse(_RawHeaders.Values("Expires"));
				_Expires = DateTime.Now.AddSeconds(LSecs);
			} else {
				_Expires = Http.GmtToLocalDateTime(_RawHeaders.Values("Expires"));
			}
			_Pragma = _RawHeaders.Values("Pragma");
		}

		internal void ProcessHeaders() {
			DoProcessHeaders();
		}

		internal void SetHeaders() {
			DoSetHeaders();
		}

		protected virtual void DoSetHeaders() {
			_RawHeaders.Clear();
			if (_Connection.Length > 0) {
				_RawHeaders.Values("Connection", _Connection);
			}
			if (_ContentVersion.Length > 0) {
				_RawHeaders.Values("Content-Version", _ContentVersion);
			}
			if (_ContentEncoding.Length > 0) {
				_RawHeaders.Values("Content-Encoding", _ContentEncoding);
			}
			if (_ContentLanguage.Length > 0) {
				_RawHeaders.Values("Content-Language", _ContentLanguage);
			}
			if (_ContentType.Length > 0) {
				_RawHeaders.Values("Content-Type", _ContentType);
			}
			if (_ContentLength >= 0) {
				_RawHeaders.Values("Content-Length", _ContentLength.ToString());
			}
			if (_CacheControl.Length > 0) {
				_RawHeaders.Values("Cache-control", _CacheControl);
			}
			if (_Date > DateTime.MinValue) {
				_RawHeaders.Values("Date", Http.DateTimeGmtToHttpStr(_Date));
			}
			if (_Expires > DateTime.MinValue) {
				_RawHeaders.Values("Expires", Http.DateTimeGmtToHttpStr(_Expires));
			}
			if (_Pragma.Length > 0) {
				_RawHeaders.Values("Pragma", _Pragma);
            }
#warning _CustomHeaders 
            //if (_CustomHeaders.Count > 0) {
            //    _RawHeaders.Text += string.IsNullOrEmpty(_RawHeaders.Text) ? string.Empty : "\r\n" + 
            //        _CustomHeaders.Text;
            //}
		}

		protected void SetContentLength(long AValue) {
			_ContentLength = AValue;
			_HasContentLength = AValue >= 0;
		}

		public EntityHeaderInfo() {
			_CustomHeaders = new HeaderList();
			_RawHeaders = new HeaderList();
			_RawHeaders.FoldLength = 1024;
			Clear();
		}

		~EntityHeaderInfo() {
			_CustomHeaders.Clear();
			_CustomHeaders = null;
			_RawHeaders.Clear();
			_RawHeaders = null;
		}

		public virtual void Clear() {
			_Connection = "close";
			_ContentVersion = "";
			_ContentEncoding = "";
			_ContentLanguage = "";
			_ContentType = "";
			_ContentLength = -1;
			_ContentRangeStart = 0;
			_ContentRangeEnd = 0;
			_Date = new DateTime(0);
			_LastModified = new DateTime(0);
			_Expires = new DateTime(0);
			if (_RawHeaders != null) {
				_RawHeaders.Clear();
			}
			if (_CustomHeaders != null) {
				_CustomHeaders.Clear();
			}
		}

		public bool HasContentLength {
			get {
				return _HasContentLength;
			}
		}

		public HeaderList RawHeaders {
			get {
				return _RawHeaders;
			}
		}

		public string CacheControl {
			get {
				return _CacheControl;
			}
			set {
				_CacheControl = value;
			}
		}

		public string Connection {
			get {
				return _Connection;
			}
			set {
				_Connection = value;
			}
		}

		public string ContentEncoding {
			get {
				return _ContentEncoding;
			}
			set {
				_ContentEncoding = value;
			}
		}

		public string ContentLanguage {
			get {
				return _ContentLanguage;
			}
			set {
				_ContentLanguage = value;
			}
		}

		public long ContentLength {
			get {
				return _ContentLength;
			}
			set {
				SetContentLength(value);
			}
		}

		public long ContentRangeEnd {
			get {
				return _ContentRangeEnd;
			}
			set {
				_ContentRangeEnd = value;
			}
		}

		public long ContentRangeStart {
			get {
				return _ContentRangeStart;
			}
			set {
				_ContentRangeStart = value;
			}
		}

		public string ContentType {
			get {
				return _ContentType;
			}
			set {
				_ContentType = value;
			}
		}

		public string ContentVersion {
			get {
				return _ContentVersion;
			}
			set {
				_ContentVersion = value;
			}
		}

		public virtual HeaderList CustomHeaders {
			get {
				return _CustomHeaders;
			}
			set {
				//_CustomHeaders.Text = value.Text;
			}
		}

		public DateTime Date {
			get {
				return _Date;
			}
			set {
				_Date = value;
			}
		}

		public DateTime Expires {
			get {
				return _Expires;
			}
			set {
				_Expires = value;
			}
		}

		public DateTime LastModified {
			get {
				return _LastModified;
			}
			set {
				_LastModified = value;
			}
		}

		public string Pragma {
			get {
				return _Pragma;
			}
			set {
				_Pragma = value;
			}
		}
	}
}
