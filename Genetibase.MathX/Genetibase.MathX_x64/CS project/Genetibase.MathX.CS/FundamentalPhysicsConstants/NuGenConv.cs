
using System;

namespace Genetibase.MathX.Constants
{

	public class NuGenConv
	{

		private NuGenConv()
		{
			
		}
		/*
 * Mathematic Units
 */

		public const double	deg2rad = Math.PI / 180.0;
		public const double	rad2deg = 1.0 / deg2rad;

		public const double	rev2rad = 2.0 * Math.PI;
		public const double	rad2rev = 1.0 / rev2rad;

		public const double	rev2deg = 360.0;
		public const double	deg2rev = 1.0 / rev2deg;

		/*
		 * Base Units (SI)
		 */

		// Time
		public const double	min2sec = 60.0;			// s,
		public const double sec2min = 1.0 / min2sec;	// m,

		public const double	hr2min = 60.0;			// m,
		public const double	min2hr = 1.0 / hr2min;		// h,

		public const double	day2hr = 24.0;			// h,
		public const double	hr2day = 1.0 / day2hr;		// d,

		public const double	s_yr2day = 365.256366;		// d, sidereal
		public const double	t_yr2day = 365.241219878;	// d, tropical
		public const double	std_yr2day = 365.25;		// d, standard calendar type reference

		public const double	yr2day = t_yr2day;		// d,
		public const double	day2yr = 1.0 / yr2day;		// y,

		public const double	yr2hr = yr2day * day2hr;	// h,
		public const double	yr2min = yr2hr * 60.0;		// m,
		public const double	yr2sec = yr2min * 60.0;		// s,

		public const double	hr2yr = 1.0 / yr2hr;		// y,
		public const double	min2yr = 1.0 / yr2min;		// y,
		public const double	sec2yr = 1.0 / yr2sec;		// y,

		public const double	day2min = day2hr * hr2min;	// m,
		public const double	day2sec = day2min * min2sec;	// s,

		public const double	min2day = 1.0 / day2min;	// d,
		public const double	sec2day = 1.0 / day2sec;	// s,

		public const double	hr2sec = hr2min * min2sec;	// s,
		public const double	sec2hr = 1.0 / hr2sec;		// h,

		// end of Time	:)

		// ------------------------------------------------

		// Mass
		public const double	u2kg = NuGenPhysico_chemical.AtomicMassConstant;	// kg, atomic mass units to kg, nist
		public const double	kg2u = 1.0 / u2kg;	// u, kg to atomic mass units 

		// end of Mass
		// ------------------------------------------------

		/*
		 * Derived Units
		 */

		// Speed
		public const double	km_h2m_s = hr2sec * 1000.0;	// m/s
		public const double	m_s2km_h = 1.0 / km_h2m_s;	// km/h

		// Mass-Energy
		public const double	u2J = NuGenPhysico_chemical.AtomicMassConstantEnergyEquivalent;			// J, NIST, m_u * c^2
		public const double	J2u = 1.0 / u2J;

		public const double	u2eV = NuGenPhysico_chemical.AtomicMassConstantEnergyEquivalentInMev * 1e6;
		public const double	eV2u = 1.0 / u2eV;

		public const double	kg2J = kg2u * u2J;
		public const double	J2kg = 1.0 / kg2J;

		public const double	kg2eV = kg2u * u2eV;
		public const double	eV2kg = 1.0 / kg2eV;

		// Energy
		public const double	J2eV = J2u * u2eV;		// eV, joules to electron volts
		public const double	eV2J = 1.0 / J2eV;		// J, 

		// Astronomical units
		public const double	ly2m = NuGenUniversalConstants.SpeedOfLightInVacuum * yr2sec;
		public const double	m2ly = 1.0 / ly2m; 
		public const double	ly2km = ly2m * 1.0 / 1000.0;
		public const double	km2ly = 1.0 / ly2km;
	}
}
