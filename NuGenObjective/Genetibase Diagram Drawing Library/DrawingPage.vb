Option Strict Off
Option Explicit On

Imports Genetibase.NuGenObjective
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Collections.ObjectModel

' This code was adapted from code written by Gustavo Arranhado. 
' It has been so heavily changed that it can be considered
' new code.

Public Class DrawingPage
    Private WithEvents m_page As Page
    Private m_defaultFont As Font = SystemFonts.DialogFont

    'Private m_elements As New Dictionary(Of Rectangle, Element)
    Private m_elements As New Dictionary(Of Element, Rectangle)
    Private m_rolePoints As New Dictionary(Of String, Point)
    Private m_actionPoints As New Dictionary(Of String, Point)
    Private m_statePoints As New Dictionary(Of String, Point)
    Private m_objectPoints As New Dictionary(Of String, Point)

    Private Sub DrawElements(Of T As Element)( _
                    ByVal pageCollection As Collection(Of String), _
                    ByVal elementCollection As ElementCollection(Of T), _
                    ByVal selectedElement As Element, _
                    ByVal g As Graphics _
                )
        For Each key As String In pageCollection
            Dim element As Element = elementCollection(key)
            Dim rectangle As Rectangle = m_elements(element) 'GetRectangleForElement(element)
            Dim elementObject As Object = element
            Me.DrawElement(elementObject, g, rectangle, selectedElement Is element)
        Next
    End Sub

#Region " Drawing of each element type "
    Public Overloads Sub DrawElement( _
                                ByVal role As Role, _
                                ByVal g As Graphics, _
                                ByVal rectangle As Rectangle, _
                                ByVal selected As Boolean _
                            )
        DrawingHelper.DrawElement( _
                g, _
                m_defaultFont, _
                Color.Black, _
                Color.FromArgb(252, 255, 247), _
                Color.FromArgb(216, 251, 164), _
                Color.Black, _
                LinearGradientMode.ForwardDiagonal, _
                9, _
                selected, _
                role.Name, _
                rectangle _
            )
    End Sub

    Public Overloads Sub DrawElement( _
                                ByVal action As Action, _
                                ByVal g As Graphics, _
                                ByVal rectangle As Rectangle, _
                                ByVal selected As Boolean _
                                                        )

        DrawingHelper.DrawElement( _
                            g, _
                            m_defaultFont, _
                            Color.Black, _
                            Color.FromArgb(255, 248, 243), _
                            Color.FromArgb(255, 188, 133), _
                            Color.Black, _
                            LinearGradientMode.ForwardDiagonal, _
                            9, _
                            selected, _
                            action.Name, _
                            rectangle _
                        )
    End Sub

    Public Overloads Sub DrawElement( _
                            ByVal state As State, _
                            ByVal g As Graphics, _
                            ByVal rectangle As Rectangle, _
                            ByVal selected As Boolean _
                                                    )

        DrawingHelper.DrawElement( _
                            g, _
                            m_defaultFont, _
                            Color.Black, _
                            Color.FromArgb(250, 248, 253), _
                            Color.FromArgb(161, 194, 254), _
                            Color.Black, _
                            LinearGradientMode.ForwardDiagonal, _
                            7, _
                            selected, _
                            state.Name, _
                            rectangle _
                        )
    End Sub


    Public Overloads Sub DrawElement( _
                        ByVal modelObject As ModelObject, _
                        ByVal g As Graphics, _
                        ByVal rectangle As Rectangle, _
                        ByVal selected As Boolean _
                    )

        DrawingHelper.DrawElement( _
                            g, _
                            m_defaultFont, _
                            Color.Black, _
                            Color.FromArgb(250, 248, 253), _
                            Color.FromArgb(199, 179, 230), _
                            Color.Black, _
                            LinearGradientMode.ForwardDiagonal, _
                            5, _
                            selected, _
                            modelObject.Name, _
                            rectangle _
                        )

    End Sub

    Public Overloads Sub DrawElement( _
                                ByVal interaction As Interaction, _
                                ByVal g As Graphics, _
                                ByVal rectangle As Rectangle, _
                                ByVal selected As Boolean _
                            )

        DrawingHelper.DrawElement( _
                g, _
                m_defaultFont, _
                Color.Black, _
                Color.FromArgb(255, 247, 246), _
                Color.FromArgb(255, 161, 159), _
                Color.Black, _
                LinearGradientMode.ForwardDiagonal, _
                10, _
                selected, _
                "", _
                rectangle _
            )

    End Sub
