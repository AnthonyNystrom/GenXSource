Public Class NuGenInterpolationForNTH_IntegerOrderBesselForm

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles y0.TextChanged

    End Sub
    Private Sub Label3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label3.Click

    End Sub

    Private Sub integer_order_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles integer_order.Click

        Dim e1 As Integer
        e1 = 0
        Dim y(4) As Decimal

        If arg.Text = "" Or steps.Text = "" Then
            MsgBox("Please Enter The Number")
        Else
            Genetibase.MathX.NuGenHyperbolicTrignometricFunction.NuGenInterpolation1.INTBESSL(Val(arg.Text), e1, y, Val(steps.Text))
            y0.Text = y(0).ToString
            y1.Text = y(1).ToString
            y2.Text = y(2).ToString
            y3.Text = y(3).ToString

        End If
    End Sub
End Class