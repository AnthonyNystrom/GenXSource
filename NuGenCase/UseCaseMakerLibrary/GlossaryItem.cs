using System;
using System.Xml;

namespace UseCaseMakerLibrary
{
	/**
	 * @brief Descrizione di riepilogo per GlossaryItem.
	 */
	public class GlossaryItem : IdentificableObject
	{
		#region Class Members
		String description = String.Empty;
		#endregion

		#region Constructors
		internal GlossaryItem()
			: base()
		{
		}

		internal GlossaryItem(String name, String prefix, Int32 id)
			: base(name,prefix,id)
		{
		}

		internal GlossaryItem(String name, String prefix, Int32 id, Package owner)
			: base(name,prefix,id,owner)
		{
		}
		#endregion

		#region Public Properties
		public String Description
		{
			get
			{
				return this.description;
			}
			set
			{
				this.description = value;
			}
		}
		#endregion
	}
}
