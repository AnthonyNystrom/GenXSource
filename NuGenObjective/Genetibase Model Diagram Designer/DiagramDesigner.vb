Option Strict Off
Option Explicit On

Imports Genetibase.ApplicationBlocks
Imports Genetibase.NuGenObjective
Imports Genetibase.NuGenObjective.Windows.DiagramClient
Imports Genetibase.NuGenObjective.Drawing
Imports Genetibase.Shared.Windows
Imports Genetibase.SmoothApplicationBlocks

Imports System.IO
Imports System.Xml
Imports System.Text
Imports System.Net
Imports System.Windows.Forms

Public Class DiagramDesigner
	Implements IDesigner

	Private WithEvents m_current As ClientDiagram

	Private m_newObjectType As String = "O"

#Region " Properties.Services "
    Dim _windowStateTracker As INuGenWindowStateTracker

    Private ReadOnly Property WindowStateTracker() As INuGenWindowStateTracker
        Get
            If _windowStateTracker Is Nothing Then
                _windowStateTracker = New NuGenWindowStateTracker()
            End If

            Return _windowStateTracker
        End Get
    End Property

#End Region

#Region " Methods.Protected.Overridden "

	Protected Overrides Sub OnFormClosing(ByVal e As System.Windows.Forms.FormClosingEventArgs)
		MyBase.OnFormClosing(e)

		My.Settings.DiagramDesigner_WindowState = Me.WindowStateTracker.GetWindowState(Me)

		If Me.WindowState = FormWindowState.Normal Then
			My.Settings.DiagramDesigner_Location = Me.Location
			My.Settings.DiagramDesigner_Size = Me.Size
		Else
			My.Settings.DiagramDesigner_Location = Me.RestoreBounds.Location
			My.Settings.DiagramDesigner_Size = Me.RestoreBounds.Size
		End If

		My.Settings.Save()
	End Sub

	Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
		MyBase.OnLoad(e)

		Me.Location = My.Settings.DiagramDesigner_Location
		Me.Size = My.Settings.DiagramDesigner_Size
		Me.WindowState = My.Settings.DiagramDesigner_WindowState
	End Sub

	Protected Overrides Sub OnSizeChanged(ByVal e As System.EventArgs)
		MyBase.OnSizeChanged(e)
		Me.WindowStateTracker.SetWindowState(Me)
	End Sub
#End Region

	Private ReadOnly Property CurrentDiagram() As Diagram Implements IDesigner.CurrentDiagram
		Get
			Dim result As Diagram
			If m_current Is Nothing Then
				result = Nothing
			Else
				result = m_current.CurrentDiagram
			End If
			Return result
		End Get
	End Property

	Private ReadOnly Property CurrentFile() As String
		Get
			Return m_current.FileName
		End Get
	End Property

	Private ReadOnly Property CurrentClient() As ClientDiagram Implements IDesigner.DiagramClient
		Get
			Return m_current
		End Get
	End Property

	Private Sub SetTitle()
		Dim titleBuilder As New StringBuilder
		If m_current.FileName <> "" Then
			titleBuilder.Append(Path.GetFileNameWithoutExtension(m_current.FileName))
		End If
		If m_current.Dirty Then
			titleBuilder.Append("*"c)
		End If
		If titleBuilder.Length > 0 Then
			titleBuilder.Append(" - ")
		End If
		titleBuilder.Append(My.Application.Info.Title)
		Text = titleBuilder.ToString
	End Sub

	Private Sub InitializeAddIns()
		For Each aStatus As AddinStatus In My.Application.Addins
			If aStatus.Active Then
				My.Application.ActivateAddin(aStatus, Me, ToolsToolStripMenuItem)
			End If
		Next
	End Sub

	Private Sub NewElementAdd()
		With DiagramViewer.CurrentPage
			Select Case m_newObjectType
				Case "O"
					.AddNewObject()
				Case "S"
					.AddNewState()
				Case "R"
					.AddNewRole()
				Case "A"
					.AddNewAction()
			End Select
		End With
	End Sub

	Private Sub SetUpDiagram()
		' Set the diagram in the viewer
		DiagramViewer.Diagram = CurrentDiagram
		' Set the title
		SetTitle()
		' Reset the new element menu Item
		NewElementToolStripSplitButton.Image = ObjectToolStripMenuItem.Image
		m_newObjectType = "O"
	End Sub

	Private Sub EnableEditingControls(ByVal enable As Boolean)
		AddPageToolStripButton.Enabled = enable
		NewElementToolStripSplitButton.Enabled = enable
		AddInteractionToolStripButton.Enabled = enable
		DeleteToolStripButton.Enabled = enable
		' Enable/disable editing
		DiagramViewer.EditingEnabled = enable
	End Sub

