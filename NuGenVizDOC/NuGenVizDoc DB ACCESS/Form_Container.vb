Imports System
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Windows.Forms

Public Class Form_Container
    'Dim FF As Form_Firms
#Region " Layered Window "

    Protected Overrides Sub WndProc(ByRef m As Message)
        If (m.Msg = 132) Then
            m.Result = CType(2, IntPtr)
            ' Disable this sub for NoMoving Window
            Return
        End If
        MyBase.WndProc(m)
    End Sub

    Protected Overrides ReadOnly Property CreateParams() As CreateParams
        Get
            Dim cp As CreateParams = MyBase.CreateParams
            cp.ExStyle = (cp.ExStyle Or 524288) ' This form has to have the WS_EX_LAYERED extended style
            Return cp
        End Get
    End Property

    Public Overloads Sub SetBitmap(ByVal bitmap As Bitmap)
        SetBitmap(bitmap, 255)
    End Sub

    Public Overloads Sub SetBitmap(ByVal bitmap As Bitmap, ByVal opacity As Byte)
        If (bitmap.PixelFormat <> PixelFormat.Format32bppArgb) Then
            Throw New ApplicationException("The bitmap must be 32ppp with alpha-channel.")
        End If
        Dim screenDc As IntPtr = Win32.GetDC(IntPtr.Zero)
        Dim memDc As IntPtr = Win32.CreateCompatibleDC(screenDc)
        Dim hBitmap As IntPtr = IntPtr.Zero
        Dim oldBitmap As IntPtr = IntPtr.Zero
        Try
            hBitmap = bitmap.GetHbitmap(Color.FromArgb(0))
            ' grab a GDI handle from this GDI+ bitmap
            oldBitmap = Win32.SelectObject(memDc, hBitmap)
            Dim size As Win32.Size = New Win32.Size(bitmap.Width, bitmap.Height)
            Dim pointSource As Win32.Point = New Win32.Point(0, 0)
            Dim topPos As Win32.Point = New Win32.Point(Left, Top)
            Dim blend As Win32.BLENDFUNCTION = New Win32.BLENDFUNCTION
            blend.BlendOp = Win32.AC_SRC_OVER
            blend.BlendFlags = 0
            blend.SourceConstantAlpha = opacity
            blend.AlphaFormat = Win32.AC_SRC_ALPHA
            Win32.UpdateLayeredWindow(Handle, screenDc, topPos, size, memDc, pointSource, 0, blend, Win32.ULW_ALPHA)
        Finally
            Win32.ReleaseDC(IntPtr.Zero, screenDc)
            If (hBitmap <> IntPtr.Zero) Then
                Win32.SelectObject(memDc, oldBitmap)
                'Windows.DeleteObject(hBitmap); // The documentation says that we have to use the Windows.DeleteObject... but since there is no such method I use the normal DeleteObject from Win32 GDI and it's working fine without any resource leak.
                Win32.DeleteObject(hBitmap)
            End If
            Win32.DeleteDC(memDc)
        End Try
    End Sub

#End Region

