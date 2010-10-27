using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using EventMonitor.Properties;
using System.Drawing.Drawing2D;
using System.Threading;
using SmoothyInterface.Util;
using System.Management;
using SmoothyInterface.Enum;

namespace SmoothyInterface.Forms
{
	public partial class EventLogViewer : UserControl
	{
		#region Delegate Definitions

		private delegate void RowStringDelegate(DataGridViewRow row, string s);
		private delegate void StringDelegate(string s);
		private delegate void EventLogDelegate(EventLogEntry entry);
		private delegate void FillMessageDelegate(DataGridViewRow row);

		#endregion

		#region Globals

		// Todo : Remove scope from global
		// Dispose all Management stuff

		private EventLog log = null;
		private BindingSource bindingSource;
		private int countInformation = 0;
		private int countWarning = 0;
		private int countError = 0;
		private int countSuccessAudit = 0;
		private int countFailureAudit = 0;
		private bool loadingGroups = false;
		private string previousGroupFilter = null;
		private string currentSearch = String.Empty;
		private string currentSort = null;
		private object lockReadWrite = new object();
		private object wmiLockObject = new object();
		private EventHandler gridSelectionChangedEventHandler;
		private uint lastIndex = 0;		
		private List<string> groupNames = new List<string>();
		private bool cancelFill = false;
		private bool localMachine = false;

		// Cell Styles for the different colors
		private DataGridViewCellStyle cellStyleInformation = CellStyleHelper.GetColorStyle(Settings.Default.ColorInformation);
		private DataGridViewCellStyle cellStyleWarning = CellStyleHelper.GetColorStyle(Settings.Default.ColorWarning);
		private DataGridViewCellStyle cellStyleError = CellStyleHelper.GetColorStyle(Settings.Default.ColorError);
		private DataGridViewCellStyle cellStyleSuccessAudit = CellStyleHelper.GetColorStyle(Settings.Default.ColorSuccesfulAudit);
		private DataGridViewCellStyle cellStyleFailureAudit = CellStyleHelper.GetColorStyle(Settings.Default.ColorFailureAudit);
				
		#endregion
		
		#region Construction

		public EventLogViewer(EventLog log_)
		{
			InitializeComponent();

			log = log_;
			gridSelectionChangedEventHandler = new EventHandler(dgEvents_SelectionChanged);
			localMachine = (log.MachineName == ".") || (log.MachineName == System.Net.Dns.GetHostName());
			this.Text = localMachine ? System.Net.Dns.GetHostName() : log.MachineName + " - [" +  log.LogDisplayName + "]";

			// Set up the grid properties for the visual event distinction
			VisualEventDistinctionType distinctionType = (VisualEventDistinctionType) System.Enum.Parse(typeof(VisualEventDistinctionType), Settings.Default.ColorMode);

			if (distinctionType == VisualEventDistinctionType.Colors)
			{
				dgEvents.RowsAdded +=new DataGridViewRowsAddedEventHandler(dgEvents_RowsAdded);

				// Remove the image column - we're only going to use colours
				dgEvents.Columns[0].Visible = false;
			}
			else
			{
				dgEvents.CellFormatting += new DataGridViewCellFormattingEventHandler(dgEvents_CellFormatting);
			}

			FillGrid();
		}		

		#endregion

		#region Properties

		public EventLog Log
		{
			get
			{
				return log;
			}
		}

		#endregion

		#region Private Members

		private void DoSearch()
		{
			currentSearch = tbSearch.Text;

			FillGrid();

			if (!btnSearch.Checked)
			{
				btnSearch.Checked = true;
			}

			btnClearSearch.Checked = false;
		}

