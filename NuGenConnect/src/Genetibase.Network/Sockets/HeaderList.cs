using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Network.Sockets
{
    public class OldHeaderList : IList<string>
    {
        private const string QuoteChars = "\"";
        private const string LWS = "\t ";
        private string _NameValueSeparator;
        private bool _CaseSensitive;
        private bool _UnfoldLines;
        private bool _FoldLines;
        private int _FoldLinesLength;
        private List<string> mBackend = new List<string>();

        private string FoldWrapText(string Line, string BreakStr, string BreakChars, int MaxCol)
        {
            const string QuoteChars = "\"";
            int Col = 0;
            int Pos = 0;
            int LinePos = 0;
            int LineLen = Line.Length;
            int BreakLen = BreakStr.Length;
            int BreakPos = 0;
            char QuoteChar = ' ';
            char CurChar;
            bool ExistingBreak = false;
            string TempResult = "";
            while (Pos < LineLen)
            {
                CurChar = Line[Pos];
                if (CurChar == BreakStr[0])
                {
                    if (QuoteChar == ' ')
                    {
                        ExistingBreak = String.Equals(BreakStr, Line.Substring(Pos, BreakLen), StringComparison.InvariantCultureIgnoreCase);
                        if (ExistingBreak)
                        {
                            Pos += BreakLen;
                            BreakPos = Pos;
                        }
                    }
                }
                else
                {
                    if (BreakChars.IndexOf(CurChar) != -1)
                    {
                        if (QuoteChar == ' ')
                        {
                            BreakPos = Pos;
                        }
                    }
                    else
                    {
                        if (QuoteChars.IndexOf(CurChar) != -1)
                        {
                            if (CurChar == QuoteChar)
                            {
                                QuoteChar = ' ';
                            }
                            else
                            {
                                if (QuoteChar == ' ')
                                {
                                    QuoteChar = CurChar;
                                }
                            }
                        }
                    }
                }
                Pos++;
                Col++;
                if (QuoteChars.IndexOf(QuoteChar) == -1
                 && (ExistingBreak || (Col > MaxCol && BreakPos > LinePos)))
                {
                    Col = Pos - BreakPos;
                    TempResult += Line.Substring(LinePos, BreakPos - LinePos - 1);
                    if (QuoteChars.IndexOf(CurChar) == -1)
                    {
                        while (Pos < LineLen && (BreakChars + "\r\n").IndexOf(Line[Pos]) != -1)
                        {
                            Pos++;
                        }
                        if (!ExistingBreak
                           && Pos < LineLen)
                        {
                            TempResult += BreakStr;
                        }
                    }
                    BreakPos++;
                    LinePos = BreakPos;
                    ExistingBreak = false;
                }
            }
            TempResult = Line.Substring(LinePos);
            return TempResult;
        }

        protected void DeleteFoldedLines(int Index)
        {
            Index++;
            if (Index < Count)
            {
                while (Index < Count
                     && mBackend[Index].IndexOf(" \t") != -1)
                {
                    mBackend.RemoveAt(Index);
                }
            }
        }

        protected List<string> FoldLine(string AString)
        {
            List<string> TempResult = new List<string>();
            string TempString = FoldWrapText(AString, Genetibase.Network.Sockets.Global.EOL + " ", LWS + ',', _FoldLinesLength);
            while (TempString != "")
            {
                TempResult.Add(Genetibase.Network.Sockets.Global.Fetch(ref TempString, Genetibase.Network.Sockets.Global.EOL).TrimEnd());
            }
            return TempResult;
        }

        protected void FoldAndInsert(string AString, int Index)
        {
            List<string> TempStrings = FoldLine(AString);
            int Idx = TempStrings.Count - 1;
            this[Index] = TempStrings[Idx];
            Idx -= 1;
            while (Idx > -1)
            {
                mBackend.Insert(Index, TempStrings[Idx]);
                Idx -= 1;
            }
        }

        protected string GetName(int Index)
        {
            string TempResult = this[Index];
            int P = TempResult.IndexOf(_NameValueSeparator);
            if (P != 0)
            {
                TempResult.Remove(P - 1, 1);
            }
            else
            {
                TempResult = "";
            }
            return TempResult;
        }

        protected string GetValue(string AName)
        {
            return GetValueFromLine(IndexOfName(AName));
        }

        protected void SetValue(string Name, string Value)
        {
            int I = IndexOfName(Name);
            if (Value != "")
            {
                if (I < 0)
                {
                    I = Add("");
                }
                if (_FoldLines)
                {
                    DeleteFoldedLines(I);
                    FoldAndInsert(Name + _NameValueSeparator + Value, I);
                }
                else
                {
                    this[I] = Name + _NameValueSeparator + Value;
                }
            }
            else
            {
                if (I > -1)
                {
                    if (_FoldLines)
                    {
                        DeleteFoldedLines(I);
                    }
                    mBackend.RemoveAt(I);
                }
            }
        }

        protected string GetValueFromLine(int ALine)
        {
            string LFoldedLine;
            string LName;
            string TempResult = "";
            if (ALine > -1
              && ALine < Count)
            {
                LName = GetNameFromLine(ALine);
                if (String.IsNullOrEmpty(LName))
                {
                    return "";
                }
                TempResult = this[ALine];
                TempResult = TempResult.Substring(LName.Length + _NameValueSeparator.Length, TempResult.Length - LName.Length - _NameValueSeparator.Length);
                if (_UnfoldLines)
                {
                    while (true)
                    {
                        ALine++;
                        if (ALine == Count)
                        {
                            break;
                        }
                        LFoldedLine = this[ALine];
                        if (LWS.IndexOf(LFoldedLine[0]) == -1)
                        {
                            break;
                        }
                        TempResult = TempResult.Trim() + " " + LFoldedLine.Trim();
                    }
                }
            }
            TempResult = TempResult.Trim();
            return TempResult;
        }

        protected string GetNameFromLine(int ALine)
        {
            int P = 0;
            string TempResult = this[ALine];
            if (!_CaseSensitive)
            {
                TempResult = TempResult.ToUpper();
            }
            P = TempResult.IndexOf(_NameValueSeparator.TrimEnd());
            if (P == -1)
            {
                //throw new Exception("Separator not found. String = '" + TempResult + "'");
                return String.Empty;
            }
            return TempResult.Substring(0, P);
        }

        public OldHeaderList()
        {
            _NameValueSeparator = ": ";
            _CaseSensitive = false;
            _UnfoldLines = true;
            _FoldLines = true;
            _FoldLinesLength = 78;
        }

        public int IndexOfName(string AName)
        {
            for (int i = 0; i < Count; i++)
            {
                if (String.Equals(GetNameFromLine(i), AName, StringComparison.InvariantCultureIgnoreCase))
                {
                    return i;
                }
            }
            return -1;
        }

        public void AddStdValues(List<string> ASrc)
        {
            for (int i = 0; i < ASrc.Count; i++)
            {
                Add(ASrc[i].Replace("=", NameValueSeparator));
            }
        }

        public void ConvertToStdValues(List<string> ADest)
        {
            for (int i = 0; i < Count; i++)
            {
                ADest.Add(this[i].Replace(NameValueSeparator, "="));
            }
        }

        public void Extract(string AName, IList<string> ADest)
        {
            if (ADest == null)
            {
                return;
            }
            for (int i = 0; i < Count; i++)
            {
                if (String.Equals(AName, GetNameFromLine(i), StringComparison.InvariantCultureIgnoreCase))
                {
                    ADest.Add(GetValueFromLine(i));
                }
            }
        }

        public string Names(int Index)
        {
            return GetName(Index);
        }

        public string ValueFromIndex(int index)
        {
            return GetValue(GetName(index));
        }

        public string Values(string Name)
        {
            return GetValue(Name);
        }

        public void Values(string Name, string Value)
        {
            SetValue(Name, Value);
        }

        public string NameValueSeparator
        {
            get
            {
                return _NameValueSeparator;
            }
            set
            {
                _NameValueSeparator = value;
            }
        }

        public bool UnfoldLines
        {
            get
            {
                return _UnfoldLines;
            }
            set
            {
                _UnfoldLines = value;
            }
        }

        public bool FoldLines
        {
            get
            {
                return _FoldLines;
            }
            set
            {
                _FoldLines = value;
            }
        }

        public int FoldLength
        {
            get
            {
                return _FoldLinesLength;
            }
            set
            {
                _FoldLinesLength = value;
            }
        }

        public int Count
        {
            get
            {
                return mBackend.Count;
            }
        }

        public void Clear()
        {
            mBackend.Clear();
        }

        public int Add(string line)
        {
            mBackend.Add(line);
            return Count - 1;
        }

        public string this[int index]
        {
            get
            {
                return mBackend[index];
            }
            set
            {
                mBackend[index] = value;
            }
        }

        public string Text
        {
            get
            {
                string TempResult = "";
                foreach (string s in mBackend)
                {
                    TempResult += s + "\r\n";
                }
                if (TempResult.Length > 0)
                {
                    TempResult = TempResult.Substring(0, TempResult.Length - 2);
                }
                return TempResult;
            }
            set
            {
                mBackend.Clear();
                if (String.IsNullOrEmpty(value))
                {
                    return;
                }
                string[] TempItems = value.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                foreach (string s in TempItems)
                {
                    mBackend.Add(s);
                }
            }
        }

        public string this[string name]
        {
            get
            {
                return GetValue(name);
            }
        }

        #region IList<string> Members

        int IList<string>.IndexOf(string item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        void IList<string>.Insert(int index, string item)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        void IList<string>.RemoveAt(int index)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        string IList<string>.this[int index]
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        #endregion

        #region ICollection<string> Members

        void ICollection<string>.Add(string item)
        {
            mBackend.Add(item);
        }

        void ICollection<string>.Clear()
        {
            mBackend.Clear();
        }

        bool ICollection<string>.Contains(string item)
        {
            return mBackend.Contains(item);
        }

        void ICollection<string>.CopyTo(string[] array, int arrayIndex)
        {
            mBackend.CopyTo(array, arrayIndex);
        }

        int ICollection<string>.Count
        {
            get
            {
                return mBackend.Count;
            }
        }

        bool ICollection<string>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        bool ICollection<string>.Remove(string item)
        {
            return mBackend.Remove(item);
        }

        #endregion

        #region IEnumerable<string> Members

        IEnumerator<string> IEnumerable<string>.GetEnumerator()
        {
            return mBackend.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return mBackend.GetEnumerator();
        }

        #endregion
    }
}