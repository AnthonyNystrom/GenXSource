using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace NuGenSVisualLib.Logging
{
    public class LoadingProgress
    {
        int numExpectedProcesses;
        List<IProcessLoadingProgress> processed;

        float progress;
        float currentProcessRange;
        int progressI;

        public delegate void ProgressUpdateHandler(LoadingProgress progress);
        public delegate void ProcessUpdateHandler(IProcessLoadingProgress process);

        public event ProgressUpdateHandler OnUpdate;
        public event ProcessUpdateHandler OnProcessUpdate;

        public LoadingProgress(int numExpectedProcesses)
        {
            this.numExpectedProcesses = numExpectedProcesses;

            processed = new List<IProcessLoadingProgress>();
            progress = 0;
        }

        public int ProgressPercent
        {
            get { return progressI; }
        }

        public string CurrentProcessName
        {
            get { return processed[processed.Count - 1].Name; }
        }

        public void MoveToNextProcess(IProcessLoadingProgress process)
        {
            // determin range
            currentProcessRange = (1.0f - progress) / (numExpectedProcesses - processed.Count);

            processed.Add(process);
            UpdateProcess(process, 0f);
        }

        public void ExpandProcesses(int byNumber)
        {
            numExpectedProcesses += byNumber;
        }

        public void UpdateProcess(IProcessLoadingProgress process, float progress)
        {
            if (OnProcessUpdate != null)
                OnProcessUpdate(process);

            this.progress += (currentProcessRange * progress);
            if ((int)(this.progress * 100) != progressI)
            {
                progressI = (int)(this.progress * 100);
                if (OnUpdate != null)
                    OnUpdate(this);
            }
        }

        public void CollateLogData(LogItem.ItemLevel levelOfInterest, ILogDataFormatting formatting)
        {
            foreach (IProcessLoadingProgress process in processed)
            {
                if (formatting.WantProcesses)
                    formatting.InsertProcess(process);
                if (formatting.WantItems)
                {
                    foreach (LogItem item in process.LogData)
                    {
                        if (item.Level >= levelOfInterest)
                            formatting.InsertItem(item);
                    }
                }
            }
        }
    }

    public interface IProcessLoadingProgress
    {
        string Name { get; }
        int ProgressPercent { get; }
        float Progress { get; }
        IEnumerable<LogItem> LogData { get; }
        int LogDataSize { get; }

        void UpdateProgress(float byValue);
        void Log(string text, LogItem.ItemLevel level);
        void Log(LogItem item);
        void LogInfo(string infoMsg);
    }

    abstract class ProcessLoadingProgress : IProcessLoadingProgress
    {
        private string name;
        private float progress;
        private LoadingProgress loadingProgress;
        protected List<LogItem> log;

        public ProcessLoadingProgress(string name, LoadingProgress loadingProgress)
        {
            this.name = name;
            this.progress = 0;
            this.loadingProgress = loadingProgress;
            this.log = new List<LogItem>();
        }

        #region IProcessLoadingProgress Members

        public string Name
        {
            get { return name; }
        }

        public int ProgressPercent
        {
            get { return (int)(progress * 100); }
        }

        public float Progress
        {
            get { return progress; }
        }

        public IEnumerable<LogItem> LogData
        {
            get { return log; }
        }

        public void UpdateProgress(float byValue)
        {
            progress += byValue;
            loadingProgress.UpdateProcess(this, byValue);
            //Thread.Sleep(50);
        }

        public void Log(string text, LogItem.ItemLevel level)
        {
            log.Add(new LogItem(text, level));
        }

        public void Log(LogItem item)
        {
            log.Add(item);
        }

        public void LogInfo(string infoMsg)
        {
            log.Add(new LogItem(infoMsg, LogItem.ItemLevel.Info));
        }

        public int LogDataSize
        {
            get { return log.Count; }
        }
        #endregion
    }

    public class LogItem
    {
        public enum ItemLevel
        {
            DebugInfo   = 1,
            Info        = 2,
            StageInfo   = 3,
            UserInfo    = 4,
            Success     = 5,
            Failure     = 6,
            Warning     = 7,
            Error       = 8,
            Critical    = 9
        }

        public string Message;
        public ItemLevel Level;

        public LogItem(string message, ItemLevel level)
        {
            this.Message = message;
            this.Level = level;
        }
    }
}