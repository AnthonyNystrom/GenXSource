Option Strict On
Option Explicit On

Imports Genetibase.NuGenObjective
Imports System.Globalization

''' <summary>
''' The ElementFactoryBase class is used to create instances of the "native"
''' element types at the time of opening diagrams from files, and 
''' during cut/copy/paste operations. Developers deriving custom element
''' types must also derive their own factory from this class, and override
''' the CreateElement method.
''' </summary>
''' <remarks></remarks>
Public Class ElementFactoryBase
    ''' <summary>
    ''' The CreateElement method takes a type name and an Xml namespace,
    ''' and returns an element-derived object. Developers that derive
    ''' a custom Element Factory should override this method, and call
    ''' the base method as appropriate.
    ''' </summary>
    ''' <param name="typeName">
    ''' The type name, without namespaces, of the element class.
    ''' </param>
    ''' <param name="nameSpace">
    ''' The XML namespace representing the element type.
    ''' </param>
    ''' <returns>
    ''' An Element object, containing the relevant element.
    ''' </returns>
    ''' <remarks></remarks>
    Public Overridable Function CreateElement(ByVal typeName As String, ByVal [nameSpace] As String) As Element
        Dim result As Element

        typeName = typeName.ToLowerInvariant
        If [nameSpace] <> Diagram.RootNameSpace Then
            Throw New ArgumentException( _
                        String.Format(CultureInfo.InvariantCulture, "Namespace '{0}' not recognized.", [nameSpace]) _
                )
        Else
            Select Case typeName
                Case "modelobject"
                    result = New ModelObject
                Case "state"
                    result = New State
                Case "role"
                    result = New Role
                Case "action"
                    result = New Action
                Case "interaction"
                    result = New Interaction(Nothing, Nothing, Nothing, Nothing, Nothing)
                Case Else
                    Throw New ArgumentException( _
                                String.Format( _
                                    CultureInfo.InvariantCulture, _
                                    "Type '{0}' in Namespace '{1}' not recognized.", _
                                    typeName, _
                                    [nameSpace]) _
                                )
            End Select

        End If
        Return result
    End Function
End Class
