/***
 * 
 *  ASMEX by RiskCare Ltd.
 * 
 * This source is copyright (C) 2002 RiskCare Ltd. All rights reserved.
 * 
 * Disclaimer:
 * This code is provided 'as is', with absolutely no warranty expressed or
 * implied.  Any use of this code is at your own risk.
 *   
 * You are hereby granted the right to redistribute this source unmodified
 * in its original archive. 
 * You are hereby granted the right to use this code, or code based on it,
 * provided that you acknowledge RiskCare Ltd somewhere in the documentation
 * of your application. 
 * You are hereby granted the right to distribute changes to this source, 
 * provided that:
 * 
 * 1 -- This copyright notice is retained unchanged 
 * 2 -- Your changes are clearly marked 
 * 
 * Enjoy!
 * 
 * --------------------------------------------------------------------
 * 
 * If you use this code or have comments on it, please mail me at 
 * support@jbrowse.com or ben.peterson@riskcare.com
 * 
 */

using System;
using System.Windows.Forms;
using System.Reflection;
using System.Resources;
using System.IO;
using System.Collections;

namespace Genetibase.Debug
{
	/// <summary>
	/// This class is the base for nodes in the tree.  My, isn't this sort of thing a lot easier than
	/// it was in MFC?
	/// </summary>
	public class BaseNode : TreeNode
	{
		protected const int IconAssembly = 0;
		protected const int IconHeader = 1;
		protected const int IconData = 2;
		protected const int IconModule = 3;
		protected const int IconFolder = 4;
		protected const int IconTable = 5;
		protected const int IconManRes = 6;
		protected const int IconManResx = 7;
		protected const int IconRes = 8;
		protected const int IconAssemblyRefs = 9;
		protected const int IconAssemblyRef = 10;
		protected const int IconError = 12;
		protected const int IconNamespace = 11;
		protected const int IconHtml = 2;
		protected const int IconClass = 13;
		protected const int IconIface = IconClass + 4;
		protected const int IconStruct = IconIface + 4;
		protected const int IconEnum = IconStruct + 4;
		protected const int IconDel = IconEnum + 4;
		protected const int IconParam = IconDel + 4;
		protected const int IconMethod = IconParam + 8;
		protected const int IconEvent = IconMethod + 8;
		protected const int IconProp = IconEvent + 1;
		protected const int IconPropWO = IconProp + 8;
		protected const int IconPropRO = IconPropWO + 8;
		protected const int IconField = IconPropRO + 8;
		protected const int IconConst = IconField + 8;
		protected const int IconParent = 86;
		protected const int IconChild = 87;

		protected object _data;
		protected bool _spinster;
		protected string _desc;

		protected BaseNode()
		{
			PrepareToGenerateChildren();
		}
		
		/// <summary>
		/// True if the node should have children, but hasn't generated any yet
		/// </summary>
		public virtual bool Spinster
		{
			get{return _spinster;}
		}

		/// <summary>
		/// True if the node's contents should not be displayed in ObjViewer
		/// </summary>
		public virtual bool Uninteresting
		{
			get{return false;}
		}

		/// <summary>
		/// Long desc of node, for ObjViewer
		/// </summary>
		public virtual string Desc
		{
			get{return _desc;}
		}


		/// <summary>
		/// A data item of any type associated with the node
		/// </summary>
		public virtual object Data
		{
			get{return _data;}
		}

		/// <summary>
		/// The 'kind' of node, for ObjViewer
		/// </summary>
		public virtual string NodeLabel{get{return "???";}}

		/// <summary>
		/// If search operations should match something other than the Text property, override this.
		/// </summary>
		public virtual string MatchingString{get{return Text;}}

		/// <summary>
		/// If the node can be root... in fact, there is no reason Type nodes etc should not be roots.
		/// </summary>
		public virtual bool CanBeRoot
		{
			get{return false;}
		}


		/// <summary>
		/// An array of menu items for the node's context menu.  It is up to the node to provide handlers
		/// </summary>
		/// <returns></returns>
		public virtual MenuItem[] GetMenu()
		{
			return null;
		}
		
		/// <summary>
		/// Generate nodes below this one when the node is expanded
		/// </summary>
		public virtual void GenerateChildren()
		{
			Nodes.Clear();
			
			_spinster = false;
		}

		/// <summary>
		/// Clear child nodes
		/// </summary>
		public virtual void PrepareToGenerateChildren()
		{
			Nodes.Clear();
			_spinster = true;
			Nodes.Add("dummy");
		}


		/// <summary>
		/// Given an object, make a suitable node to wrap it.
		/// This should be in a separate nodefactory class.
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static BaseNode MakeNode(object o)
		{
			if (o is Assembly)
			{
				return new AsmNode((Assembly)o);
			}

			if (o is Module)
			{
				return new ModuleNode(new AModule((Module)o));
			}

			if (o is string)
			{
				try
				{
					Assembly asm = Assembly.Load((string)o);
					return new AsmNode(asm);
				}
				catch{}

				if (File.Exists((string)o))
				{
					return new ModuleNode(new AModule((string)o));
				}

			}

			return new ErrorNode("Can't make a useful object out of: " + o.ToString());
		}

