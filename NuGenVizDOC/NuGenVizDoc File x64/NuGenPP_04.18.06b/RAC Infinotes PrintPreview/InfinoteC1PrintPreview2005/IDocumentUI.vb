Public Interface IDocumentUI
    Property CutEnabled() As Boolean
    Property CopyEnabled() As Boolean
    Property DeleteEnabled() As Boolean

    Property PasteEnabled() As Boolean
    Property SelectAllEnabled() As Boolean
    Property FindEnabled() As Boolean
    Property FindNextEnabled() As Boolean
    Property ReplaceEnabled() As Boolean

    Property TextColorEnabled() As Boolean

    Property BoldChecked() As Boolean
    Property ItalicChecked() As Boolean
    Property UnderlineChecked() As Boolean
    Property StrikethroughChecked() As Boolean
    Property AlignLeftChecked() As Boolean
    Property AlignRightChecked() As Boolean
    Property AlignCenterChecked() As Boolean

    Property BoldEnabled() As Boolean
    Property ItalicEnabled() As Boolean
    Property UnderlineEnabled() As Boolean
    Property StrikethroughEnabled() As Boolean
    Property AlignLeftEnabled() As Boolean
    Property AlignRightEnabled() As Boolean
    Property AlignCenterEnabled() As Boolean

    Property FontSelectionEnabled() As Boolean
    'Sub SetSelectionFont(ByVal font As Font)
    Property FontSizeEnabled() As Boolean
    Property FontSize() As Int32
End Interface
