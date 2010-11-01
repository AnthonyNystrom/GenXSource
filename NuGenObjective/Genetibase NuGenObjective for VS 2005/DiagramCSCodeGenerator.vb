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

<ComClass(DiagramCSCodeGenerator.ClassId, DiagramCSCodeGenerator.InterfaceId, DiagramCSCodeGenerator.EventsId)> _
Public Class DiagramCSCodeGenerator
    Implements IVsSingleFileGenerator

#Region "COM GUIDs"
    ' These  GUIDs provide the COM identity for this class 
    ' and its COM interfaces. If you change them, existing 
    ' clients will no longer be able to access the class.
    Public Const ClassId As String = "087532b1-3aca-47cc-bb6d-a8624a6559d9"
    Public Const InterfaceId As String = "633a4a59-96ca-4e69-8f5c-fef2348c8aa2"
    Public Const EventsId As String = "65b1513f-6b72-459c-80a2-0bcb736de3eb"
#End Region

    ' A creatable COM class must have a Public Sub New() 
    ' with no parameters, otherwise, the class will not be 
    ' registered in the COM registry and cannot be created 
    ' via CreateObject.
    Public Sub New()
        MyBase.New()
    End Sub

    Public Function DefaultExtension(ByRef pbstrDefaultExtension As String) As Integer Implements Microsoft.VisualStudio.Shell.Interop.IVsSingleFileGenerator.DefaultExtension
        pbstrDefaultExtension = ".cs"
        Return 0
    End Function

    Public Function Generate(ByVal wszInputFilePath As String, ByVal bstrInputFileContents As String, ByVal wszDefaultNamespace As String, ByVal rgbOutputFileContents() As System.IntPtr, ByRef pcbOutput As UInteger, ByVal pGenerateProgress As Microsoft.VisualStudio.Shell.Interop.IVsGeneratorProgress) As Integer Implements Microsoft.VisualStudio.Shell.Interop.IVsSingleFileGenerator.Generate
        Try
            Dim outputCode As String
            Dim outputCodeBytes() As Byte

            ' Read the XSLT that transforms 
            Dim resReader As XmlReader = XmlReader.Create( _
                    Assembly.GetExecutingAssembly.GetManifestResourceStream( _
                            "Genetibase.NuGenObjective.VisualStudio.GenerateCSCode.xslt" _
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


