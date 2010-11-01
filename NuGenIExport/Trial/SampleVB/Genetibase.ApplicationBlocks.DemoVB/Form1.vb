Imports Genetibase.ApplicationBlocks
Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms


Public Class Form1


    Private Sub goButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles goButton.Click


        Dim exportForm As NuGenImageExportForm = New NuGenImageExportForm

        Try
            Dim image As Image = image.FromFile(Me.openFileSelector.GetPath)

            exportForm.ShowDialog(image)

        Catch oE As ArgumentException

            MessageBox.Show("Specify the path.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        Catch oE As FileNotFoundException

            MessageBox.Show("Specified file could not be found. Make sure the file exists.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        End Try

    End Sub
End Class