#End Region

    Private Sub CalculateElements()
        Dim i As Integer
        Dim x As Integer
        Dim y As Integer

        m_elements.Clear()
        m_rolePoints.Clear()
        m_actionPoints.Clear()
        m_statePoints.Clear()
        m_objectPoints.Clear()

        ' Calculate role positions
        i = 0
        For Each roleKey As String In m_page.Roles
            Dim role As Role = m_page.System.Roles(roleKey)
            ' Each role plus the space below it
            ' is 64 (unscaled) pixels
            y = i * 64
            ' Each role will appear starting from 
            ' position 15, 192 (untransformed), and 
            ' be 96x48 (unscaled) pixels in size
            m_elements.Add( _
                    role, _
                    New Rectangle(15, 192 + y, 96, 48) _
            )
            ' The center/bottom point of the first role, therefore, will
            ' be 111,216 (untransformed)
            m_rolePoints.Add(roleKey, New Point(111, 216 + y))
            ' Increase vertical position counter
            i += 1
        Next

        ' Calculate action positions
        i = 0
        ' Actions are drawn at the right edge of the page
        ' The X position of the right edge changes based
        ' on the presence of Roles and States
        If m_page.Roles.Count > 0 Then
            ' If there is one or more
            ' roles on the page, the states
            ' start at x position 192
            x = 192
        ElseIf m_page.States.Count > 0 Then
            ' If the are no roles, but there
            ' are states, the first state
            ' appears at position 48
            x = 48
        Else
            ' If there are neither roles
            ' nor states in this page,
            ' drawing starts at position
            ' 15
            x = 15
        End If
        ' To this add 144 for each state
        ' present to reach the right edge
        ' of the page.
        x += 144 * m_page.States.Count
        ' Now calculate the position of the Actions
        For Each actionKey As String In m_page.Actions
            Dim action As Action = m_page.System.Actions(actionKey)
            ' Each role plus the space below it
            ' is 64 (unscaled) pixels
            y = i * 64
            ' Each action will appear starting from 
            ' position x, 192 (untransformed), and 
            ' be 96x48 (unscaled) pixels in size
            m_elements.Add( _
                    action, _
                    New Rectangle(x, 192 + y, 96, 48) _
            )
            ' The center/bottom point of the first role, therefore, will
            ' be x,216 (untransformed). 
            m_actionPoints.Add(actionKey, New Point(x, 216 + y))
            ' Increase vertical position counter
            i += 1
        Next

        ' Calculate State positions
        i = 0
        ' States are above and to the right of Roles
        ' If there are no roles, State drawing starts 
        ' at position 15,96. Otherwise,  at position 
        ' 160,96 (both untransformed).
        If m_page.Roles.Count > 0 Then
            x = 160
        Else
            x = 15
        End If
        ' Now calculate the state positions
        For Each stateKey As String In m_page.States
            Dim state As State = m_page.System.States(stateKey)
            ' Each state plus the space after it will
            ' be 144 pixels wide
            Dim xOffset As Integer = i * 144
            ' Each State will appear starting from 
            ' position x, 96 (untransformed), and 
            ' be 128x32 (unscaled) pixels in size.
            m_elements.Add( _
                state, _
                New Rectangle(x + xOffset, 96, 128, 32) _
            )
            ' The center/bottom point of each state, 
            ' therefore, will be x + xOffset + 64,96
            ' (untransformed). 
            m_statePoints.Add(stateKey, New Point(x + xOffset + 64, 96))
            ' Increase horizontal position counter
            i += 1
        Next

        ' Calculate ModelObject positions
        i = 0
        ' If there are Roles on the page, start
        ' calculating ModelObject positions at 
        ' position 160; otherwise start at position
        ' 15 (both untransformed).
        If m_page.Roles.Count > 0 Then
            x = 160
        Else
            x = 15
        End If
        ' ModelObjects appear above states, and are 
        ' "centered" among the states if applicable.
        If m_page.States.Count > 0 Then
            ' Simple centering calculation. Subtract half of the total
            ' width of all ModelObjects from half of the total width
            ' of all States.
            ' TODO: Some testing required here. Testing failed. Do something.
            x += (m_page.States.Count * 72) - (m_page.Objects.Count * 56)
        End If
        ' Now calculate the ModelObject positions
        For Each objectKey As String In m_page.Objects
            Dim modelObject As ModelObject = m_page.System.Objects(objectKey)
            ' Each ModelObject plus the space after it will
            ' be 112 pixels wide
            Dim xOffset As Integer = i * 112
            ' Each ModelObject will appear starting from 
            ' position x, 15 (untransformed), and 
            ' be 96x32 (unscaled) pixels in size.
            m_elements.Add( _
                modelObject, _
                New Rectangle(x + xOffset, 15, 96, 32) _
            )
            ' The center/bottom point of each state, 
            ' therefore, will be x + xOffset + 48,47
            ' (untransformed). 
            m_objectPoints.Add(objectKey, New Point(x + xOffset + 48, 47))
            ' Increase horizontal position counter
            i += 1
        Next

        ' Calculate Interaction positions
        For Each interactionKey As String In m_page.Interactions
            Dim interaction As Interaction = m_page.System.GetInteraction(interactionKey)
            ' An interaction is drawn at the intersection of two lines:
            ' The line joining its Role and Action, and the line drawn from
            ' its State to the bottom of the page
            Dim bottomPoint As New Point
            ' The bottom point for the state line has the same X
            ' co-ordinate as the State line. Its Y co-ordinate
            ' is the bottom of the page.
            With bottomPoint
                .X = m_statePoints(interaction.State.Key).X
                .Y = DrawingHelper.PageSize(m_page).Height
            End With
            Dim interactionPoint As Point = _
                    DrawingHelper.LineLineIntersection( _
                                m_rolePoints(interaction.Role.Key), _
                                m_actionPoints(interaction.Action.Key), _
                                m_statePoints(interaction.State.Key), _
                                bottomPoint _
                                )
            ' The interaction itself is a 20x20 rectangle (which gets
            ' drawn as a circle in the default implementation).
            m_elements.Add( _
                    interaction, _
                    New Rectangle( _
                            interactionPoint.X - 10, _
                            interactionPoint.Y - 10, _
                            20, _
                            20 _
                    ) _
            )
        Next
    End Sub

    Public Property DefaultFont() As Font
        Get
            Return m_defaultFont
        End Get
        Set(ByVal value As Font)
            m_defaultFont = value
        End Set
    End Property

    Public Sub Draw( _
                            ByVal selectedElement As Element, _
                            ByVal g As Graphics, _
                            ByVal originOffsetX As Single, _
                            ByVal originOffsetY As Single, _
                            ByVal scaleFactor As Single, _
                            ByVal diagramDivisionsPen As Pen, _
                            ByVal stateLinePen As Pen, _
                            ByVal stateSelectedLinePen As Pen, _
                            ByVal objectLinePen As Pen, _
                            ByVal objectSelectedLinePen As Pen _
                        )
        Dim gc As GraphicsContainer

        With g
            .TranslateTransform(originOffsetX, originOffsetY)
            .ScaleTransform(scaleFactor, scaleFactor)

            gc = .BeginContainer
            .TextRenderingHint = Text.TextRenderingHint.ClearTypeGridFit
            .SmoothingMode = SmoothingMode.HighQuality
            .InterpolationMode = InterpolationMode.HighQualityBilinear
            .CompositingQuality = CompositingQuality.HighQuality
        End With

        ' If there are any shapes on the page, we need
        ' to draw the diagram division lines
        If m_page.Roles.Count > 0 _
                OrElse _
            m_page.Actions.Count > 0 _
                OrElse _
            m_page.States.Count > 0 _
            Then

            Dim leftEdge, rightEdge As Integer
            leftEdge = 15
            If m_page.Roles.Count > 0 Then
                ' If there are any roles, right edge
                ' calculation starts at position 192
                rightEdge = 192
            ElseIf m_page.States.Count > 0 Then
                ' If there are no roles, but there are
                ' states, right edge calculations begin
                ' at position 48
                rightEdge = 48
            End If
            ' Each state plus the space after it is 144
            ' pixels wide.
            rightEdge += (144 * m_page.States.Count)
            If m_page.Actions.Count > 0 Then
                ' If there are any Actions, the right edge
                ' should be adjusted by a further 96 pixels
                ' (for the width of each action)
                rightEdge += 96 '+ 15
            End If
            ' Draw a dividing line between States
            ' and  rest of diagram
            g.DrawLine( _
                    diagramDivisionsPen, _
                    leftEdge, _
                    160, _
                    rightEdge, _
                    160 _
            )



            ' " Draw vertical diagram division line "
            '    If m_page.Actions.Count > 0 Then
            '        Dim x As Integer

            '        If m_page.Roles.Count > 0 Then
            '            x = 192
            '        ElseIf m_page.States.Count > 0 Then
            '            x = 48
            '        Else
            '            x = 15
            '        End If
            '        x += 144 * m_page.States.Count + 128

            '        e.Graphics.DrawLine( _
            '            diagramDivisionsPen, _
            '            x, _
            '            192, _
            '            x, _
            '            176 + m_page.Actions.Count * 64 _
            '        )

            '        m_bottomRight.X += (x * m_scaleFactor)
            '    Else
            '        m_bottomRight.X += (dw * m_scaleFactor)
            '    End If

        End If

        ' Draw roles
        DrawElements(m_page.Roles, m_page.System.Roles, selectedElement, g)

        ' Draw Actions
        DrawElements(m_page.Actions, m_page.System.Actions, selectedElement, g)


        ' Draw ModelObjects
        DrawElements(m_page.Objects, m_page.System.Objects, selectedElement, g)

        ' Draw connection lines
        Dim lineDrawPen As Pen

        ' If there are any Roles or Actions
        ' Draw state lines
        If m_page.Roles.Count > 0 OrElse m_page.Actions.Count > 0 Then
            Dim bottomYCoordinate As Integer = DrawingHelper.PageSize(m_page).Height
            For Each stateKey As String In m_page.States

                If selectedElement IsNot Nothing _
                        AndAlso _
                    selectedElement.Key = stateKey Then
                    lineDrawPen = stateSelectedLinePen
                Else
                    lineDrawPen = stateLinePen
                End If

                ' Draw a line from the middle, bottom of each state
                ' to the bottom of the diagram
                With m_statePoints(stateKey)
                    g.DrawLine( _
                        lineDrawPen, _
                        .X, _
                        .Y, _
                        .X, _
                        bottomYCoordinate _
                    )
                End With
            Next
        End If

        ' Draw States
        DrawElements(m_page.States, m_page.System.States, selectedElement, g)

        ' Draw interactions and associated lines
        For Each interactionKey As String In m_page.Interactions
            Dim interaction As Interaction = m_page.System.GetInteraction(interactionKey)
            Dim rectangle As Rectangle = m_elements(interaction)

            With interaction
                ' Draw object/state connector
                Dim startPoint As Point = m_objectPoints(.Object.Key)
                Dim endPoint As Point = m_statePoints(.State.Key)

                If selectedElement Is .Object Then
                    lineDrawPen = objectSelectedLinePen
                Else
                    lineDrawPen = objectLinePen
                End If

                g.DrawBezier( _
                    lineDrawPen, _
                    startPoint, _
                    New Point(startPoint.X, startPoint.Y + 32), _
                    New Point(endPoint.X, endPoint.Y - 32), _
                    endPoint _
                )

                ' Draw role/action line
                startPoint = m_rolePoints(.Role.Key)
                endPoint = m_actionPoints(.Action.Key)
                If selectedElement Is interaction _
                        OrElse _
                    selectedElement Is .Role _
                        OrElse _
                    selectedElement Is .Action _
                        Then
                    lineDrawPen = objectSelectedLinePen
                Else
                    lineDrawPen = objectLinePen
                End If

                g.DrawLine( _
                    lineDrawPen, _
                    startPoint, _
                    endPoint _
                )
            End With

            Dim interactionObject As Object = interaction
            DrawElement(interactionObject, g, rectangle, selectedElement Is interaction)
        Next

        g.EndContainer(gc)
    End Sub

    Private Sub m_page_PageElementsChanged(ByVal sender As Object, ByVal e As PageElementsChangedEventArgs) Handles m_page.PageElementsChanged
        ' Do not recalculate when an interaction has been renamed
        If e.ChangeType = PageElementsChangedEventArgs.TypeOfChange.NameChanged _
                AndAlso _
            TypeOf e.GetChangedElements(0) Is Interaction Then
            Exit Sub
        End If
        CalculateElements()
    End Sub

    Public Function GetElementAtPosition( _
                        ByVal x As Integer, _
                        ByVal y As Integer, _
                        ByVal originOffsetX As Single, _
                        ByVal originOffsetY As Single, _
                        ByVal scalefactor As Single _
                        ) As Element

        Dim result As Element = m_page
        For Each elementAndRect As KeyValuePair(Of Element, Rectangle) In m_elements
            Dim elementRect As Rectangle = elementAndRect.Value
            Dim adjustedMouseRectangle As _
                New Rectangle( _
                    CInt(x - originOffsetX), _
                    CInt(Y - originOffsetY), _
                    1, _
                    1 _
                )

            Dim adjustedElementRectangle As _
                New Rectangle( _
                    CInt(CSng(elementRect.X) * scalefactor), _
                    CInt(CSng(elementRect.Y) * scalefactor), _
                    CInt(CSng(elementRect.Width * scalefactor)), _
                    CInt(CSng(elementRect.Height * scalefactor)) _
                )

            If adjustedMouseRectangle.IntersectsWith(adjustedElementRectangle) Then
                result = elementAndRect.Key
                Exit For
            End If
        Next
        Return result
    End Function

    Public Function GetRectangleForElement(ByVal element As Element) As Rectangle
        Return m_elements(element)
    End Function

    Public Sub New(ByVal page As Page)
        m_page = page
        CalculateElements()
    End Sub
End Class
