Option Strict On
Option Explicit On

Imports System.Drawing
Imports System.Windows.Forms
Imports System.Drawing.Printing

Public MustInherit Class NuGenPreviewerBase

    Protected m_document As PrintDocument
    Protected m_currentPageNumber As Integer

    Protected m_previewPrintsAutomatically As Boolean = True

    'Private m_previewControl As PrintPreviewControl
    'Private m_previewDialog As PrintPreviewDialog



    '''' <summary>
    '''' This property allows a developer to provide their own
    '''' PrintPreviewControl or any derivative, before calling 
    '''' the DoPreview method. 
    '''' </summary>
    '''' <value>A PrintPreviewControl, or any derivative class.</value>
    '''' <returns>A PrintPreviewControl, or Nothing(null in less
    '''' civilized langauages).</returns>
    '''' <remarks></remarks>
    'Public Property PreviewControl() As PrintPreviewControl
    '    Get
    '        If m_previewControl Is Nothing Then
    '            m_previewControl = New PrintPreviewControl
    '        End If

    '        Return m_previewControl
    '    End Get

    '    Set(ByVal value As PrintPreviewControl)
    '        m_previewControl = value
    '    End Set
    'End Property

    '''' <summary>
    '''' This property allows a developer to provide their own
    '''' PrintPreviewDialog or any derivative, before calling 
    '''' the DoPreviewDialog method. 
    '''' </summary>
    '''' <value>A PrintPreviewDialog, or any derivative class.</value>
    '''' <returns>A PrintPreviewDialog, or Nothing(null in less
    '''' civilized langauages).</returns>
    '''' <remarks></remarks>
    'Public Property PreviewDialog() As PrintPreviewDialog
    '    Get
    '        If m_previewDialog Is Nothing Then
    '            m_previewDialog = New PrintPreviewDialog
    '        End If

    '        Return m_previewDialog
    '    End Get
    '    Set(ByVal value As PrintPreviewDialog)
    '        m_previewDialog = value
    '    End Set
    'End Property

    Public ReadOnly Property Document() As PrintDocument
        Get
            If m_document Is Nothing Then
                m_document = New PrintDocument
            End If

            Return m_document
        End Get
    End Property

    Protected Sub CreateDocument( _
                    ByVal printPageDeletegate As PrintPageEventHandler, _
                    ByVal fromPage As Integer, _
                    ByVal toPage As Integer _
                )

        m_currentPageNumber = 0

        With Document
            With .PrinterSettings
                .FromPage = fromPage
                .ToPage = toPage
                .MinimumPage = fromPage
                .MaximumPage = toPage
            End With
            .OriginAtMargins = True
            .PrintController = New PreviewPrintController

            AddHandler m_document.PrintPage, printPageDeletegate

            If Not m_previewPrintsAutomatically Then
                .Print()
            End If
        End With
    End Sub
End Class
