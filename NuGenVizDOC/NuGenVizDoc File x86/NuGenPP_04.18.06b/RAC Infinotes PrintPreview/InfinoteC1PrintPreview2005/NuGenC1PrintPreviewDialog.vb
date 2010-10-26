Option Strict On
Option Explicit On

Imports System.Drawing.Printing

Public Class NuGenC1PrintPreviewDialog
Inherits System.Windows.Forms.Form

    Public Property Document() As PrintDocument
        Get
            Return C1PrintPreviewControl.PrintDocument
        End Get
        Set(ByVal Value As PrintDocument)
            C1PrintPreviewControl.PrintDocument = Value
        End Set
    End Property

    'Public Property AlignCenterChecked() As Boolean Implements IDocumentUI.AlignCenterChecked
    '    Get

    '    End Get
    '    Set(ByVal value As Boolean)

    '    End Set
    'End Property

    'Public Property AlignCenterEnabled() As Boolean Implements IDocumentUI.AlignCenterEnabled
    '    Get

    '    End Get
    '    Set(ByVal value As Boolean)

    '    End Set
    'End Property

    'Public Property AlignLeftChecked() As Boolean Implements IDocumentUI.AlignLeftChecked
    '    Get

    '    End Get
    '    Set(ByVal value As Boolean)

    '    End Set
    'End Property

    'Public Property AlignLeftEnabled() As Boolean Implements IDocumentUI.AlignLeftEnabled
    '    Get

    '    End Get
    '    Set(ByVal value As Boolean)

    '    End Set
    'End Property

    'Public Property AlignRightChecked() As Boolean Implements IDocumentUI.AlignRightChecked
    '    Get

    '    End Get
    '    Set(ByVal value As Boolean)

    '    End Set
    'End Property

    'Public Property AlignRightEnabled() As Boolean Implements IDocumentUI.AlignRightEnabled
    '    Get

    '    End Get
    '    Set(ByVal value As Boolean)

    '    End Set
    'End Property

    'Public Property BoldChecked() As Boolean Implements IDocumentUI.BoldChecked
    '    Get

    '    End Get
    '    Set(ByVal value As Boolean)

    '    End Set
    'End Property

    'Public Property BoldEnabled() As Boolean Implements IDocumentUI.BoldEnabled
    '    Get

    '    End Get
    '    Set(ByVal value As Boolean)

    '    End Set
    'End Property

    'Public Property CopyEnabled() As Boolean Implements IDocumentUI.CopyEnabled
    '    Get

    '    End Get
    '    Set(ByVal value As Boolean)

    '    End Set
    'End Property

    'Public Property CutEnabled() As Boolean Implements IDocumentUI.CutEnabled
    '    Get

    '    End Get
    '    Set(ByVal value As Boolean)

    '    End Set
    'End Property

    'Public Property DeleteEnabled() As Boolean Implements IDocumentUI.DeleteEnabled
    '    Get

    '    End Get
    '    Set(ByVal value As Boolean)

    '    End Set
    'End Property

    'Public Property FindEnabled() As Boolean Implements IDocumentUI.FindEnabled
    '    Get

    '    End Get
    '    Set(ByVal value As Boolean)

    '    End Set
    'End Property

    'Public Property FindNextEnabled() As Boolean Implements IDocumentUI.FindNextEnabled
    '    Get

    '    End Get
    '    Set(ByVal value As Boolean)

    '    End Set
    'End Property

    'Public Property FontSelectionEnabled() As Boolean Implements IDocumentUI.FontSelectionEnabled
    '    Get

    '    End Get
    '    Set(ByVal value As Boolean)

    '    End Set
    'End Property

    'Public Property FontSize() As Integer Implements IDocumentUI.FontSize
    '    Get

    '    End Get
    '    Set(ByVal value As Integer)

    '    End Set
    'End Property

    'Public Property FontSizeEnabled() As Boolean Implements IDocumentUI.FontSizeEnabled
    '    Get

    '    End Get
    '    Set(ByVal value As Boolean)

    '    End Set
    'End Property

    'Public Property ItalicChecked() As Boolean Implements IDocumentUI.ItalicChecked
    '    Get

    '    End Get
    '    Set(ByVal value As Boolean)

    '    End Set
    'End Property

    'Public Property ItalicEnabled() As Boolean Implements IDocumentUI.ItalicEnabled
    '    Get

    '    End Get
    '    Set(ByVal value As Boolean)

    '    End Set
    'End Property

    'Public Property PasteEnabled() As Boolean Implements IDocumentUI.PasteEnabled
    '    Get

    '    End Get
    '    Set(ByVal value As Boolean)

    '    End Set
    'End Property

    'Public Property ReplaceEnabled() As Boolean Implements IDocumentUI.ReplaceEnabled
    '    Get

    '    End Get
    '    Set(ByVal value As Boolean)

    '    End Set
    'End Property

    'Public Property SelectAllEnabled() As Boolean Implements IDocumentUI.SelectAllEnabled
    '    Get

    '    End Get
    '    Set(ByVal value As Boolean)

    '    End Set
    'End Property

    'Public Property StrikethroughChecked() As Boolean Implements IDocumentUI.StrikethroughChecked
    '    Get

    '    End Get
    '    Set(ByVal value As Boolean)

    '    End Set
    'End Property

    'Public Property StrikethroughEnabled() As Boolean Implements IDocumentUI.StrikethroughEnabled
    '    Get

    '    End Get
    '    Set(ByVal value As Boolean)

    '    End Set
    'End Property

    'Public Property TextColorEnabled() As Boolean Implements IDocumentUI.TextColorEnabled
    '    Get

    '    End Get
    '    Set(ByVal value As Boolean)

    '    End Set
    'End Property

    'Public Property UnderlineChecked() As Boolean Implements IDocumentUI.UnderlineChecked
    '    Get

    '    End Get
    '    Set(ByVal value As Boolean)

    '    End Set
    'End Property

    'Public Property UnderlineEnabled() As Boolean Implements IDocumentUI.UnderlineEnabled
    '    Get

    '    End Get
    '    Set(ByVal value As Boolean)

    '    End Set
    'End Property

    'Private Sub RibbonPanel3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    'End Sub
End Class