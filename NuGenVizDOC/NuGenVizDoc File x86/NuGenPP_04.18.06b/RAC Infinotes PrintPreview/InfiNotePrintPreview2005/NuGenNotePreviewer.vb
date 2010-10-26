Option Strict On
Option Explicit On

Imports System.Windows.Forms

Public Class NuGenNotePreviewer
    Inherits NuGenNotePreviewerBase

    Private m_previewControl As PrintPreviewControl
    Private m_previewDialog As PrintPreviewDialog

    ''' <summary>
    ''' This property allows a developer to provide their own
    ''' PrintPreviewControl or any derivative, before calling 
    ''' the DoPreview method. 
    ''' </summary>
    ''' <value>A PrintPreviewControl, or any derivative class.</value>
    ''' <returns>A PrintPreviewControl, or Nothing(null in less
    ''' civilized langauages).</returns>
    ''' <remarks></remarks>
    Public Property PreviewControl() As PrintPreviewControl
        Get
            If m_previewControl Is Nothing Then
                m_previewControl = New PrintPreviewControl
            End If

            Return m_previewControl
        End Get

        Set(ByVal value As PrintPreviewControl)
            m_previewControl = value
        End Set
    End Property

    ''' <summary>
    ''' This property allows a developer to provide their own
    ''' PrintPreviewDialog or any derivative, before calling 
    ''' the DoPreviewDialog method. 
    ''' </summary>
    ''' <value>A PrintPreviewDialog, or any derivative class.</value>
    ''' <returns>A PrintPreviewDialog, or Nothing(null in less
    ''' civilized langauages).</returns>
    ''' <remarks></remarks>
    Public Property PreviewDialog() As PrintPreviewDialog
        Get
            If m_previewDialog Is Nothing Then
                m_previewDialog = New PrintPreviewDialog
            End If

            Return m_previewDialog
        End Get
        Set(ByVal value As PrintPreviewDialog)
            m_previewDialog = value
        End Set
    End Property

    Public Overrides Sub DoPreview(ByVal note As Agilix.Ink.Note.Note)
        MyBase.DoPreview(note)
        PreviewControl.Document = Document
    End Sub

    Public Overrides Sub DoPreviewDialog(ByVal note As Agilix.Ink.Note.Note)
        MyBase.DoPreviewDialog(note)
        With PreviewDialog
            .Document = Document
            .ShowDialog()
        End With
    End Sub
End Class
