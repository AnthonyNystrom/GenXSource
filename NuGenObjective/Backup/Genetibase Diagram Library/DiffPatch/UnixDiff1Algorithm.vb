Option Strict On
Option Explicit On

Imports System.Collections.ObjectModel

Namespace DiffPatch
    Public Class UnixDiff1Algorithm(Of T As Element)
        Inherits DiffAlgorithm(Of T)

        Private m_firstList As Collection(Of T)
        Private m_secondList As Collection(Of T)
        Private m_includeChangedData As Boolean
        Private m_buffer() As Integer

        Private Structure MiddleSnake
            Public x, y, u, v As Integer
        End Structure

        Private Sub _setv(ByVal k As Integer, ByVal r As Integer, ByVal val As Integer)
            ' Pack -N to N into 0 to N * 2
            Dim j As Integer
            If k <= 0 Then
                j = -k * 4 + r
            Else
                j = k * 4 + (r - 2)
            End If
            If (j >= m_buffer.Length) Then
                ReDim Preserve m_buffer(m_buffer.Length + (j - m_buffer.Length) + 100)
                'Dim b(m_buffer.Length * 2) As Integer
                'Array.Copy(m_buffer, b, m_buffer.Length)
                '' TODO: Check this m_buffer = b
            End If
            m_buffer(j) = val
        End Sub

        Private Function _v(ByVal k As Integer, ByVal r As Integer) As Integer
            Dim j As Integer
            If k <= 0 Then
                j = -k * 4 + r
            Else
                j = k * 4 + (r - 2)
            End If
            If (j >= m_buffer.Length) Then
                ReDim Preserve m_buffer(m_buffer.Length + (j - m_buffer.Length) + 100)
                'Dim b(m_buffer.Length * 2) As Integer
                'Array.Copy(m_buffer, b, m_buffer.Length)
                '' TODO: Check this m_buffer = b
            End If
            'If (j <= m_buffer.Length) Then
            '    Dim b(m_buffer.Length * 2) As Integer
            '    Array.Copy(m_buffer, b, m_buffer.Length)
            'End If
            Return m_buffer(j)
        End Function

        Private Function FindMiddleSnake( _
            ByVal firstListOffset As Integer, _
            ByVal firstListLength As Integer, _
            ByVal secondListOffset As Integer, _
            ByVal secondListLength As Integer, _
            ByRef ms As MiddleSnake _
            ) As Integer

            Dim delta, odd, mid, d As Integer

            ms = New MiddleSnake()

            delta = firstListLength - secondListLength
            odd = delta And 1
            mid = ((firstListLength + secondListLength) \ 2)
            mid += odd

            _setv(1, 0, 0)
            _setv(delta - 1, 1, firstListLength)

            For d = 0 To mid
                Dim k, x, y As Integer

                For k = d To -d Step -2
                    If k = -d _
                        OrElse _
                        (k <> d AndAlso _v(k - 1, 0) < _v(k + 1, 0)) _
                        Then
                        x = _v(k + 1, 0)
                    Else
                        x = _v(k - 1, 0) + 1
                    End If
                    y = x - k

                    ms.x = x
                    ms.y = y

                    Do While _
                            x < firstListLength _
                                AndAlso _
                            y < secondListLength _
                                AndAlso _
                            m_firstList(firstListOffset + x).EqualsTo(m_secondList(secondListOffset + y)) '_comparer(_a[aoff + x], _b[boff + y]))
                        x += 1
                        y += 1
                    Loop

                    _setv(k, 0, x)

                    If odd <> 0 _
                        AndAlso _
                        k >= (delta - (d - 1)) _
                        AndAlso _
                        k <= (delta + (d - 1)) Then

                        If x >= _v(k, 1) Then

                            ms.u = x
                            ms.v = y
                            Return 2 * d - 1
                        End If
                    End If
                Next
                For k = d To -d Step -2

                    Dim kr As Integer = (firstListLength - secondListLength) + k

                    If k = d _
                        OrElse _
                        ( _
                            k <> -d _
                                AndAlso _
                            _v(kr - 1, 1) < _v(kr + 1, 1) _
                        ) _
                    Then
                        x = _v(kr - 1, 1)
                    Else
                        x = _v(kr + 1, 1) - 1
                    End If
                    y = x - kr

                    ms.u = x
                    ms.v = y
                    Do While _
                        x > 0 _
                            AndAlso _
                        y > 0 _
                            AndAlso _
                        m_firstList(firstListOffset + (x - 1)).EqualsTo(m_secondList(secondListOffset + (y - 1))) '*/ _comparer(_a[aoff + (x - 1)],_b[boff + (y - 1)]))

                        x -= 1
                        y -= 1
                    Loop
                    _setv(kr, 1, x)

                    If (odd = 0 AndAlso kr >= -d AndAlso kr <= d) Then

                        If (x <= _v(kr, 0)) Then
                            ms.x = x
                            ms.y = y
                            Return 2 * d
                        End If
                    End If
                Next
            Next

            Return -1
        End Function

        Private Sub AddDifference( _
                        ByVal operation As DiffType, _
                        ByVal offset As Integer, _
                        ByVal length As Integer _
            )
            If length = 0 Then
                Return
            End If

            ' Add an edit to the SES (or
            ' coalesce if the op is the same)
            Dim e As DiffEdit(Of T)

            If Differences.Count = 0 Then
                e = Nothing
            Else
                e = Differences(Differences.Count - 1)
            End If

            If e Is Nothing OrElse e.DiffOperation <> operation Then
                e = New DiffEdit(Of T)
                e.DiffOperation = operation
                e.StartOffset = offset
                e.ChangeLength = length
                'e.ChangedElements = New Collection(Of T)
                Differences.Add(e)
            Else
                e.ChangeLength += length
            End If

            For x As Integer = 0 To length - 1
                Select Case operation
                    Case DiffType.Delete
                        If m_includeChangedData Then
                            e.ChangedElements.Add(m_firstList(offset + x))
                        End If
                    Case DiffType.Insert
                        e.ChangedElements.Add(m_secondList(offset + x))
                    Case DiffType.Match
                        If m_includeChangedData Then
                            e.ChangedElements.Add(m_firstList(offset + x))
                        End If
                End Select
            Next
        End Sub

        Private Function FindDifferences( _
                ByVal firstListOffset As Integer, _
                ByVal firstListLength As Integer, _
                ByVal secondListStartOffset As Integer, _
                ByVal secondListLength As Integer _
            ) As Integer

            Dim editDistance As Integer

            If firstListLength = 0 Then
                AddDifference(DiffType.Insert, secondListStartOffset, secondListLength)
                editDistance = secondListLength

            ElseIf secondListLength = 0 Then
                AddDifference(DiffType.Delete, firstListOffset, firstListLength)
                editDistance = firstListLength
            Else
                Dim ms As MiddleSnake
                ' find the middle "snake" around which we
                ' recursively solve the sub-problems.
                editDistance = FindMiddleSnake(firstListOffset, firstListLength, secondListStartOffset, secondListLength, ms)
                If editDistance = -1 Then
                    Return -1
                ElseIf editDistance > 1 Then
                    If FindDifferences(firstListOffset, ms.x, secondListStartOffset, ms.y) = -1 Then
                        Return -1
                    End If

                    AddDifference(DiffType.Match, firstListOffset + ms.x, ms.u - ms.x)

                    firstListOffset += ms.u
                    secondListStartOffset += ms.v
                    firstListLength -= ms.u
                    secondListLength -= ms.v
                    If FindDifferences(firstListOffset, firstListLength, secondListStartOffset, secondListLength) = -1 Then
                        Return -1
                    End If
                Else

                    Dim x As Integer = ms.x
                    Dim u As Integer = ms.u

                    '/* There are only 4 base cases when the
                    ' * edit distance is 1.
                    ' *
                    ' * n > m   m > n
                    ' *
                    ' *   -       |
                    ' *    \       \    x != u
                    ' *     \       \
                    ' *
                    ' *   \       \
                    ' *    \       \    x == u
                    ' *     -       |
                    ' */

                    If (secondListLength > firstListLength) Then

                        If (x = u) Then
                            AddDifference(DiffType.Match, firstListOffset, firstListLength)
                            AddDifference(DiffType.Insert, secondListStartOffset + (secondListLength - 1), 1)
                        Else

                            AddDifference(DiffType.Insert, secondListStartOffset, 1)
                            AddDifference(DiffType.Match, firstListOffset, firstListLength)
                        End If
                    Else

                        If x = u Then
                            AddDifference(DiffType.Match, firstListOffset, secondListLength)
                            AddDifference(DiffType.Delete, firstListOffset + (firstListLength - 1), 1)

                        Else

                            AddDifference(DiffType.Delete, firstListOffset, 1)
                            AddDifference(DiffType.Match, firstListOffset + 1, secondListLength)
                        End If
                    End If
                End If
            End If

            Return editDistance
        End Function

        Private Function _diff() As Integer
            Dim x, y As Integer

            ' the _ses function assumes the SES will begin or end with a delete
            ' or insert. The following will insure this is true by eating any
            ' beginning matches. This is also a quick to process sequences
            ' that match entirely.
            x = 0
            y = 0

            Do While _
                    x < m_firstList.Count _
                        AndAlso _
                    y < m_secondList.Count _
                        AndAlso _
                    m_firstList(x).EqualsTo(m_secondList(y))

                x += 1
                y += 1
            Loop
            If m_includeChangedData Then
                AddDifference(DiffType.Match, 0, x)
            End If

            Return FindDifferences(x, m_firstList.Count - x, y, m_secondList.Count - y)
        End Function

        Public Sub New( _
                    ByVal firstList As Collection(Of T), _
                    ByVal secondList As Collection(Of T), _
                    ByVal changes As Boolean _
                    )

            m_firstList = firstList
            m_secondList = secondList
            m_includeChangedData = changes
            ReDim m_buffer(100)
            _diff()
            Erase m_buffer
        End Sub
    End Class
End Namespace