#Region " New/Open/Save/Save As "
	Public Sub NewDiagram()
		' Try and unlock any exiting locks
		UnlockDiagram(False)
		' Create a new local diagram and page
		m_current = New LocalDiagram
		' Set up editing environment
		SetUpDiagram()
		' Enable editing controls and disable
		' server menu item, because
		' this is a local diagram
		EnableEditingControls(True)
		ServerMenuItem.Visible = False
	End Sub

	Public Sub OpenDiagram(ByVal filePath As String)
		If Path.GetExtension(filePath) = My.Resources.Diagram_ServerExtension Then
			m_current = ServerDiagram.Open(filePath, ServerDiagram.OpenMode.ForEditing)
			' Disable editing controls
			EnableEditingControls(False)
			ServerMenuItem.Visible = True
		Else
			m_current = LocalDiagram.Open(filePath)
			' Enable editing controls
			EnableEditingControls(True)
			ServerMenuItem.Visible = False
		End If
		SetUpDiagram()
	End Sub

	Public Sub OpenDiagram()
		With DiagramOpenFileDialog
			.Filter = String.Format( _
			  "Diagram files (*{0})|*{0}|Server Diagram files (*{1})|*{1}", _
			  My.Resources.Diagram_Extension, _
			  My.Resources.Diagram_ServerExtension _
			  )
			.DefaultExt = My.Resources.Diagram_Extension

			If .ShowDialog() = System.Windows.Forms.DialogResult.OK Then
				Dim currFilePath As String
				currFilePath = DiagramOpenFileDialog.FileName
				OpenDiagram(currFilePath)
			End If
		End With
	End Sub

	Public Sub SaveDiagram()
		If CurrentFile = "" Then
			SaveDiagramAs()
		Else
			m_current.Save(CurrentFile)
		End If
	End Sub

	Public Sub SaveDiagramAs()
		With DiagramSaveFileDialog
			If TypeOf m_current Is ServerDiagram Then
				.DefaultExt = My.Resources.Diagram_ServerExtension
				.Filter = String.Format( _
				  "Server Diagram files (*{0})|*{0}", _
				  My.Resources.Diagram_ServerExtension _
				  )
			Else
				.DefaultExt = My.Resources.Diagram_Extension
				.Filter = String.Format( _
				 "Diagram files (*{0})|*{0}", _
				 My.Resources.Diagram_Extension _
				 )
			End If
			If .ShowDialog = System.Windows.Forms.DialogResult.OK Then
				If .FileName <> "" Then
					m_current.Save(.FileName)
				End If
			End If
		End With
	End Sub

	Private Function AddInSaveDialog(ByVal extension As String, ByVal filter As String) As String Implements IDesigner.SaveFileDialog
		Dim result As String
		With DiagramSaveFileDialog
			.DefaultExt = extension
			.Filter = filter
			If .ShowDialog = System.Windows.Forms.DialogResult.OK Then
				result = .FileName
			Else
				result = ""
			End If
		End With
		Return result
	End Function
