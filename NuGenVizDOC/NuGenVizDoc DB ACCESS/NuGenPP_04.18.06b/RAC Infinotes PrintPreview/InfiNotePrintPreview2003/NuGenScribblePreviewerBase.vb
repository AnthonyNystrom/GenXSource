Option Strict On
Option Explicit On 

Imports System.Drawing.Printing
Imports System.Drawing.Drawing2D
Imports Agilix.Ink.Scribble

''' -----------------------------------------------------------------------------
''' Project	 : InfiNotePrintPreview2003
''' Class	 : UI.NuGenScribblerPreviewer
''' 
''' -----------------------------------------------------------------------------
''' <summary>
''' This class provides base print preview facility for the Agilix InfiNotes
''' Scribble class, or any class derived from it.
''' </summary>
''' <remarks>
''' This class should be subclassed for different types of preview controls.
''' It is not meant to be used directly.
''' </remarks>
''' -----------------------------------------------------------------------------
Public MustInherit Class NuGenScribblePreviewerBase
    Inherits NuGenPreviewerBase


    Private m_scribble As Scribble

    Private m_physicalrectangles As New ArrayList
    Private m_currentRectangle As Integer

    Private m_showPrintDialog As Boolean = False

    Public Property ShowPrintDialog() As Boolean
        Get
            Return m_showPrintDialog
        End Get
        Set(ByVal Value As Boolean)
            m_showPrintDialog = True
        End Set
    End Property
    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Call this method to render a preview on to a preview control. Displaying the
    ''' control itself is the caller's responsibility.
    ''' </summary>
    ''' <param name="scribble">An Agilix Scribble control.</param>
    ''' <remarks>
    ''' The contents of the Scribble control will be rendered to the relevant 
    ''' preview control.
    ''' </remarks>
    ''' -----------------------------------------------------------------------------
    Public Overridable Sub DoPreview(ByVal scribble As Agilix.Ink.Scribble.Scribble)
        m_scribble = scribble
        m_currentRectangle = 0


        CreateDocument(AddressOf ScribblePrintPageHandler, _
                        1, _
                        scribble.Document.Pages.Count _
                )

        If m_showPrintDialog Then
            Dim pd As New PrintDialog
            With pd
                .Document = Document
                .AllowSomePages = True
                .AllowSelection = True
                .ShowDialog()

                Application.DoEvents()

                With .PrinterSettings
                    If .PrintRange = PrintRange.Selection Then
                        Dim currPageNumber As Integer = _
                                scribble.Document.Pages.IndexOf( _
                                    scribble.Page _
                                )
                        .FromPage = currPageNumber + 1
                        .ToPage = currPageNumber + 1
                    End If
                End With
            End With
        End If
        'm_scribble = Nothing
    End Sub

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' Call this method to display a print preview dialog.
    ''' </summary>
    ''' <param name="Scribble">An Agilix Scribble control.</param>
    ''' <remarks>
    ''' The contents of the Scribble control will be displayed in a print preview
    ''' dialog. 
    ''' </remarks>
    ''' -----------------------------------------------------------------------------
    Public Overridable Sub DoPreviewDialog(ByVal Scribble As Agilix.Ink.Scribble.Scribble)
        DoPreview(Scribble)
    End Sub

    Private Sub ScribblePrintPageHandler( _
                        ByVal sender As Object, _
                        ByVal e As PrintPageEventArgs _
                        )

        Dim doc As PrintDocument = DirectCast(sender, PrintDocument)

        Dim firstPage As Integer = doc.PrinterSettings.FromPage - 1
        Dim lastPage As Integer = Math.Min(m_scribble.Document.Pages.Count, doc.PrinterSettings.ToPage) - 1

        ' If we have reached the last page, exit
        If m_currentPageNumber > lastPage Then
            Return
        End If

        ' If the current page number is less than the
        ' first page to be printed, adjust it
        If m_currentPageNumber < firstPage Then
            m_currentPageNumber = firstPage
        End If

        ' Take the current PrintDocument's printable area, and
        ' convert the units to HiMetric, because Scribble
        ' measures things in HiMetric
        Dim paper As Rectangle = New Rectangle(CInt(e.MarginBounds.Left * 2540 / 100), _
                 CInt(e.MarginBounds.Top * 2540 / 100), _
                 CInt(e.MarginBounds.Width * 2540 / 100), _
                 CInt(e.MarginBounds.Height * 2540 / 100) _
                 )

        Dim renderer As Agilix.Ink.Scribble.Renderer = _
                New Agilix.Ink.Scribble.Renderer(m_scribble.Document)

        ' The first time this event handler is called, find out how many physical
        ' (printed) pages are neccessary for each "page" on a Scribble.
        If (m_physicalrectangles.Count = 0) Then

            For Page As Integer = firstPage To lastPage
                ' Get the size of the area in which printing can happen,
                ' by using the printable area and the current Scribble
                ' page as parameters
                Dim pagePaper As Rectangle = GetAdjustedSize(m_scribble.Document.Pages(Page), paper)
                ' Place this area at the top left of the printable area
                pagePaper.Location = New Point(0, 0)

                ' Call GetPhysicalPages. This will return an array of rectangles.
                ' Each rectangle will specify an area on the Scribble page
                ' that can fit in one printed page. Add the array returned to
                ' the m_physicalRectangles list.
                m_physicalrectangles.Add( _
                    renderer.GetPhysicalPages(Page, pagePaper.Size, pagePaper.Size) _
                    )
            Next
        End If


        ' Get the collection of rectangles corresponding to the current
        ' Scribble page
        Dim clipRectangles() As Rectangle = _
                    DirectCast( _
                        m_physicalrectangles(m_currentPageNumber - firstPage), _
                        Rectangle() _
                        )

        ' Choose the one to print this time
        Dim clipRectangle As Rectangle = clipRectangles(m_currentRectangle)

        ' Get the size of the area in which printing can happen,
        ' by using the printable area and the current Scribble
        ' page as parameters
        Dim adjustedPaper As Rectangle = GetAdjustedSize(m_scribble.Document.Pages(m_currentPageNumber), paper)

        Dim state As GraphicsState = e.Graphics.Save()

        ' The clipRectangle may be larger than what we asked for,
        ' As the GetPhysicalPages call automatically scales to fit whatever width.
        ' So we calculate a factor to scale the rectangle that we have to print
        ' into the area that we have.
        Dim scale As Single = CSng(adjustedPaper.Width) / CSng(clipRectangle.Width)

        ' Set the graphics context to HiMetric, and translate so that the print origin is at (0,0)
        Dim offset As SizeF = New SizeF(e.Graphics.Transform.OffsetX, e.Graphics.Transform.OffsetY)

        With e.Graphics
            ' Set the origin to 0,0
            .TranslateTransform(-offset.Width, -offset.Height)
            ' Make sure pixels are being used as units
            .PageUnit = GraphicsUnit.Pixel
            ' Apply a scale as follows:
            '   Find out the pixels-per-inch value of the current surface, on
            '   each axis, and divide that by 2540 (1 himetric point is 
            '   1/2540 of one inch). Multiply the result with the scale factor
            '   we calculated earlier.
            .ScaleTransform(e.Graphics.DpiX / 2540.0F * scale, e.Graphics.DpiY / 2540.0F * scale)
            .TranslateTransform((offset.Width * 25.4F + adjustedPaper.X) / scale, (offset.Height * 25.4F + adjustedPaper.Y) / scale, MatrixOrder.Prepend)
            .TranslateTransform(0, -clipRectangle.Top)
        End With

        ' Draw into the print preview surface (e.Graphics) the current Scribble
        ' Page (currentPageNumber), using the cliprectangle that we 
        ' retrieved earlier.
        renderer.Draw(e.Graphics, m_currentPageNumber, clipRectangle, m_scribble.Document.Pages(m_currentPageNumber).Size)

        e.Graphics.Restore(state)

        ' Increase the current clip rectangle counter
        m_currentRectangle += 1

        ' If there are more clip rectangles for the current
        ' Scribble page, do NOT increase the logical page
        ' count.
        If (m_currentRectangle <> clipRectangles.Length) Then
            e.HasMorePages = True
        Else
            ' Otherwise increment the current logical page
            ' count, reset the current rectangle counter
            m_currentRectangle = 0
            m_currentPageNumber += 1

            ' Indicate if there are more logical pages
            If m_currentPageNumber <= lastPage Then
                e.HasMorePages = True
            Else
                e.HasMorePages = False
                m_currentPageNumber = 0
            End If
        End If

    End Sub

    Private Function GetAdjustedSize( _
                            ByVal page As Agilix.Ink.Scribble.Page, _
                            ByVal originalSize As Rectangle _
                    ) As Rectangle

        ' If the size of the printable area is not the "standard"
        ' 8.5x11 inches with one-inch margins, use the size of the
        ' printable area
        If (Rectangle.op_Inequality(originalSize, New Rectangle(0, 0, CInt(6.5 * 2540), 9 * 2540))) Then
            Return originalSize
        End If

        ' If the width of the Scribble page is less than that of 
        ' the printable area, use the size of the printable area
        If (page.Width <= originalSize.Width) Then
            Return originalSize
        End If

        ' This one I have kept as is, because IBinder is not
        ' well documented
        If Not page.IBinder Is Nothing Then
            ' Return the original IBinder size
            Return New Rectangle(CInt(-0.75 * 2540), CInt(-0.75 * 2540), 8 * 2540, CInt(10.25 * 2540))
        End If

        ' Center the width of the Scribble page horizontally in
        ' the total width of the printable area
        Dim newWidth As Integer = Math.Min(page.Width, 8 * 2540)
        Dim offset As Integer = CInt((originalSize.Width - newWidth) / 2)

        Return New Rectangle(offset, originalSize.Y, newWidth, originalSize.Height)
    End Function

End Class
