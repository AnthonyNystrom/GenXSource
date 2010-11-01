using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble;
using System.Windows.Forms;

namespace Dile.UI
{
	public static class NuGenTreeViewSearcher
	{
		private static TreeNode FindNodeByName(TreeNodeCollection nodes, string text)
		{
			TreeNode result = null;
			int index = 0;

			while (result == null && index < nodes.Count)
			{
				TreeNode node = nodes[index++];

				if (node.Text == text)
				{
					result = node;
				}
			}

			return result;
		}

		private static TreeNode SearchTokenNode(TreeNode parentNode, NuGenTokenBase parentTokenObject, string subNodeText, string nodeText)
		{
			TreeNode result = SearchNodes(parentNode, parentTokenObject);

			if (subNodeText != null && result != null)
			{
				result.Expand();
				result = FindNodeByName(result.Nodes, subNodeText);
			}

			if (result != null)
			{
				result.Expand();
				result = FindNodeByName(result.Nodes, nodeText);
			}

			return result;
		}

		private static TreeNode SearchNodes(TreeNode parentNode, NuGenTokenBase tokenObject)
		{
			TreeNode result = null;

			if (parentNode != null)
			{
				switch (tokenObject.ItemType)
				{
					case SearchOptions.Assembly:
						NuGenAssembly assembly = (NuGenAssembly)tokenObject;
						parentNode.Expand();
						result = FindNodeByName(parentNode.Nodes, assembly.FileName);
						break;

					case SearchOptions.AssemblyReference:
						NuGenAssemblyReference assemblyReference = (NuGenAssemblyReference)tokenObject;

						result = SearchTokenNode(parentNode, assemblyReference.Assembly, " References", assemblyReference.Name);
						break;

					case SearchOptions.FieldDefintion:
						NuGenFieldDefinition fieldDefinition = (NuGenFieldDefinition)tokenObject;

						result = SearchTokenNode(parentNode, fieldDefinition.BaseTypeDefinition, "Fields", fieldDefinition.Name);
						break;

					case SearchOptions.File:
						NuGenFile file = (NuGenFile)tokenObject;

						result = SearchTokenNode(parentNode, file.Assembly, " Files", file.Name);
						break;

					case SearchOptions.ManifestResource:
						NuGenManifestResource manifestResource = (NuGenManifestResource)tokenObject;

						result = SearchTokenNode(parentNode, manifestResource.Assembly, " Manifest Resources", manifestResource.Name);
						break;

					case SearchOptions.MethodDefinition:
						NuGenMethodDefinition methodDefinition = (NuGenMethodDefinition)tokenObject;

						if (methodDefinition.OwnerProperty == null)
						{
							result = SearchTokenNode(parentNode, methodDefinition.BaseTypeDefinition, "Methods", methodDefinition.DisplayName);
						}
						else
						{
							result = SearchNodes(parentNode, methodDefinition.OwnerProperty);
							result.Expand();
							result = FindNodeByName(result.Nodes, methodDefinition.DisplayName);
						}
						break;

					case SearchOptions.ModuleReference:
						NuGenModuleReference moduleReference = (NuGenModuleReference)tokenObject;

						result = SearchTokenNode(parentNode, moduleReference.Assembly, " Module References", moduleReference.Name);
						break;

					case SearchOptions.ModuleScope:
						NuGenModuleScope moduleScope = (NuGenModuleScope)tokenObject;

						result = SearchTokenNode(parentNode, moduleScope.Assembly, null, moduleScope.Name);
						break;

					case SearchOptions.Property:
						NuGenProperty property = (NuGenProperty)tokenObject;

						result = SearchTokenNode(parentNode, property.BaseTypeDefinition, "Properties", property.Name);
						break;

					case SearchOptions.TypeDefinition:
						NuGenTypeDefinition typeDefinition = (NuGenTypeDefinition)tokenObject;
						string typeNamespace = typeDefinition.Namespace;

						if (typeNamespace.Length == 0)
						{
							typeNamespace = NuGenConstants.DefaultNamespaceName;
						}

						result = SearchTokenNode(parentNode, typeDefinition.ModuleScope, typeNamespace, typeDefinition.FullName);
						break;
				}
			}

			return result;
		}

		public static TreeNode LocateNode(TreeNode startingNode, NuGenTokenBase tokenObject)
		{
			return SearchNodes(startingNode, tokenObject);
		}
	}
}