#End Region

	Private Function GetUrl(ByVal mode As UrlInputDialogMode) As String
		Using urlDialog As New UrlInputDialog(mode)
			If urlDialog.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
				Dim url As String = urlDialog.URL
				If Not String.IsNullOrEmpty(url) Then
					If Not url.EndsWith(String.Format(".{0}", My.Resources.Diagram_Extension), StringComparison.InvariantCultureIgnoreCase) Then
						url = String.Format("{0}.{1}", url, My.Resources.Diagram_Extension)
					End If
					Return url
				End If
			End If
		End Using
		Return Nothing
	End Function

	Public Sub PublishDiagram()
		Dim url As String = Me.GetUrl(UrlInputDialogMode.Publish)
		If Not String.IsNullOrEmpty(url) Then
			m_current.ServerUrl = url
			m_current.Publish()
		End If
	End Sub

	Public Sub CheckOutDiagram()
		Dim url As String = Me.GetUrl(UrlInputDialogMode.CheckOut)
		If Not String.IsNullOrEmpty(url) Then
			m_current = ServerDiagram.CheckOut(url)
			SetUpDiagram()
			EnableEditingControls(False)
			ServerMenuItem.Visible = True
		End If
	End Sub

	Private Sub LockDiagram()
		Dim sd As ServerDiagram = TryCast(m_current, ServerDiagram)
		If sd IsNot Nothing Then
			Try
				sd.Lock()
				If sd.Locked Then
					MsgBox("Diagram has been locked on the server.")
					EnableEditingControls(True)
				End If
			Catch
				MsgBox("Failed to obtain a lock.")
			End Try
		End If
	End Sub

	Private Sub UnlockDiagram(ByVal interactive As Boolean)
		Dim sd As ServerDiagram = TryCast(m_current, ServerDiagram)
		If sd IsNot Nothing Then
			If sd.Locked Then
				Try
					sd.Unlock()
					If Not sd.Locked Then
						MsgBox("Lock removed.")
						EnableEditingControls(False)
						Exit Sub
					End If
				Catch

				End Try
				MsgBox("Could not unlock.")
			Else
				If interactive Then
					MsgBox("Diagram is not locked")
				End If
			End If
		End If
	End Sub

	Private Sub PublishServerDiagram()
		Try
			Dim sd As ServerDiagram = TryCast(m_current, ServerDiagram)
			If sd IsNot Nothing Then
				sd.Publish()
			End If
		Catch ex As Exception
			MsgBox(ex.Message)
		End Try
	End Sub

	Private Sub UpdateServerDiagram()
		Dim sd As ServerDiagram = TryCast(m_current, ServerDiagram)
		If sd IsNot Nothing Then
			sd.UpdateFromServer()
			SetUpDiagram()
		End If
	End Sub

	Private Sub DiagramDesigner_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
		UnlockDiagram(False)
	End Sub

	Private Sub DiagramDesigner_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
		' Ignore keys if editing is disabled
		If Not DiagramViewer.EditingEnabled Then
			Exit Sub
		End If
		If e.KeyCode = Keys.F2 Then
			DiagramViewer.LabelEdit()
		ElseIf e.KeyCode = Keys.Delete Then
			' Delete the currently selected element from the page
			' (from the diagram if Shift+Delete was pressed).
			DiagramViewer.DeleteSelectedElement(fromDiagram:=e.Shift)
		End If
	End Sub

	Private Sub DiagramDesigner_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
		InitializeAddIns()
		If My.Application.CommandLineArgs.Count > 0 Then
			Try
				OpenDiagram(My.Application.CommandLineArgs(0))
			Catch
				MsgBox("An error occured. Opening a new diagram.")
				NewDiagram()
			End Try
		Else
			NewDiagram()
		End If
	End Sub

	Private Sub DeleteToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteToolStripButton.Click
		Dim currentElement As Object = DiagramViewer.SelectedElement
		If currentElement IsNot Nothing Then
			If Not TypeOf currentElement Is Page Then
				DiagramViewer.CurrentPage.RemoveElement(currentElement)
				' TODO: Deletion should trigger an Invalidate. 
				DiagramViewer.Invalidate()
			End If
		End If
	End Sub

	Private Sub NewElementToolStripSplitButton_ButtonClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewElementToolStripSplitButton.ButtonClick
		NewElementAdd()
	End Sub

	Private Sub ObjectToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ObjectToolStripMenuItem.Click
		m_newObjectType = "O"
		NewElementAdd()
		NewElementToolStripSplitButton.ToolTipText = ObjectToolStripMenuItem.ToolTipText
		NewElementToolStripSplitButton.Image = ObjectToolStripMenuItem.Image
	End Sub

	Private Sub StateToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StateToolStripMenuItem.Click
		m_newObjectType = "S"
		NewElementAdd()
		NewElementToolStripSplitButton.ToolTipText = StateToolStripMenuItem.ToolTipText
		NewElementToolStripSplitButton.Image = StateToolStripMenuItem.Image
	End Sub

	Private Sub RoleToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RoleToolStripMenuItem.Click
		m_newObjectType = "R"
		NewElementAdd()
		NewElementToolStripSplitButton.ToolTipText = RoleToolStripMenuItem.ToolTipText
		NewElementToolStripSplitButton.Image = RoleToolStripMenuItem.Image
	End Sub

	Private Sub ActionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ActionToolStripMenuItem.Click
		m_newObjectType = "A"
		NewElementAdd()
		NewElementToolStripSplitButton.ToolTipText = ActionToolStripMenuItem.ToolTipText
		NewElementToolStripSplitButton.Image = ActionToolStripMenuItem.Image
	End Sub

	Private Sub SaveToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripButton.Click
		SaveDiagram()
	End Sub

	Private Sub OpenToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripButton.Click
		OpenDiagram()
	End Sub

	Private Sub NewToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripButton.Click
		NewDiagram()
	End Sub

	Private Sub AddInteractionToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddInteractionToolStripButton.Click
		If Not TypeOf DiagramViewer.SelectedElement Is ModelObject Then
			MsgBox("Please select an object first.")
		Else
			Dim f As New InteractionDialog(DiagramViewer.SelectedElement, CurrentDiagram.System, DiagramViewer.CurrentPage)
			f.ShowDialog()
		End If
	End Sub

	Private Sub FiftyPercentMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FiftyPercentMenuItem.Click
		DiagramViewer.SetScale(0.5!)
	End Sub

	Private Sub HunderedPercentMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HunderedPercentMenuItem.Click
		DiagramViewer.SetScale(1.0!)
	End Sub

	Private Sub OneFiftyPercentMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OneFiftyPercentMenuItem.Click
		DiagramViewer.SetScale(1.5!)
	End Sub

	Private Sub TwoHundredPercentMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TwoHundredPercentMenuItem.Click
		DiagramViewer.SetScale(2.0!)
	End Sub

	Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
		SaveDiagram()
	End Sub

	Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
		OpenDiagram()
	End Sub

	Private Sub SaveAsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveAsToolStripMenuItem.Click
		SaveDiagramAs()
	End Sub

	Private Sub AddPageToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddPageToolStripButton.Click
		CurrentDiagram.AddNewPage()
	End Sub

	Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
		NewDiagram()
	End Sub

	Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
		Close()
	End Sub

	Private Sub PublishMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PublishMenuItem.Click
		PublishDiagram()
	End Sub

	Private Sub OpenFromServerMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenFromServerMenuItem.Click
		CheckOutDiagram()
	End Sub

	Private Sub LockMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LockMenuItem.Click
		LockDiagram()
	End Sub

	Private Sub UnlockMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UnlockMenuItem.Click
		UnlockDiagram(True)
	End Sub

	Private Sub PublishServerDiagramMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PublishServerDiagramMenuItem.Click
		PublishServerDiagram()
	End Sub

	Private Sub UpdateMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UpdateMenuItem.Click
		UpdateServerDiagram()
	End Sub

	Private Sub m_current_DirtyStateChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_current.DirtyStateChanged
		SetTitle()
	End Sub

	Private Sub m_current_FileNameChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_current.FileNameChanged
		SetTitle()
	End Sub

	Private Sub m_current_Message(ByVal sender As Object, ByVal e As DiagramMessageEventArgs) Handles m_current.Message
		e.MessageResult = MessageBox.Show(e.Message, My.Application.Info.Title, e.MessageButtons, e.MessageIcon)
	End Sub

	Private Sub OptionsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OptionsToolStripMenuItem.Click
		Dim o As New OptionsDialog
		o.Designer = Me
		o.ShowDialog(Me)
	End Sub

