Option Strict On
Option Explicit On

Imports EnvDTE80
Imports VSLangProj80
Imports Microsoft.VisualStudio.Shell.Interop
Imports System.Text
Imports System.Runtime.InteropServices
Imports Microsoft.Win32
Imports System.IO
Imports System.Runtime
Imports System.Reflection
Imports System.Xml
Imports System.Xml.Serialization

<ComClass(DiagramVBCodeGenerator.ClassId, DiagramVBCodeGenerator.InterfaceId, DiagramVBCodeGenerator.EventsId)> _
Public Class DiagramVBCodeGenerator
    Implements IVsSingleFileGenerator

#Region "COM GUIDs"
    ' These  GUIDs provide the COM identity for this class 
    ' and its COM interfaces. If you change them, existing 
    ' clients will no longer be able to access the class.
    Public Const ClassId As String = "12dffb40-f91c-4b07-a3fe-9e5a9dd508b8"
    Public Const InterfaceId As String = "2db4e007-3fdf-44b7-9ef8-ae44783da581"
    Public Const EventsId As String = "8226df4f-7095-4287-8cb5-7d8479f67d54"
#End Region

    ' A creatable COM class must have a Public Sub New() 
    ' with no parameters, otherwise, the class will not be 
    ' registered in the COM registry and cannot be created 
    ' via CreateObject.
    Public Sub New()

    End Sub

    <ComRegisterFunction()> _
    Public Shared Sub SelfRegister(ByVal t As Type)
        Dim vbGeneratorKey As RegistryKey = My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\VisualStudio\8.0\Generators\{164b10b9-b200-11d0-8c61-00a0c91e29d5}", True)

        Dim vbCustomGeneratorKey As RegistryKey = vbGeneratorKey.CreateSubKey("NuGenObjectiveCustomGenerator")
        vbCustomGeneratorKey.SetValue(Nothing, "NuGenObjective Diagram Code Generator for VB")
        vbCustomGeneratorKey.SetValue("CLSID", "{" & ClassId & "}")
        vbCustomGeneratorKey.SetValue("GeneratesDesignTimeSource", 1)

        vbCustomGeneratorKey.Close()
        vbGeneratorKey.Close()

        Dim csGeneratorKey As RegistryKey = My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\VisualStudio\8.0\Generators\{fae04ec1-301f-11d3-bf4b-00c04f79efbc}", True)

        Dim csCustomGeneratorKey As RegistryKey = csGeneratorKey.CreateSubKey("NuGenObjectiveCustomGenerator")
        csCustomGeneratorKey.SetValue(Nothing, "NuGenObjective Diagram Code Generator for C#")
        csCustomGeneratorKey.SetValue("CLSID", "{" & DiagramCSCodeGenerator.ClassId & "}")
        csCustomGeneratorKey.SetValue("GeneratesDesignTimeSource", 1)

        csCustomGeneratorKey.Close()
        csGeneratorKey.Close()

    End Sub

    <ComUnregisterFunction()> _
    Public Shared Sub SelfUnRegister(ByVal t As Type)
        Try
            Dim vbGeneratorKey As RegistryKey = My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\VisualStudio\8.0\Generators\{164b10b9-b200-11d0-8c61-00a0c91e29d5}", True)

            Try
                vbGeneratorKey.DeleteSubKeyTree("NuGenObjectiveCustomGenerator")
            Finally
                vbGeneratorKey.Close()
            End Try


            Dim csGeneratorKey As RegistryKey = My.Computer.Registry.LocalMachine.OpenSubKey("SOFTWARE\Microsoft\VisualStudio\8.0\Generators\{fae04ec1-301f-11d3-bf4b-00c04f79efbc}", True)

            Try
                csGeneratorKey.DeleteSubKeyTree("NuGenObjectiveCustomGenerator")
            Finally
                csGeneratorKey.Close()
            End Try

        Catch

        End Try
    End Sub

    Public Function DefaultExtension(ByRef pbstrDefaultExtension As String) As Integer Implements Microsoft.VisualStudio.Shell.Interop.IVsSingleFileGenerator.DefaultExtension
        pbstrDefaultExtension = ".vb"
        Return 0
    End Function

    Public Function Generate( _
                ByVal wszInputFilePath As String, _
                ByVal bstrInputFileContents As String, _
                ByVal wszDefaultNamespace As String, _
                ByVal rgbOutputFileContents() As System.IntPtr, _
                ByRef pcbOutput As UInteger, _
                ByVal pGenerateProgress As Microsoft.VisualStudio.Shell.Interop.IVsGeneratorProgress _
                ) As Integer Implements Microsoft.VisualStudio.Shell.Interop.IVsSingleFileGenerator.Generate
        Try

            Dim outputCode As String
            Dim outputCodeBytes() As Byte

            ' Read the XSLT that transforms 
            Dim resReader As XmlReader = XmlReader.Create( _
                    Assembly.GetExecutingAssembly.GetManifestResourceStream( _
                            "Genetibase.NuGenObjective.VisualStudio.GenerateVBCode.xslt" _
                            ) _
            )

            Dim xslt As New Xml.Xsl.XslCompiledTransform()
            xslt.Load(resReader)

            Dim diagramXML As New XmlDocument()
            diagramXML.LoadXml(bstrInputFileContents)

            Dim outputBuilder As New StringBuilder("")
            Dim outputWriter As New StringWriter(outputBuilder)

            xslt.Transform(diagramXML, Nothing, outputWriter)
            outputWriter.Flush()

            outputCode = outputBuilder.ToString
            outputCodeBytes = Encoding.UTF8.GetBytes(outputCode)

            rgbOutputFileContents(0) = Marshal.AllocCoTaskMem(outputCodeBytes.Length)
            Marshal.Copy(outputCodeBytes, 0, rgbOutputFileContents(0), outputCodeBytes.Length)
            pcbOutput = CType(outputCodeBytes.Length, UInteger)

            pGenerateProgress.Progress(100, 100)
            Return 0
        Catch Ex As Exception
            pGenerateProgress.GeneratorError(0, 0, "ERROR:" & Ex.Message, 0, 0)
        End Try
    End Function
End Class


