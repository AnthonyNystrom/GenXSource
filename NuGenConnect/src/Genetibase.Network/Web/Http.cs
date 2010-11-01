using System;
using System.Globalization;

namespace Genetibase.Network.Web {
	public sealed class Http {
		static Http() {
			HttpCommandNames = new string[8];
			HttpCommandNames[(int)HttpCommandEnum.Unknown] = "UNKNOWN";
			HttpCommandNames[(int)HttpCommandEnum.Head] = "HEAD";
			HttpCommandNames[(int)HttpCommandEnum.Get] = "GET";
			HttpCommandNames[(int)HttpCommandEnum.Post] = "POST";
			HttpCommandNames[(int)HttpCommandEnum.Delete] = "DELETE";
			HttpCommandNames[(int)HttpCommandEnum.Put] = "PUT";
			HttpCommandNames[(int)HttpCommandEnum.Trace] = "TRACE";
			HttpCommandNames[(int)HttpCommandEnum.Options] = "OPTIONS";
			HttpProtocolVersionNames = new string[2];
			HttpProtocolVersionNames[(int)HttpProtocolVersionEnum.v1_0] = "1.0";
			HttpProtocolVersionNames[(int)HttpProtocolVersionEnum.v1_1] = "1.1";
		}

        private static string[] HttpDateFormats = new string[] {
            @"ddd, dd MMM yyyy HH:mm:ss ""GMT""", // rfc1123-date
            @"ddd, dd MMM yyyy HH:mm:ss GMT", // rfc1123-date without quoted GMT            
            @"dddd, dd-MMM-yy HH:mm:ss ""GMT""", // rfc850-date
            @"dddd, dd-MMM-yy HH:mm:ss GMT", // rfc850-date without quoted GMT
            @"ddd MMM d HH:mm:ss yyyy" // asctime-date
        };

		public const bool HttpServer_KeepAlive = false;
		public const bool HttpServer_ParseParams = true;
		public const bool HttpServer_SessionState = false;
		public const int HttpServer_SessionTimeOut = 0;
		public const bool HttpServer_AutoStartSession = false;
		public const int HttpServer_MaximumHeaderLineCount = 1024;
		public const HttpProtocolVersionEnum HttpClient_ProtocolVersion = HttpProtocolVersionEnum.v1_1;
		public const int HttpClient_RedirectMax = 15;
		public const int HttpClient_MaxHeaderLines = 255;
		public const bool HttpClient_HandleRedirects = false;
		public const int HttpClient_MaxAuthRetries = 3;
		public const int ResponseNo = 200;
		public const int ContentLength = -1;
		public static readonly string HttpServer_ServerSoftware = "Genetibase.Network.NET/1.0"; // add real version
		public const string ContentType = "text/html";
		public const string SessionIdCookie = "IndyNETHTTPSessionID";
		public static readonly string[] HttpCommandNames;
		public const string DefaultUserAgent = "Mozilla/3.0 (compatible; Indy#)";
		public static readonly string[] HttpProtocolVersionNames;
		public static readonly string[] WeekDays = new string[7] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
		public static readonly string[] MonthNames = new string[12] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };


		public static DateTime OffsetFromUtc() {
			return new DateTime(TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Ticks);
		}

		public static int TimeStampInterval(DateTime AStart, DateTime AEnd) {
			return (int)((AEnd.Ticks / 10) - (AStart.Ticks / 10));
		}

		public static string GetRandomString(int NumChar) {
			string CharMap = "qwertzuiopasdfghjklyxcvbnmQWERTZUIOPASDFGHJKLYXCVBNM1234567890";
			Random Temp = new Random();
			int MaxChar = CharMap.Length - 1;
			string TempResult = "";
			while (TempResult.Length < NumChar) {
				TempResult += CharMap[Temp.Next(0, MaxChar)];
			}
			return TempResult;
		}

