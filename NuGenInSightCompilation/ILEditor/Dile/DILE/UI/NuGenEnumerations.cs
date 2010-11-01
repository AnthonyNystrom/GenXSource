using System;
using System.Collections.Generic;
using System.Text;

using Dile.Metadata;

namespace Dile.UI
{
	[Flags()]
	public enum SearchOptions
	{
		None = 1,
		Assembly = 2,
		AssemblyReference = 4,
		ModuleScope = 8,
		ModuleReference = 16,
		File = 32,
		ManifestResource = 64,
		TypeDefinition = 128,
		MethodDefinition = 256,
		Property = 512,
		FieldDefintion = 1024,
		TokenValues = 2048
	}

	public enum ValueFieldGroup : uint
	{
		PrivateScope = CorFieldAttr.fdPrivateScope,
		Private = CorFieldAttr.fdPrivate,
		FamilyAndAssembly = CorFieldAttr.fdFamANDAssem,
		Assembly = CorFieldAttr.fdAssembly,
		Family = CorFieldAttr.fdFamily,
		FamilyOrAssembly = CorFieldAttr.fdFamORAssem,
		Public = CorFieldAttr.fdPublic,
		ObjectInformation,
		EvaluationException,
		MissingModule
	}

	public enum MenuFunction
	{
		//File menu
		NewProject,
		OpenProject,
		SaveProject,
		SaveProjectAs,
		Settings,
		Exit,

		//Project menu
		ProjectProperties,
		AddAssembly,
		RemoveAssembly,
		OpenReferenceInProject,

		//Debug menu
		AttachToProcess,
		RunDebuggee,
		PauseDebuggee,
		StopDebuggee,
		Detach,
		Step,
		StepInto,
		StepOut,
		ObjectViewer,

		//View menu
		WordWrap,

		//View/Panels menu
		InformationPanel = 1000,
		DebugOutputPanel = 1001,
		LogMessagePanel = 1002,
		ThreadsPanel = 1003,
		ModulesPanel = 1004,
		CallStackPanel = 1005,
		BreakpointsPanel = 1006,
		LocalVariablesPanel = 1007,
		ArgumentsPanel = 1008,
		AutoObjectsPanel = 1009,
		ProjectExplorerPanel = 1010,
		QuickSearchPanel = 1011,

		//Windows menu
		CloseAllWindows,

		//Help menu
		About,

		RunToCursor
	}

	public enum ObjectsPanelMode
	{
		LocalVariables,
		Arguments,
		AutoObjects,
		Watch
	}

	public enum ExtendedDialogResult
	{
		None,
		Yes,
		No,
		YesToAll,
		NoToAll
	}
}