Imports System
Imports System.Collections.Generic
Imports System.Windows.Forms

Module Program


    Sub Main()

        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)


        Dim F As New Form_Container
        Dim FF As New Form_Firms
        F.Prepare(New Drawing.Point(10, 20), FF)

        Application.Run(F)

    End Sub

End Module
