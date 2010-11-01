Option Strict On
Option Explicit On

Imports System.ComponentModel
Imports System.Xml

''' <summary>
''' The Element class is the MustInherit (abstract) base class of all diagram 
''' elements, such as Action, Role and State.
''' </summary>
''' <remarks>
''' All Element-derived objects must belong to a ModelSystem. The Friend 
''' (internal) SetSystem method provides this functionality, and is called by
''' the ElementCollection class when an Element is added to it.
''' New element authors should not derive directly from Element. Instead, they
''' should derive from one of the predefined Element classes such as Action,
''' Role or State.
''' </remarks>
Public MustInherit Class Element

    Private m_name As String
    Private m_system As ModelSystem
    Private m_revision As Integer = -1
    Private m_description As String = ""

    Public Event Renamed As EventHandler(Of ElementRenamedEventArgs)
    Public Event Changed As EventHandler
    Friend Event Deleted As EventHandler

    ''' <summary>
    ''' Every Element must have a Name. This name needs to be unique among
    ''' elements of the same type in a ModelSystem.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>
    ''' Changing the name causes a Renamed event to be triggered.
    ''' </remarks>
    <Browsable(True), _
    ParenthesizePropertyName(True)> _
    Public Property Name() As String ' , [ReadOnly](True)> _
        Get
            Return m_name
        End Get
        Set(ByVal value As String)
            If value <> m_name Then
                OnRenaming(value)
                Dim oldKey As String = Key
                m_name = value
                OnRename(oldKey)
            End If
        End Set
    End Property

    ''' <summary>
    ''' Every Element must have a Key. The Key should ideally be a 
    ''' function of the name, but can be anything.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Browsable(False)> _
    Public MustOverride ReadOnly Property Key() As String

    ''' <summary>
    ''' Every Element must belong to a ModelSystem.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Browsable(False)> _
    Public ReadOnly Property System() As ModelSystem
        Get
            Return m_system
        End Get
    End Property

    Friend Sub SetSystem(ByVal system As ModelSystem)
        m_system = system
        OnSetSystem(system)
    End Sub

    ''' <summary>
    ''' The Description property can be used to describe any element
    ''' in detail.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <Browsable(True), Editor(GetType(System.ComponentModel.Design.MultilineStringEditor), GetType(System.Drawing.Design.UITypeEditor))> _
    Public Property Description() As String
        Get
            Return m_description
        End Get
        Set(ByVal value As String)
            m_description = value
        End Set
    End Property

    ''' <summary>
    ''' The NameSpace property provides an XML namespace that is
    ''' used to save and retrieve elements of a particular type.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>All NugenObjective "native" elements use 
    ''' the NuGenObjective default namespace. Authors of 
    ''' derived classes are strongly encouraged to provide
    ''' their own namespace.</remarks>
    Public Overridable ReadOnly Property [NameSpace]() As String
        Get
            Return Diagram.RootNameSpace
        End Get
    End Property

    ''' <summary>
    ''' The OnDeleted method will be called by the holding collection
    ''' on an element that has just been deleted. It will simply
    ''' raise the Deleted event.
    ''' </summary>
    ''' <remarks></remarks>
    Friend Sub OnDeleted()
        RaiseEvent Deleted(Me, EventArgs.Empty)
    End Sub

    ''' <summary>
    ''' The Revision property specifies the revision number of an 
    ''' element in a collaborative editing scenario, on a server
    ''' hosted diagram.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>The default value is -1. This is the value
    ''' that all local diagrams retain for all elements.
    ''' </remarks>
    <Browsable(True)> _
    Public ReadOnly Property Revision() As Integer
        Get
            Return m_revision
        End Get
    End Property

    Friend Sub SetRevision(ByVal revision As Integer)
        m_revision = revision
    End Sub

    ''' <summary>
    ''' The OnSetSystem method is called when an Element-derived class
    ''' is added to an ElementCollection(Of T)
    ''' </summary>
    ''' <param name="system"></param>
    ''' <remarks></remarks>
    Protected Overridable Sub OnSetSystem(ByVal system As ModelSystem)

    End Sub

    ''' <summary>
    ''' The OnRenaming method is called just before a name change.
    ''' </summary>
    ''' <param name="newName"></param>
    ''' <remarks>
    ''' This method does not raise any event, and cannot be cancelled.
    ''' It should not be used for validations.
    ''' </remarks>
    Protected Overridable Sub OnRenaming(ByVal newName As String)

    End Sub

    Protected Overridable Sub OnRename(ByVal oldKey As String)
        Dim e As New ElementRenamedEventArgs(oldKey)
        RaiseEvent Renamed(Me, e)
    End Sub

    ''' <summary>
    ''' Derived classes should call this method to indicate that the
    ''' element has changed in some way.
    ''' </summary>
    ''' <remarks>Do not call this to indicate a change in the Name
    ''' property of the element. That is automatically taken care 
    ''' of.</remarks>
    Protected Sub IndicateChange()
        RaiseEvent Changed(Me, EventArgs.Empty)
    End Sub

    Friend Sub WriteXML(ByVal writer As Xml.XmlWriter)
        With writer
            '.WriteComment(Me.GetType().FullName)
            .WriteElementString("Key", Diagram.RootNameSpace, Key)
            .WriteElementString("Name", Diagram.RootNameSpace, Name)
            If m_revision <> -1 Then
                .WriteElementString("Revision", Diagram.RootNameSpace, m_revision.ToString(Globalization.CultureInfo.InvariantCulture))
            End If
            If m_description.Length <> 0 Then
                .WriteStartElement("Description", Diagram.RootNameSpace)
                .WriteCData(m_description)
                .WriteEndElement()
            End If
        End With
        OnSave(writer)
    End Sub

    ''' <summary>
    ''' The OnSave method must be overridded by any Element-derived class
    ''' to save any properties it needs to save. The Key and Name properties
    ''' are automatically saved by the Element base class.
    ''' </summary>
    ''' <param name="writer">
    ''' An XmlWriter that represents the data store.
    ''' </param>
    ''' <remarks>
    ''' By the time the OnSave method is invoked, the Key and Name properties
    ''' have already been written; so they should not be written again.
    ''' Authors of derived classes are strongly encouraged to use XML
    ''' namespaces while saving their properties.
    ''' </remarks>
    Protected MustOverride Sub OnSave(ByVal writer As XmlWriter)

    Friend Sub ReadXML(ByVal reader As XmlReader)
        With reader
            .ReadElementContentAsString("Key", Diagram.RootNameSpace)
            m_name = .ReadElementContentAsString("Name", Diagram.RootNameSpace)
            If .LocalName = "Revision" AndAlso .NamespaceURI = Diagram.RootNameSpace Then
                m_revision = .ReadElementContentAsInt("Revision", Diagram.RootNameSpace)
            End If
            If .LocalName = "Description" AndAlso .NamespaceURI = Diagram.RootNameSpace Then
                ' Jump over the Desc
                .Read()
                m_description = .ReadString()
                .Read()
            End If
        End With
        OnOpen(reader)
    End Sub

    ''' <summary>
    ''' The OnOpen method must be overridded by any Element-derived class
    ''' to load any properties it needs to load. The Key and Name properties
    ''' are automatically loaded by the Element base class.
    ''' </summary>
    ''' <param name="reader">
    ''' An XmlReader that represents the data store.
    ''' </param>
    ''' <remarks>
    ''' By the time the OnOpen method is invoked, the Key and Name properties
    ''' have already been read; so they should not be read again.
    ''' Authors of derived classes are strongly encouraged to use XML
    ''' namespaces while reading their properties.
    ''' </remarks>
    Protected MustOverride Sub OnOpen(ByVal reader As XmlReader)

    ''' <summary>
    ''' The EqualsTo function must be overridden by any Element-derived class
    ''' to indicate whether two instances are equivalent.
    ''' </summary>
    ''' <param name="other">
    ''' The Element instance against which the current instance
    ''' is to be compared.
    ''' </param>
    ''' <returns>True if the two instances are equivalent, false otherwise.</returns>
    ''' <remarks>
    ''' The default implementation compares the keys of the two elements.
    ''' If they are the same, the elements are equal.
    ''' </remarks>
    Public Overridable Function EqualsTo(ByVal other As Element) As Boolean
        If Not other Is Nothing Then
            Return Key = other.Key
        Else
            Return False
        End If
    End Function

    Protected Sub New(ByVal system As ModelSystem)
        m_system = system
    End Sub

    Protected Sub New()

    End Sub

End Class