		private void FillMessage(DataGridViewRow selectedRow)
		{
			try
			{
				// Have a delay before we actually retrieve the message
				System.Threading.Thread.Sleep(200);

				if (selectedRow.Index == -1)
				{
					return;
				}

				// Check that the event id is still the same as the one selected
				uint thisIndex = (uint)selectedRow.Cells["Index"].Value;

				if (thisIndex == lastIndex)
				{
					string message = String.Empty;

					if (
						(selectedRow.Cells["Message"].Value == DBNull.Value) ||						
						(((string)selectedRow.Cells["Message"].Value) == String.Empty)
					   )
					{
						// This message has not been cached yet.  Retrieve it.
						try
						{
							/*
							int index = System.Convert.ToInt32(thisIndex - smallestRecordNumber);
							message = log.Entries[index].Message;
							*/

							if (!localMachine)
							{
								// Set the event detail text to Loading...
								if (this.InvokeRequired)
								{
									this.Invoke(new StringDelegate(SetEventDetailText), new object[] { "Loading..." });
								}
								else
								{
									SetEventDetailText("Loading...");
								}
							}

							message = GetEventLogItemMessage(thisIndex);
						}
						catch
						{
							message = "Message not found.";
						}
					}
					else
					{
						message = (string)selectedRow.Cells[6].Value;
					}

					if (this.InvokeRequired)
					{
						this.Invoke(new RowStringDelegate(UpdateEventDetailText), new object[] { selectedRow, message });
					}
					else
					{
						UpdateEventDetailText(selectedRow, message);
					}
				}
			}
			catch (ObjectDisposedException) { }
		}
		
		public void SetEventDetailText(string str)
		{
			tbEventDetail.Text = str;
		}


		private void SetAutoRefreshEnabled(bool enabled)
		{
			toolStripButtonRefresh.Enabled = !enabled;
			log.EnableRaisingEvents = enabled;
		}

		private string GetEventLogItemMessage(uint thisIndex)
		{
			lock (wmiLockObject)
			{
				ManagementScope messageScope = new ManagementScope(
							GetStandardPath()
				);

				messageScope.Connect();

				StringBuilder query = new StringBuilder();
				query.Append("select Message, InsertionStrings from Win32_NTLogEvent where LogFile ='");
				query.Append(log.LogDisplayName.Replace("'", "''"));
				query.Append("' AND RecordNumber='");
				query.Append(thisIndex);
				query.Append("'");

				System.Management.ObjectQuery objectQuery = new System.Management.ObjectQuery(
					query.ToString()
				);

				using (ManagementObjectSearcher objectSearcher = new ManagementObjectSearcher(messageScope, objectQuery))
				{
					using (ManagementObjectCollection collection = objectSearcher.Get())
					{
						// Execute the query
						using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator enumerator = collection.GetEnumerator())
						{
							if (enumerator.MoveNext())
							{
								string message = (string) enumerator.Current["Message"];
								string[] insertionStrings = (string[])enumerator.Current["InsertionStrings"];

								if (message == null)
								{
									if (insertionStrings.Length > 0)
									{
										StringBuilder sb = new StringBuilder();

										for (int i = 0; i < insertionStrings.Length; i++)
										{
											sb.Append(insertionStrings[i]);
											sb.Append(" ");
										}

										return sb.ToString();
									}
									else
									{
										return String.Empty;
									}
								}
								else
								{
									return message;
								}								
							}
						}
					}
				}

				return "Message not found.";
			}
		}

		private ManagementObjectCollection GetEvents()
		{
			lock (wmiLockObject)
			{
				ManagementScope scope = new ManagementScope(GetStandardPath());				
				
				scope.Connect();

				StringBuilder query = new StringBuilder();
				query.Append("select EventType, TimeWritten, Category, SourceName, EventIdentifier, RecordNumber from Win32_NTLogEvent where LogFile ='");
				query.Append(log.LogDisplayName.Replace("'", "''"));
				query.Append("'");

				if (currentSearch != String.Empty)
				{
					query.Append(" AND Message LIKE '%");
					AnalyzeAndFixSearch(ref currentSearch);
					query.Append(currentSearch);
					query.Append("%'");
					/*
					query.Append("%' OR InsertionStrings LIKE '%");
					query.Append(currentSearch);
					query.Append("%')");
					 */
				}

				System.Management.ObjectQuery objectQuery = new System.Management.ObjectQuery(
					query.ToString()
				);


				using (ManagementObjectSearcher objectSearcher = new ManagementObjectSearcher(scope, objectQuery))
				{
					//Execute the query 			
					ManagementObjectCollection collection = objectSearcher.Get();

					return collection;
				}				
			}
		}

