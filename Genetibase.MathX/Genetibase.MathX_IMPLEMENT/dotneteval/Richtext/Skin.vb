Imports System.Windows.Forms
Imports System.Drawing

Public Class Skin
    Private Shared mFirstInitDone As Boolean = False
    Private Shared mSkinLoaded As Boolean = False
    Private Shared mSkinFileName As String

    Shared Sub FirstInit()
        If mFirstInitDone Then Exit Sub
        Try
            If TryLoadingSkin("Skin.xml") Then Exit Sub
            If TryLoadingSkin("Skin\Skin.xml") Then Exit Sub
        Catch ex As Exception
            MsgBox("Error while initializing Skin" & vbCrLf & ex.Message, MsgBoxStyle.Critical)
        Finally
            mFirstInitDone = True
        End Try
    End Sub

    Private Shared Function TryLoadingSkin(ByVal filename As String) As Boolean
        Dim fi As New IO.FileInfo(filename)
        If fi.Exists Then
            mSkinFileName = filename
            Dim skinLoader As New SkinLoader
            Dim s As New Skin
            skinLoader.Load(s, fi.OpenRead)
            mSkinLoaded = True
            Return True
        End If
    End Function

    Shared Sub SkinPaint(ByVal c As Control, _
        ByVal g As Graphics, ByVal clipRect As Rectangle, _
        Optional ByVal SkinPart As iSkinable = Nothing)

        If mFirstInitDone = False Then FirstInit()

        Dim fullName As String
        Dim br As New SolidBrush(Color.FromArgb(CInt(Rnd() * 255), CInt(Rnd() * 255), CInt(Rnd() * 255)))
        With c
            Dim parent As Control = .Parent
            Do While Not parent Is Nothing
                If TypeOf parent Is Form Or TypeOf parent Is UserControl Then Exit Do
                parent = parent.Parent
            Loop
            If parent Is Nothing Then
                fullName = .Name
            Else
                fullName = parent.Name & .Name
            End If
        End With
        Dim l As String = c.GetType.Name & " " & c.GetType.Assembly.Location
        'c.Text = l
        g.FillRectangle(br, 0, 0, c.Width, c.Height)

    End Sub

    Public Shared Function GetImage(ByVal imagename As String) As Drawing.Image
        If mFirstInitDone = False Then FirstInit()
        Try
            Dim bitmap As Drawing.Image = bitmap.FromFile(imagename)
            Return bitmap
        Catch ex As Exception
            ' ignore errors
        End Try
    End Function
End Class
