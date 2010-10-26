Option Strict On
Option Explicit On 

Imports System.Drawing
Imports System.Windows.Forms
Imports System.Drawing.Printing

''' -----------------------------------------------------------------------------
''' Project	 : InfiNotePrintPreview2003
''' Class	 : UI.NuGenPreviewerBase
''' 
''' -----------------------------------------------------------------------------
''' <summary>
''' This is the base class of all previewers. It is not meant to be used directly.
''' </summary>
''' <remarks>
''' </remarks>
''' -----------------------------------------------------------------------------
Public MustInherit Class NuGenPreviewerBase

    Protected m_document As PrintDocument
    Protected m_currentPageNumber As Integer

    Protected m_previewPrintsAutomatically As Boolean = True

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The Document property represents the document being previewed. 
    ''' </summary>
    ''' <value>A PrintDocument.</value>
    ''' <remarks>
    ''' If a document has not been created when this property is accessed, a new
    ''' one will be created automatically. 
    ''' </remarks>
    ''' <history>
    ''' 	[Raj]	4/18/2006	Created
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public ReadOnly Property Document() As PrintDocument
        Get
            If m_document Is Nothing Then
                m_document = New PrintDocument
            End If

            Return m_document
        End Get
    End Property

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' This method creates the PrintDocument object that will be previewed, and sets
    ''' up the print handler method and the starting and ending pages for the preview.
    ''' </summary>
    ''' <param name="printPageDeletegate">A PrintPageEventHandler delegate.</param>
    ''' <param name="fromPage">The zero-based start page for the preview run.</param>
    ''' <param name="toPage">The zero-based end page for the preview run.</param>
    ''' <remarks>
    ''' This method should be called by subclasses as appropriate.
    ''' </remarks>
    ''' -----------------------------------------------------------------------------
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
