Imports System
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Windows.Forms
Imports System.Runtime.InteropServices
Imports System.ComponentModel
<LicenseProvider(GetType(Xheo.Licensing.ExtendedLicenseProvider))> _
Public Class Form_Splash
    Dim _license As License = Nothing
#Region " Layered Window "

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

    Private Sub Form_Splash_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SetBitmap(My.Resources.NuGenVizCap_LOGO_VEC)
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.


        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _license = LicenseManager.Validate(GetType(Form_Splash), Me)
    End Sub
End Class