		public static DateTime GmtToLocalDateTime(string S) {
			if (S == "") {
				return new DateTime(0);
			} else {
				return TimeZone.CurrentTimeZone.ToLocalTime(StrInternetToDateTime(S));
			}
		}

		public static DateTime StrInternetToDateTime(string AValue) {
			return Http.RawStrInternetToDateTime(ref AValue);
		}

		public static string DateTimeGmtToHttpStr(DateTime GmtValue) {
			return GmtValue.ToString("dddd, dd MMMM yyyy HH\":\"nn\":\"SS");
		}

		public static string DomainName(string AHost) {
			return AHost.Substring(AHost.IndexOf('.'));
		}

		public static byte StrToDay(string ADay) {
			return (byte)Array.IndexOf<string>(WeekDays, ADay);
		}

		public static byte StrToMonth(string AMonth) {
			return (byte)Array.IndexOf<string>(MonthNames, AMonth);
		}

		private static short ParseDayOfMonth(ref string AValue, string ADelim) {
			short Dt = Genetibase.Network.Sockets.Global.StrToInt16Def(Genetibase.Network.Sockets.Global.Fetch(ref AValue, ADelim), 1);
			AValue = AValue.TrimStart();
			return Dt;
		}

		private static short ParseMonth(ref string AValue, string ADelim) {
			short Mo = StrToMonth(Genetibase.Network.Sockets.Global.Fetch(ref AValue, ADelim));
			AValue = AValue.TrimStart();
			return Mo;
		}

