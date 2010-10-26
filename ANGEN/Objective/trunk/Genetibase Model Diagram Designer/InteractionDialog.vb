Option Strict On
Option Explicit On

Imports Genetibase.NuGenObjective
Imports Genetibase.NuGenObjective.Drawing

Friend Class InteractionDialog

    Private m_system As ModelSystem
    Private m_object As ModelObject
    Private m_page As Page

    Private Function ReduceRectangle(ByVal rect As Rectangle) As Rectangle
        Dim result As New Rectangle(rect.Location, rect.Size)
        result.Width -= 2
        result.Height -= 2
        Return result
    End Function

    Private Sub ObjectPanel_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles ObjectPanel.Paint
        DrawingHelper.DrawElement( _
                        e.Graphics, _
                        Me.Font, _
                        Color.Black, _
                        Color.FromArgb(250, 248, 253), _
                        Color.FromArgb(199, 179, 230), _
                        Color.Black, _
                        Drawing2D.LinearGradientMode.ForwardDiagonal, _
                        5, False, m_object.Name, ReduceRectangle(ObjectPanel.ClientRectangle))
    End Sub

    Private Sub StatePanel_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles StatePanel.Paint
        DrawingHelper.DrawElement( _
                        e.Graphics, _
                        Me.Font, _
                        Color.Black, _
                        Color.FromArgb(250, 248, 253), _
                        Color.FromArgb(161, 194, 254), _
                        Color.Black, _
                        Drawing2D.LinearGradientMode.ForwardDiagonal, _
                        5, False, "", ReduceRectangle(StatePanel.ClientRectangle))
    End Sub

    Private Sub RolePanel_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles RolePanel.Paint
        DrawingHelper.DrawElement( _
                        e.Graphics, _
                        Me.Font, _
                        Color.Black, _
                        Color.FromArgb(252, 255, 247), _
                        Color.FromArgb(216, 251, 164), _
                        Color.Black, _
                        Drawing2D.LinearGradientMode.ForwardDiagonal, _
                        5, False, "", ReduceRectangle(RolePanel.ClientRectangle))
    End Sub

    Private Sub ActionPanel_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles ActionPanel.Paint
        DrawingHelper.DrawElement( _
                        e.Graphics, _
                        Me.Font, _
                        Color.Black, _
                        Color.FromArgb(255, 248, 243), _
                        Color.FromArgb(255, 188, 133), _
                        Color.Black, _
                        Drawing2D.LinearGradientMode.ForwardDiagonal, _
                        5, False, "", ReduceRectangle(ActionPanel.ClientRectangle))
    End Sub

    Private Sub InteractionDialog_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        Dim xx As Single = CSng(StatePanel.Left + (StatePanel.Width / 2))
        e.Graphics.DrawLine( _
            New Pen(Color.SlateGray, 3.0!), _
            xx, _
            StatePanel.Bottom, _
            xx, _
            txtInteraction.Top _
        )

    End Sub

    Private Sub cmdOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOk.Click
        If cmbRole.SelectedItem Is Nothing Then
            MsgBox("Please select a role.")
            Exit Sub
        End If

        If cmbState.SelectedItem Is Nothing Then
            MsgBox("Please select a state.")
            Exit Sub
        End If

        If cmbAction.SelectedItem Is Nothing Then
            MsgBox("Please select an action.")
            Exit Sub
        End If

        If txtInteraction.Text = "" _
                OrElse _
            m_object.Interactions.Contains(m_object.Key & "_i_" & txtInteraction.Text) _
            Then
            MsgBox("Please provide an unique name for the interaction.")
            Exit Sub
        End If

        Dim newInteraction As Interaction = _
                m_object.AddNewInteraction( _
                    DirectCast(cmbRole.SelectedItem, Role), _
                    DirectCast(cmbAction.SelectedItem, Action), _
                    DirectCast(cmbState.SelectedItem, State), _
                    txtInteraction.Text _
                    )
        m_page.AddExistingElement(newInteraction)
        Close()
    End Sub

    Public Sub New(ByVal modelObject As ModelObject, ByVal systemModel As ModelSystem, ByVal page As Page)
        InitializeComponent()

        m_object = modelObject
        m_system = systemModel
        m_page = page

        With cmbState
            .DataSource = m_system.States
            .DisplayMember = "Name"
        End With
        With cmbRole
            .DataSource = m_system.Roles
            .DisplayMember = "Name"
        End With
        With cmbAction
            .DataSource = m_system.Actions
            .DisplayMember = "Name"
        End With

    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Close()
    End Sub
End Class