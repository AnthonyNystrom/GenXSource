Public Interface iSkinable
   Property SkinClass() As String
   Sub SetStyle(ByVal flag As System.Windows.Forms.ControlStyles, ByVal value As Boolean)
   Sub BasePaint(ByVal e As System.Windows.Forms.PaintEventArgs)
   Sub BasePaintBackground(ByVal e As System.Windows.Forms.PaintEventArgs)
   Sub Refresh()

   Public Interface iHasIcon
      Property Icon() As String
   End Interface
End Interface
