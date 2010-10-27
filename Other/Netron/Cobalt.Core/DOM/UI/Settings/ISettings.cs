using System;
using System.Collections.Generic;
using System.Text;

namespace Netron.Cobalt
{
    internal interface ISettings: IDisposable
    {
        MainForm MainForm { get;set;}
        void Load();
        void Save();
    }
}
