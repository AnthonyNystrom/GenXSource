using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Network.Web {
	/// <summary>
	/// This interface lets you implement a request handler for the <see cref="HttpServer"/> class.
	/// </summary>
	public interface IWebRequestHandler {
		void HandleCommand(HttpServer server, HttpRequestInfo request, HttpResponseInfo response, ref bool handled);
		void SessionStart(HttpSession session);
		void SessionEnd(HttpSession session);
	}
}
