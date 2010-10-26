using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace NuGenSVisualLib.Logging
{
    public interface ILog : IDisposable
    {
        void AddItem(LogItem item);
    }

    class DirectFileLog : ILog
    {
        FileStream file;
        StreamWriter output;
 
        public DirectFileLog(string file)
        {
            this.file = new FileStream(file, FileMode.Create);
            this.output = new StreamWriter(this.file);
        }

        #region ILog Members

        public void AddItem(LogItem item)
        {
            output.Write(item.Level.ToString());
            output.WriteLine(item.Message);
            output.Flush();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            output.Close();
            file.Close();
        }

        #endregion
    }
}
