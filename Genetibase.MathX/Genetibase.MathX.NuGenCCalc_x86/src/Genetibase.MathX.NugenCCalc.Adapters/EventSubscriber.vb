Imports System.Reflection
Imports System.Collections
Imports System.CodeDom
Imports System.CodeDom.Compiler

Public Delegate Sub CustomEventHandler(ByVal args() As Object)

Public Class EventSubscriber

    Private Shared ReadOnly _compiler As ICodeCompiler = New Microsoft.CSharp.CSharpCodeProvider().CreateCompiler()
    Private _eventInfo As EventInfo
    Private _compiledClass As Type

    Public Sub New(ByVal ei As EventInfo)
        _eventInfo = ei
        Compile()
    End Sub

    Public Sub Subscribe(ByVal obj As Object, ByVal handler As CustomEventHandler)
        Activator.CreateInstance(_compiledClass, New Object() {obj, handler})
    End Sub

    Private Function GetAssemblyReferencesByType(ByVal t As Type) As Hashtable
        Dim result As Hashtable = New Hashtable
        GetAssemblyReferencesByType(t, result)
        Return result
    End Function

    Private Sub GetAssemblyReferencesByType(ByVal t As Type, ByVal references As Hashtable)
        If (Not t.BaseType Is Nothing) Then
            GetAssemblyReferencesByType(t.BaseType, references)
        End If
        For Each i As Type In t.GetInterfaces()
            GetAssemblyReferencesByType(i, references)
        Next
        If (Not references.ContainsKey(t.Assembly.Location)) Then
            references.Add(t.Assembly.Location, t.Assembly)
        End If
    End Sub

    Private Sub Compile()
        Dim eventParams() As ParameterInfo = _eventInfo.EventHandlerType.GetMethod("Invoke").GetParameters()
        Dim className As String = String.Format("{0}{1}EventSubscriber", _eventInfo.DeclaringType.Name, _eventInfo.Name)

        Dim cu As CodeCompileUnit = New CodeCompileUnit()
        Dim ns As CodeNamespace = New CodeNamespace("Genetibase.MathX.NugenCCalc.Runtime")

        Dim field As CodeMemberField = New CodeMemberField(GetType(CustomEventHandler), "_handler")

        Dim constructor As CodeConstructor = New CodeConstructor()
        constructor.Attributes = MemberAttributes.Public

        Dim method As CodeMemberMethod = New CodeMemberMethod()
        Dim cl As CodeTypeDeclaration = New CodeTypeDeclaration(className)
        Dim compile_result As CompilerResults

        cu.Namespaces.Add(ns)
        ns.Imports.Add(New CodeNamespaceImport("System"))
        ns.Imports.Add(New CodeNamespaceImport("Genetibase.MathX.NugenCCalc.Adapters"))

        ns.Types.Add(cl)

        cl.TypeAttributes = TypeAttributes.Public
        cl.Members.Add(field)
        cl.Members.Add(method)
        cl.Members.Add(constructor)

        cu.ReferencedAssemblies.Add(Me.GetType().Assembly.Location)

        For Each o As Object In GetAssemblyReferencesByType(_eventInfo.DeclaringType).Keys
            cu.ReferencedAssemblies.Add(o.ToString())
        Next

        constructor.Attributes = MemberAttributes.Public
        constructor.Parameters.Add(New CodeParameterDeclarationExpression(GetType(Object), "obj"))
        constructor.Parameters.Add(New CodeParameterDeclarationExpression(GetType(CustomEventHandler), "handler"))
        constructor.Statements.Clear()

        constructor.Statements.Add(New CodeExpressionStatement(New CodeSnippetExpression("_handler = handler")))
        constructor.Statements.Add(New CodeExpressionStatement(New CodeSnippetExpression(String.Format("(({0})obj).{1} += new {2}(Event)", _eventInfo.DeclaringType.FullName, _eventInfo.Name, _eventInfo.EventHandlerType.FullName))))

        method.ReturnType = Nothing
        method.Attributes = MemberAttributes.Public
        method.Name = "Event"

        Dim paramNames As ArrayList = New ArrayList()

        For Each pi As ParameterInfo In eventParams
            method.Parameters.Add(New CodeParameterDeclarationExpression(pi.ParameterType, pi.Name))
            paramNames.Add(pi.Name)
        Next


        method.Statements.Clear()
        Dim params As String = ""
        For Each par As String In paramNames.ToArray(GetType(String))
            params = par & ", "
        Next
        If (params.Length > 2) Then
            params = params.Substring(0, params.Length - 2)
        End If
        method.Statements.Add(New CodeExpressionStatement(New CodeSnippetExpression(String.Format("_handler({0})", params))))


        Dim compparams As CompilerParameters = New CompilerParameters()
        compparams.GenerateInMemory = True

        compile_result = _compiler.CompileAssemblyFromDom(compparams, cu)

        If (compile_result Is Nothing Or compile_result.Errors.Count > 0) Then
            Dim errorString As String = ""
            For Each err As CompilerError In compile_result.Errors
                errorString = errorString & err.ErrorText + System.Environment.NewLine
            Next

            Throw New Exception("Cant compile. Error: " & errorString)
        End If

        _compiledClass = compile_result.CompiledAssembly.GetType("Genetibase.MathX.NugenCCalc.Runtime." + className)

    End Sub


End Class