#Region " Printing "
	Private m_currentPrinterPage As Integer
	Private m_currentXSection As Integer
	Private m_currentYSection As Integer
	Private m_currentDiagramPage As Page
	Private m_diagramPageEnum As IEnumerator(Of Page)

	Private Sub PageSetupToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PageSetupToolStripMenuItem.Click
		DiagramPageSetupDialog.ShowDialog()
	End Sub

	Private Sub PrintPreviewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrintPreviewToolStripMenuItem.Click
		DiagramPrintPreview.ShowDialog()
	End Sub

	Private Sub PrintToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrintToolStripMenuItem.Click
		If DiagramPrintDialog.ShowDialog = System.Windows.Forms.DialogResult.OK Then
			DiagramPrintDocument.Print()
		End If
	End Sub

	Private Sub DiagramPrintDocument_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles DiagramPrintDocument.BeginPrint
		m_currentPrinterPage = 1
		' Start X and Y sections at 1,1
		m_currentXSection = 1
		m_currentYSection = 1
		' Obtain an enumerator on the diagram pages collection, and prime it
		m_diagramPageEnum = CurrentDiagram.Pages.GetEnumerator()
		m_diagramPageEnum.MoveNext()
		' Set the print document name
		If m_current.FileName <> "" Then
			DiagramPrintDocument.DocumentName = New FileInfo(m_current.FileName).Name
		End If
	End Sub

	Private Sub DiagramPrintDocument_EndPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles DiagramPrintDocument.EndPrint
		m_currentPrinterPage = 1
		m_diagramPageEnum = Nothing
		m_currentXSection = 1
		m_currentYSection = 1
		DiagramPrintDocument.DocumentName = ""
	End Sub

	Private Sub DiagramPrintDocument_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles DiagramPrintDocument.PrintPage

		' By default, assume that there will be more pages to print
		e.HasMorePages = True

		' Calculate number of X-sections and Y-sections required 
		' to print the current diagram page.
		Dim printPageSize As Size
		Dim diagramPageSize As Size
		Dim diagramCurrentPage As Page = m_diagramPageEnum.Current
		Dim numberOfXSections As Integer
		Dim numberOfYSections As Integer

		printPageSize = e.MarginBounds.Size
		diagramPageSize = DrawingHelper.PageSize(diagramCurrentPage)

		If printPageSize.Width < diagramPageSize.Width _
		  OrElse _
		 printPageSize.Height < diagramPageSize.Height _
		  Then
			numberOfXSections = CInt(Math.Ceiling(diagramPageSize.Width / printPageSize.Width))
			numberOfYSections = CInt(Math.Ceiling(diagramPageSize.Height / printPageSize.Height))

			If numberOfXSections = 0 Then numberOfXSections = 1
			If numberOfYSections = 0 Then numberOfYSections = 1
		End If

		' Calculate the origin offset for printing a cross-section of
		' the diagram page on the current printed page
		Dim printOriginX As Integer = (m_currentXSection - 1) * printPageSize.Width
		Dim printOriginY As Integer = (m_currentYSection - 1) * printPageSize.Height

		' Clip anything beyond the print margins
		e.Graphics.SetClip(New Rectangle(New Point(0, 0), printPageSize))

		' Set up a page drawing object and relevant pens
		Dim p As New DrawingPage(diagramCurrentPage)

		Dim diagramDivisionsPen As New Pen(Color.SlateGray, 1.5F)
		Dim stateLinePen As New Pen(Color.SlateGray, 3.0!)
		Dim objectLinePen As New Pen(Color.FromArgb(64, Color.SlateGray), 5.0F)
		objectLinePen.EndCap = Drawing2D.LineCap.ArrowAnchor

		' Draw the cross section, shifting the origin on the drawing
		' page as needed
		p.Draw( _
		   Nothing, _
		   e.Graphics, _
		   -printOriginX, _
		   -printOriginY, _
		   1.0, _
		   diagramDivisionsPen, _
		   stateLinePen, _
		   stateLinePen, _
		   objectLinePen, _
		   objectLinePen _
		 )

		' We print across, then down. So, increase the current X-section.
		' If we have reached the last X-Section, increase the Y-section
		' and reset the X-section to 1
		m_currentXSection += 1
		If m_currentXSection > numberOfXSections Then
			m_currentXSection = 1
			m_currentYSection += 1
		End If

		' Increase the printed page count
		m_currentPrinterPage += 1

		' If we have reached the end of all cross-sections of the current
		' diagram page, reset the current X and Y sections to 1. If there
		' are no more diagram pages to print, stop.
		If m_currentYSection > numberOfYSections Then
			If Not m_diagramPageEnum.MoveNext Then
				e.HasMorePages = False
			Else
				m_currentXSection = 1
				m_currentYSection = 1
			End If
		End If
	End Sub

