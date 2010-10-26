using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace NuGenSVisualLib.Logging
{
    public interface ILogDataFormatting
    {
        void InsertItem(LogItem item);
        void InsertProcess(IProcessLoadingProgress process);
        bool WantProcesses { get; }
        bool WantItems { get; }
    }

    class LogDataStandardTextFormatting : ILogDataFormatting
    {
        StringBuilder data;

        public LogDataStandardTextFormatting()
        {
            data = new StringBuilder();
        }

        public string GetData()
        {
            return data.ToString();
        }

        #region ILogDataFormatting Members

        public void InsertItem(LogItem item)
        {
            data.AppendLine(item.Message);
        }

        public void InsertProcess(IProcessLoadingProgress process)
        { }

        public bool WantProcesses
        {
            get { return false; }
        }

        public bool WantItems
        {
            get { return true; }
        }

        #endregion
    }

    class LogDataTreeNodeFormatting : ILogDataFormatting
    {
        TreeNode root;

        public TreeNode GetRoot()
        {
            return root;
        }

        public LogDataTreeNodeFormatting()
        {
            root = new TreeNode("Processed");
        }

        #region ILogDataFormatting Members
        public void InsertItem(LogItem item)
        { }

        public void InsertProcess(IProcessLoadingProgress process)
        {
            TreeNode[] children = new TreeNode[process.LogDataSize];
            int idx = 0;
            foreach (LogItem item in process.LogData)
            {
                children[idx++] = new TreeNode(string.Format("{0} [{1}]", item.Message, item.Level), (int)item.Level, (int)item.Level);
            }

            root.Nodes.Add(new TreeNode(process.Name, children));
        }

        public bool WantProcesses
        {
            get { return true; }
        }

        public bool WantItems
        {
            get { return false; }
        }
        #endregion
    }
}
