Option Strict On
Option Explicit On

Imports Genetibase.NuGenObjective

Public Interface IDesigner
    ReadOnly Property CurrentDiagram() As Diagram
    ReadOnly Property DiagramClient() As ClientDiagram

    Function SaveFileDialog(ByVal extension As String, ByVal filter As String) As String

End Interface