        private static DateTime RawStrInternetToDateTime(ref string AValue)
        {
            AValue = AValue.TrimStart();
            try
            {
                return DateTime.ParseExact(AValue, HttpDateFormats, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        //private static DateTime RawStrInternetToDateTime(ref string AValue) {
        //    int i;
        //    short Dt;
        //    short Mo;
        //    short Yr;
        //    short Ho = 0;
        //    short Min = 0;
        //    short Sec = 0;
        //    string sTime;
        //    string ADelim;
        //    bool LAM = false;
        //    bool LPM = false;
        //    DateTime TempResult = new DateTime(0);
        //    AValue = AValue.Trim();
        //    if (AValue.Length == 0) {
        //        return TempResult;
        //    }
        //    try {
        //        if (Http.StrToDay(AValue.Substring(0, 3)) > 0) {
        //            if ((AValue[3] == ',')
        //                && (AValue[4] != ' ')) {
        //                AValue = AValue.Substring(0, 4) + ' ' + AValue.Substring(4);
        //            }
        //            Genetibase.Network.Sockets.Global.Fetch(ref AValue);
        //            AValue = AValue.TrimStart();
        //        }
        //        if ((AValue.IndexOf('-') > 0)
        //            && (AValue.IndexOf('-') < AValue.IndexOf(' '))) {
        //            ADelim = "-";
        //        } else {
        //            ADelim = " ";
        //        }
        //        if (StrToMonth(Genetibase.Network.Sockets.Global.Fetch(ref AValue, ADelim, false)) > 0) {
        //            Mo = ParseMonth(ref AValue, ADelim);
        //            Dt = ParseDayOfMonth(ref AValue, ADelim);
        //        } else {
        //            Dt = ParseDayOfMonth(ref AValue, ADelim);
        //            Mo = ParseMonth(ref AValue, ADelim);
        //        }
        //        sTime = Genetibase.Network.Sockets.Global.Fetch(ref AValue);
        //        Yr = Genetibase.Network.Sockets.Global.StrToInt16Def(sTime, 1900);
        //        if (Yr == 1900) {
        //            Yr = Genetibase.Network.Sockets.Global.StrToInt16Def(AValue, 1900);
        //            AValue = sTime;
        //        }
        //        if (Yr < 80) {
        //            Yr += 2000;
        //        } else
        //            if (Yr < 100) {
        //                Yr += 1900;
        //            }
        //        TempResult = new DateTime(Yr, Mo, Dt);
        //        if (AValue.IndexOf("AM") > -1) {
        //            LAM = true;
        //            Genetibase.Network.Sockets.Global.Fetch(ref AValue, "AM");
        //        }
        //        if (AValue.IndexOf("PM") > -1) {
        //            LPM = true;
        //            Genetibase.Network.Sockets.Global.Fetch(ref AValue, "PM");
        //        }
        //        i = AValue.IndexOf(':');
        //        if (i > -1) {
        //            sTime = Genetibase.Network.Sockets.Global.Fetch(ref AValue, " ");
        //            Ho = Genetibase.Network.Sockets.Global.StrToInt16Def(Genetibase.Network.Sockets.Global.Fetch(ref sTime, ":"), 0);
        //            Min = Genetibase.Network.Sockets.Global.StrToInt16Def(Genetibase.Network.Sockets.Global.Fetch(ref sTime, ":"), 0);
        //            Sec = Genetibase.Network.Sockets.Global.StrToInt16Def(Genetibase.Network.Sockets.Global.Fetch(ref sTime), 0);
        //            AValue = AValue.TrimStart();
        //            if (LAM) {
        //                if (Ho == 12) {
        //                    Ho = 0;
        //                }
        //            } else {
        //                if (LPM) {
        //                    if (Ho < 12) {
        //                        Ho += 12;
        //                    }
        //                }
        //            }
        //        }
        //        TempResult = new DateTime(Yr, Mo, Dt, Ho, Min, Sec);
        //        AValue = AValue.TrimStart();
        //    } catch {
        //        return new DateTime(0);
        //    }
        //    return TempResult;
        //}


		public static string ProcessPath(string ABasePath, string APath) {
			return ProcessPath(ABasePath, APath, '/');
		}

		public static string ProcessPath(string ABasePath, string APath, char APathDelim) {
			int i;
			bool LPreserveTrail;
			string LWork;
			string TempResult = "";
			if (APath.IndexOf(APathDelim) == 0) {
				return APath;
			} else {
				LPreserveTrail = APath.Length == 0 || (APath.Substring(APath.Length - 1)[0] == APathDelim);
				LWork = ABasePath;
				if (LWork.Length > 0
					&& LWork.Substring(LWork.Length - 1, 1)[0] != APathDelim) {
					LWork += APathDelim;
				}
				LWork += APath;
				if (LWork.Length > 0) {
					i = 0;
					while (i < LWork.Length) {
						if (LWork[i] == APathDelim) {
							if (i == 0) {
								TempResult = APathDelim.ToString();
							} else {
								if (TempResult.Substring(TempResult.Length - 1, 1)[0] != APathDelim) {
									TempResult += LWork[i];
								}
							}
						} else {
							if (LWork[i] == '.') {
								if (TempResult.Substring(TempResult.Length - 1)[0] == APathDelim
									&& LWork.Substring(i, 2) == "..") {
									TempResult = TempResult.Substring(0, TempResult.Length - 1);
									while (TempResult.Length > 0
											 && TempResult.Substring(TempResult.Length - 1)[0] != APathDelim) {
										TempResult = TempResult.Substring(0, TempResult.Length - 1);
									}
									i++;
								} else {
									TempResult += LWork[i];
								}
							} else {
								TempResult += LWork[i];
							}
						}
						i++;
					}
					if (TempResult != APathDelim.ToString()
						&& TempResult.Substring(TempResult.Length - 1)[0] == APathDelim
						&& !LPreserveTrail) {
						TempResult = TempResult.Substring(0, TempResult.Length - 1);
					}
				}
			}
			return TempResult;
		}

		public static DateTime TimeZoneBias() {
			return Http.OffsetFromUtc().Subtract(new TimeSpan(Http.OffsetFromUtc().Ticks).Subtract(new TimeSpan(Http.OffsetFromUtc().Ticks)));
		}
	}
}
