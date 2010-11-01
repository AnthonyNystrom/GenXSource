using System;
using System.Collections.Generic;

namespace Genetibase.Network.Web
{
    public class NetscapeCookie : IDisposable
    {
        void IDisposable.Dispose()
        {
        }
        protected string _CookieText = "";
        protected string _Domain = "";
        protected string _Expires = "";
        protected string _Name = "";
        protected string _Path = "";
        protected bool _Secure = false;
        protected string _Value = "";
        protected CookieVersionEnum _InternalVersion = CookieVersionEnum.Netscape;
        protected virtual string GetCookie()
        {
            string TempResult = Cookie.AddCookieProperty(_Name, _Value, "");
            TempResult = Cookie.AddCookieProperty("path", _Path, TempResult);
            if (_InternalVersion == CookieVersionEnum.Netscape)
            {
                TempResult = Cookie.AddCookieProperty("expires", _Expires, TempResult);
            }
            TempResult = Cookie.AddCookieProperty("domain", _Domain, TempResult);
            if (_Secure)
            {
                TempResult = Cookie.AddCookieFlag("secure", TempResult);
            }
            return TempResult;
        }

        protected virtual void SetExpires(string AValue)
        {
        }

        protected void SetCookie(string AValue)
        {
            if (AValue != _CookieText)
            {
                _CookieText = AValue;
                SortedList<string, string> CookieProp = new SortedList<string, string>();

                bool name = false;
                foreach (string cookiePart in AValue.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    string[] TempStrings = cookiePart.Trim().Split('=');
                    if (!name)
                    {
                        _Name = TempStrings[0];
                        _Value = TempStrings[1];
                        name = true;
                    }
                    else
                    {
                        CookieProp.Add(TempStrings[0].ToUpperInvariant(), TempStrings[1]);
                    }
                }

                LoadProperties(CookieProp);
            }
        }

        protected virtual string GetServerCookie()
        {
            return GetCookie();
        }

        protected virtual string GetClientCookie()
        {
            return _Name + "=" + _Value;
        }

        protected virtual void LoadProperties(SortedList<string, string> APropertyList)
        {
            _Path = APropertyList.ContainsKey("PATH") ? APropertyList["PATH"] : "";
            if (_Path.Length > 0)
            {
                if (_Path[0] == '"')
                {
                    _Path = _Path.Substring(1);
                }
                if (_Path[_Path.Length - 1] == '"')
                {
                    _Path = _Path.Substring(0, _Path.Length - 1);
                }
            }
            else
            {
                _Path = "/";
            }
            _Expires =  APropertyList.ContainsKey("EXPIRES") ? APropertyList["EXPIRES"] : "";
            _Domain =APropertyList.ContainsKey("DOMAIN") ? APropertyList["DOMAIN"] : "";
            _Secure = APropertyList.ContainsKey("SECURE"); 
        }

        public NetscapeCookie()
        {

        }

        ~NetscapeCookie()
        {

        }

        public virtual bool IsValidCookie(string AServerHost)
        {
#warning TODO: add IPv6 support
            //if (System.Stack.IsIPv4(AServerHost))
            //{
            return AServerHost == _Domain;
            //}
            //else
            //{
            //  if (System.Global.IsHostName(AServerHost))
            //  {
            //    if (System.Global.IsHostName(_Domain))
            //    {
            //      return _Domain == Genetibase.Network.Sockets.Global.RightStr(AServerHost, _Domain.Length);
            //    }
            //    else
            //    {
            //      return _Domain == Genetibase.Network.Sockets.Global.DomainName(AServerHost);
            //    }
            //  }
            //  else
            //  {
            //    return _Domain.Equals(Genetibase.Network.Sockets.Global.DomainName(AServerHost));
            //  }
            //}
        }

        public string CookieText
        {
            get
            {
                return GetCookie();
            }
            set
            {
                SetCookie(value);
            }
        }

        public string ServerCookie
        {
            get
            {
                return GetServerCookie();
            }
        }

        public string ClientCookie
        {
            get
            {
                return GetClientCookie();
            }
        }

        public string Domain
        {
            get
            {
                return _Domain;
            }
            set
            {
                _Domain = value;
            }
        }

        public string Expires
        {
            get
            {
                return _Expires;
            }
            set
            {
                SetExpires(value);
            }
        }

        public string CookieName
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }

        public string Path
        {
            get
            {
                return _Path;
            }
            set
            {
                _Path = value;
            }
        }

        public bool Secure
        {
            get
            {
                return _Secure;
            }
            set
            {
                _Secure = value;
            }
        }

        public string Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
            }
        }
    }
}
