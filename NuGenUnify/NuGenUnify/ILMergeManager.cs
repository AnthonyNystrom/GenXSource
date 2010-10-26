using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Microsoft.Win32;
using NuGenUnify.Properties;
using System.ComponentModel;
using System.Diagnostics;

namespace NuGenUnify
{
    public delegate void StatusEventHandler(object sender, StatusEventArgs e);
    public delegate void ErrorEventHandler(object sender, ErrorEventArgs e);
    public class ErrorEventArgs : EventArgs
    {
        private string _message = string.Empty;
        private Exception _exception = null;

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        public Exception Exception
        {
            get { return _exception; }
            set { _exception = value; }
        }

        public ErrorEventArgs(string message, Exception ex)
        {
            _message = message;
            _exception = ex;
        }
    }

    public class StatusEventArgs : EventArgs
    {
        private string _status = string.Empty;

        public string Status
        {
            get { return _status; }
            set { _status = value; }
        }

        public StatusEventArgs(string status)
        {
            _status = status;
        }
    }


    internal class ILMergeManager : IDisposable
    {
        private string defaultILMergePath = Environment.GetEnvironmentVariable("ProgramFiles") + @"\Microsoft\ILMerge\ILMerge.exe";

        public string PrimaryAssembly
        {
            get;
            set;
        }
        public List<string> OtherAssemblies
        {
            get;
            set;
        }
        public string AttributeFile
        {
            get;
            set;
        }
        public string ExcludeFile
        {
            get;
            set;
        }
        public string KeyFile
        {
            get;
            set;
        }
        public string LogFile
        {
            get;
            set;
        }
        public string OutputFile
        {
            get;
            set;
        }


        //    Thread mergeThread = null;
        //    public void Merge()
        //    {
        //        mergeThread = new Thread(new ThreadStart(DoMerge));
        //        mergeThread.Start();
        //    }

        //    private void DoMerge()
        //    {
        //        
        //    }

        public event StatusEventHandler StatusChanged;
        public event ErrorEventHandler Error;


        protected void OnStatusChanged(string status)
        {
            if (StatusChanged != null)
                StatusChanged(null, new StatusEventArgs(status));
        }

        protected void OnError(string messsage, Exception ex)
        {
            if (Error != null)
                Error(null, new NuGenUnify.ErrorEventArgs(messsage, ex));
        }


        #region IDisposable Members

        public void Dispose()
        {
            //if (mergeThread != null && mergeThread.ThreadState == ThreadState.Running )
            //    mergeThread.Abort();
        }

        #endregion