		//below here are utility functions that should be in child classes but are here instead.

		public static string Assembly2String(Assembly asm, bool bShort)
		{
			return asm.FullName;
		}

		public static string AsmName2String(AssemblyName an, bool bShort)
		{
			if (bShort)
			{
				return an.Name + " " + an.Version.ToString();
			}


			return an.FullName;
		}

		
		public static string Object2String(object o, bool bShort)
		{
			if (bShort)
			{
				return o.ToString();
			}

			string tname = o.GetType().FullName;
			string obval = o.ToString();

			if (tname == obval) return tname;

			return tname + " :: " + obval;
		}
		
	
		public static string Type2String(Type type, bool bShort)
		{
			string s = "";

			if (bShort)
			{
				return type.Name;
			}

			if (type.IsPrimitive)
				s += "[primitive] ";

			if (type.IsValueType)
				s += "[value] ";

			if (type.IsByRef)
				s += "[byref] ";

			if (type.IsArray)
				s += "[array] ";

			if (type.IsPublic)
				s += "public ";

			if (type.IsAbstract)
				s += "abstract ";

			if (type.IsSealed)
				s += "sealed ";

			if (type.IsNestedAssembly)
				s += "internal ";

			if (type.IsClass)
			{
				s += "class ";
			}
			else if (type.IsEnum)
			{
				s += "enum ";
			}
			else if (type.IsInterface)
			{
				s += "interface ";
			}


			s += type.FullName;


			return s;

		}
		
		public static string ManRes2String(AManifestResource amr, bool bShort)
		{
			if (bShort)
			{
				return amr.Name;
			}

			string s = amr.Name;

			s += "(" + amr.ResourceLocation + ")";

			return s;
		}

		
		public static string Method2String(MethodBase meth, bool bShort)
		{
			string s = "";
			int i;

			if (!bShort)
			{
				if (meth.IsStatic)
				{
					s += "static ";
				}

				if (meth.IsPrivate)
				{
					s += "private ";
				}

				if (meth.IsPublic)
				{
					s += "public ";
				}

				if (meth.IsFamily)
				{
					s += "protected ";
				}

				if (meth.IsAbstract)
				{
					s += "abstract ";
				}

				if (meth.IsFinal)
				{
					s += "final ";
				}
			
				
				if (meth is MethodInfo)
				{
					s += ((MethodInfo)meth).ReturnType.FullName;
					s += " ";
				}
			}

			s += meth.Name;
			s += " (";

			ParameterInfo[] pi = meth.GetParameters();
			
			if (pi.Length !=0)
			{
				for(i=0;i<pi.Length;++i)
				{
					s += Param2String(pi[i], true);
					if (i < pi.Length-1)
						s += ", ";
				}
			}

			s += ")";

			if (bShort)
			{
				if (meth is MethodInfo)
				{
					s += "  :: ";
					s += ((MethodInfo)meth).ReturnType.Name;
					s += " ";
				}
			}

			return s;
		}


		public static string Property2String(PropertyInfo prop, bool bShort)
		{
			string s = "";

			if (!bShort)
			{
				if (prop.CanRead)
				{
					s += "[get] ";
				}

				if (prop.CanWrite)
				{
					s += "[set] ";
				}
			}

			s += prop.PropertyType.FullName;
			s += " ";
			s += prop.Name;

			return s;
		}

		public static string Event2String(EventInfo ev, bool bShort)
		{
			string s = "";

			if (bShort)
			{
				return ev.Name;
			}


			if (ev.IsMulticast)
			{
				s += "[multi] ";
			}
			
			s += ev.Name;

			s += " " + Type2String(ev.EventHandlerType, true);

			return s;
		}

		public static string Field2String(FieldInfo field, bool bShort)
		{
			string s = "";

			if (bShort)
			{
				return field.Name;
			}

			if (field.IsStatic)
			{
				s += "static ";
			}

			if (field.IsPrivate)
			{
				s += "private ";
			}

			if (field.IsPublic)
			{
				s += "public ";
			}

			if (field.IsFamily)
			{
				s += "protected ";
			}

			s += field.FieldType.Name;
			s += " ";
			s += field.Name;

			return s;
		}


		public static string Param2String(ParameterInfo param, bool bShort)
		{
			string s = "";

			if (!bShort)
			{

				if (param.IsIn)
				{
					s += "[in] ";
				}

				if (param.IsOut)
				{
					s += "[out] ";
				}

				if (param.IsOptional)
				{
					s += "[optional] ";
				}

				if (param.IsLcid)
				{
					s += "[islcid] ";
				}

				if (param.IsRetval)
				{
					s += "[retval] ";
				}
			}


			if (bShort)
				s += param.ParameterType.Name;
			else
				s += param.ParameterType.FullName;


			s += " ";
			s += param.Name;

			return s;
		}





	}

		
		
}
