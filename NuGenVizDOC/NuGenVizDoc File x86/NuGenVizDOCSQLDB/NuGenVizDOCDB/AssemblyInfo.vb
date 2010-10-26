Imports System.Reflection

' C1DataObjects-specific attibutes

<Assembly: C1.Data.SchemaClass(GetType(DataClass))>
<Assembly: C1.Data.RemoteServiceClass(GetType(RemoteService))>


' General Information about an assembly is controlled through the following 
' set of attributes. Change these attribute values to modify the information
' associated with an assembly.

' Review the values of the assembly attributes

<Assembly: AssemblyTitle("")> 
<Assembly: AssemblyDescription("")> 
<Assembly: AssemblyCompany("")> 
<Assembly: AssemblyProduct("")> 
<Assembly: AssemblyCopyright("")> 
<Assembly: AssemblyTrademark("")> 
<Assembly: CLSCompliant(True)> 


' Version information for an assembly consists of the following four values:
'
'      Major Version
'      Minor Version 
'      Build Number
'      Revision
'
' You can specify all the values or you can default the Build and Revision Numbers 
' by using the '*' as shown below:

<Assembly: AssemblyVersion("1.0.*")> 

' C1.Data.DataClass attributes will be placed below, if you add more data
' classes using the 'Add new item' wizard (Local Project Items | C1DataObjects)