#Region " Procedures "

    Private MyBitmap As Bitmap = Nothing
    Private F As Form
    Private F_Active As Boolean = False
    Private Me_Active As Boolean = True

    Public Sub Prepare(ByVal m_Location As Drawing.Point, ByVal m_ChildForm As Form)
        F = m_ChildForm
        '
        Me.Location = m_Location
        Me.Size = New Drawing.Size(F.Width + 30, F.Height + 51)
        F.Location = New Drawing.Point(Me.Location.X + 15, Me.Location.Y + 32)
        F.Opacity = 75.0
        F.ShowInTaskbar = False
        AddHandler F.FormClosed, AddressOf Handle_ChildFormClose
        AddHandler F.Activated, AddressOf Handle_ChildFormActivated
        AddHandler F.Deactivate, AddressOf Handle_ChildFormDeactivate
        ' L15, R15, T32, B19
        MyBitmap = New Bitmap(Me.Width, Me.Height, PixelFormat.Format32bppArgb)
        Dim Gr As Graphics = Graphics.FromImage(MyBitmap)
        '
        Dim DestLU As Rectangle = New Rectangle(0, 0, 15, 32)
        Dim DestLM As Rectangle = New Rectangle(0, 32, 15, Me.Height - 51)
        Dim DestLD As Rectangle = New Rectangle(0, Me.Height - 19, 15, 19)
        Dim DestMU As Rectangle = New Rectangle(15, 0, Me.Width - 30, 32)
        Dim DestMM As Rectangle = New Rectangle(15, 32, Me.Width - 30, Me.Height - 51)
        Dim DestMD As Rectangle = New Rectangle(15, Me.Height - 19, Me.Width - 30, 19)
        Dim DestRU As Rectangle = New Rectangle(Me.Width - 15, 0, 15, 32)
        Dim DestRM As Rectangle = New Rectangle(Me.Width - 15, 32, 15, Me.Height - 51)
        Dim DestRD As Rectangle = New Rectangle(Me.Width - 15, Me.Height - 19, 15, 19)
        '
        Dim SourceLU As Rectangle = New Rectangle(0, 0, 15, 32)
        Dim SourceLM As Rectangle = New Rectangle(0, 32, 15, My.Resources.Window.Height - 51)
        Dim SourceLD As Rectangle = New Rectangle(0, My.Resources.Window.Height - 19, 15, 19)
        Dim SourceMU As Rectangle = New Rectangle(15, 0, My.Resources.Window.Width - 30, 32)
        Dim SourceMM As Rectangle = New Rectangle(15, 32, My.Resources.Window.Width - 30, My.Resources.Window.Height - 51)
        Dim SourceMD As Rectangle = New Rectangle(15, My.Resources.Window.Height - 19, My.Resources.Window.Width - 30, 19)
        Dim SourceRU As Rectangle = New Rectangle(My.Resources.Window.Width - 15, 0, 15, 32)
        Dim SourceRM As Rectangle = New Rectangle(My.Resources.Window.Width - 15, 32, 15, My.Resources.Window.Height - 51)
        Dim SourceRD As Rectangle = New Rectangle(My.Resources.Window.Width - 15, My.Resources.Window.Height - 19, 15, 19)
        '
        Gr.DrawImage(My.Resources.Window, DestLU, SourceLU, GraphicsUnit.Pixel)
        Gr.DrawImage(My.Resources.Window, DestLM, SourceLM, GraphicsUnit.Pixel)
        Gr.DrawImage(My.Resources.Window, DestLD, SourceLD, GraphicsUnit.Pixel)
        Gr.DrawImage(My.Resources.Window, DestMU, SourceMU, GraphicsUnit.Pixel)
        Gr.DrawImage(My.Resources.Window, DestMM, SourceMM, GraphicsUnit.Pixel)
        Gr.DrawImage(My.Resources.Window, DestMD, SourceMD, GraphicsUnit.Pixel)
        Gr.DrawImage(My.Resources.Window, DestRU, SourceRU, GraphicsUnit.Pixel)
        Gr.DrawImage(My.Resources.Window, DestRM, SourceRM, GraphicsUnit.Pixel)
        Gr.DrawImage(My.Resources.Window, DestRD, SourceRD, GraphicsUnit.Pixel)
        '
        Dim TextRect As Rectangle = New Rectangle(15, 8, Me.Width - 130, 23)
        Gr.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAliasGridFit
        Dim SF As New StringFormat
        SF.Alignment = StringAlignment.Near
        SF.LineAlignment = StringAlignment.Center
        Gr.DrawString(m_ChildForm.Text, New Font("Tahoma", 8, FontStyle.Bold), New SolidBrush(Color.FromArgb(50, 50, 50)), TextRect, SF)
    End Sub

    Private Sub Handle_ChildFormClose(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs)
        Me.Dispose()
    End Sub

    Private Sub Handle_ChildFormActivated(ByVal sender As Object, ByVal e As System.EventArgs)
        F_Active = True
        SetBitmap(MyBitmap)
        F.Opacity = 75.0
    End Sub

    Private Sub Handle_ChildFormDeactivate(ByVal sender As Object, ByVal e As System.EventArgs)
        F_Active = False
        If Me_Active = False Then
            SetBitmap(MyBitmap, 150)
            F.Opacity = 75.0
        End If
    End Sub

#End Region

#Region " Form Events "

    Private Sub Form_Container_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated
        Me_Active = True
        SetBitmap(MyBitmap)
        F.Opacity = 75.0
    End Sub

    Private Sub Form_Container_Deactivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Deactivate
        Me_Active = False
        If F_Active = False Then
            SetBitmap(MyBitmap, 150)
            F.Opacity = 75.0
        End If
    End Sub

    Private Sub Form_Container_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        SetBitmap(MyBitmap)
        F.Show(Me)
    End Sub

    Private Sub Form_Container_LocationChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LocationChanged
        F.Location = New Drawing.Point(Me.Location.X + 15, Me.Location.Y + 32)
        Dim SnapUI As New Genetibase.UI.NuGenUISnap(Me)
        SnapUI.StickOnMove = True
        SnapUI.StickToScreen = True
        SnapUI.StickToOther = True
    End Sub

#End Region

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        'SetBitmap(MyBitmap)
        'FF = New Form_Firms
        'Prepare(New Drawing.Point(1, 0), FF)
        'F.Show()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
End Class