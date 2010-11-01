using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;
using System.Collections;

namespace Genetibase.Network.Sockets
{
    public class HeaderList : IList<string>
    {
        private HeadersNameValueCollection _headers;
        private string _nameValueSeparator;

        public HeaderList(bool ignoreCase, string nameValueSeparator, string valueSeparator)
        {
            _nameValueSeparator = nameValueSeparator;
            _headers = new HeadersNameValueCollection(ignoreCase, valueSeparator);
        }
        public HeaderList(bool ignoreCase)
            : this(ignoreCase, ": ", ",")
        {}
        public HeaderList()
            : this(true, ": ", ",")
        { }


        public void Remove(string header)
        {
            _headers.Remove(header);
        }

        public void Add(string header, string value)
        {
            _headers.Add(header, value);
        }

        public bool AddRawHeaderString(string rawHeaderString)
        {
            string[] parts = rawHeaderString.Split(new string[] { _nameValueSeparator }, StringSplitOptions.None);
            if (parts.Length != 2) return false;
            _headers.Add(parts[0], parts[1]);
            return true;
        }
        
        public string GetRawHeaderString(string header)
        {
            return string.Format("{0}{1}{2}", header, _nameValueSeparator, _headers.Get(header));
        }

        public string GetRawHeaderString(int index)
        {
            return string.Format("{0}{1}{2}", Headers[index], _nameValueSeparator, _headers.Get(index));
        }
       
        public string Get(int index)
        {
            return _headers.Get(index);
        }
        
        public string Get(string header)
        {
            return _headers.Get(header);
        }

        public string[] GetValues(string header)
        {
            string[] temp = _headers.GetValues(header);
            return temp != null ? temp : new string[] { };
        }

        public string[] GetValues(int index)
        {
            string[] temp = _headers.GetValues(index);
            return temp != null ? temp : new string[] { };
        }

        public void Clear()
        {
            _headers.Clear();
        }

        public string this[int index]
        {
            get { return GetRawHeaderString(index); }
#warning remove setter
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public string this[string header]
        {
            get { return _headers[header]; }
            set { _headers[header] = value;}
        }

        public string[] Headers
        {
            get { return _headers.AllKeys; }
        }

        public int Count
        {
            get { return _headers.Count; }
        }




        #region Old HeaderList support methods

        public void Values(string header, string value)
        {
            this[header] = value;
        }

        public string Values(string header)
        {
            return Get(header);
        }

        public string Names(int index)
        {
            return _headers.AllKeys[index];
        }

        public void ConvertToStdValues(List<string> TempList)
        {
        
        }
        public void AddStdValues(List<string> TempList)
        {
            foreach (string value in TempList)
            {
                string[] parts = value.Split('=');
                this.Add(parts[0],parts[1]);
            }        
        }

        public void Extract(string AName, IList<string> ADest)
        {
            if (ADest == null)
            {
                return;
            }

            foreach (string value in this.GetValues(AName))
            {
                ADest.Add(value);                
            }
        }

        public string NameValueSeparator
        {
            get { return _nameValueSeparator; }
        }

        public bool FoldLines
        {
            get
            {
                return false;
            }
            set
            {                
            }
        }

        public int FoldLength
        {
            get
            {
                return 0;
            }
            set
            {                
            }
        }

        public string ValueFromIndex(int index)
        {
            return this[index];
        }

        #endregion


        #region IList<string> Members

        public int IndexOf(string item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Insert(int index, string item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void RemoveAt(int index)
        {
            throw new Exception("The method or operation is not implemented.");
        }
       
        #endregion

        #region ICollection<string> Members

        public void Add(string item)
        {
            this.AddRawHeaderString(item);
        }

        public bool Contains(string item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool IsReadOnly
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        bool ICollection<string>.Remove(string item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IEnumerable<string> Members

        public IEnumerator<string> GetEnumerator()
        {
            foreach (string header in Headers)
            {
                yield return GetRawHeaderString(header);
            }
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }

    internal class HeadersNameValueCollection : NameValueCollection
    {
        private static OrdinalComparer _caseSensitiveComparer;
        private static OrdinalComparer _caseInsensitiveComparer;

        static HeadersNameValueCollection()
        {
            _caseSensitiveComparer = new OrdinalComparer(false);
            _caseInsensitiveComparer = new OrdinalComparer(true);
        }

        private string[] _valueSeparator = null;

        public HeadersNameValueCollection(bool ignoreCase, string valueSeparator) 
            : base (ignoreCase ? _caseInsensitiveComparer : _caseSensitiveComparer)
        {
            _valueSeparator = new string[] { valueSeparator };
        }

        public HeadersNameValueCollection(bool ignoreCase) : this (ignoreCase, ",")
        {}

        public override void Add(string name, string value)
        {
            if (value == null || value.Trim() == string.Empty) return;
            base.Add(name, value);
        }

        public override string Get(int index)
        {
            string temp = GetAsOneString((ArrayList)base.BaseGet(index));
            return temp != null ? temp : string.Empty;
        }

        public override string Get(string name)
        {
            string temp = GetAsOneString((ArrayList)base.BaseGet(name));
            return temp != null ? temp : string.Empty;
        }

        private string GetAsOneString(ArrayList list)
        {
            int count = (list != null) ? list.Count : 0;
            if (count == 1)
            {
                return (string)list[0];
            }
            if (count <= 1)
            {
                return null;
            }
            StringBuilder sb = new StringBuilder((string)list[0]);
            for (int i = 1; i < count; i++)
            {
                sb.Append(_valueSeparator[0]);
                sb.Append((string)list[i]);
            }
            return sb.ToString();
        }

        public new string this[string name]
        {
            get { return base[name]; }
            set 
            {
                Remove(name);
                Add(name, value);
            }
        }
    }

    
    [Serializable]
    internal sealed class OrdinalComparer : StringComparer
    {
        internal OrdinalComparer(bool ignoreCase)
        {
            this._ignoreCase = ignoreCase;
        }

        public override int Compare(string x, string y)
        {
            if (object.ReferenceEquals(x, y))
            {
                return 0;
            }
            if (x == null)
            {
                return -1;
            }
            if (y == null)
            {
                return 1;
            }
            if (this._ignoreCase)
            {
                return string.Compare(x, y, true);
            }
            return string.CompareOrdinal(x, y);
        }

        public override bool Equals(object obj)
        {
            OrdinalComparer comparer1 = obj as OrdinalComparer;
            if (comparer1 == null)
            {
                return false;
            }
            return (this._ignoreCase == comparer1._ignoreCase);
        }

        public override bool Equals(string x, string y)
        {
            if (object.ReferenceEquals(x, y))
            {
                return true;
            }
            if ((x == null) || (y == null))
            {
                return false;
            }
            if (!this._ignoreCase)
            {
                return x.Equals(y);
            }
            if (x.Length != y.Length)
            {
                return false;
            }
            return (string.Compare(x, y, true) == 0);
        }

        public override int GetHashCode()
        {
            int num1 = "OrdinalComparer".GetHashCode();
            if (!this._ignoreCase)
            {
                return num1;
            }
            return ~num1;
        }

        public override int GetHashCode(string obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }
            if (this._ignoreCase)
            {
                return obj.ToLower().GetHashCode();
            }
            return obj.GetHashCode();
        }

        private bool _ignoreCase;
    }

}
