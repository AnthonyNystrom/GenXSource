using System;

namespace UseCaseMakerLibrary
{
	/**
	 * @brief Descrizione di riepilogo per GlossaryItems.
	 */
	public class GlossaryItems : IdentificableObjectCollection
	{
		internal GlossaryItems(Package owner)
		{
			base.Owner = owner;
		}
	}
}