#End Region

	Private Sub ExplorerWindowToolStripMenuItem_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExplorerWindowToolStripMenuItem.CheckedChanged
		DiagramViewer.ExplorerWindowVisible = DirectCast(sender, ToolStripMenuItem).Checked
	End Sub

	Private Sub PropertiesWindowToolStripMenuItem_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PropertiesWindowToolStripMenuItem.CheckedChanged
		DiagramViewer.PropertiesWindowVisible = DirectCast(sender, ToolStripMenuItem).Checked
	End Sub

	Private Sub CutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CutToolStripMenuItem.Click
		DiagramViewer.Cut()
	End Sub

	Private Sub CopyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyToolStripMenuItem.Click
		DiagramViewer.Copy()
	End Sub

	Private Sub PasteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PasteToolStripMenuItem.Click
		DiagramViewer.Paste()
	End Sub

	Private Sub ExportMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExportMenuItem.Click
        Dim imageExportBlock As NuGenImageExportBlock = New NuGenSmoothImageExportBlock()

        Dim currentPage As Page
        For Each currentPage In m_current.CurrentDiagram.Pages
            Dim diagramPageSize As Size = DrawingHelper.PageSize(currentPage)
            Dim drawingPage As New DrawingPage(currentPage)
            Dim diagramDivisionsPen As New Pen(Color.SlateGray, 1.5F)
            Dim stateLinePen As New Pen(Color.SlateGray, 3.0!)
            Dim objectLinePen As New Pen(Color.FromArgb(64, Color.SlateGray), 5.0F)
            objectLinePen.EndCap = Drawing2D.LineCap.ArrowAnchor

            Dim pageImage As Image = New Bitmap(diagramPageSize.Width, diagramPageSize.Height)

            Using g As Graphics = Graphics.FromImage(pageImage)
                g.Clear(Color.White)
                drawingPage.Draw(Nothing, g, 0, 0, 1.0, diagramDivisionsPen, stateLinePen, stateLinePen, objectLinePen, objectLinePen)
            End Using

            imageExportBlock.Images.Add(pageImage)
        Next

        imageExportBlock.ShowDialog()
    End Sub

	Public Sub New()
		Me.InitializeComponent()
	End Sub
End Class