		private string GetStandardPath()
		{
			return String.Concat(
						@"\\",
						log.MachineName,
						@"\root\CIMV2"
			);
		}		

		private void FillGrid()
		{
			lock (lockReadWrite)
			{
				// Progress bar for loading the items in the event log
				Progress progress = null;				

				// Reset the counters for event log types
				ResetCounters();
								
				// Show the progress bar only if the event log is not on the local machine
				if (!((log.MachineName == ".") || (log.MachineName == System.Net.Dns.GetHostName())))
				{
					progress = new Progress();
					progress.Show();
					progress.btnCancel.Click += new EventHandler(Progress_Cancel_Click);
				}

				Application.DoEvents();

				this.Cursor = Cursors.WaitCursor;

				try
				{
					dgEvents.SelectionChanged -= dgEvents_SelectionChanged;

					DataSet currentEntries = null;

					// Retrieve the Event Log Entries via WMI
					using (ManagementObjectCollection entries = GetEvents())
					{
						currentEntries = BuildDataSet(entries, progress);
					}

					// The request was cancelled
					if (currentEntries == null)
					{
						return;
					}
										
					if (bindingSource == null)
					{						
						bindingSource = new BindingSource(currentEntries, "Entries");
					}
					else
					{
						bindingSource.DataSource = currentEntries;
					}

					UpdateBindingSourceFilter();
					dgEvents.DataSource = bindingSource;
					dgEvents.Refresh();

					UpdateCounterButtons();
					dgEvents.SelectionChanged += dgEvents_SelectionChanged;
				}
				catch (Exception ex)
				{
					MessageBox.Show("Exception caught : " + ex.ToString(), "EventLog Viewer", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				finally
				{
					this.Cursor = Cursors.Default;

					if (progress != null)
					{
						progress.Close();
					}

					this.Focus();
				}
			}
		}

		void Progress_Cancel_Click(object sender, EventArgs e)
		{			
			CancelFill();
		}

		private void CancelFill()
		{
			cancelFill = true;			
		}

		private void FillSourceItems()
		{
			FillSourceItems(groupNames);
		}

		
		private void UpdateCountersAndButtons(ManagementBaseObject entry)
		{
			UpdateCounters(entry);
			UpdateCounterButtons();			
		}

		private void ResetCounters()
		{
			countError = 0;
			countFailureAudit = 0;
			countInformation = 0;
			countSuccessAudit = 0;
			countWarning = 0;
		}

		private void UpdateBindingSourceFilter()
		{
			bindingSource.Filter = GetBindingSourceFilterString();

			if (dgEvents.SortedColumn == null)
			{
				currentSort = null;
			}
			else
			{
				if (dgEvents.SortOrder != SortOrder.None)
				{
					currentSort = dgEvents.SortedColumn.Name +
						(dgEvents.SortOrder == SortOrder.Ascending ? " ASC" : " DESC");
				}
				else
				{
					currentSort = null;
				}
			}

			if (currentSort == null)
			{
				bindingSource.Sort = "Index DESC";
			}
			else
			{
				bindingSource.Sort = currentSort;
			}
		}

		private string GetBindingSourceFilterString()
		{
			List<string> filterTypes = new List<string>();

			if (btnErrors.Checked)
			{
				filterTypes.Add("(EntryType='Error')");
			}

			if (btnFailureAudit.Checked)
			{
				filterTypes.Add("(EntryType='FailureAudit')");
			}

			if (btnInformation.Checked)
			{
				filterTypes.Add("(EntryType='Information')");
			}

			if (btnSuccessAudit.Checked)
			{
				filterTypes.Add("(EntryType='SuccessAudit')");
			}

			if (btnWarnings.Checked)
			{
				filterTypes.Add("(EntryType='Warning')");
			}

			StringBuilder sb = new StringBuilder();

			if (dlFilterSource.SelectedIndex != 0)
			{
				sb.Append("(Source='" + ((string)dlFilterSource.SelectedItem).Replace("'", "''") + "')");
			}

			if (filterTypes.Count > 0)
			{
				if (sb.Length > 0)
				{
					sb.Append(" AND (");
				}
				else
				{
					sb.Append("(");
				}

				sb.Append(filterTypes[0]);

				for (int i = 1; i < filterTypes.Count; i++)
				{
					sb.Append(" OR ");
					sb.Append(filterTypes[i]);
				}

				sb.Append(")");
			}		

			return sb.ToString();
		}

		private void AnalyzeAndFixSearch(ref string currentSearch)
		{
			for (int i = 0; i < currentSearch.Length; i++)
			{
				if (Char.IsPunctuation(currentSearch[i])) {
					currentSearch = currentSearch.Replace(System.Convert.ToString(currentSearch[i]), String.Empty);
					i--;
				}
			}

			tbSearch.Text = currentSearch;
		}

		private void UpdateCounterButtons()
		{
			btnErrors.Text = countError.ToString() + " Errors";
			btnErrors.ToolTipText = btnErrors.Text;

			btnFailureAudit.Text = countFailureAudit.ToString() + " Failure Audits";
			btnFailureAudit.ToolTipText = btnFailureAudit.Text;

			btnInformation.Text = countInformation.ToString() + " Information";
			btnInformation.ToolTipText = btnInformation.Text;

			btnSuccessAudit.Text = countSuccessAudit.ToString() + " Success Audits";
			btnSuccessAudit.ToolTipText = btnSuccessAudit.Text;

			btnWarnings.Text = countWarning.ToString() + " Warnings";
			btnWarnings.ToolTipText = btnWarnings.Text;
		}

		private DataSet BuildDataSet(ManagementObjectCollection entries, Progress progress)
		{
			// Create the dataset that the datagrid is going to bind to
			DataSet ds = new DataSet();
			DataTable table = new DataTable("Entries");
			table.Columns.Add("EntryType");
			table.Columns.Add("TimeWritten", typeof(DateTime));
			table.Columns.Add("Category", typeof(ushort));
			table.Columns.Add("Source");
			table.Columns.Add("EventID", typeof(uint));
			table.Columns.Add("Message");
			table.Columns.Add("Index", typeof(uint));

			ds.Tables.Add(table);

			groupNames = new List<String>();

			// Enumerate through the events and add each item to the dataset
			using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator enumerator = entries.GetEnumerator())
			{

				while (enumerator.MoveNext())
				{
					if (progress != null)
					{
						if (cancelFill)
						{
							return null;
						}

						MethodInvoker invoker = new MethodInvoker(progress.IncrementValue);
						invoker.BeginInvoke(null, null);
					}

					ManagementBaseObject entry = enumerator.Current;

					if (!groupNames.Contains((string)(entry["SourceName"])))
					{
						groupNames.Add((string)(entry["SourceName"]));
					}

					AddTableRow(table, entry);

					Application.DoEvents();
				}
			}

			FillSourceItems(groupNames);

			return ds;
		}

		private void FillSourceItems(List<string> groupNames)
		{
			loadingGroups = true;

			dlFilterSource.BeginUpdate();

			dlFilterSource.Items.Clear();
			dlFilterSource.Items.Add("<All Sources>");

			groupNames.Sort();

			for (int i = 0; i < groupNames.Count; i++)
			{
				dlFilterSource.Items.Add(groupNames[i]);
			}

			dlFilterSource.EndUpdate();

			if (previousGroupFilter == null)
			{
				dlFilterSource.SelectedIndex = 0;
			}
			else
			{
				dlFilterSource.SelectedItem = previousGroupFilter;
			}

			loadingGroups = false;
		}

		private string GetEventTypeString(NTLogEvent.EventTypeValues val)
		{
			switch (val) {
				case NTLogEvent.EventTypeValues.Error :
					return EventTypeDescription.Error;
				case NTLogEvent.EventTypeValues.Warning :
					return EventTypeDescription.Warning;
				case NTLogEvent.EventTypeValues.Information :
					return EventTypeDescription.Information;
				case NTLogEvent.EventTypeValues.Security_audit_success :
					return EventTypeDescription.SuccessAudit;
				case NTLogEvent.EventTypeValues.Security_audit_failure :
					return EventTypeDescription.FailureAudit;
				default :
					return EventTypeDescription.Unknown;
			}
		}

		private void AddTableRow(DataTable table, ManagementBaseObject entry)
		{
			
			DataRow row = table.NewRow();

			row["EntryType"] = GetEventTypeString(((SmoothyInterface.Util.NTLogEvent.EventTypeValues)(System.Convert.ToInt32(entry["EventType"]))));
			row["TimeWritten"] = WMIUtil.ToDateTime(((string)(entry["TimeWritten"])));
			row["Category"] = ((ushort)(entry["Category"]));
			row["Source"] = ((string)(entry["SourceName"]));
			row["EventID"] = ((uint)(entry["EventIdentifier"]));
			row["Index"] = ((uint)(entry["RecordNumber"]));
			row["Message"] = String.Empty;

			table.Rows.Add(row);

			UpdateCounters(entry);
		}
				
		private void UpdateCounters(ManagementBaseObject entry)
		{
			switch (((SmoothyInterface.Util.NTLogEvent.EventTypeValues)(System.Convert.ToInt32(entry["EventType"]))))
			{
				// Update the counters for the entry type
				case NTLogEvent.EventTypeValues.Error :
					countError++;
					break;
				
				case NTLogEvent.EventTypeValues.Security_audit_failure :
					countFailureAudit++;
					break;

				case NTLogEvent.EventTypeValues.Information :
					countInformation++;
					break;
				
				case NTLogEvent.EventTypeValues.Security_audit_success :
					countSuccessAudit++;
					break;				
				
				case NTLogEvent.EventTypeValues.Warning :
					countWarning++;
					break;
				
				default :
					break;
			}						
		}
		
		private void UpdateEventDetailText(DataGridViewRow row, string message)
		{
			row.Cells[6].Value = message;
			tbEventDetail.Text = message;
		}

		private void toolStripButtonClearEventLog_Click_1(object sender, EventArgs e)
		{
			if (Settings.Default.ShowConfirmationsForCleaningEventLogs)
			{
				if (MessageBox.Show("Are you sure you want to clear this log?", "Smoothy", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				{
					ClearLog();
				}
			}
			else
			{
				ClearLog();
			}
		}

		private void ClearLog()
		{
			log.Clear();
			RefreshGrid();
		}


		#endregion

		#region Public Members

		public void RefreshGrid()
		{
			FillGrid();
		}

		#endregion

		#region Events

		void log_EntryWritten(object sender, EntryWrittenEventArgs e)
		{
			/*
			lock (lockReadWrite)
			{
				EventLogEntry entry = e.Entry;

				if (!groupNames.Contains(entry.Source))
				{
					groupNames.Add(entry.Source);

					if (this.InvokeRequired)
					{
						this.Invoke(new MethodInvoker(FillSourceItems));
					}
					else
					{
						FillSourceItems(groupNames);
					}
				}

				AddTableRow(currentEntries.Tables[0], entry);

				if (this.InvokeRequired)
				{
					this.Invoke(new EventLogDelegate(UpdateCountersAndButtons), new object[] { entry });
				}
				else
				{
					UpdateCountersAndButtons(entry);
				}
			}
			 */
		}

		private void EventLogViewer_Load(object sender, EventArgs e)
		{

		}

		private void toolStripButtonClearEventLog_Click(object sender, EventArgs e)
		{
			this.log.Clear();
		}

		private void On_FilterButton_CheckChanged(object sender, EventArgs e)
		{
			UpdateBindingSourceFilter();
		}

		private void dlFilterSource_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!loadingGroups)
			{
				UpdateBindingSourceFilter();
				previousGroupFilter = (string) dlFilterSource.SelectedItem;
			}
		}

		private void EventLogViewer_KeyUp(object sender, KeyEventArgs e)
		{
			if (!log.EnableRaisingEvents)
			{
				if (e.KeyCode == Keys.F5)
				{
					RefreshGrid();
				}
			}
		}

		private void btnSearch_Click(object sender, EventArgs e)
		{
			DoSearch();
		}

		private void btnClearSearch_Click(object sender, EventArgs e)
		{
			if (btnClearSearch.Checked)
			{
				if (btnSearch.Checked)
				{
					btnSearch.Checked = false;
					currentSearch = String.Empty;					
					tbSearch.Text = String.Empty;
					FillGrid();
				}
			}

			btnClearSearch.Checked = true;
		}

		private void toolStripButtonRefresh_Click(object sender, EventArgs e)
		{
			RefreshGrid();
		}

		private void toolStripButtonAutoRefresh_Click(object sender, EventArgs e)
		{
			SetAutoRefreshEnabled(toolStripButtonAutoRefresh.Checked);
		}


		#region Grid Events

		private void dgEvents_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			// If the typeimage column is being formatted
			if (dgEvents.Columns[e.ColumnIndex].Name == "TypeImage")
			{
				// Get the type from the entrytype column

				object objType = dgEvents["EntryType", e.RowIndex].Value;

				if (objType != null)
				{
					string strType = objType.ToString();

					if (strType == "Information")
					{
						e.Value = EventMonitor.Properties.Resources.information;
					}
					else if (strType == "Warning")
					{
						e.Value = EventMonitor.Properties.Resources.warning;
					}
					else if (strType == "Error")
					{
						e.Value = EventMonitor.Properties.Resources.cancel;
					}
					else if (strType == "FailureAudit")
					{
						e.Value = EventMonitor.Properties.Resources.silk_lock;
					}
					else if (strType == "SuccessAudit")
					{
						e.Value = EventMonitor.Properties.Resources.key;
					}
				}
			}
		}

