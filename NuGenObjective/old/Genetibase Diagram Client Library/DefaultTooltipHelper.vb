Option Strict On
Option Explicit On

Imports Genetibase.NuGenObjective

Public Class DefaultTooltipHelper
    Private m_toolTipEventArgs As TooltipEventArgs

    Public Sub SetToolTip(ByVal role As Role)
        With m_toolTipEventArgs
            .Title = "Role:" & role.Name
            .Text = role.Description
        End With
    End Sub

    Public Sub SetTooltip(ByVal action As Action)
        With m_toolTipEventArgs
            .Title = "Action:" & action.Name
            .Text = action.Description
        End With
    End Sub

    Public Sub SetTooltip(ByVal state As State)
        With m_toolTipEventArgs
            .Title = "State:" & state.Name
            .Text = state.Description
        End With
    End Sub

    Public Sub SetTooltip(ByVal modelObject As ModelObject)
        With m_toolTipEventArgs
            .Title = "Object:" & modelObject.Name
            .Text = "Interactions:" & vbCrLf
            For Each i As Interaction In modelObject.Interactions
                .Text &= vbTab & i.Name & vbCrLf
            Next
        End With
    End Sub

    Public Sub SetTooltip(ByVal interaction As Interaction)
        With m_toolTipEventArgs
            .Title = "Interaction:" & interaction.Name
            .Text = String.Format( _
                        "Object :{1}{0}Role:{2}{0}Action:{3}{0}State:{4}{0}", _
                        vbCrLf, _
                        interaction.Object.Name, _
                        interaction.Role.Name, _
                        interaction.Action.Name, _
                        interaction.State.Name _
                    )
        End With
    End Sub

    Public Sub SetTooltip(ByVal page As Page)
        m_toolTipEventArgs.Cancel = True
    End Sub

    Public Sub New(ByVal args As TooltipEventArgs)
        m_toolTipEventArgs = args
    End Sub
End Class
