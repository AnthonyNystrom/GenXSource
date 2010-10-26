using System;
using System.IO;

namespace Genetibase.NuGenRenderCore.Logging
{
    public class LogItem
    {
        public enum ItemLevel
        {
            DebugInfo = 1,
            Info = 2,
            StageInfo = 3,
            UserInfo = 4,
            Success = 5,
            Failure = 6,
            Warning = 7,
            Error = 8,
            Critical = 9
        }

        public string Message;
        public ItemLevel Level;

        public LogItem(string message, ItemLevel level)
        {
            Message = message;
            Level = level;
        }
    }

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
            output = new StreamWriter(this.file);
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