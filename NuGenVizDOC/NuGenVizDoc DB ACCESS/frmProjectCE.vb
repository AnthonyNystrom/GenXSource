Imports Agilix.Ink
Imports Agilix.Ink.Scribble
Imports Genetibase.UI
Imports System.Reflection

Public Class frmProjectCE

    Private Sub UiButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UiButton3.Click

        If EditBox2.Text = Nothing Then
            MsgBox("You are required to enter a value for Project Identity")
            Return
        End If

        Dim cindex As Integer = GridEX1.CurrentRow.RowIndex
        DSTBLPROJECTS.Update()
        GridEX1.CollapseCards()
        GridEX1.MoveToRowIndex(cindex)
        cindex = Nothing
    End Sub

    Private Sub UiButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UiButton4.Click
        DSTBLPROJECTS.RejectChanges()
        GridEX1.CollapseCards()
    End Sub

    Private Sub UiButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UiButton1.Click

        Dim cm As CurrencyManager
        cm = DirectCast(Me.BindingContext(TSTBLPROJECTS), CurrencyManager)
        cm.AddNew()
        cm = Nothing
    End Sub

    Private Sub frmProjectCE_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Scribble1.Dispose()
    End Sub

    Private Sub frmProjectCE_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim tabp As New TabOrder(Me)
        tabp.SetTabOrder(TabOrder.TabScheme.AcrossFirst)
        tabp = Nothing
        Scribble1.ZoomOut()
        Scribble1.ZoomOut()
        Scribble1.ZoomOut()
        GridEX1.CollapseCards()
    End Sub

    Function currentindex() As Integer
        Return GridEX1.CurrentRow.RowIndex
    End Function

    Private Sub UiButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UiButton2.Click
        Application.DoEvents()
        DSTBLPROJECTS.RejectChanges()
        Me.Close()
    End Sub

    Private Sub TSTBLPROJECTS_AfterAddNew(ByVal sender As Object, ByVal e As C1.Data.RowChangeEventArgs) Handles TSTBLPROJECTS.AfterAddNew

        Dim guid As New Guid
        e.DataTable.DataSet.PushExecutionMode(C1.Data.ExecutionModeEnum.Deferred)
        e.Row("datCreation") = Today.Now
        e.Row("datUpdate") = Today.Now
        e.Row("intProjectID") = guid
        e.Row.EndEdit()
        e.DataTable.DataSet.PopExecutionMode()
        guid = Nothing
    End Sub

    Private Sub UiButton5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UiButton5.Click

        Select Case GridEX1.View

            Case Janus.Windows.GridEX.View.CardView
                GridEX1.View = Janus.Windows.GridEX.View.TableView
                UiButton5.Text = "Card View On"

            Case Janus.Windows.GridEX.View.TableView
                GridEX1.View = Janus.Windows.GridEX.View.CardView
                UiButton5.Text = "Card View Off"
        End Select

    End Sub

    Private Sub DeleteProjectToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteProjectToolStripMenuItem.Click
        GridEX1.Delete()
    End Sub

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ApplicationWaitCursor.Cursor = Cursors.WaitCursor
        ApplicationWaitCursor.Delay = New TimeSpan(0, 0, 0, 0, 250)
        ' Add any initialization after the InitializeComponent() call.
    End Sub



    Private Sub GridEX1_RecordsDeleted1(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridEX1.RecordsDeleted

        Dim ans As Double = MsgBox("Are you sure you want to delete this project?", MsgBoxStyle.YesNo)

        If Not ans = 6 Then
            DSTBLPROJECTS.RejectChanges()
            GridEX1.CollapseCards()
            Return

        Else
            DSTBLPROJECTS.Update()
            GridEX1.CollapseCards()
        End If

    End Sub

    Private Sub Disposer()

        Try

            Dim type As Type = Me.GetType()

            If Not type Is Nothing Then

                For Each fi As FieldInfo In type.GetFields(BindingFlags.NonPublic Or BindingFlags.Instance)

                    Dim field As Object = fi.GetValue(Me)

                    If TypeOf field Is IDisposable Then
                        CType(field, IDisposable).Dispose()
                    End If

                Next fi

            End If

        Catch
        End Try

    End Sub



End Class
