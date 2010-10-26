Option Strict On
Option Explicit On 

Imports C1.Win.C1PrintPreview

Public Class NuGenC1NotePreviewer
    Inherits NuGenNotePreviewerBase

    Private m_previewControl As C1PrintPreview
    Private m_previewDialog As NuGenC1PrintPreviewDialog

    ''' <summary>
    ''' This property allows a developer to provide their own
    ''' PrintPreviewControl or any derivative, before calling 
    ''' the DoPreview method. 
    ''' </summary>
    ''' <value>A PrintPreviewControl, or any derivative class.</value>
    ''' <returns>A PrintPreviewControl, or Nothing(null in less
    ''' civilized langauages).</returns>
    ''' <remarks></remarks>
    Public Property PreviewControl() As C1PrintPreview
        Get
            If m_previewControl Is Nothing Then
                m_previewControl = New C1PrintPreview
            End If

            Return m_previewControl
        End Get

        Set(ByVal value As C1PrintPreview)
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
    Public Property PreviewDialog() As NuGenC1PrintPreviewDialog
        Get
            If m_previewDialog Is Nothing Then
                m_previewDialog = New NuGenC1PrintPreviewDialog
            End If

            Return m_previewDialog
        End Get
        Set(ByVal value As NuGenC1PrintPreviewDialog)
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

    Public Sub New()
        ' m_previewPrintsAutomatically = False
    End Sub
End Class
