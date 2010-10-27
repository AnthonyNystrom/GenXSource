namespace FacScan
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct LoadSeqState
    {
        public bool Cancel;
        public int Start;
        public int Length;
        public bool Append;
    }
}

