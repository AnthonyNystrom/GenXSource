Option Strict On
Option Explicit On

Imports System.Windows.Forms

Public Interface IClientAddin
    ReadOnly Property Name() As String

    Sub Activate(ByVal designer As IDesigner, ByVal rootAddinMenu As ToolStripMenuItem)

    Sub Deactivate(ByVal designer As IDesigner, ByVal rootAddinMenu As ToolStripMenuItem)

    Sub Invoke(ByVal client As ClientDiagram)
End Interface
