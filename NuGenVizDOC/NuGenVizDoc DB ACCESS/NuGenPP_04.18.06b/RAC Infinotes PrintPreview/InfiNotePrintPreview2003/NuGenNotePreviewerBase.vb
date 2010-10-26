Option Strict On
Option Explicit On 

Imports Agilix.Ink.Note
Imports System.Drawing.Printing
Imports System.Drawing.Drawing2D

''' -----------------------------------------------------------------------------
''' Project	 : InfiNotePrintPreview2003
''' Class	 : UI.NuGenNotePreviewer
''' 
''' -----------------------------------------------------------------------------
''' <summary>
''' This class provides a a print preview facility for the Agilix InfiNotes
''' Note class, or any class derived from it.
''' </summary>
''' <remarks>
''' </remarks>
''' -----------------------------------------------------------------------------
Public MustInherit Class NuGenNotePreviewerBase
    Inherits NuGenPreviewerBase

    Protected m_note As Note


    Public Overridable Sub DoPreview(ByVal note As Agilix.Ink.Note.Note)
        m_note = note

        CreateDocument(AddressOf NotePrintPageHandler, _
                1, _
                5 _
        )

        'm_note = Nothing
    End Sub

    Public Overridable Sub DoPreviewDialog(ByVal note As Agilix.Ink.Note.Note)
        MyClass.DoPreview(note)
    End Sub

    Protected Sub NotePrintPageHandler( _
                            ByVal sender As Object, _
                            ByVal e As PrintPageEventArgs _
                            )

        ' Translate the graphics by the printable margin
        Dim hDC As IntPtr = e.Graphics.GetHdc()
        Dim offsetX As Integer = UnsafeCode.GetDeviceCaps(hDC, PHYSICALOFFSETX)
        Dim offsetY As Integer = UnsafeCode.GetDeviceCaps(hDC, PHYSICALOFFSETY)
        e.Graphics.ReleaseHdc(hDC)

        e.Graphics.PageUnit = GraphicsUnit.Pixel

        ' Build paper clip rectangle
        Dim paper As Rectangle = New Rectangle( _
          CInt((e.Graphics.DpiX / 4.0!) - offsetX), _
          CInt((e.Graphics.DpiY / 4.0!) - offsetY), _
          CInt((e.PageBounds.Width * e.Graphics.DpiX / 100.0!) - (e.Graphics.DpiX / 2.0!)), _
          CInt((e.PageBounds.Height * e.Graphics.DpiY / 100.0!) - (e.Graphics.DpiY * 3.0! / 4.0!)))
        'Dim paper As Rectangle = New Rectangle(CInt(e.MarginBounds.Left * 2540 / 100), _
        '                 CInt(e.MarginBounds.Top * 2540 / 100), _
        '                 CInt(e.MarginBounds.Width * 2540 / 100), _
        '                 CInt(e.MarginBounds.Height * 2540 / 100) _
        '                 )

        ' Convert width and height to hi-metric
        Dim pageWidth As Integer = CInt(paper.Width * 2540.0! / e.Graphics.DpiX)
        Dim pageHeight As Integer = CInt(paper.Height * 2540.0! / e.Graphics.DpiY)

        ' Adjust document position if it is smaller than clip rectangle
        Dim offset As Integer = CInt(Math.Min(Math.Max(pageWidth - m_note.Document.Stationery.MinWidth, 0) / 2, 2540 * 3 / 4))
        Dim dx As Integer = CInt(offset * e.Graphics.DpiX / 2540)
        Dim dy As Integer = CInt(offset * e.Graphics.DpiY / 2540)
        paper.Inflate(-dx, -dy)

        ' Transform graphics to map hi-metric units to paper pixels
        e.Graphics.TranslateTransform(paper.X, paper.Y)
        e.Graphics.ScaleTransform(e.Graphics.DpiX / 2540.0!, e.Graphics.DpiY / 2540.0!)
        e.Graphics.TranslateTransform(0, -pageHeight * m_currentPageNumber)

        ' Convert paper rectangle to hi-metric for clip rectangle
        Dim pts() As Point = {paper.Location, New Point(paper.Right, paper.Bottom)}
        Dim invert As Matrix = e.Graphics.Transform
        invert.Invert()
        invert.TransformPoints(pts)
        Dim clipRectangle As Rectangle = Rectangle.FromLTRB(pts(0).X, pts(0).Y, pts(1).X, pts(1).Y)

        ' Render a physical page of the note
        Dim renderer As Agilix.Ink.Note.Renderer = New Agilix.Ink.Note.Renderer(m_note.Document)
        renderer.Draw(e.Graphics, clipRectangle, m_note.Document.Size)

        ' Check to see if the whole note has printed or if it requires more physical pages
        If ((m_currentPageNumber + 1) * pageHeight < m_note.Document.Height) Then
            e.HasMorePages = True
            m_currentPageNumber += 1
        End If

    End Sub
End Class
