using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Network.Sockets
{
    internal class MyHeaderList 
    {
        private string _valueSeparator = ",";
        private bool _caseSensitive = true;

        private Dictionary<string, string> _headers = new Dictionary<string,string>();

        public void Add(string header, string value)
        {
            
            value = value.Trim();
            if (value == string.Empty) Remove(header);
            else
            {
                if (_headers.ContainsKey(header))
                    _headers[header] = value;
                else
                    _headers.Add(header, value);
            }
        }

        public void AddValue(string header, string value)
        {
            if (value == string.Empty) return;
            else
            {
                if (_headers.ContainsKey(header))
                    _headers[header] += _valueSeparator + value;
                else
                    _headers.Add(header, value);
            }
        }

        public string[] GetValues(string header)
        {
            List<string> result = new List<string>();
            foreach (string value in this[header].Split(new string[] { _valueSeparator }, StringSplitOptions.RemoveEmptyEntries))
                result.Add(value.Trim());
            return result.ToArray();
        }


        public bool Contains(string header)
        {
            return _headers.ContainsKey(header);
        }

        public ICollection<string> Headers
        {
            get { return _headers.Keys; }
        }

        public bool Remove(string header)
        {
            return _headers.Remove(header);
        }     

        public ICollection<string> Values
        {
            get { return _headers.Values; }
        }

        public string this[string header]
        {
            get
            {
                if (_headers.ContainsKey(header)) return _headers[header];
                else return string.Empty;
            }
            set
            {
                value = value != null ? value.Trim() : string.Empty;
                if (value == string.Empty) _headers.Remove(header);
                else
                    if (_headers.ContainsKey(header))
                        _headers[header] = value;
                    else
                        _headers.Add(header, value);
            }
        }


        public void Clear()
        {
            _headers.Clear();
        }

        public int Count
        {
            get { return _headers.Count; }
        }


    }
}
