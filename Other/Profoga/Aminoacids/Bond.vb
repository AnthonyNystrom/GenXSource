Imports System.Xml
<Serializable(), Serialization.XmlRoot("bond")> _
Public Class Bond
    <Serialization.XmlAttribute("atomRefs2")> _
    Public LinkAtom As Integer

    <Serialization.XmlIgnore()> _
    Public LinkMolecule As Integer

    <Serialization.XmlAttribute("order")> _
    Public Order As BondOrder

    Public SpecialAngle As BondAngle

    Public Sub New(ByVal aLink As Integer, ByVal anOrder As BondOrder)
        LinkAtom = aLink
        Order = anOrder
        LinkMolecule = 0
    End Sub

    Public Sub New()
        LinkAtom = 0
        LinkMolecule = 0
        Order = BondOrder.None
    End Sub

End Class

Public Enum BondOrder As Integer
    None = 0
    [Single] = 1
    [Double] = 2
    Triple = 3
    Peptide = 4
End Enum

Public Enum BondAngle As Integer
    None = 0
    Psi = 1
    Phi = 2
End Enum