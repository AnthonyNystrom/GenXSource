Module UnsafeCode
    Public Const PHYSICALOFFSETX As Integer = 112 ' Physical Printable Area x margin
    Public Const PHYSICALOFFSETY As Integer = 113 ' Physical Printable Area y margin

    Public Declare Auto Function GetDeviceCaps Lib "gdi32" (ByVal hdc As IntPtr, ByVal nIndex As Integer) As Integer
End Module
