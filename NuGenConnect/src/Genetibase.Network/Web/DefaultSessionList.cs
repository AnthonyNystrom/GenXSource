using System;
using System.Collections.Generic;

namespace Genetibase.Network.Web {
	public class DefaultSessionList: CustomSessionList {
		protected List<HttpSession> _SessionList;
		protected object _SessionListLock = new Object();

		protected override void DoRemoveSession(HttpSession Session) {
			lock (_SessionListLock) {
				if (_SessionList.Contains(Session)) {
					_SessionList.Remove(Session);
				}
			}
		}

		public DefaultSessionList() {
			lock (_SessionListLock) {
				_SessionList = new List<HttpSession>();
			}
		}

		public override void Clear() {
			lock (_SessionListLock) {
				while (_SessionList.Count > 0) {
					((HttpSession)_SessionList[0]).SessionEnd();
					using (HttpSession Temp = (HttpSession)_SessionList[0]) {
						_SessionList.RemoveAt(0);
					}
				}
				_SessionList.Clear();
			}
		}

		public override void Add(HttpSession ASession) {
			lock (_SessionListLock) {
				_SessionList.Add(ASession);
			}
		}

		public override void PurgeStaleSessions(bool PurgeAll) {
			lock (_SessionListLock) {
				for (int i = _SessionList.Count - 1; i >= 0; i--) {
					if (_SessionList[i] != null
						&& (PurgeAll || ((HttpSession)_SessionList[i]).IsSessionStale())) {
						RemoveSession((HttpSession)_SessionList[i]);
					}
				}
			}
		}

		public override HttpSession CreateUniqueSession(string RemoteIP) {
			string SessionId = Http.GetRandomString(15);
			while (GetSession(SessionId, RemoteIP) != null) {
				SessionId = Http.GetRandomString(15);
			}
			return CreateSession(RemoteIP, SessionId);
		}

		public override HttpSession CreateSession(string RemoteIP, string SessionId) {
			lock (_SessionListLock) {
				HttpSession Temp = new HttpSession(this, SessionId, RemoteIP);
				_SessionList.Add(Temp);
				return Temp;
			}
		}

		public override HttpSession GetSession(string SessionId, string RemoteIP) {
			lock (_SessionListLock) {
				foreach (HttpSession hs in _SessionList) {
					if (hs != null) {
						if (hs.SessionId.Equals(SessionId, StringComparison.InvariantCultureIgnoreCase)
							&& (RemoteIP.Length == 0)
							 || hs.RemoteHost.Equals(RemoteIP, StringComparison.InvariantCultureIgnoreCase)) {
							hs.SetLastTimeStamp(DateTime.Now);
							return hs;
						}
					}
				}
			}
			return null;
		}
	}
}
