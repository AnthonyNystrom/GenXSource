using System;

namespace Genetibase.MathX.Core
{
	/// <summary><para>Identifies the type of function definition.</para></summary>
	public enum DefinitionType
	{
		/// <summary>Function defined by expression.</summary>
		Analytic,
		/// <summary>Function defined by delegate.</summary>
		Numerical
	}
}
