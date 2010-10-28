Imports Genetibase.MathX.Distribution
Imports Genetibase.MathX





Public Class NuGen_Distribution

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dist_poisson.Click
        Dim pass_val, result_val As Decimal
        pass_val = Val(dist_2_first_no.Text)
        Genetibase.MathX.NeGenDistribution.NuGenDistributions.POISSON(pass_val, result_val)
        dist_2_result.Text = result_val.ToString()

    End Sub

    Private Sub dist_cauchy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dist_cauchy.Click

        Dim pass_val, result_val As Decimal
        pass_val = Val(dist_2_first_no.Text)
        Genetibase.MathX.NeGenDistribution.NuGenDistributions.CAUCHY(pass_val, result_val)
        dist_2_result.Text = result_val.ToString()




    End Sub

    Private Sub dist_exp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dist_exp.Click
        Dim pass_val, result_val As Decimal
        pass_val = Val(dist_2_first_no.Text)
        Genetibase.MathX.NeGenDistribution.NuGenDistributions.EXPONENT(pass_val, result_val)
        dist_2_result.Text = result_val.ToString()


    End Sub

    Private Sub CauchyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CauchyToolStripMenuItem.Click
        dist_three_para.Visible = False
        dist_two_para.Visible = True




    End Sub

    Private Sub dist_gamma_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dist_gamma.Click
        Dim pass_val, result_val As Decimal
        pass_val = Val(dist_2_first_no.Text)
        Genetibase.MathX.NeGenDistribution.NuGenDistributions.GAMMA(pass_val, result_val)
        dist_2_result.Text = result_val.ToString()

    End Sub

    Private Sub dist_2_clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dist_2_clear.Click
        dist_2_first_no.Text = ""
        dist_2_result.Text = ""

        dist_2_first_no.Focus()

    End Sub

    Private Sub dist_beta_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dist_beta.Click
        Dim pass_val, pass_val1, result_val As Decimal
        pass_val = Val(dist_first_no.Text)
        pass_val1 = Val(dist_second_no.Text)
        Genetibase.MathX.NeGenDistribution.NuGenDistributions.BETA(pass_val, pass_val1, result_val)
        dist_result.Text = result_val.ToString()

    End Sub

    Private Sub dist_normal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dist_normal.Click
        Dim pass_val, pass_val1, result_val As Decimal
        pass_val = Val(dist_first_no.Text)
        pass_val1 = Val(dist_second_no.Text)
        Genetibase.MathX.NeGenDistribution.NuGenDistributions.NORMAL(pass_val, pass_val1, result_val)
        dist_result.Text = result_val.ToString()
    End Sub

    Private Sub dist_binomial_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dist_binomial.Click
        Dim pass_val, pass_val1, result_val As Decimal
        pass_val = Val(dist_first_no.Text)
        pass_val1 = Val(dist_second_no.Text)
        Genetibase.MathX.NeGenDistribution.NuGenDistributions.BINOMIAL(pass_val, pass_val1, result_val)
        dist_result.Text = result_val.ToString()
    End Sub

    Private Sub dist_weibull_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dist_weibull.Click
        Dim pass_val, pass_val1, result_val As Decimal
        pass_val = Val(dist_first_no.Text)
        pass_val1 = Val(dist_second_no.Text)
        Genetibase.MathX.NeGenDistribution.NuGenDistributions.WEIBULL(pass_val, pass_val1, result_val)
        dist_result.Text = result_val.ToString()
    End Sub

    Private Sub dist_fermi_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dist_fermi.Click
        Dim pass_val, pass_val1, result_val As Decimal
        pass_val = Val(dist_first_no.Text)
        pass_val1 = Val(dist_second_no.Text)
        Genetibase.MathX.NeGenDistribution.NuGenDistributions.FERMI(pass_val, pass_val1, result_val)
        dist_result.Text = result_val.ToString()
    End Sub

    Private Sub dist_clear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dist_clear.Click
        dist_first_no.Text = ""
        dist_second_no.Text = ""
        dist_result.Text = ""
        dist_first_no.Focus()

    End Sub

    Private Sub BETAToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BETAToolStripMenuItem.Click
        dist_two_para.Visible = False
        dist_three_para.Visible = True

    End Sub

    Private Sub BinomialToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BinomialToolStripMenuItem.Click
        dist_two_para.Visible = False
        dist_three_para.Visible = True

    End Sub

    Private Sub PermiToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PermiToolStripMenuItem.Click
        dist_two_para.Visible = False
        dist_three_para.Visible = True

    End Sub

    Private Sub NormalToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NormalToolStripMenuItem.Click
        dist_two_para.Visible = False
        dist_three_para.Visible = True

    End Sub

    Private Sub WeibullToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WeibullToolStripMenuItem.Click
        dist_two_para.Visible = False
        dist_three_para.Visible = True

    End Sub

    Private Sub ExponentToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExponentToolStripMenuItem.Click
        dist_three_para.Visible = False
        dist_two_para.Visible = True
    End Sub

    Private Sub GammaToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GammaToolStripMenuItem.Click
        dist_three_para.Visible = False
        dist_two_para.Visible = True
    End Sub

    Private Sub LinearToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LinearToolStripMenuItem.Click
        dist_three_para.Visible = True
        dist_two_para.Visible = False
        
    End Sub

    Private Sub PossionToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PossionToolStripMenuItem.Click
        dist_three_para.Visible = False
        dist_two_para.Visible = True
    End Sub

    Private Sub dist_linear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dist_linear.Click
        Dim pass_val, pass_val1, result_val As Decimal
        Dim temp_res As Integer
        pass_val = Val(dist_first_no.Text)
        pass_val1 = Val(dist_second_no.Text)
        Genetibase.MathX.NeGenDistribution.NuGenDistributions.LINEAR(pass_val, pass_val1, temp_res, result_val)
        dist_result.Text = result_val.ToString()
    End Sub

   

    Private Sub MainMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'Dim next_form As New NuGenScientificCalculator.NuGenSciCalc
        ''Dim next_form As New nu
        'Me.Visible = False
        'next_form.Visible = True
    End Sub
End Class