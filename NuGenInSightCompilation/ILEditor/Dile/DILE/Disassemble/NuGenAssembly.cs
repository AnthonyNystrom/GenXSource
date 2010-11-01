using System;
using System.Collections.Generic;
using System.Text;

using Dile.Debug;
using Dile.Disassemble.ILCodes;
using Dile.Metadata;
using Dile.UI;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace Dile.Disassemble
{
	public class NuGenAssembly : NuGenTokenBase, NuGenIMultiLine, IDisposable
	{
		#region IMultiLine Members
		private List<NuGenCodeLine> codeLines;
		[XmlIgnore()]
		public List<NuGenCodeLine> CodeLines
		{
			get
			{
				return codeLines;
			}
			set
			{
				codeLines = value;
			}
		}

		[XmlIgnore()]
		public string HeaderText
		{
			get
			{
				return FileName;
			}
		}

		private bool isInMemory;
		[XmlIgnore()]
		public bool IsInMemory
		{
			get
			{
				return isInMemory;
			}
		}
		#endregion

		private SearchOptions searchOptions = SearchOptions.TypeDefinition;
		public SearchOptions SearchOptions
		{
			get
			{
				return searchOptions;
			}
			set
			{
				searchOptions = value;
			}
		}

		public override SearchOptions ItemType
		{
			get
			{
				return SearchOptions.Assembly;
			}
		}

		private string fullPath;
		public string FullPath
		{
			get
			{
				return fullPath;
			}

			set
			{
				fullPath = value;
				FileName = Path.GetFileName(FullPath);
			}
		}

		private string fileName;
		[XmlIgnore()]
		public string FileName
		{
			get
			{
				return fileName;
			}
			set
			{
				fileName = value;
			}
		}

		private NuGenModuleScope moduleScope;
		[XmlIgnore()]
		public NuGenModuleScope ModuleScope
		{
			get
			{
				return moduleScope;
			}

			private set
			{
				moduleScope = value;
			}
		}

		private List<NuGenModuleReference> moduleReferences;
		[XmlIgnore()]
		public List<NuGenModuleReference> ModuleReferences
		{
			get
			{
				return moduleReferences;
			}

			private set
			{
				moduleReferences = value;
			}
		}

		private List<NuGenTypeReference> typeReferences;
		[XmlIgnore()]
		public List<NuGenTypeReference> TypeReferences
		{
			get
			{
				return typeReferences;
			}

			private set
			{
				typeReferences = value;
			}
		}

		private BinaryReader assemblyReader;
		[XmlIgnore()]
		public BinaryReader AssemblyReader
		{
			get
			{
				return assemblyReader;
			}

			private set
			{
				assemblyReader = value;
			}
		}

		private bool isPe32;
		[XmlIgnore()]
		public bool IsPe32
		{
			get
			{
				return isPe32;
			}
			private set
			{
				isPe32 = value;
			}
		}

		private uint entryPointToken = 0;
		[XmlIgnore()]
		public uint EntryPointToken
		{
			get
			{
				return entryPointToken;
			}
			private set
			{
				entryPointToken = value;
			}
		}

		private Dictionary<uint, NuGenUserString> userStrings;
		[XmlIgnore()]
		public Dictionary<uint, NuGenUserString> UserStrings
		{
			get
			{
				return userStrings;
			}
			private set
			{
				userStrings = value;
			}
		}

		private Dictionary<uint, NuGenTokenBase> allTokens = new Dictionary<uint, NuGenTokenBase>();
		[XmlIgnore()]
		public Dictionary<uint, NuGenTokenBase> AllTokens
		{
			get
			{
				return allTokens;
			}
			private set
			{
				allTokens = value;
			}
		}

		private Dictionary<uint, NuGenStandAloneSignature> standAloneSignatures;
		[XmlIgnore()]
		public Dictionary<uint, NuGenStandAloneSignature> StandAloneSignatures
		{
			get
			{
				return standAloneSignatures;
			}
			private set
			{
				standAloneSignatures = value;
			}
		}

		private Dictionary<uint, NuGenAssemblyReference> assemblyReferences;
		[XmlIgnore()]
		public Dictionary<uint, NuGenAssemblyReference> AssemblyReferences
		{
			get
			{
				return assemblyReferences;
			}
			private set
			{
				assemblyReferences = value;
			}
		}

		private List<NuGenCustomAttribute> assemblyCustomAttributes;
		[XmlIgnore()]
		public List<NuGenCustomAttribute> AssemblyCustomAttributes
		{
			get
			{
				return assemblyCustomAttributes;
			}
			private set
			{
				assemblyCustomAttributes = value;
			}
		}

		private NuGenTypeDefinition globalType;
		[XmlIgnore()]
		public NuGenTypeDefinition GlobalType
		{
			get
			{
				return globalType;
			}
			private set
			{
				globalType = value;
			}
		}

		private NuGenIMetaDataDispenserEx dispenser;
		[XmlIgnore()]
		public NuGenIMetaDataDispenserEx Dispenser
		{
			get
			{
				return dispenser;
			}
			private set
			{
				dispenser = value;
			}
		}

		private NuGenIMetaDataImport2 import;
		[XmlIgnore()]
		public NuGenIMetaDataImport2 Import
		{
			get
			{
				return import;
			}
			private set
			{
				import = value;
			}
		}

		private NuGenIMetaDataAssemblyImport assemblyImport;
		[XmlIgnore()]
		public NuGenIMetaDataAssemblyImport AssemblyImport
		{
			get
			{
				return assemblyImport;
			}
			private set
			{
				assemblyImport = value;
			}
		}

		private IntPtr publicKey;
		[XmlIgnore()]
		public IntPtr PublicKey
		{
			get
			{
				return publicKey;
			}
			private set
			{
				publicKey = value;
			}
		}

		private uint publicKeyLength;
		[XmlIgnore()]
		public uint PublicKeyLength
		{
			get
			{
				return publicKeyLength;
			}
			private set
			{
				publicKeyLength = value;
			}
		}

		private uint hashAlgorithmID;
		[XmlIgnore()]
		public uint HashAlgorithmID
		{
			get
			{
				return hashAlgorithmID;
			}
			private set
			{
				hashAlgorithmID = value;
			}
		}

		private NuGenAssemblyMetadata metadata;
		[XmlIgnore()]
		public NuGenAssemblyMetadata Metadata
		{
			get
			{
				return metadata;
			}
			private set
			{
				metadata = value;
			}
		}

		private CorAssemblyFlags flags;
		[XmlIgnore()]
		public CorAssemblyFlags Flags
		{
			get
			{
				return flags;
			}
			private set
			{
				flags = value;
			}
		}

		private uint imageBase;
		[XmlIgnore()]
		public uint ImageBase
		{
			get
			{
				return imageBase;
			}
			private set
			{
				imageBase = value;
			}
		}

		private uint fileAlignment;
		[XmlIgnore()]
		public uint FileAlignment
		{
			get
			{
				return fileAlignment;
			}
			private set
			{
				fileAlignment = value;
			}
		}

		private ulong stackReserve;
		[XmlIgnore()]
		public ulong StackReserve
		{
			get
			{
				return stackReserve;
			}
			private set
			{
				stackReserve = value;
			}
		}

		private ushort subsystem;
		[XmlIgnore()]
		public ushort Subsystem
		{
			get
			{
				return subsystem;
			}
			private set
			{
				subsystem = value;
			}
		}

		private uint corFlags;
		[XmlIgnore()]
		public uint CorFlags
		{
			get
			{
				return corFlags;
			}
			private set
			{
				corFlags = value;
			}
		}

		private List<NuGenSectionHeader> sectionHeaders = new List<NuGenSectionHeader>();
		[XmlIgnore()]
		public List<NuGenSectionHeader> SectionHeaders
		{
			get
			{
				return sectionHeaders;
			}
			private set
			{
				sectionHeaders = value;
			}
		}

		private List<NuGenManifestResource> manifestResources;
		[XmlIgnore()]
		public List<NuGenManifestResource> ManifestResources
		{
			get
			{
				return manifestResources;
			}
			private set
			{
				manifestResources = value;
			}
		}

		private List<NuGenFile> files;
		[XmlIgnore()]
		public List<NuGenFile> Files
		{
			get
			{
				return files;
			}
			private set
			{
				files = value;
			}
		}

		private string frameworkVersion;
		[XmlIgnore()]
		public string FrameworkVersion
		{
			get
			{
				return frameworkVersion;
			}
			private set
			{
				frameworkVersion = value;
			}
		}

		private bool displayInTree;
		[XmlIgnore()]
		public bool DisplayInTree
		{
			get
			{
				return displayInTree;
			}
			private set
			{
				displayInTree = value;
			}
		}

		private List<NuGenPermissionSet> permissionSets;
		[XmlIgnore()]
		public List<NuGenPermissionSet> PermissionSets
		{
			get
			{
				return permissionSets;
			}
			private set
			{
				permissionSets = value;
			}
		}

		private IntPtr fileContentUnmanaged;
		[XmlIgnore()]
		private IntPtr FileContentUnmanaged
		{
			get
			{
				return fileContentUnmanaged;
			}
			set
			{
				fileContentUnmanaged = value;
			}
		}

		private uint fileContentLength;
		[XmlIgnore()]
		private uint FileContentLength
		{
			get
			{
				return fileContentLength;
			}
			set
			{
				fileContentLength = value;
			}
		}

		private ProcessWrapper debuggedProcess;
		[XmlIgnore()]
		public ProcessWrapper DebuggedProcess
		{
			get
			{
				return debuggedProcess;
			}
			private set
			{
				debuggedProcess = value;
			}
		}

		public NuGenAssembly()
			: this(false)
		{
		}

		public NuGenAssembly(bool isInMemory)
		{
			this.isInMemory = isInMemory;
		}

		public NuGenAssembly(string fullPath, bool isDynamicAssembly)
			: this(isDynamicAssembly)
		{
			FullPath = fullPath;
			LoadAssembly();
		}

		~NuGenAssembly()
		{
			Dispose();
		}

		#region IDisposable Members

		public void Dispose()
		{
			try
			{
				if (AssemblyReader != null)
				{
					AssemblyReader.Close();
					AssemblyReader = null;
				}

				CloseMetadataInterfaces();
				//Todo The FileContentUnmanaged pointer and this point might be invalid thus the memory should be freed earlier/differently.
				Marshal.FreeHGlobal(FileContentUnmanaged);
			}
			catch
			{
			}
		}

		#endregion

		public void CloseMetadataInterfaces()
		{
			if (!IsInMemory)
			{
				ReleaseObject(Dispenser);
				ReleaseObject(Import);
				ReleaseObject(AssemblyImport);
			}
		}

		private void ReleaseObject(object comObject)
		{
			if (comObject != null)
			{
				Marshal.ReleaseComObject(comObject);
				comObject = null;
			}
		}

		public void CloseAssemblyReader()
		{
			AssemblyReader.Close();
			AssemblyReader = null;
			GC.Collect();
		}

		public void OpenAssemblyReader()
		{
			if (AssemblyReader == null)
			{
				byte[] fileContent = new byte[FileContentLength];
				Marshal.Copy(FileContentUnmanaged, fileContent, 0, Convert.ToInt32(FileContentLength));

				AssemblyReader = new BinaryReader(new MemoryStream(fileContent));
			}

			AssemblyReader.BaseStream.Seek(0, SeekOrigin.Begin);
		}

		public void OpenMetadataInterfaces()
		{
			if (!IsInMemory)
			{
				Dispenser = new NuGenMetaDataDispenserEx();
				object rawScope = null;
				Guid assemblyImportGuid = NuGenGuids.IID_IMetaDataAssemblyImport;
				Dispenser.OpenScopeOnMemory(FileContentUnmanaged, FileContentLength, (uint)CorOpenFlags.ofRead, ref assemblyImportGuid, out rawScope);
				AssemblyImport = (NuGenIMetaDataAssemblyImport)rawScope;

				object rawScope2 = null;
				Guid metaDataImportGuid = NuGenGuids.IID_IMetaDataImport2;
				Dispenser.OpenScopeOnMemory(FileContentUnmanaged, FileContentLength, (uint)CorOpenFlags.ofRead, ref metaDataImportGuid, out rawScope2);
				Import = (NuGenIMetaDataImport2)rawScope2;
			}
		}

		public void LoadAssembly()
		{
			byte[] fileContent;
			using (FileStream fileStream = new FileStream(FullPath, FileMode.Open, FileAccess.Read))
			{
				FileContentLength = Convert.ToUInt32(fileStream.Length);
				fileContent = new byte[FileContentLength];
				fileStream.Seek(0, SeekOrigin.Begin);
				fileStream.Read(fileContent, 0, Convert.ToInt32(FileContentLength));
			}

			FileContentUnmanaged = Marshal.AllocHGlobal(fileContent.Length);
			Marshal.Copy(fileContent, 0, FileContentUnmanaged, fileContent.Length);

			AssemblyReader = new BinaryReader(new MemoryStream(fileContent));
			OpenMetadataInterfaces();

			LoadAssemblyFromMetadataInterfaces();

			CloseMetadataInterfaces();
			CloseAssemblyReader();
		}

		public void LoadAssemblyFromMetadataInterfaces(NuGenIMetaDataDispenserEx dispenser, NuGenIMetaDataAssemblyImport assemblyImport, NuGenIMetaDataImport2 import, ModuleWrapper debuggedModule)
		{
			Dispenser = dispenser;
			AssemblyImport = assemblyImport;
			Import = import;

			uint bufferCount;
			Import.GetVersionString(NuGenProject.DefaultCharArray, Convert.ToUInt32(NuGenProject.DefaultCharArray.Length), out bufferCount);

			if (bufferCount > NuGenProject.DefaultCharArray.Length)
			{
				NuGenProject.DefaultCharArray = new char[bufferCount];
				Import.GetVersionString(NuGenProject.DefaultCharArray, bufferCount, out bufferCount);
			}

			FrameworkVersion = NuGenHelperFunctions.GetString(NuGenProject.DefaultCharArray, 0, bufferCount);
			ProcessWrapper debuggedProcess = debuggedModule.GetProcess();
			Process process = Process.GetProcessById(Convert.ToInt32(debuggedProcess.GetID()));
			FullPath = process.MainModule.FileName;
			FileName = Name;

			LoadAssemblyFromMetadataInterfaces(debuggedModule);
		}

		public void LoadAssemblyFromMetadataInterfaces()
		{
			LoadAssemblyFromMetadataInterfaces(null);
		}

		public void LoadAssemblyFromMetadataInterfaces(ModuleWrapper debuggedModule)
		{
			string assemblyPath = (IsInMemory ? Name : FullPath);
			NuGenUIHandler.Instance.ResetProgressBar();

			if (IsInMemory)
			{
				NuGenUIHandler.Instance.SetProgressBarMaximum(15);
				DebuggedProcess = debuggedModule.GetProcess();
			}
			else
			{
				NuGenUIHandler.Instance.SetProgressBarMaximum(16);
				NuGenUIHandler.Instance.SetProgressText(assemblyPath, "Reading header...", true);
				ReadHeader();
				NuGenUIHandler.Instance.StepProgressBar(1);
			}

			NuGenUIHandler.Instance.SetProgressText(assemblyPath, "Opening assembly references...", true);
			OpenAssemblyRefs();
			NuGenUIHandler.Instance.StepProgressBar(1);

			NuGenUIHandler.Instance.SetProgressText(assemblyPath, "Loading user strings...", true);
			GetUserStrings();
			NuGenUIHandler.Instance.StepProgressBar(1);

			NuGenUIHandler.Instance.SetProgressText(assemblyPath, "Loading manifest resources...", true);
			GetManifestResources();
			NuGenUIHandler.Instance.StepProgressBar(1);

			NuGenUIHandler.Instance.SetProgressText(assemblyPath, "Loading files...", true);
			GetFiles();
			NuGenUIHandler.Instance.StepProgressBar(1);

			NuGenUIHandler.Instance.SetProgressText(assemblyPath, "Loading module references...", true);
			GetModuleReferences();
			NuGenUIHandler.Instance.StepProgressBar(1);
			NuGenUIHandler.Instance.SetProgressText(assemblyPath, "Loading type references...", true);
			GetTypeReferences();
			NuGenUIHandler.Instance.StepProgressBar(1);
			NuGenUIHandler.Instance.SetProgressText(assemblyPath, "Loading global type's references...", true);
			NuGenHelperFunctions.GetMemberReferences(this, 0);
			NuGenUIHandler.Instance.StepProgressBar(1);
			NuGenUIHandler.Instance.SetProgressText(assemblyPath, "Loading type specifications...", true);
			GetTypeSpecs();
			NuGenUIHandler.Instance.StepProgressBar(1);
			NuGenUIHandler.Instance.SetProgressText(assemblyPath, "Loading standalone signatures...", true);
			GetSignatures();
			NuGenUIHandler.Instance.StepProgressBar(1);

			NuGenUIHandler.Instance.SetProgressText(assemblyPath, "Loading module scope...", true);
			ModuleScope = new NuGenModuleScope(Import, this);
			ModuleScope.EnumerateTypeDefinitions(Import, debuggedModule);
			NuGenUIHandler.Instance.StepProgressBar(1);
			NuGenUIHandler.Instance.SetProgressText(assemblyPath, "Loading global type...", true);
			GlobalType = new NuGenTypeDefinition(Import, ModuleScope, 0);
			NuGenUIHandler.Instance.StepProgressBar(1);
			AllTokens[0] = GlobalType;

			NuGenUIHandler.Instance.SetProgressText(assemblyPath, "Associating properties with methods...", true);
			AssociatePropertiesWithMethods();
			NuGenUIHandler.Instance.StepProgressBar(1);
			NuGenUIHandler.Instance.SetProgressText(assemblyPath, "Reading assembly properties...", true);

			DisplayInTree = false;
			try
			{
				ReadProperties();
				DisplayInTree = true;
			}
			catch (COMException comException)
			{
				unchecked
				{
					if (comException.ErrorCode != (int)0x80131130)
					{
						throw;
					}
				}
			}

			NuGenUIHandler.Instance.StepProgressBar(1);
			NuGenUIHandler.Instance.SetProgressText(assemblyPath, "Loading resolving resolution scopes...", true);
			ResolveResolutionScopes();
			NuGenUIHandler.Instance.StepProgressBar(1);
			NuGenUIHandler.Instance.SetProgressText(assemblyPath, "Searching for entry method...", true);
			SearchEntryPoint();
			NuGenUIHandler.Instance.StepProgressBar(1);
		}

		private void AssociatePropertyWithMethod(NuGenProperty property, uint methodToken)
		{
			if (AllTokens.ContainsKey(methodToken))
			{
				NuGenMethodDefinition methodDefinition = (NuGenMethodDefinition)AllTokens[methodToken];
				methodDefinition.OwnerProperty = property;
			}
		}

		private void AssociatePropertiesWithMethods()
		{
			foreach (NuGenTokenBase tokenObject in AllTokens.Values)
			{
				NuGenProperty property = tokenObject as NuGenProperty;

				if (property != null)
				{
					AssociatePropertyWithMethod(property, property.GetterMethodToken);
					AssociatePropertyWithMethod(property, property.SetterMethodToken);

					for (int index = 0; index < property.OtherMethodsCount; index++)
					{
						uint methodToken = property.OtherMethods[index];
						AssociatePropertyWithMethod(property, methodToken);
					}
				}
			}
		}

		private void GetModuleReferences()
		{
			IntPtr enumHandle = IntPtr.Zero;
			uint[] moduleRefs = new uint[NuGenProject.DefaultArrayCount];
			uint count = 0;
			Import.EnumModuleRefs(ref enumHandle, moduleRefs, Convert.ToUInt32(moduleRefs.Length), out count);

			if (count > 0)
			{
				ModuleReferences = new List<NuGenModuleReference>();
			}

			while (count > 0)
			{
				for (uint moduleRefsIndex = 0; moduleRefsIndex < count; moduleRefsIndex++)
				{
					uint token = moduleRefs[moduleRefsIndex];
					uint moduleRefNameLength;

					Import.GetModuleRefProps(token, NuGenProject.DefaultCharArray, Convert.ToUInt32(NuGenProject.DefaultCharArray.Length), out moduleRefNameLength);

					if (moduleRefNameLength > NuGenProject.DefaultCharArray.Length)
					{
						NuGenProject.DefaultCharArray = new char[moduleRefNameLength];

						Import.GetModuleRefProps(token, NuGenProject.DefaultCharArray, Convert.ToUInt32(NuGenProject.DefaultCharArray.Length), out moduleRefNameLength);
					}

					NuGenModuleReference moduleReference = new NuGenModuleReference(this, token, NuGenHelperFunctions.GetString(NuGenProject.DefaultCharArray, 0, moduleRefNameLength));
					ModuleReferences.Add(moduleReference);
					AllTokens[token] = moduleReference;
				}

				Import.EnumModuleRefs(ref enumHandle, moduleRefs, Convert.ToUInt32(moduleRefs.Length), out count);
			}

			Import.CloseEnum(enumHandle);
		}

		private void GetTypeReferences()
		{
			IntPtr enumHandle = IntPtr.Zero;
			uint[] typeRefs = new uint[NuGenProject.DefaultArrayCount];
			uint count = 0;
			Import.EnumTypeRefs(ref enumHandle, typeRefs, Convert.ToUInt32(typeRefs.Length), out count);

			if (count > 0)
			{
				TypeReferences = new List<NuGenTypeReference>();
			}

			while (count > 0)
			{
				for (uint typeRefsIndex = 0; typeRefsIndex < count; typeRefsIndex++)
				{
					uint token = typeRefs[typeRefsIndex];
					uint typeRefNameLength;
					uint resolutionScope;

					Import.GetTypeRefProps(token, out resolutionScope, NuGenProject.DefaultCharArray, Convert.ToUInt32(NuGenProject.DefaultCharArray.Length), out typeRefNameLength);

					if (typeRefNameLength > NuGenProject.DefaultCharArray.Length)
					{
						NuGenProject.DefaultCharArray = new char[typeRefNameLength];

						Import.GetTypeRefProps(token, out resolutionScope, NuGenProject.DefaultCharArray, Convert.ToUInt32(NuGenProject.DefaultCharArray.Length), out typeRefNameLength);
					}

					NuGenTypeReference typeReference = new NuGenTypeReference(Import, this, NuGenHelperFunctions.GetString(NuGenProject.DefaultCharArray, 0, typeRefNameLength), token, resolutionScope);
					TypeReferences.Add(typeReference);
					AllTokens[token] = typeReference;
				}

				Import.EnumTypeRefs(ref enumHandle, typeRefs, Convert.ToUInt32(typeRefs.Length), out count);
			}

			Import.CloseEnum(enumHandle);
		}

		public void GetUserStrings()
		{
			IntPtr enumHandle = IntPtr.Zero;
			uint[] rStrings = new uint[NuGenProject.DefaultArrayCount];
			uint count = 0;
			Import.EnumUserStrings(ref enumHandle, rStrings, Convert.ToUInt32(rStrings.Length), out count);

			if (count > 0)
			{
				UserStrings = new Dictionary<uint, NuGenUserString>();
			}

			while (count > 0)
			{
				for (uint rStringsIndex = 0; rStringsIndex < count; rStringsIndex++)
				{
					uint token = rStrings[rStringsIndex];
					uint userStringLength;

					Import.GetUserString(token, NuGenProject.DefaultCharArray, Convert.ToUInt32(NuGenProject.DefaultCharArray.Length), out userStringLength);

					if (userStringLength > NuGenProject.DefaultCharArray.Length)
					{
						NuGenProject.DefaultCharArray = new char[userStringLength];

						Import.GetUserString(token, NuGenProject.DefaultCharArray, Convert.ToUInt32(NuGenProject.DefaultCharArray.Length), out userStringLength);
					}

					NuGenUserString userStringObject = new NuGenUserString(token, NuGenHelperFunctions.GetString(NuGenProject.DefaultCharArray, 0, userStringLength));
					UserStrings[token] = userStringObject;
					AllTokens[token] = userStringObject;
				}

				Import.EnumUserStrings(ref enumHandle, rStrings, Convert.ToUInt32(rStrings.Length), out count);
			}

			Import.CloseEnum(enumHandle);
		}

		private void GetTypeSpecs()
		{
			IntPtr enumHandle = IntPtr.Zero;
			uint[] typeSpecs = new uint[NuGenProject.DefaultArrayCount];
			uint count = 0;
			Import.EnumTypeSpecs(ref enumHandle, typeSpecs, Convert.ToUInt32(typeSpecs.Length), out count);

			while (count > 0)
			{
				for (uint typeSpecsIndex = 0; typeSpecsIndex < count; typeSpecsIndex++)
				{
					uint token = typeSpecs[typeSpecsIndex];
					IntPtr signature;
					uint signatureLength;

					Import.GetTypeSpecFromToken(token, out signature, out signatureLength);
					NuGenTypeSpecification typeSpecification = new NuGenTypeSpecification(this, token, signature, signatureLength);
					AllTokens[token] = typeSpecification;
				}

				Import.EnumTypeSpecs(ref enumHandle, typeSpecs, Convert.ToUInt32(typeSpecs.Length), out count);
			}

			Import.CloseEnum(enumHandle);
		}

		private void GetSignatures()
		{
			IntPtr enumHandle = IntPtr.Zero;
			uint[] signatures = new uint[NuGenProject.DefaultArrayCount];
			uint count = 0;
			Import.EnumSignatures(ref enumHandle, signatures, Convert.ToUInt32(signatures.Length), out count);

			if (count > 0)
			{
				StandAloneSignatures = new Dictionary<uint, NuGenStandAloneSignature>();
			}

			while (count > 0)
			{
				for (uint signaturesIndex = 0; signaturesIndex < count; signaturesIndex++)
				{
					uint token = signatures[signaturesIndex];
					IntPtr signature;
					uint signatureLength;

					Import.GetSigFromToken(token, out signature, out signatureLength);
					NuGenStandAloneSignature standAloneSignature = new NuGenStandAloneSignature(this, token, signature, signatureLength);
					StandAloneSignatures[token] = standAloneSignature;
					AllTokens[token] = standAloneSignature;
				}

				Import.EnumSignatures(ref enumHandle, signatures, Convert.ToUInt32(signatures.Length), out count);
			}

			Import.CloseEnum(enumHandle);
		}

		private void GetManifestResources()
		{
			IntPtr enumHandle = IntPtr.Zero;
			uint[] manifestResources = new uint[NuGenProject.DefaultArrayCount];
			uint count = 0;
			AssemblyImport.EnumManifestResources(ref enumHandle, manifestResources, Convert.ToUInt32(manifestResources.Length), out count);

			if (count > 0)
			{
				ManifestResources = new List<NuGenManifestResource>();
			}

			while (count > 0)
			{
				for (uint index = 0; index < count; index++)
				{
					uint token = manifestResources[index];
					uint nameCount;
					uint providerToken;
					uint offset;
					uint flags;

					AssemblyImport.GetManifestResourceProps(token, NuGenProject.DefaultCharArray, Convert.ToUInt32(NuGenProject.DefaultCharArray.Length), out nameCount, out providerToken, out offset, out flags);

					if (nameCount > NuGenProject.DefaultCharArray.Length)
					{
						NuGenProject.DefaultCharArray = new char[nameCount];

						AssemblyImport.GetManifestResourceProps(token, NuGenProject.DefaultCharArray, Convert.ToUInt32(NuGenProject.DefaultCharArray.Length), out nameCount, out providerToken, out offset, out flags);
					}

					NuGenManifestResource manifestResource = new NuGenManifestResource(this, token, NuGenHelperFunctions.GetString(NuGenProject.DefaultCharArray, 0, nameCount), providerToken, offset, flags);
					ManifestResources.Add(manifestResource);
					AllTokens[token] = manifestResource;
				}

				AssemblyImport.EnumManifestResources(ref enumHandle, manifestResources, Convert.ToUInt32(manifestResources.Length), out count);
			}

			AssemblyImport.CloseEnum(enumHandle);
		}

		private void GetFiles()
		{
			IntPtr enumHandle = IntPtr.Zero;
			uint[] files = new uint[NuGenProject.DefaultArrayCount];
			uint count = 0;
			AssemblyImport.EnumFiles(ref enumHandle, files, Convert.ToUInt32(files.Length), out count);

			if (count > 0)
			{
				Files = new List<NuGenFile>();
			}

			while (count > 0)
			{
				for (uint index = 0; index < count; index++)
				{
					uint token = files[index];
					uint nameCount;
					IntPtr hash;
					uint hashLength;
					uint flags;

					AssemblyImport.GetFileProps(token, NuGenProject.DefaultCharArray, Convert.ToUInt32(NuGenProject.DefaultCharArray.Length), out nameCount, out hash, out hashLength, out flags);

					if (nameCount > NuGenProject.DefaultCharArray.Length)
					{
						NuGenProject.DefaultCharArray = new char[nameCount];

						AssemblyImport.GetFileProps(token, NuGenProject.DefaultCharArray, Convert.ToUInt32(name.Length), out nameCount, out hash, out hashLength, out flags);
					}

					NuGenFile file = new NuGenFile(this, token, NuGenHelperFunctions.GetString(NuGenProject.DefaultCharArray, 0, nameCount), hash, hashLength, flags);
					Files.Add(file);
					AllTokens[token] = file;
				}

				AssemblyImport.EnumFiles(ref enumHandle, files, Convert.ToUInt32(files.Length), out count);
			}

			AssemblyImport.CloseEnum(enumHandle);
		}

		private void ReadHeader()
		{
			AssemblyReader.BaseStream.Position = 0;
			byte[] dosHeader = AssemblyReader.ReadBytes(128);

			if (dosHeader[0] != 0x4d || dosHeader[1] != 0x5a)
			{
				throw new InvalidProgramException("The assembly doesn't contain a valid DOS header.");
			}

			uint lfanew = BitConverter.ToUInt32(dosHeader, 0x3c);
			if (lfanew == 0)
			{
				lfanew = 128;
			}

			AssemblyReader.BaseStream.Seek(lfanew, SeekOrigin.Begin);
			byte[] peSignature = AssemblyReader.ReadBytes(24);

			if (peSignature[0] != 0x50 || peSignature[1] != 0x45)
			{
				throw new InvalidProgramException("The assembly doesn't contain a valid PE signature.");
			}

			int numberOfSections = BitConverter.ToUInt16(peSignature, 6);

			AssemblyReader.BaseStream.Seek(lfanew + 24, SeekOrigin.Begin);
			IsPe32 = (AssemblyReader.ReadUInt16() == 0x010b);

			if (IsPe32)
			{
				AssemblyReader.BaseStream.Seek(lfanew + 24 + 224, SeekOrigin.Begin);
			}
			else
			{
				AssemblyReader.BaseStream.Seek(lfanew + 24 + 240, SeekOrigin.Begin);
			}

			for (int sectionIndex = 0; sectionIndex < numberOfSections; sectionIndex++)
			{
				NuGenSectionHeader sectionHeader = new NuGenSectionHeader();

				AssemblyReader.BaseStream.Seek(12, SeekOrigin.Current);
				sectionHeader.VirtualAddress = AssemblyReader.ReadUInt32();
				sectionHeader.SizeOfRawData = AssemblyReader.ReadUInt32();
				sectionHeader.PointerToRawData = AssemblyReader.ReadUInt32();
				AssemblyReader.BaseStream.Seek(16, SeekOrigin.Current);

				SectionHeaders.Add(sectionHeader);
			}

			if (IsPe32)
			{
				AssemblyReader.BaseStream.Seek(lfanew + 24 + 28, SeekOrigin.Begin);
			}
			else
			{
				AssemblyReader.BaseStream.Seek(lfanew + 24 + 24, SeekOrigin.Begin);
			}

			ImageBase = AssemblyReader.ReadUInt32();

			if (IsPe32)
			{
				AssemblyReader.ReadUInt32();
			}
			else
			{
				AssemblyReader.ReadUInt32();
				AssemblyReader.ReadUInt32();
			}

			FileAlignment = AssemblyReader.ReadUInt32();

			AssemblyReader.BaseStream.Seek(lfanew + 24 + 68, SeekOrigin.Begin);
			Subsystem = AssemblyReader.ReadUInt16();
			AssemblyReader.ReadUInt16();

			if (IsPe32)
			{
				StackReserve = AssemblyReader.ReadUInt32();
			}
			else
			{
				StackReserve = AssemblyReader.ReadUInt64();
			}

			if (IsPe32)
			{
				AssemblyReader.BaseStream.Seek(lfanew + 24 + 208, SeekOrigin.Begin);
			}
			else
			{
				AssemblyReader.BaseStream.Seek(lfanew + 24 + 224, SeekOrigin.Begin);
			}

			uint cliHeaderAddress = GetMethodAddress(AssemblyReader.ReadUInt32());
			uint cliHeaderSize = AssemblyReader.ReadUInt32();

			AssemblyReader.BaseStream.Seek(cliHeaderAddress + 8, SeekOrigin.Begin);
			uint metadataRootRva = AssemblyReader.ReadUInt32();
			AssemblyReader.BaseStream.Seek(4, SeekOrigin.Current);
			CorFlags = AssemblyReader.ReadUInt32();
			EntryPointToken = AssemblyReader.ReadUInt32();

			AssemblyReader.BaseStream.Seek(GetMethodAddress(metadataRootRva) + 12, SeekOrigin.Begin);
			int frameworkVersionStringLength = AssemblyReader.ReadInt32();
			char[] frameworkVersionString = AssemblyReader.ReadChars(frameworkVersionStringLength);
			FrameworkVersion = NuGenHelperFunctions.TrimString(frameworkVersionString);
		}

		public uint GetMethodAddress(uint rva)
		{
			uint result = 0;
			bool found = false;
			int index = 0;

			while (!found && index < SectionHeaders.Count)
			{
				NuGenSectionHeader sectionHeader = SectionHeaders[index++];

				if (sectionHeader.ContainsRvaAddress(rva))
				{
					found = true;
					result = rva - sectionHeader.VirtualAddress + sectionHeader.PointerToRawData;
				}
			}

			if (!found)
			{
				throw new InvalidOperationException("The given RVA does not seem to belong to any section. Perhaps this is not a valid .NET assembly.");
			}

			return result;
		}

		private void SearchEntryPoint()
		{
			if (AllTokens.ContainsKey(EntryPointToken))
			{
				NuGenTokenBase token = AllTokens[EntryPointToken];

				if (token is NuGenMethodDefinition)
				{
					((NuGenMethodDefinition)token).EntryPoint = true;
				}
			}
		}

		private void OpenAssemblyRefs()
		{
			IntPtr enumHandle = IntPtr.Zero;
			uint[] assemblyRefs = new uint[NuGenProject.DefaultArrayCount];
			uint count = 0;
			AssemblyImport.EnumAssemblyRefs(ref enumHandle, assemblyRefs, Convert.ToUInt32(assemblyRefs.Length), out count);

			uint systemDirectoryLength;
			Dispenser.GetCORSystemDirectory(NuGenProject.DefaultCharArray, Convert.ToUInt32(NuGenProject.DefaultCharArray.LongLength), out systemDirectoryLength);
			string systemDirectoryPath = NuGenHelperFunctions.GetString(NuGenProject.DefaultCharArray, 0, systemDirectoryLength);
			systemDirectoryPath = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(systemDirectoryPath)), FrameworkVersion) + "\\";
			string assemblyDirectoryPath = string.Format("{0}\\", Path.GetDirectoryName(FullPath));

			if (count > 0)
			{
				AssemblyReferences = new Dictionary<uint, NuGenAssemblyReference>();
			}

			while (count > 0)
			{
				for (uint assemblyRefsIndex = 0; assemblyRefsIndex < count; assemblyRefsIndex++)
				{
					uint token = assemblyRefs[assemblyRefsIndex];
					IntPtr PublicKeyOrToken;
					uint PublicKeyOrTokenLength;
					uint nameStringLength;
					NuGenAssemblyMetadata metadata = new NuGenAssemblyMetadata();
					IntPtr hashBlob;
					uint hashBlobLength;
					uint assemblyFlags;

					AssemblyImport.GetAssemblyRefProps(token, out PublicKeyOrToken, out PublicKeyOrTokenLength, NuGenProject.DefaultCharArray, Convert.ToUInt32(NuGenProject.DefaultCharArray.LongLength), out nameStringLength, ref metadata, out hashBlob, out hashBlobLength, out assemblyFlags);

					if (nameStringLength > NuGenProject.DefaultCharArray.Length)
					{
						NuGenProject.DefaultCharArray = new char[nameStringLength];

						AssemblyImport.GetAssemblyRefProps(token, out PublicKeyOrToken, out PublicKeyOrTokenLength, NuGenProject.DefaultCharArray, Convert.ToUInt32(NuGenProject.DefaultCharArray.LongLength), out nameStringLength, ref metadata, out hashBlob, out hashBlobLength, out assemblyFlags);
					}

					NuGenAssemblyReference assemblyRef = new NuGenAssemblyReference(this, token, NuGenHelperFunctions.GetString(NuGenProject.DefaultCharArray, 0, nameStringLength), PublicKeyOrToken, PublicKeyOrTokenLength, metadata, hashBlob, hashBlobLength, assemblyFlags);

					string exeAssemblyPath = string.Format("{0}.exe", assemblyRef.Name);
					string dllAssemblyPath = string.Format("{0}.dll", assemblyRef.Name);
					string exeAssemblyRefPath = Path.Combine(assemblyDirectoryPath, exeAssemblyPath);
					string dllAssemblyRefPath = Path.Combine(assemblyDirectoryPath, dllAssemblyPath);
					string dllSystemPath = Path.Combine(systemDirectoryPath, dllAssemblyPath);
					string exeSystemPath = Path.Combine(systemDirectoryPath, dllAssemblyPath);

					if (System.IO.File.Exists(dllAssemblyRefPath))
					{
						assemblyRef.FileName = dllAssemblyPath;
						assemblyRef.FullPath = dllAssemblyRefPath;
					}
					else if (System.IO.File.Exists(exeAssemblyRefPath))
					{
						assemblyRef.FileName = exeAssemblyPath;
						assemblyRef.FullPath = exeAssemblyRefPath;
					}
					else if (System.IO.File.Exists(dllSystemPath))
					{
						assemblyRef.FileName = dllAssemblyPath;
						assemblyRef.FullPath = dllSystemPath;
					}
					else if (System.IO.File.Exists(exeSystemPath))
					{
						assemblyRef.FileName = dllAssemblyPath;
						assemblyRef.FullPath = exeSystemPath;
					}
					else
					{
						AssemblyName assemblyName = new AssemblyName();
						assemblyName.Name = assemblyRef.Name;
						assemblyName.Version = new Version(assemblyRef.Metadata.usMajorVersion, assemblyRef.Metadata.usMinorVersion, assemblyRef.Metadata.usBuildNumber, assemblyRef.Metadata.usRevisionNumber);

						if (assemblyRef.Metadata.szLocale != IntPtr.Zero)
						{
							throw new NotImplementedException("The assembly local is different from default.");
						}

						assemblyName.CultureInfo = CultureInfo.InvariantCulture;

						byte[] publicKey = NuGenHelperFunctions.ReadBlobAsByteArray(assemblyRef.PublicKeyOrToken, assemblyRef.PublicKeyOrTokenLength);

						if ((assemblyRef.Flags & CorAssemblyFlags.afPublicKey) == CorAssemblyFlags.afPublicKey)
						{
							assemblyName.SetPublicKey(publicKey);
						}
						else
						{
							assemblyName.SetPublicKeyToken(publicKey);
						}

						try
						{
							System.Reflection.Assembly referencedAssembly = System.Reflection.Assembly.Load(assemblyName);

							assemblyRef.FullPath = Path.GetFullPath(new Uri(referencedAssembly.CodeBase).AbsolutePath);
							assemblyRef.FileName = Path.GetFileName(assemblyRef.FullPath);
						}
						catch
						{
						}
					}

					AssemblyReferences[assemblyRef.Token] = assemblyRef;
					AllTokens[assemblyRef.Token] = assemblyRef;
				}

				AssemblyImport.EnumAssemblyRefs(ref enumHandle, assemblyRefs, Convert.ToUInt32(assemblyRefs.Length), out count);
			}

			AssemblyImport.CloseEnum(enumHandle);
		}

		private void FindReferencedAssembly(NuGenTypeReference typeReference, uint resolutionScope, bool recursiveCall)
		{
			NuGenTokenBase tokenObject = AllTokens[resolutionScope];

			if (tokenObject is NuGenTypeReference)
			{
				NuGenTypeReference typeReferenceToken = (NuGenTypeReference)tokenObject;

				FindReferencedAssembly(typeReference, typeReferenceToken.ResolutionScope, true);
				typeReference.FullName = string.Format("{0}/{1}", typeReferenceToken.FullName, typeReference.Name);
			}
			else if (tokenObject is NuGenModuleScope)
			{
				typeReference.FullName = typeReference.Name;
			}
			else
			{
				typeReference.ReferencedAssembly = tokenObject.Name;

				if (recursiveCall)
				{
					typeReference.FullName = string.Format("[{0}]{1}", typeReference.ReferencedAssembly, tokenObject.Name);
				}
				else
				{
					typeReference.FullName = string.Format("[{0}]{1}", typeReference.ReferencedAssembly, typeReference.Name);
				}
			}
		}

		private void ResolveResolutionScopes()
		{
			if (TypeReferences != null)
			{
				foreach (NuGenTypeReference typeReference in TypeReferences)
				{
					if (AllTokens.ContainsKey(typeReference.ResolutionScope))
					{
						FindReferencedAssembly(typeReference, typeReference.ResolutionScope, false);
					}
				}
			}
		}

		private void ReadProperties()
		{
			uint token;
			AssemblyImport.GetAssemblyFromScope(out token);
			Token = token;
			IntPtr publicKey;
			uint publicKeyLength;
			uint hashAlgorithmId;
			uint nameLength;
			NuGenAssemblyMetadata metadata = new NuGenAssemblyMetadata();
			uint flags;
			AssemblyImport.GetAssemblyProps(Token, out publicKey, out publicKeyLength, out hashAlgorithmId, NuGenProject.DefaultCharArray, Convert.ToUInt32(NuGenProject.DefaultCharArray.Length), out nameLength, ref metadata, out flags);

			if (nameLength > NuGenProject.DefaultCharArray.Length)
			{
				NuGenProject.DefaultCharArray = new char[nameLength];

				AssemblyImport.GetAssemblyProps(Token, out publicKey, out publicKeyLength, out hashAlgorithmId, NuGenProject.DefaultCharArray, Convert.ToUInt32(NuGenProject.DefaultCharArray.Length), out nameLength, ref metadata, out flags);
			}

			PublicKey = publicKey;
			PublicKeyLength = publicKeyLength;
			HashAlgorithmID = hashAlgorithmId;
			Name = NuGenHelperFunctions.GetString(NuGenProject.DefaultCharArray, 0, nameLength);
			Metadata = metadata;
			Flags = (CorAssemblyFlags)flags;

			if (Flags != CorAssemblyFlags.afPublicKey && Flags != CorAssemblyFlags.afLongevityUnspecified)
			{
				throw new NotImplementedException("Unknown assembly flag value.");
			}

			AssemblyCustomAttributes = NuGenHelperFunctions.EnumCustomAttributes(Import, this, this);
			PermissionSets = NuGenHelperFunctions.EnumPermissionSets(Import, Token);
		}

		public void Initialize()
		{
			CodeLines = new List<NuGenCodeLine>();
			CodeLines.Add(new NuGenCodeLine(0, ".assembly " + Name));
			CodeLines.Add(new NuGenCodeLine(0, "{"));
			CodeLines.Add(new NuGenCodeLine(1, "//Full Path: " + FullPath));
			CodeLines.Add(new NuGenCodeLine(1, "//Metadata version: " + FrameworkVersion));

			if (AssemblyCustomAttributes != null)
			{
				foreach (NuGenCustomAttribute customAttribute in AssemblyCustomAttributes)
				{
					customAttribute.SetText(AllTokens);
					CodeLines.Add(new NuGenCodeLine(1, customAttribute.Name));
				}

				CodeLines.Add(new NuGenCodeLine(1, string.Empty));
			}

			if (PermissionSets != null)
			{
				foreach (NuGenPermissionSet permissionSet in PermissionSets)
				{
					CodeLines.Add(new NuGenCodeLine(1, permissionSet.Name));
				}

				CodeLines.Add(new NuGenCodeLine(1, string.Empty));
			}

			if (PublicKeyLength > 0)
			{
				CodeLines.Add(new NuGenCodeLine(1, ".publickey = " + NuGenHelperFunctions.ReadBlobAsString(PublicKey, PublicKeyLength)));
			}

			CodeLines.Add(new NuGenCodeLine(1, ".hash algorithm 0x" + NuGenHelperFunctions.FormatAsHexNumber(HashAlgorithmID, 8)));
			CodeLines.Add(new NuGenCodeLine(1, string.Format(".ver {0}:{1}:{2}:{3}", Metadata.usMajorVersion, Metadata.usMinorVersion, Metadata.usBuildNumber, Metadata.usRevisionNumber)));

			CodeLines.Add(new NuGenCodeLine(0, "} // end of assembly " + FileName));
		}
	}
}