		private void dgEvents_KeyUp(object sender, KeyEventArgs e)
		{
			if (!log.EnableRaisingEvents)
			{
				if (e.KeyCode == Keys.F5)
				{
					RefreshGrid();
				}
			}
		}	
		


		private void dgEvents_SelectionChanged(object sender, EventArgs e)
		{
			if (dgEvents.SelectedRows.Count > 0)
			{
				// Get the current select item
				DataGridViewRow selectedRow = dgEvents.SelectedRows[0];

				if (selectedRow.Index != -1)
				{
					lastIndex = (uint) selectedRow.Cells["Index"].Value;

					// Do this asynchronously
					FillMessageDelegate del = new FillMessageDelegate(FillMessage);
					IAsyncResult result = del.BeginInvoke(selectedRow, null, null);
				}
			}
		}

		private void dgEvents_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
		{
			int startIndex = e.RowIndex;
			int rowCount = e.RowCount;
			int endIndex = startIndex + rowCount;

			for (int i = startIndex; i < endIndex; i++)
			{
				DataGridViewRow row = dgEvents.Rows[i];
				string strType = (string)row.Cells["EntryType"].Value;

				if (strType == "Information")
				{
					row.DefaultCellStyle = cellStyleInformation;
				}
				else if (strType == "Warning")
				{
					row.DefaultCellStyle = cellStyleWarning;
				}
				else if (strType == "Error")
				{
					row.DefaultCellStyle = cellStyleError;
				}
				else if (strType == "FailureAudit")
				{
					row.DefaultCellStyle = cellStyleFailureAudit;

				}
				else if (strType == "SuccessAudit")
				{
					row.DefaultCellStyle = cellStyleSuccessAudit;
				}
			}
		}

		#endregion		

		private void tbSearch_KeyPress(object sender, KeyPressEventArgs e)
		{			
			// Avoid the possibility of invalid characters in the 
			// filtering query by eliminating punctuation characters.
			if (Char.IsPunctuation(e.KeyChar))
			{
				e.Handled = true;
			}
			else
			{
				e.Handled = false;
			}
		}

		private void tbSearch_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				DoSearch();
			}
		}

		private void btnCopy_Click(object sender, EventArgs e)
		{
			// Copy the current message text to the clipboard
			Clipboard.SetText(tbEventDetail.Text, TextDataFormat.Text);
		}

		#endregion		

	}
}