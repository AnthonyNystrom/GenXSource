Public Class Timer
    'Performance Timer--------------------------------------------
    Private Declare Function QueryPerformanceCounter Lib "Kernel32" (ByRef X As Int64) As Boolean
    Private Declare Function QueryPerformanceFrequency Lib "Kernel32" (ByRef X As Int64) As Boolean

    Private mTimerStart As Int64
    Private mTimerFreq As Int64

    Public Sub New()
        'Start Performance Timer
        If QueryPerformanceCounter(mTimerStart) Then
            QueryPerformanceFrequency(mTimerFreq)
        End If
    End Sub

    Public Function ms() As Int64
        Dim TimerEnd As Int64
        If QueryPerformanceCounter(TimerEnd) Then
            Return CInt((TimerEnd - mTimerStart) / mTimerFreq * 1000)
        Else
            Return 0
        End If
    End Function

End Class
