/* -----------------------------------------------
 * NuGenFormulaCollectionBase.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.MathX
{
	/// <summary>
	/// </summary>
	public class NuGenFormulaCollectionBase : CollectionBase
	{
		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentOutOfRangeException"/>
		public virtual NuGenFormula this[int index]
		{
			get
			{
				return (NuGenFormula)this.InnerList[index];
			}
			set
			{
				this.InnerList[index] = value;
			}
		}

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="item"/> is <see langword="null"/>.</para>
		/// </exception>
		public virtual void Add(NuGenFormula item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}

			this.InnerList.Add(item);
		}

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="items"/> is <see langword="null"/>.</para>
		/// </exception>
		public virtual void AddRange(NuGenFormula[] items)
		{
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}

			for (int i = 0; i < items.Length; i++)
			{
				NuGenFormula formula = items[i];

				if (formula != null)
				{
					this.InnerList.Add(formula);
				}
			}
		}

		/// <summary>
		/// </summary>
		public TreeNode[] GetTreeNodes()
		{
			TreeNode[] nodes = new TreeNode[this.InnerList.Count];

			for (int i = 0; i < nodes.Length; i++)
			{
				nodes[i] = ((NuGenFormula)this.InnerList[i]).TreeNode;
			}

			return nodes;
		}

		/// <summary>
		/// </summary>
		/// <param name="item">Can be <see langword="null"/>.</param>
		public int IndexOf(NuGenFormula item)
		{
			return this.InnerList.IndexOf(item);
		}

		/// <summary>
		/// </summary>
		/// <param name="item">Can be <see langword="null"/>.</param>
		public virtual void Remove(NuGenFormula item)
		{
			this.InnerList.Remove(item);
		}

		/// <summary>
		/// </summary>
		public NuGenFormula[] ToArray()
		{
			if (this.InnerList.Count == 0)
			{
				return new NuGenFormula[] { };
			}

			NuGenFormula[] formulas = new NuGenFormula[this.InnerList.Count];
			this.InnerList.CopyTo(formulas);
			return formulas;
		}
	}
}
