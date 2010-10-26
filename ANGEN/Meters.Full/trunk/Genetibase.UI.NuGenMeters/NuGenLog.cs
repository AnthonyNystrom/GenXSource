/* -----------------------------------------------
 * NuGenLog.cs
 * Copyright © 2006 Alex Nesterov
 * --------------------------------------------- */

using Genetibase.Shared.Design;
using Genetibase.Shared;
using Genetibase.UI.NuGenMeters.ComponentModel;

using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.UI.NuGenMeters
{
	/// <summary>
	/// Implements a component to log changes on multiple <see cref="T:Genetibase.Shareed.INuGenCounter"/> sources.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	[DefaultEvent("LogOutputChanged")]
	[DefaultProperty("LogOutput")]
	[ProvideProperty("Log", typeof(INuGenCounter))]
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenLog), "Toolbox.NuGen.bmp")]
	public class NuGenLog : Component, IExtenderProvider
	{
		#region IExtenderProvider Members

		/// <summary>
		/// Specifies whether this object can provide its extender properties to the specified object.
		/// </summary>
		/// <param name="extendee">The Object to receive the extender properties.</param>
		/// <returns>If this object can provide extender properties to the specified object returns <c>true</c>; otherwise, <c>false</c>.</returns>
		public bool CanExtend(object extendee)
		{
			return extendee is INuGenCounter;
		}

		#endregion

		#region Declarations

		private StreamWriter fileStreamWriter = null;

		#endregion
		
		#region Properties.Behavior
		
		/*
		 * LogOutput
		 */

		/// <summary>
		/// Determines the path to write the log file to.
		/// </summary>
		private string logOutput = "";

		/// <summary>
		/// Gets or sets the output path for the log.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("BehaviorCategory")]
		[NuGenSRDescription("LogOutputDescription")]
		[Editor(typeof(NuGenOutputEditor), typeof(UITypeEditor))]
		public virtual string LogOutput
		{
			get 
			{ 
				if (this.logOutput == "") 
				{
					return Properties.Resources.OutputLogFileName;
				}
				else 
				{
					return this.logOutput;
				}
			}
			set 
			{
				FileInfo checkFileInfo = new FileInfo(value);

				if (checkFileInfo.Extension == "") 
				{
					throw new ArgumentException(Properties.Resources.LogOutputException);
				}
				if (this.logOutput != value)
				{
					this.logOutput = value;
					this.OnLogOutputChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object EventLogOutputChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenLog.LogOutput"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("LogOutputChangedDescription")]
		public event EventHandler LogOutputChanged
		{
			add
			{
				this.Events.AddHandler(EventLogOutputChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventLogOutputChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenLog.LogOutputChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnLogOutputChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventLogOutputChanged, e);
		}

		/*
		 * MaxLogFileSize
		 */

		/// <summary>
		/// Determines the maximum log file size in KB.
		/// </summary>
		private long maxLogFileSize = 1024L;

		/// <summary>
		/// Gets or sets the maximum log file size in KB.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("BehaviorCategory")]
		[NuGenSRDescription("MaxLogFileSizeDescription")]
		[DefaultValue(1024L)]
		public virtual long MaxLogFileSize
		{
			get { return this.maxLogFileSize; }
			set 
			{
				if (this.maxLogFileSize != value)
				{
					this.maxLogFileSize = value;
					this.OnMaxLogFileSizeChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object EventMaxLogFileSizeChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenLog.MaxLogFileSize"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("MaxLogFileSizeChangedDescription")]
		public event EventHandler MaxLogFileSizeChanged
		{
			add
			{
				this.Events.AddHandler(EventMaxLogFileSizeChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventMaxLogFileSizeChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenLog.MaxLogFileSizeChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnMaxLogFileSizeChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventMaxLogFileSizeChanged, e);
		}

		/*
		 * SplitLogFile
		 */

		/// <summary>
		/// Indicates whether to split a log file if its size exceeds MaxLogFileSize.
		/// </summary>
		private bool splitLogFile = false;

		/// <summary>
		/// Gets or sets a value indicating whether to split a log file if its size exceeds MaxLogFileSize.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("BehaviorCategory")]
		[NuGenSRDescription("SplitLogFileDescription")]
		[DefaultValue(false)]
		public virtual bool SplitLogFile
		{
			get { return this.splitLogFile; }
			set 
			{
				if (this.splitLogFile != value) 
				{
					this.splitLogFile = value;
					this.OnSplitLogFileChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object EventSplitLogFileChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenLog.SplitLogFile"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("SplitLogFileChangedDescription")]
		public event EventHandler SplitLogFileChanged
		{
			add
			{
				this.Events.AddHandler(EventSplitLogFileChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventSplitLogFileChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenLog.SplitLogFileChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnSplitLogFileChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventSplitLogFileChanged, e);
		}

		#endregion

		#region Properties.Counter

		/*
		 * Digits
		 */

		/// <summary>
		/// Determines the number of digits in the postfix.
		/// </summary>
		private int digits = 3;

		/// <summary>
		/// Gets or sets the number of digits in the postfix.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("CounterCategory")]
		[NuGenSRDescription("DigitsDescription")]
		[DefaultValue(3)]
		public int Digits
		{
			get
			{
				return this.digits;
			}
			set
			{
				if (this.digits != value)
				{
					if (value < 1)
					{
						throw new ArgumentException(Properties.Resources.DigitsException);
					}

					this.digits = value;
					this.OnDigitsChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object EventDigitsChangedDescription = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenLog.Digits"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("DigitsChangedDescription")]
		public event EventHandler DigitsChanged
		{
			add
			{
				this.Events.AddHandler(EventDigitsChangedDescription, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventDigitsChangedDescription, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenLog.DigitsChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnDigitsChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventDigitsChangedDescription, e);
		}

		/*
		 * Separator
		 */

		/// <summary>
		/// Determines the separator of the postfix.
		/// </summary>
		private string separator = "_";

		/// <summary>
		/// Gets or sets the separator of the postfix.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("CounterCategory")]
		[NuGenSRDescription("SeparatorDescription")]
		[DefaultValue("_")]
		public string Separator
		{
			get { return this.separator; }
			set 
			{
				if (!this.CheckFileName(value)) 
				{
					throw new ArgumentException(
						string.Format(Properties.Resources.CheckFileNameException, Environment.NewLine, "\\ / : * ? \" < > |")
					);
				}
				if (value != null) 
				{
					this.separator = value;
					this.OnSeparatorChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object EventSeparatorChanged = new object();

		/// <summary>
		/// Occurs when the value of <see cref="P:Genetibase.UI.NuGenMeters.NuGenLog.Separator"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[NuGenSRDescription("SeparatorChangedDescription")]
		public event EventHandler SeparatorChanged
		{
			add
			{
				this.Events.AddHandler(EventSeparatorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventSeparatorChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenLog.SeparatorChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnSeparatorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventSeparatorChanged, e);
		}

		/*
		 * Step
		 */

		/// <summary>
		/// Determines the step of the counter.
		/// </summary>
		private int step = 1;

		/// <summary>
		/// Gets or sets the step of the counter.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("CounterCategory")]
		[NuGenSRDescription("CounterStepDescription")]
		[DefaultValue(1)]
		public int Step
		{
			get { return this.step; }
			set 
			{
				if (this.step != value) 
				{
					if (value < 1) 
					{
						throw new ArgumentException(Properties.Resources.CounterStepException);
					}

					this.step = value;
					this.OnStepChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object EventStepChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.UI.NuGenMeters.NuGenLog.Step"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("PropertyChangedCategory")]
		[Description("StepChangedDescription")]
		public event EventHandler StepChanged
		{
			add
			{
				this.Events.AddHandler(EventStepChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(EventStepChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.UI.NuGenMeters.NuGenLog.StepChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnStepChanged(EventArgs e)
		{
			this.InvokePropertyChanged(EventStepChanged, e);
		}

		#endregion

		#region Properties.Extended.Log

		/// <summary>
		/// Contains all the controls of type <see cref="T:Genetibase.UI.NuGenMeters.NuGenMeterBase"/> on the form.
		/// </summary>
		private Dictionary<INuGenCounter, bool> logTargets = new Dictionary<INuGenCounter, bool>();

		/// <summary>
		/// Gets the value of the <c>Log</c> property for the specified control.
		/// </summary>
		/// <param name="counter">The control to get the property for.</param>
		/// <returns>The value of the property for the specified control.</returns>
		[NuGenSRCategory("LogCategory")]
		[NuGenSRDescription("LogDescription")]
		[DefaultValue(false)]
		public bool GetLog(INuGenCounter counter)
		{
			return this.logTargets.ContainsKey(counter);
		}

		/// <summary>
		/// Sets the value of the <c>Log</c> property for the specified control.
		/// </summary>
		/// <param name="counter">The control to set the property for.</param>
		/// <param name="value">The value of the property for the specified control.</param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="counter"/> is <see langword="null"/>.</para>
		/// </exception>
		public void SetLog(INuGenCounter counter, bool value)
		{
			if (counter == null)
			{
				throw new ArgumentNullException("counter");
			}

			if (!this.logTargets.ContainsKey(counter)) 
			{
				this.logTargets.Add(counter, value);

				if (!base.DesignMode) 
				{
					if (value) 
					{
						counter.ValueChanged += this.counter_ValueChanged;
					}
					else
					{
						counter.ValueChanged -= this.counter_ValueChanged;
					}
				}
			}
			else
			{
				this.logTargets[counter] = value;
			}
		}

		#endregion

		#region Methods.Protected.Virtual

		/// <summary>
		/// Invokes event handlers specified by the <paramref name="key"/>.
		/// <param name="key">Specifies the event handlers to invoke.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		/// </summary>
		protected virtual void InvokePropertyChanged(object key, EventArgs e)
		{
			EventHandler handler = (EventHandler)this.Events[key];

			if (handler != null)
			{
				handler(this, e);
			}
		}

		#endregion

		#region Methods.Private

		/// <summary>
		/// Checks if the specified file name is correct.
		/// </summary>
		/// <param name="fileName">The file name to validate.</param>
		/// <returns><see langword="true"/> if the specified file name is correct; otherwise, <c>false</c>.</returns>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="fileName"/> is <see langword="null"/>.</para>
		/// </exception>
		private bool CheckFileName(string fileName)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}

			if (fileName.IndexOfAny(new char[] {'\\', '/', ':', '*', '?', '"', '<', '>', '|'}) > -1) 
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Gets the <see cref="T:System.IO.FileInfo"/> object for the last log file in the output directory.
		/// </summary>
		/// <param name="fileInfo">The full path to the default log file.</param>
		/// <returns><see cref="T:System.IO.FileInfo"/> object for the last log file in the output directory.</returns>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="fileInfo"/> is <see langword="null"/>.</para>
		/// </exception>
		private FileInfo GetLastLogFile(FileInfo fileInfo)
		{
			if (fileInfo == null)
			{
				throw new ArgumentNullException("fileInfo");
			}

			// Get all the log files in the directory.
			FileInfo[] files = fileInfo.Directory.GetFiles("*" + fileInfo.Extension);

			if (files != null && files.Length > 0)
			{
				// Get the last log file from the array.
				return files[files.Length - 1];
			}
			else
			{
				return fileInfo;
			}
		}

		/// <summary>
		/// Gets the <see cref="T:System.IO.FileInfo"/> object for the next log file.
		/// </summary>
		/// <param name="lastLogFile">Current log file.</param>
		/// <returns><see cref="T:System.IO.FileInfo"/> object for the next log file.</returns>
		private FileInfo GetNextLogFile(FileInfo lastLogFile)
		{
			// Full path prefix.
			string prefix = "";

			// Log file extension.
			string ext = lastLogFile.Extension;

			// Log file name without extension and additional postfix.
			string baseName = "";

			// Used to format the prefix.
			string formatString = "";

			// Log file name postfix.
			int count = 0;

			int separatorIndex = lastLogFile.Name.LastIndexOf(this.Separator);

			if (separatorIndex > -1)
			{
				baseName = lastLogFile.Name.Substring(0, separatorIndex);

				try
				{
					count = int.Parse(lastLogFile.Name.Substring(separatorIndex + 1, lastLogFile.Name.LastIndexOf(ext) - (separatorIndex + 1)));
				}
				catch (FormatException)
				{
					baseName = lastLogFile.Name.Substring(0, lastLogFile.Name.LastIndexOf(ext));
					count = 0;
				}
			}
			else
			{
				baseName = lastLogFile.Name.Substring(0, lastLogFile.Name.IndexOf(ext));
			}

			count += this.Step;

			for (int i = 0; i < this.Digits; i++)
			{
				formatString += "0";
			}

			if (lastLogFile.DirectoryName.Length > 0)
			{
				prefix = "\\";
			}

			string path = lastLogFile.DirectoryName
				+ prefix
				+ baseName
				+ this.Separator
				+ count.ToString(formatString)
				+ ext
				;

			return new FileInfo(path);
		}

		/// <summary>
		/// Gets the <see cref="T:System.IO.StreamWriter"/> object according to the specified <see cref="T:System.IO.FileInfo"/> parameters.
		/// </summary>
		/// <param name="fileInfo">Specifies the parameters for reading files.</param>
		/// <returns>Resulting <see cref="T:System.IO.StreamWriter"/> object.</returns>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="fileInfo"/> is <see langword="null"/>.</para>
		/// </exception>
		private StreamWriter GetStreamWriter(FileInfo fileInfo)
		{
			if (fileInfo == null)
			{
				throw new ArgumentNullException("fileInfo");
			}

			if (!fileInfo.Exists)
			{
				try
				{
					return fileInfo.CreateText();
				}
				catch (DirectoryNotFoundException)
				{
					DirectoryInfo dirInfo = fileInfo.Directory;
					dirInfo.Create();

					return fileInfo.CreateText();
				}
			}

			if (this.SplitLogFile)
			{
				return this.GetNextLogFile(this.GetLastLogFile(fileInfo)).CreateText();
			}
			else
			{
				return fileInfo.AppendText();
			}
		}

		/*
		 * Log
		 */

		private void Log(StreamWriter sw, NuGenTargetEventArgs e)
		{
			Debug.Assert(sw != null, "sw != null");
			Debug.Assert(e.Target is INuGenCounter, "e.Target is INuGenCounter");
			Debug.Assert(e.TargetData is float, "e.TargetData is float");

			if (e.Target is INuGenCounter && e.TargetData is float)
			{
				INuGenCounter counter = (INuGenCounter)e.Target;
				float counterValue = (float)e.TargetData;

				sw.WriteLine("\n------------- {0} -------------", counter.Name);
				sw.WriteLine("DATE: {0}", DateTime.Now.ToShortDateString());
				sw.WriteLine("TIME: {0}.{1}", DateTime.Now.ToLongTimeString(), DateTime.Now.Millisecond.ToString());
				sw.WriteLine("MACHINE: {0}", counter.MachineName);
				sw.WriteLine("CATEGORY: {0}", counter.CategoryName);
				sw.WriteLine("NAME: {0}", counter.CounterName);
				sw.WriteLine("INSTANCE: {0}", counter.InstanceName);
				sw.WriteLine("VALUE: {0}{1}", counterValue, counter.CounterFormat);
			}
		}

		#endregion
		
		#region EventHandlers
		
		private void counter_ValueChanged(object sender, NuGenTargetEventArgs e)
		{
			if (this.SplitLogFile && this.fileStreamWriter != null && this.fileStreamWriter.BaseStream != null)
			{
				if (this.fileStreamWriter.BaseStream.Length >= (this.MaxLogFileSize << 0xa))
				{
					this.fileStreamWriter.Dispose();
					this.fileStreamWriter = null;
				}
			}

			if (this.fileStreamWriter == null)
			{
				this.fileStreamWriter = this.GetStreamWriter(new FileInfo(this.LogOutput));
			}

			this.Log(this.fileStreamWriter, e);
		}

		#endregion

		#region Constructors
		
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenLog"/> class.
		/// </summary>
		/// <param name="container">The container.</param>
		public NuGenLog(IContainer container) : this()
		{
			container.Add(this);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenLog"/> class.
		/// </summary>
		public NuGenLog()
		{
		}

		#endregion

		#region Dispose

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.fileStreamWriter != null)
				{
					this.fileStreamWriter.Dispose();
				}
			}

			base.Dispose(disposing);
		}

		#endregion
	}
}