        public bool CheckILMerge()
        {
            if (File.Exists(Settings.Default.ILMergePath))
                return true;
            if (File.Exists(defaultILMergePath))
            {
                Settings.Default.ILMergePath = defaultILMergePath;
                Settings.Default.Save();
                return true;
            }
            else
            {
                OpenFileDialog openFile = new OpenFileDialog();
                openFile.InitialDirectory = "C:\\";
                openFile.Filter = "ILMerge Executable|ILMerge.exe";
                openFile.Title = "Choose ILMerge utility file";
                if (openFile.ShowDialog().GetValueOrDefault(false))
                {
                    Settings.Default.ILMergePath = openFile.FileName;
                    Settings.Default.Save();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        internal void Merge()
        {
            Type ilMergeType = null;
            Type ilMergeKind = null;
            try
            {
                Assembly assembly = Assembly.LoadFrom(Settings.Default.ILMergePath);

                ilMergeType = assembly.GetType("ILMerging.ILMerge");
                ilMergeKind = assembly.GetType("ILMerging.ILMerge+Kind");
            }
            catch (Exception e)
            {
                OnError("Can't create ILMerge object.", e);
                return;
            }

            try
            {
                //Create ILMerge object
                Object[] args = new Object[] { };
                Object obj = ilMergeType.InvokeMember(null,
                    BindingFlags.DeclaredOnly |
                    BindingFlags.Public | BindingFlags.NonPublic |
                    BindingFlags.Instance | BindingFlags.CreateInstance, null, null, args);

                ilMergeType.InvokeMember("Log",
                    BindingFlags.DeclaredOnly |
                    BindingFlags.Public | BindingFlags.NonPublic |
                    BindingFlags.Instance | BindingFlags.SetProperty, null, obj, new Object[] { Settings.Default.ShouldLog });

                Object objKind = ilMergeKind.InvokeMember(null,
                    BindingFlags.DeclaredOnly |
                    BindingFlags.Public | BindingFlags.NonPublic |
                    BindingFlags.Instance | BindingFlags.CreateInstance, null, null, args);

                ilMergeKind.InvokeMember("value__",
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static |
                    BindingFlags.Instance | BindingFlags.SetField, null, objKind, new Object[] { (int)Settings.Default.TargetKind });

                ilMergeType.InvokeMember("TargetKind",
                    BindingFlags.DeclaredOnly |
                    BindingFlags.Public | BindingFlags.NonPublic |
                    BindingFlags.Instance | BindingFlags.SetProperty, null, obj, new Object[] { objKind });

                ilMergeType.InvokeMember("AllowWildCards",
                    BindingFlags.DeclaredOnly |
                    BindingFlags.Public | BindingFlags.NonPublic |
                    BindingFlags.Instance | BindingFlags.SetProperty, null, obj, new Object[] { Settings.Default.AllowWildCards });

                ilMergeType.InvokeMember("AllowZeroPeKind",
                    BindingFlags.DeclaredOnly |
                    BindingFlags.Public | BindingFlags.NonPublic |
                    BindingFlags.Instance | BindingFlags.SetProperty, null, obj, new Object[] { Settings.Default.AllowZeroPEKind });

                ilMergeType.InvokeMember("Closed",
                    BindingFlags.DeclaredOnly |
                    BindingFlags.Public | BindingFlags.NonPublic |
                    BindingFlags.Instance | BindingFlags.SetProperty, null, obj, new Object[] { Settings.Default.Closed });

                PropertyInfo pi = ilMergeType.GetProperty("XmlDocumentation");
                if (pi != null)
                {
                    ilMergeType.InvokeMember("XmlDocumentation",
                        BindingFlags.DeclaredOnly |
                        BindingFlags.Public | BindingFlags.NonPublic |
                        BindingFlags.Instance | BindingFlags.SetProperty, null, obj, new Object[] { Settings.Default.XmlDocumentation });
                }

                ilMergeType.InvokeMember("CopyAttributes",
                    BindingFlags.DeclaredOnly |
                    BindingFlags.Public | BindingFlags.NonPublic |
                    BindingFlags.Instance | BindingFlags.SetProperty, null, obj, new Object[] { Settings.Default.CopyAttributes });

                ilMergeType.InvokeMember("DebugInfo",
                    BindingFlags.DeclaredOnly |
                    BindingFlags.Public | BindingFlags.NonPublic |
                    BindingFlags.Instance | BindingFlags.SetProperty, null, obj, new Object[] { Settings.Default.DebugInfo });

                ilMergeType.InvokeMember("Internalize",
                    BindingFlags.DeclaredOnly |
                    BindingFlags.Public | BindingFlags.NonPublic |
                    BindingFlags.Instance | BindingFlags.SetProperty, null, obj, new Object[] { Settings.Default.Internalize });

                ilMergeType.InvokeMember("PublicKeyTokens",
                    BindingFlags.DeclaredOnly |
                    BindingFlags.Public | BindingFlags.NonPublic |
                    BindingFlags.Instance | BindingFlags.SetProperty, null, obj, new Object[] { Settings.Default.PublicKeyTokens });

                pi = ilMergeType.GetProperty("DelaySign");
                if (pi != null)
                {
                    ilMergeType.InvokeMember("DelaySign",
                        BindingFlags.DeclaredOnly |
                        BindingFlags.Public | BindingFlags.NonPublic |
                        BindingFlags.Instance | BindingFlags.SetProperty, null, obj, new Object[] { Settings.Default.DelaySign });
                }


                pi = ilMergeType.GetProperty("AllowDuplicateType");
                if (Settings.Default.AllowDuplicateType && pi != null)
                {
                    object[] dublArgs;
                    if (Settings.Default.DuplicateTypeName == "All types")
                        dublArgs = new object[] { null };
                    else
                        dublArgs = new object[] { Settings.Default.DuplicateTypeName };

                    ilMergeType.InvokeMember("AllowDuplicateType",
                        BindingFlags.DeclaredOnly |
                        BindingFlags.Public | BindingFlags.NonPublic |
                        BindingFlags.Instance | BindingFlags.InvokeMethod, null, obj, dublArgs);
                }

                pi = ilMergeType.GetProperty("Version");

                if (pi != null && Settings.Default.Version.Trim() != "")
                {
                    Version version = new Version(Settings.Default.Version);
                    ilMergeType.InvokeMember("Version",
                        BindingFlags.DeclaredOnly |
                        BindingFlags.Public | BindingFlags.NonPublic |
                        BindingFlags.Instance | BindingFlags.SetProperty, null, obj, new Object[] { version });
                }


                if (!string.IsNullOrEmpty(KeyFile))
                {
                    ilMergeType.InvokeMember("SnkFile",
                        BindingFlags.DeclaredOnly |
                        BindingFlags.Public | BindingFlags.NonPublic |
                        BindingFlags.Instance | BindingFlags.SetProperty, null, obj, new Object[] { KeyFile });

                }
                if (!string.IsNullOrEmpty(AttributeFile))
                {
                    ilMergeType.InvokeMember("AttributeFile",
                        BindingFlags.DeclaredOnly |
                        BindingFlags.Public | BindingFlags.NonPublic |
                        BindingFlags.Instance | BindingFlags.SetProperty, null, obj, new Object[] { AttributeFile });

                }
                if (!string.IsNullOrEmpty(ExcludeFile))
                {
                    ilMergeType.InvokeMember("ExcludeFile",
                        BindingFlags.DeclaredOnly |
                        BindingFlags.Public | BindingFlags.NonPublic |
                        BindingFlags.Instance | BindingFlags.SetProperty, null, obj, new Object[] { ExcludeFile });
                }

                if (!string.IsNullOrEmpty(LogFile))
                {
                    ilMergeType.InvokeMember("LogFile",
                        BindingFlags.DeclaredOnly |
                        BindingFlags.Public | BindingFlags.NonPublic |
                        BindingFlags.Instance | BindingFlags.SetProperty, null, obj, new Object[] { LogFile });
                }
                ilMergeType.InvokeMember("OutputFile",
                    BindingFlags.DeclaredOnly |
                    BindingFlags.Public | BindingFlags.NonPublic |
                    BindingFlags.Instance | BindingFlags.SetProperty, null, obj, new Object[] { OutputFile });

                string[] assemblies = new string[OtherAssemblies.Count + 1];
                assemblies[0] = PrimaryAssembly;
                int i = 1;
                foreach (string item in OtherAssemblies )
                {
                    assemblies[i] = item;
                    i++;
                }
                ilMergeType.InvokeMember("SetInputAssemblies",
                    BindingFlags.DeclaredOnly |
                    BindingFlags.Public | BindingFlags.NonPublic |
                    BindingFlags.Instance | BindingFlags.InvokeMethod, null, obj, new Object[] { assemblies });


                BackgroundWorker logReaderWorker = new BackgroundWorker();
                logReaderWorker.DoWork += new DoWorkEventHandler(logReaderWorker_DoWork);
                logReaderWorker.WorkerSupportsCancellation = true;
                logReaderWorker.RunWorkerAsync();
                

                ilMergeType.InvokeMember("Merge",
                    BindingFlags.DeclaredOnly |
                    BindingFlags.Public | BindingFlags.NonPublic |
                    BindingFlags.Instance | BindingFlags.InvokeMethod, null, obj, new Object[] { });

                obj = null;
                objKind = null;
                ilMergeType = null;
                ilMergeKind = null;
                logReaderWorker.CancelAsync();
            }
            catch (Exception ex)
            {
                OnError("Merge error", ex);
            }
        }

        void logReaderWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true && !((BackgroundWorker)sender).CancellationPending)
            {
                try
                {
                    ReadLog();
                    Thread.Sleep(500);
                }
                catch
                {

                }
            }
        }

        string lastLogEntry = "";
        private void ReadLog()
        {
            if (File.Exists(LogFile))
            {
                if (lastLogEntry.Length>0)
                    lastLogEntry = File.ReadAllText(LogFile).Replace(lastLogEntry, "");
                else
                    lastLogEntry = File.ReadAllText(LogFile);
                Trace.WriteLine(lastLogEntry);
                OnStatusChanged(lastLogEntry);
            }
        }
    }
}
