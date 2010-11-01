using System;

namespace Genetibase.Network.Sockets {
	public sealed class TcpPorts {
		private TcpPorts() {
		}

		public const int Echo = 7;
		public const int Ftp_Data = 20;
		public const int Ftp = 21;
		public const int Http = 80;
		public const int Https = 443;
		public const int Ftps_Data = 989;
		public const int Ftps = 990;
		public const int Socks = 1080;
	}
}
