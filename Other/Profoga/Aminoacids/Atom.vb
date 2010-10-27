Imports System.Xml

<Serializable(), Xml.Serialization.XmlRoot("atom")> _
Public Class Atom
    Implements Serialization.IXmlSerializable
    '<Serialization.XmlArrayItem("bond"), Serialization.XmlArray("bondArray")> _

    <Serialization.XmlIgnore()> _
    Public Bonds As List(Of Bond) 'Dictionary(Of Integer, BondType)

    <Serialization.XmlAttribute("id")> _
    Public Id As Integer

    Public Element As String
    Public X As Double
    Public Y As Double
    Public Z As Double

    Public Charge As SByte

    Public MoleculeId As Integer

    Public SpecialRole As AtomSpecialRole

    Public ReadOnly Property Name() As String
        Get
            Return AtomName(Me.MoleculeId, Me.Id)
        End Get
    End Property
    Public Sub New()
        SpecialRole = AtomSpecialRole.None
        Me.Bonds = New List(Of Bond)
    End Sub

    Public Shared Function DistanceSquared(ByVal atom1 As Atom, ByVal atom2 As Atom) As Double
        Dim dx, dy, dz As Double
        dx = atom1.X - atom2.X
        dy = atom1.Y - atom2.Y
        dz = atom1.Z - atom2.Z
        Return dx ^ 2 + dy ^ 2 + dz ^ 2
    End Function

    Public Shared Function Distance(ByVal atom1 As Atom, ByVal atom2 As Atom) As Double
        Return Math.Sqrt(DistanceSquared(atom1, atom2))
    End Function

    Public Function GetSchema() As System.Xml.Schema.XmlSchema Implements System.Xml.Serialization.IXmlSerializable.GetSchema

    End Function

    Public Sub ReadXml(ByVal reader As System.Xml.XmlReader) Implements System.Xml.Serialization.IXmlSerializable.ReadXml
        Dim tmpId, tmpCharge, tmpRole As String
        tmpId = reader.GetAttribute("id")
        tmpCharge = reader.GetAttribute("formalCharge")
        tmpRole = reader.GetAttribute("role")

        Me.MoleculeId = Asc(tmpId.Substring(0, 1)) - 96
        Me.Id = Integer.Parse(tmpId.Substring(1))
        Me.Element = reader.GetAttribute("elementType")
        Me.X = Double.Parse(reader.GetAttribute("x3"), Globalization.CultureInfo.InvariantCulture)
        Me.Y = Double.Parse(reader.GetAttribute("y3"), Globalization.CultureInfo.InvariantCulture)
        Me.Z = Double.Parse(reader.GetAttribute("z3"), Globalization.CultureInfo.InvariantCulture)

        If tmpCharge IsNot Nothing Then Me.Charge = SByte.Parse(tmpCharge)
        If tmpRole IsNot Nothing Then
            tmpRole = tmpRole.Substring(0, 1).ToUpper & tmpRole.Substring(1)
            Me.SpecialRole = System.Enum.Parse(GetType(AtomSpecialRole), tmpRole)
        End If
    End Sub

    Public Sub WriteXml(ByVal writer As System.Xml.XmlWriter) Implements System.Xml.Serialization.IXmlSerializable.WriteXml
        'writer.WriteAttributeString("id", Me.Id)
        writer.WriteAttributeString("id", Me.Name)
        writer.WriteAttributeString("elementType", Me.Element)
        writer.WriteAttributeString("x3", Me.X.ToString(Globalization.CultureInfo.InvariantCulture))
        writer.WriteAttributeString("y3", Me.Y.ToString(Globalization.CultureInfo.InvariantCulture))
        writer.WriteAttributeString("z3", Me.Z.ToString(Globalization.CultureInfo.InvariantCulture))

        If Me.Charge <> 0 Then
            writer.WriteAttributeString("formalCharge", Me.Charge)
        End If
        If SpecialRole <> AtomSpecialRole.None Then
            writer.WriteAttributeString("role", Me.SpecialRole.ToString.Substring(0, 1).ToLower & Me.SpecialRole.ToString.Substring(1))
        End If
    End Sub

    Public Shared Function AtomName(ByVal moleculeid As Integer, ByVal atomId As Integer) As String
        Dim sb As New Text.StringBuilder
        Const values = "abcdefghijklmnpq"
        Const hex = "0123456789abcdef"

        Dim tempMol As String = (moleculeid - 1).ToString("x")
        For i As Integer = 0 To tempMol.Length - 1
            If i = 0 Then
                sb.Append(values.Substring(hex.IndexOf(tempMol.Substring(i, 1)) - 1, 1))
            Else
                sb.Append(values.Substring(hex.IndexOf(tempMol.Substring(i, 1)), 1))
            End If
        Next
        sb.Append(atomId)
        Return sb.ToString
    End Function
End Class

Public Enum AtomSpecialRole
    None
    SkeletonC
    SkeletonH
    PeptideN
    PeptideC
    PeptideO
    PeptideH
    DehydrateNitrogenH
    DehydrateCarbonO
    DehydrateCarbonH
End Enum