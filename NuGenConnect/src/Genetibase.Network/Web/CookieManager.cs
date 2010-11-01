using System;
using System.Collections.Generic;

using Genetibase.Network.Sockets.Protocols;

namespace Genetibase.Network.Web {
  public class CookieManager {
    protected CookieManagerEventHandler _OnCreate;
    protected CookieManagerEventHandler _OnDestroy;
    protected CookieManagerNewCookieEventHandler _OnNewCookie;
    protected CookieCollection _Cookies;

    protected void CleanupCookieList() {
      try {
        string S = "";
        CookieList LCookieList;
        if (_Cookies != null) {
          lock (_Cookies.GetCookieListByDomain()) {
            if (_Cookies.GetCookieListByDomain() != null) {
              if (_Cookies.GetCookieListByDomain().Count > 0) {
                for (int i = 0; i < _Cookies.GetCookieListByDomain().Count; i++) {
                  LCookieList = (CookieList)_Cookies.GetCookieListByDomain().Values[i];
                  for (int j = LCookieList.Count - 1; j >= 0; j--) {
                    S = LCookieList.Cookie(j).Expires;
                    if (S.Length > 0
											&& Http.GmtToLocalDateTime(S) < DateTime.Now) {
                      LCookieList.RemoveAt(j);
                    }
                  }
                }
              }
            }
          }
        }
      } catch {
      }
    }

    protected void DoAdd(RFC2109Cookie ACookie, string ACookieText, string AHost) {
      string LDomain;
      ACookie.CookieText = ACookieText;
      if (ACookie.Domain.Length == 0) {
        LDomain = AHost;
      } else {
        LDomain = ACookie.Domain;
      }
      ACookie.Domain = LDomain;
      if (ACookie.IsValidCookie(AHost)) {
        if (DoOnNewCookie(ACookie)) {
          _Cookies.AddCookie(ACookie);
        } else {
          using (ACookie) {
          }
        }
      } else {
        using (ACookie) {
        }
      }
    }

    protected virtual void DoOnCreate() {
      if (_OnCreate != null) {
        _OnCreate(this, _Cookies);
      }
    }

    protected virtual void DoOnDestroy() {
      if (_OnDestroy != null) {
        _OnDestroy(this, _Cookies);
      }
    }

    protected virtual bool DoOnNewCookie(RFC2109Cookie ACookie) {
      bool TempResult = true;
      if (_OnNewCookie != null) {
        _OnNewCookie(this, ACookie, ref TempResult);
      }
      return TempResult;
    }

    public CookieManager() {
      _Cookies = new CookieCollection();
    }

    ~CookieManager() {
      CleanupCookieList();
      DoOnDestroy();
      _Cookies = null;
    }

    public void AddCookie(string ACookie, string AHost) {
      DoAdd(_Cookies.Add(), ACookie, AHost);
    }

    public void AddCookie2(string ACookie, string AHost) {
      DoAdd(_Cookies.Add2(), ACookie, AHost);
    }

    public string GenerateCookieList(URI AUri) {
      return GenerateCookieList(AUri, false);
    }

    public string GenerateCookieList(URI AUri, bool SecureConnection) {
      string S = "";
      CookieList LCookieList;
      SortedList<string, string> LResultList;
      CleanupCookieList();
      lock (_Cookies.GetCookieListByDomain()) {
        if (_Cookies.GetCookieListByDomain().Count > 0) {
          LResultList = new SortedList<string,string>();
          for (int i = 0; i < _Cookies.GetCookieListByDomain().Count; i++) {
            if (_Cookies.GetCookieListByDomain().IndexOfKey(AUri.Host) > 0) {
              LCookieList = (CookieList)_Cookies.GetCookieListByDomain()[AUri.Host];
              for (int j = 0; j < LCookieList.Count; i++) {
                if (LCookieList.Cookie(j).Path.IndexOf(AUri.Path) == 0) {
                  if ((LCookieList.Cookie(j).Secure || SecureConnection)
                    && (LCookieList.Cookie(j).Value != "")) {
                    LResultList.Add(LCookieList.Cookie(j).CookieName, LCookieList.Cookie(j).Value);
                  }
                }
              }
            }
          }
          for (int i = LResultList.Count; i > 0; i--) {
            if (S.Length > 0) {
              S += "; ";
            }
            S += LResultList.Keys[i] + "=" + LResultList.Values[i];
          }

        }
      }
      return S;
    }

    public CookieCollection Cookies {
      get {
        return _Cookies;
      }
    }

    public event CookieManagerEventHandler OnCreate {
      add {
        _OnCreate += value;
      }
      remove {
        _OnCreate -= value;
      }
    }

    public event CookieManagerEventHandler OnDestroy {
      add {
        _OnDestroy += value;
      }
      remove {
        _OnDestroy -= value;
      }
    }

    public event CookieManagerNewCookieEventHandler OnNewCookie {
      add {
        _OnNewCookie += value;
      }
      remove {
        _OnNewCookie -= value;
      }
    }
  }
}
