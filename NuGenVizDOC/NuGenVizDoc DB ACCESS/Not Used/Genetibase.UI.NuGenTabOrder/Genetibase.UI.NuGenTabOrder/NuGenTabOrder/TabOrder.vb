
Imports System
Imports System.Windows.Forms
Imports System.Collections
Imports System.Collections.Specialized
Imports System.Diagnostics


'/// <summary>
'/// Dynamically determine and set a tab order for a container and children according to a given strategy.
'/// </summary>
Public Class TabOrder
    '/// <summary>
    '/// Compare two controls in the selected tab scheme.
    '/// </summary>
    Private Class TabSchemeComparer
        Implements IComparer

        Private comparisonScheme As TabScheme

#Region "IComparer Members"

        Public Overridable Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare

            Dim control1 As Control = CType(x, Control)
            Dim control2 As Control = CType(y, Control)

            If IsNothing(control1) Or IsNothing(control2) Then
                Debug.Assert(False, "Attempting to compare a non-control")
                Return 0
            End If

            If comparisonScheme = TabScheme.AcrossFirst Then

                '// The primary direction to sort is the y direction (using the Top property).
                '// If two controls have the same y coordination, then we sort them by their x's.
                If control1.Top < control2.Top Then
                    Return -1
                ElseIf control1.Top > control2.Top Then
                    Return 1
                Else
                    Return (control1.Left.CompareTo(control2.Left))
                End If
            Else    '// comparisonScheme = TabScheme.DownFirst

                '// The primary direction to sort is the x direction (using the Left property).
                '// If two controls have the same x coordination, then we sort them by their y's.
                If control1.Left < control2.Left Then
                    Return -1
                ElseIf control1.Left > control2.Left Then
                    Return 1
                Else
                    Return (control1.Top.CompareTo(control2.Top))
                End If
            End If
        End Function

#End Region

        '// Create a tab scheme comparer that uses the given scheme.
        Public Sub New(ByVal scheme As TabScheme)
            comparisonScheme = scheme
        End Sub

    End Class

    '/// <summary>
    '/// The container whose tab order we manage.
    '/// </summary>
    Private container As Control

    '/// <summary>
    '/// Hash of controls to schemes so that individual containers can have different ordering
    '/// strategies than their parents.
    '/// </summary>
    Private schemeOverrides As Hashtable

    '/// <summary>
    '/// The tab index we start numbering from when the tab order is applied.
    '/// </summary>
    Private curTabIndex As Integer = 0

    '/// <summary>
    '/// The general tab-ordering strategy (i.e. whether we tab across rows first, or down columns).
    '/// </summary>
    Public Enum TabScheme
        AcrossFirst
        DownFirst
    End Enum

    '/// <summary>
    '/// Constructor
    '/// </summary>
    '/// <param name="container">The container whose tab order we manage.</param>
    Public Sub New(ByVal container As Control)
        Me.container = container
        Me.curTabIndex = 0
        Me.schemeOverrides = New Hashtable
    End Sub

    '/// <summary>
    '/// Construct a tab order manager that starts numbering at the given tab index.
    '/// </summary>
    '/// <param name="container">The container whose tab order we manage.</param>
    '/// <param name="curTabIndex">Where to start numbering.</param>
    '/// <param name="schemeOverrides">List of controls with explicitly defined schemes.</param>
    Private Sub New(ByVal container As Control, ByVal curTabIndex As Integer, ByVal schemeOverrides As Hashtable)
        Me.container = container
        Me.curTabIndex = curTabIndex
        Me.schemeOverrides = schemeOverrides
    End Sub

    '/// <summary>
    '/// Explicitly set a tab order scheme for a given (presumably container) control.
    '/// </summary>
    '/// <param name="c">The control to set the scheme for.</param>
    '/// <param name="scheme">The requested scheme.</param>
    Public Sub SetSchemeForControl(ByVal c As Control, ByVal scheme As TabScheme)
        schemeOverrides(c) = scheme
    End Sub

    '/// <summary>
    '/// Recursively set the tab order on this container and all of its children.
    '/// </summary>
    '/// <param name="scheme">The tab ordering strategy to apply.</param>
    '/// <returns>The next tab index to be used.</returns>
    Public Function SetTabOrder(ByVal scheme As TabScheme) As Integer

        '// Tab order isn't important enough to ever cause a crash, so replace any exceptions
        '// with assertions.
        Try
            Dim controlArraySorted As New ArrayList
            controlArraySorted.AddRange(container.Controls)
            controlArraySorted.Sort(New TabSchemeComparer(scheme))

            Dim controlsWithScheme As ArrayList
            Dim c As Control
            For Each c In controlArraySorted
                c.TabIndex = curTabIndex
                curTabIndex = curTabIndex + 1

                If c.Controls.Count > 0 Then
                    '// Control has children -- recurse.
                    Dim childScheme As TabScheme = scheme
                    If schemeOverrides.Contains(c) Then
                        childScheme = CType(schemeOverrides(c), TabScheme)
                    End If
                    curTabIndex = (New TabOrder(c, curTabIndex, schemeOverrides)).SetTabOrder(childScheme)
                End If
            Next c

            Return curTabIndex
        Catch e As Exception
            Debug.Assert(False, "Exception in TabOrderManager.SetTabOrder:  " + e.Message)
            Return 0
        End Try
    End Function
End Class

