/* -----------------------------------------------
 * NuGenLog.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Design;
using Genetibase.Shared;
using Genetibase.Meters.ComponentModel;

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

namespace Genetibase.Meters
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
		/// Specifies whether this Object can provide its extender properties to the specified Object.
		/// </summary>
		/// <param name="extendee">The Object to receive the extender properties.</param>
		/// <returns>If this Object can provide extender properties to the specified Object returns <c>true</c>; otherwise, <c>false</c>.</returns>
		public Boolean CanExtend(Object extendee)
		{
			return extendee is INuGenCounter;
		}

		#endregion

		#region Properties.Behavior

		/*
		 * LogOutput
		 */

		/// <summary>
		/// Determines the path to write the log file to.
		/// </summary>
		private String _logOutput = "";

		/// <summary>
		/// Gets or sets the output path for the log.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_LogOutput")]
		[Editor(typeof(NuGenOutputEditor), typeof(UITypeEditor))]
		public virtual String LogOutput
		{
			get
			{
				if (_logOutput == "")
				{
					return Properties.Resources.OutputLogFileName;
				}
				else
				{
					return _logOutput;
				}
			}
			set
			{
				FileInfo checkFileInfo = new FileInfo(value);

				if (checkFileInfo.Extension == "")
				{
					throw new ArgumentException(Properties.Resources.LogOutputException);
				}
				if (_logOutput != value)
				{
					_logOutput = value;
					this.OnLogOutputChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly Object _logOutputChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenLog.LogOutput"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_LogOutputChanged")]
		public event EventHandler LogOutputChanged
		{
			add
			{
				this.Events.AddHandler(_logOutputChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_logOutputChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenLog.LogOutputChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnLogOutputChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_logOutputChanged, e);
		}

		/*
		 * MaxLogFileSize
		 */

		/// <summary>
		/// Determines the maximum log file size in KB.
		/// </summary>
		private long _maxLogFileSize = 1024L;

		/// <summary>
		/// Gets or sets the maximum log file size in KB.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_MaxLogFileSize")]
		[DefaultValue(1024L)]
		public virtual long MaxLogFileSize
		{
			get
			{
				return _maxLogFileSize;
			}
			set
			{
				if (_maxLogFileSize != value)
				{
					_maxLogFileSize = value;
					this.OnMaxLogFileSizeChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly Object _maxLogFileSizeChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenLog.MaxLogFileSize"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_MaxLogFileSizeChanged")]
		public event EventHandler MaxLogFileSizeChanged
		{
			add
			{
				this.Events.AddHandler(_maxLogFileSizeChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_maxLogFileSizeChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenLog.MaxLogFileSizeChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnMaxLogFileSizeChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_maxLogFileSizeChanged, e);
		}

		/*
		 * SplitLogFile
		 */

		/// <summary>
		/// Indicates whether to split a log file if its size exceeds MaxLogFileSize.
		/// </summary>
		private Boolean _splitLogFile;

		/// <summary>
		/// Gets or sets a value indicating whether to split a log file if its size exceeds MaxLogFileSize.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Behavior")]
		[NuGenSRDescription("Description_SplitLogFile")]
		[DefaultValue(false)]
		public virtual Boolean SplitLogFile
		{
			get
			{
				return _splitLogFile;
			}
			set
			{
				if (_splitLogFile != value)
				{
					_splitLogFile = value;
					this.OnSplitLogFileChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly Object _splitLogFileChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenLog.SplitLogFile"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_SplitLogFileChanged")]
		public event EventHandler SplitLogFileChanged
		{
			add
			{
				this.Events.AddHandler(_splitLogFileChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_splitLogFileChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenLog.SplitLogFileChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnSplitLogFileChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_splitLogFileChanged, e);
		}

		#endregion

		#region Properties.Counter

		/*
		 * Digits
		 */

		/// <summary>
		/// Determines the number of digits in the postfix.
		/// </summary>
		private Int32 _digits = 3;

		/// <summary>
		/// Gets or sets the number of digits in the postfix.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Counter")]
		[NuGenSRDescription("Description_Digits")]
		[DefaultValue(3)]
		public Int32 Digits
		{
			get
			{
				return _digits;
			}
			set
			{
				if (_digits != value)
				{
					if (value < 1)
					{
						throw new ArgumentException(Properties.Resources.DigitsException);
					}

					_digits = value;
					this.OnDigitsChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly Object _digitsChangedDescription = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenLog.Digits"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_DigitsChanged")]
		public event EventHandler DigitsChanged
		{
			add
			{
				this.Events.AddHandler(_digitsChangedDescription, value);
			}
			remove
			{
				this.Events.RemoveHandler(_digitsChangedDescription, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenLog.DigitsChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnDigitsChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_digitsChangedDescription, e);
		}

		/*
		 * Separator
		 */

		/// <summary>
		/// Determines the separator of the postfix.
		/// </summary>
		private String _separator = "_";

		/// <summary>
		/// Gets or sets the separator of the postfix.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Counter")]
		[NuGenSRDescription("Description_Separator")]
		[DefaultValue("_")]
		public String Separator
		{
			get
			{
				return _separator;
			}
			set
			{
				if (!this.CheckFileName(value))
				{
					throw new ArgumentException(
						String.Format(Properties.Resources.CheckFileNameException, Environment.NewLine, "\\ / : * ? \" < > |")
					);
				}
				if (value != null)
				{
					_separator = value;
					this.OnSeparatorChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly Object _separatorChanged = new Object();

		/// <summary>
		/// Occurs when the value of <see cref="P:Genetibase.Meters.NuGenLog.Separator"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[NuGenSRDescription("Description_SeparatorChanged")]
		public event EventHandler SeparatorChanged
		{
			add
			{
				this.Events.AddHandler(_separatorChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_separatorChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenLog.SeparatorChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnSeparatorChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_separatorChanged, e);
		}

		/*
		 * Step
		 */

		/// <summary>
		/// Determines the step of the counter.
		/// </summary>
		private Int32 _step = 1;

		/// <summary>
		/// Gets or sets the step of the counter.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_Counter")]
		[NuGenSRDescription("Description_CounterStep")]
		[DefaultValue(1)]
		public Int32 Step
		{
			get
			{
				return _step;
			}
			set
			{
				if (_step != value)
				{
					if (value < 1)
					{
						throw new ArgumentException(Properties.Resources.CounterStepException);
					}

					_step = value;
					this.OnStepChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly Object _stepChanged = new Object();

		/// <summary>
		/// Occurs when the value of the <see cref="P:Genetibase.Meters.NuGenLog.Step"/> property changes.
		/// </summary>
		[Browsable(true)]
		[NuGenSRCategory("Category_PropertyChanged")]
		[Description("Description_StepChanged")]
		public event EventHandler StepChanged
		{
			add
			{
				this.Events.AddHandler(_stepChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_stepChanged, value);
			}
		}

		/// <summary>
		/// Raises the <see cref="E:Genetibase.Meters.NuGenLog.StepChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		[DebuggerStepThrough()]
		protected virtual void OnStepChanged(EventArgs e)
		{
			this.InvokePropertyChanged(_stepChanged, e);
		}

		#endregion

		#region Properties.Extended.Log

		/// <summary>
		/// Contains all the controls of type <see cref="T:Genetibase.Meters.NuGenMeterBase"/> on the form.
		/// </summary>
		private Dictionary<INuGenCounter, Boolean> _logTargets = new Dictionary<INuGenCounter, Boolean>();

		/// <summary>
		/// Gets the value of the <c>Log</c> property for the specified control.
		/// </summary>
		/// <param name="counter">The control to get the property for.</param>
		/// <returns>The value of the property for the specified control.</returns>
		[NuGenSRCategory("Category_Log")]
		[NuGenSRDescription("Description_Log")]
		[DefaultValue(false)]
		public Boolean GetLog(INuGenCounter counter)
		{
			return _logTargets.ContainsKey(counter);
		}

		/// <summary>
		/// Sets the value of the <c>Log</c> property for the specified control.
		/// </summary>
		/// <param name="counter">The control to set the property for.</param>
		/// <param name="value">The value of the property for the specified control.</param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="counter"/> is <see langword="null"/>.</para>
		/// </exception>
		public void SetLog(INuGenCounter counter, Boolean value)
		{
			if (counter == null)
			{
				throw new ArgumentNullException("counter");
			}

			if (!_logTargets.ContainsKey(counter))
			{
				_logTargets.Add(counter, value);

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
				_logTargets[counter] = value;
			}
		}

		#endregion

		#region Methods.Protected.Virtual

		/// <summary>
		/// Invokes event handlers specified by the <paramref name="key"/>.
		/// <param name="key">Specifies the event handlers to invoke.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		/// </summary>
		protected virtual void InvokePropertyChanged(Object key, EventArgs e)
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
		private Boolean CheckFileName(String fileName)
		{
			if (fileName == null)
			{
				throw new ArgumentNullException("fileName");
			}

			if (fileName.IndexOfAny(new Char[] { '\\', '/', ':', '*', '?', '"', '<', '>', '|' }) > -1)
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// Gets the <see cref="T:System.IO.FileInfo"/> Object for the last log file in the output directory.
		/// </summary>
		/// <param name="fileInfo">The full path to the default log file.</param>
		/// <returns><see cref="T:System.IO.FileInfo"/> Object for the last log file in the output directory.</returns>
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
		/// Gets the <see cref="T:System.IO.FileInfo"/> Object for the next log file.
		/// </summary>
		/// <param name="lastLogFile">Current log file.</param>
		/// <returns><see cref="T:System.IO.FileInfo"/> Object for the next log file.</returns>
		private FileInfo GetNextLogFile(FileInfo lastLogFile)
		{
			// Full path prefix.
			String prefix = "";

			// Log file extension.
			String ext = lastLogFile.Extension;

			// Log file name without extension and additional postfix.
			String baseName = "";

			// Used to format the prefix.
			String formatString = "";

			// Log file name postfix.
			Int32 count = 0;

			Int32 separatorIndex = lastLogFile.Name.LastIndexOf(this.Separator);

			if (separatorIndex > -1)
			{
				baseName = lastLogFile.Name.Substring(0, separatorIndex);

				try
				{
					count = Int32.Parse(lastLogFile.Name.Substring(separatorIndex + 1, lastLogFile.Name.LastIndexOf(ext) - (separatorIndex + 1)));
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

			for (Int32 i = 0; i < this.Digits; i++)
			{
				formatString += "0";
			}

			if (lastLogFile.DirectoryName.Length > 0)
			{
				prefix = "\\";
			}

			String path = lastLogFile.DirectoryName
				+ prefix
				+ baseName
				+ this.Separator
				+ count.ToString(formatString)
				+ ext
				;

			return new FileInfo(path);
		}

		/// <summary>
		/// Gets the <see cref="T:System.IO.StreamWriter"/> Object according to the specified <see cref="T:System.IO.FileInfo"/> parameters.
		/// </summary>
		/// <param name="fileInfo">Specifies the parameters for reading files.</param>
		/// <returns>Resulting <see cref="T:System.IO.StreamWriter"/> Object.</returns>
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

		private void counter_ValueChanged(Object sender, NuGenTargetEventArgs e)
		{
			if (this.SplitLogFile && _streamWriter != null && _streamWriter.BaseStream != null)
			{
				if (_streamWriter.BaseStream.Length >= (this.MaxLogFileSize << 0xa))
				{
					_streamWriter.Dispose();
					_streamWriter = null;
				}
			}

			if (_streamWriter == null)
			{
				_streamWriter = this.GetStreamWriter(new FileInfo(this.LogOutput));
			}

			this.Log(_streamWriter, e);
		}

		#endregion

		private StreamWriter _streamWriter;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenLog"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="container"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenLog(IContainer container)
			: this()
		{
			if (container == null)
			{
				throw new ArgumentNullException("container");
			}

			container.Add(this);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenLog"/> class.
		/// </summary>
		public NuGenLog()
		{
		}

		#region Dispose

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(Boolean disposing)
		{
			if (disposing)
			{
				if (_streamWriter != null)
				{
					_streamWriter.Dispose();
				}
			}

			base.Dispose(disposing);
		}

		#endregion
	}
}
