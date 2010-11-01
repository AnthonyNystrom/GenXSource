Option Strict On
Option Explicit On

Imports Microsoft.VisualStudio.TemplateWizard

Public Class NuGenObjectiveDiagramWizard
    Implements IWizard

    Public Sub BeforeOpeningFile(ByVal projectItem As EnvDTE.ProjectItem) Implements Microsoft.VisualStudio.TemplateWizard.IWizard.BeforeOpeningFile

    End Sub

    Public Sub ProjectFinishedGenerating(ByVal project As EnvDTE.Project) Implements Microsoft.VisualStudio.TemplateWizard.IWizard.ProjectFinishedGenerating

    End Sub

    Public Sub ProjectItemFinishedGenerating(ByVal projectItem As EnvDTE.ProjectItem) Implements Microsoft.VisualStudio.TemplateWizard.IWizard.ProjectItemFinishedGenerating
        projectItem.Properties.Item("ItemType").Value = "Template"
        projectItem.Properties.Item("CustomTool").Value = "NuGenObjectiveCustomGenerator"
    End Sub

    Public Sub RunFinished() Implements Microsoft.VisualStudio.TemplateWizard.IWizard.RunFinished

    End Sub

    Public Sub RunStarted(ByVal automationObject As Object, ByVal replacementsDictionary As System.Collections.Generic.Dictionary(Of String, String), ByVal runKind As Microsoft.VisualStudio.TemplateWizard.WizardRunKind, ByVal customParams() As Object) Implements Microsoft.VisualStudio.TemplateWizard.IWizard.RunStarted

    End Sub

    Public Function ShouldAddProjectItem(ByVal filePath As String) As Boolean Implements Microsoft.VisualStudio.TemplateWizard.IWizard.ShouldAddProjectItem
        Return True
    End Function
End Class
