Option Strict On
Option Explicit On

Imports Genetibase.NuGenObjective.Windows.DiagramClient

<Serializable()> _
Friend Class AddinStatus
    Implements IEquatable(Of AddinStatus), IComparable(Of AddinStatus)

    Private m_name As String
    Private m_className As String
    Private m_assemblyName As String
    Private m_active As Boolean

    Public Property Name() As String
        Get
            Return m_name
        End Get
        Set(ByVal value As String)
            m_name = value
        End Set
    End Property

    Friend Property ClassName() As String
        Get
            Return m_className
        End Get
        Set(ByVal value As String)
            m_className = value
        End Set
    End Property

    Friend Property AssemblyName() As String
        Get
            Return m_assemblyName
        End Get
        Set(ByVal value As String)
            m_assemblyName = value
        End Set
    End Property

    Public Property Active() As Boolean
        Get
            Return m_active
        End Get
        Set(ByVal value As Boolean)
            m_active = value
        End Set
    End Property

    Public Sub New( _
                                    ByVal newName As String, _
                                    ByVal newClassName As String, _
                                    ByVal newAssemblyName As String, _
                                    ByVal newActive As Boolean _
                                        )
        Name = newName
        ClassName = newClassName
        AssemblyName = newAssemblyName
        Active = newActive
    End Sub

    Public Function CompareTo(ByVal other As AddinStatus) As Integer Implements System.IComparable(Of AddinStatus).CompareTo
        Return String.Compare(AssemblyName & ClassName, other.AssemblyName & other.ClassName, True)
    End Function

    Public Function EqualsToAddinStatus(ByVal other As AddinStatus) As Boolean Implements System.IEquatable(Of AddinStatus).Equals
        Return (ClassName = other.ClassName And AssemblyName = other.AssemblyName)
    End Function
End Class
