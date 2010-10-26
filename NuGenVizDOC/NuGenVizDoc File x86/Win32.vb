Imports System.Runtime.InteropServices

Module Win32

    Public Const ULW_COLORKEY As Int32 = 1
    Public Const ULW_ALPHA As Int32 = 2
    Public Const ULW_OPAQUE As Int32 = 4
    Public Const AC_SRC_OVER As Byte = 0
    Public Const AC_SRC_ALPHA As Byte = 1
    Public Declare Function UpdateLayeredWindow Lib "user32.dll" (ByVal hwnd As IntPtr, ByVal hdcDst As IntPtr, ByRef pptDst As Point, ByRef psize As Size, ByVal hdcSrc As IntPtr, ByRef pprSrc As Point, ByVal crKey As Int32, ByRef pblend As BLENDFUNCTION, ByVal dwFlags As Int32) As Bool
    Public Declare Function GetDC Lib "user32.dll" (ByVal hWnd As IntPtr) As IntPtr
    Public Declare Function ReleaseDC Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal hDC As IntPtr) As Integer
    Public Declare Function CreateCompatibleDC Lib "gdi32.dll" (ByVal hDC As IntPtr) As IntPtr
    Public Declare Function DeleteDC Lib "gdi32.dll" (ByVal hdc As IntPtr) As Bool
    Public Declare Function SelectObject Lib "gdi32.dll" (ByVal hDC As IntPtr, ByVal hObject As IntPtr) As IntPtr
    Public Declare Function DeleteObject Lib "gdi32.dll" (ByVal hObject As IntPtr) As Bool
    Public Enum Bool As Integer
        F = 0 ' False
        T = 1 ' True
    End Enum

    <StructLayout(LayoutKind.Sequential)> _
    Public Structure Point
        Public x As Int32
        Public y As Int32
        Public Sub New(ByVal x As Int32, ByVal y As Int32)
            Me.x = x
            Me.y = y
        End Sub

    End Structure

    <StructLayout(LayoutKind.Sequential)> _
    Public Structure Size
        Public cx As Int32
        Public cy As Int32
        Public Sub New(ByVal cx As Int32, ByVal cy As Int32)
            Me.cx = cx
            Me.cy = cy
        End Sub

    End Structure

    <StructLayout(LayoutKind.Sequential, Pack:=1)> _
    Structure ARGB

        Public Blue As Byte
        Public Green As Byte
        Public Red As Byte
        Public Alpha As Byte
    End Structure

    <StructLayout(LayoutKind.Sequential, Pack:=1)> _
    Public Structure BLENDFUNCTION
        Public BlendOp As Byte
        Public BlendFlags As Byte
        Public SourceConstantAlpha As Byte
        Public AlphaFormat As Byte
    End Structure

End Module
