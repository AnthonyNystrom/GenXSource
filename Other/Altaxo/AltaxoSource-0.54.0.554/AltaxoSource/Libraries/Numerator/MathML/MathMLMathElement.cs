//This file is part of the gNumerator MathML DOM library, a complete 
//implementation of the w3c mathml dom specification
//Copyright (C) 2003, Andy Somogyi
//
//This library is free software; you can redistribute it and/or
//modify it under the terms of the GNU Lesser General Public
//License as published by the Free Software Foundation; either
//version 2.1 of the License, or (at your option) any later version.
//
//This library is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//Lesser General Public License for more details.
//
//You should have received a copy of the GNU Lesser General Public
//License along with this library; if not, write to the Free Software
//Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
//
//For details, see http://numerator.sourceforge.net, or send mail to
//andy@epsilon3.net

using System;
using System.Xml;

namespace MathML
{
	/// <summary>
	/// This interface represents the top-level MathML math element. It may become useful 
	/// for interfacing between the Document Object Model objects encoding an enclosing 
	/// document and the MathML DOM elements that are its children. It could also be used 
	/// for some purposes as a MathML DOM surrogate for a Document object. For instance, 
	/// MathML-specific factory methods could be placed here, as could methods for 
	/// creating MathML-specific Iterators or TreeWalkers. However, this functionality is 
	/// as yet undefined.
	/// </summary>
	public class MathMLMathElement : MathMLElement, MathMLContainer
	{
		private MathMLContainerImpl container_impl;

		/// <summary>
		/// creates a new MathMLOperatorElement. 
		/// </summary>
		/// <param name="prefix">The prefix of the new element (if any).</param>
		/// <param name="localName">The local name of the new element.</param>
		/// <param name="namespaceURI">The namespace URI of the new element (if any).</param>
		/// <param name="doc">The owner document</param>		
		public MathMLMathElement(string prefix, string localName, string namespaceURI, MathMLDocument doc)
			: base(prefix, localName, namespaceURI, doc)
		{
			container_impl = new MathMLContainerImpl(this);
		}

		/// <summary>
		/// Represents the macros attribute of the math element. See Section 7.1.2.
		/// </summary>
		public string Macros 
		{
			get { return GetAttribute("macros"); }
			set { SetAttribute("macros", value); }
		}

		/// <summary>
		/// Represents the display attribute of the math element. This value is either block 
		/// or inline. The display attribute replaces the deprecated mode attribute. 
		/// It specifies whether the enclosed MathML expression should be
		/// rendered in a display style or an in-line style. Allowed values are "block" 
		/// and "inline" (default).
		/// </summary>
		public Display Display
		{
			get { return Utility.ParseDisplay(GetAttribute("display"), Display.Block); }
			set { SetAttribute("display", value == Display.Block ? "block" : "inline"); }
		}

		/// <summary>
		/// The number of child elements of this element which represent arguments of the element, 
		/// as opposed to qualifiers or declare elements. Thus for a MathMLContentContainer 
		/// it does not contain elements representing bound variables, conditions, separators, 
		/// degrees, or upper or lower limits (bvar, condition, sep, degree, lowlimit, or uplimit).
		/// </summary>
		public int ArgumentCount
		{
			get { return container_impl.ArgumentCount; }
		}

		/// <summary>
		/// This attribute accesses the child MathMLElements of this element which are arguments 
		/// of it, as a MathMLNodeList. Note that this list does not contain any MathMLElements 
		/// representing qualifier elements or declare elements.
		/// </summary>
		public MathMLNodeList Arguments
		{
			get { return container_impl.Arguments; }
		}

		/// <summary>
		/// Provides access to the declare elements which are children of this element, 
		/// in a MathMLNodeList. All Nodes in this list must be MathMLDeclareElements.
		/// </summary>
		public MathMLNodeList Declarations
		{
			get { return container_impl.Declarations; }
		}

		/// <summary>
		/// This method returns the indexth child argument element of this element. 
		/// This frequently differs from the value of Node::childNodes().item(index),
		/// as qualifier elements and declare elements are not counted.
		/// </summary>
		public MathMLElement GetArgument(int index)
		{
			return container_impl.GetArgument(index);
		}

		/// <summary>
		/// This method sets newArgument as the index-th argument of this element. 
		/// If there is currently an index-th argument, it is replaced by newArgument.
		/// This frequently differs from setting the node at Node::childNodes().item(index), 
		/// as qualifier elements and declare elements are not counted.
		/// </summary>
		public MathMLElement SetArgument(MathMLElement newArgument, int index)
		{
			return container_impl.SetArgument(newArgument, index);
		}

		/// <summary>
		/// This method inserts newArgument before the current index-th argument of this 
		/// element. If index is 0, or if index is one more than the current number
		/// of arguments, newArgument is appended as the last argument. This frequently 
		/// differs from setting the node at Node::childNodes().item(index),
		/// as qualifier elements and declare elements are not counted.
		/// </summary>
		public MathMLElement InsertArgument(MathMLElement newArgument, int index)
		{
			return container_impl.InsertArgument(newArgument, index);
		}

		/// <summary>
		/// This method deletes the index-th child element that is an argument of this element. 
		/// Note that child elements which are qualifier elements or declare
		/// elements are not counted in determining the index-th argument.
		/// </summary>
		public void DeleteArgument(int index)
		{
			container_impl.DeleteArgument(index);
		}

		/// <summary>
		/// This method deletes the index-th child element that is an argument of this element, 
		/// and returns it to the caller. Note that child elements that are qualifier
		/// elements or declare elements are not counted in determining the index-th argument.
		/// </summary>
		public MathMLElement RemoveArgument(int index)
		{
			return container_impl.RemoveArgument(index);
		}

		/// <summary>
		/// This method retrieves the index-th child declare element of this element.
		/// </summary>
		public MathMLDeclareElement GetDeclaration(int index)
		{
			return container_impl.GetDeclaration(index);
		}

		/// <summary>
		/// This method inserts newDeclaration as the index-th child declaration of this element. 
		/// If there is already an index-th declare child element, it is replaced by newDeclaration.
		/// </summary>
		public MathMLDeclareElement SetDeclaration(MathMLDeclareElement newDeclaration, int index)
		{
			return container_impl.SetDeclaration(newDeclaration, index);
		}

		/// <summary>
		/// This method inserts newDeclaration before the current index-th child declare element 
		/// of this element. If index is 0, newDeclaration is appended as the last child declare element.
		/// </summary>
		public MathMLDeclareElement InsertDeclaration(MathMLDeclareElement newDeclaration, int index)
		{
			return container_impl.InsertDeclaration(newDeclaration, index);
		}

		/// <summary>
		/// This method removes the MathMLDeclareElement representing the index-th declare child 
		/// element of this element, and returns it to the caller. Note that index is the position 
		/// in the list of declare element children, as opposed to the position in the list of all 
		/// child Nodes.
		/// </summary>
		public MathMLDeclareElement RemoveDeclaration(int index)
		{
			return container_impl.RemoveDeclaration(index);
		}

		/// <summary>
		/// This method deletes the MathMLDeclareElement representing the index-th declare child 
		/// element of this element. Note that index is the position in the list of declare 
		/// element children, as opposed to the position in the list of all child Nodes.
		/// </summary>
		public void DeleteDeclaration(int index)
		{
			container_impl.DeleteDeclaration(index);
		}

		/// <summary>
		 /// accept a visitor.
		 /// return the return value of the visitor's visit method
		 /// </summary>
		public override object Accept(MathMLVisitor v, object args)
		{
			return v.Visit(this, args);
		}
	}
}
