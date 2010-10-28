using System;
using Genetibase.MathX.NuGenPhysicsConstants;


namespace Genetibase.MathX.NuGenSpatial
{

    /// <summary>
    /// Solar constants
    /// </summary>
    public class NuGenSolar
    {

        private NuGenSolar()
        {

        }

        
        
        /// <summary>
        /// standard gravitational acceleration (Earth)
        /// </summary>
        public const double g = 9.80665;		// standard gravitational acceleration (Earth)

        // conversions and physical constants
        //public const double	au2km = 1.49597891e8;		// km, derived value (self consistant!)
        public const double au2km = 1.4959787066e8;			// km, (2), 
        public const double km2au = 1.0 / au2km;			// au, astronomical units

        public const double ly2km = Genetibase.MathX.NuGenPhysicsConstants.NuGenUniversalConstants.SpeedOfLightInVacuum * NuGenConv.yr2sec / 1000.0;	// km, light years
        public const double km2ly = 1.0 / ly2km;			// ly,

        //public const double	pc2km = au2km / (2.0 * sin( 0.5 * sec2hr * deg2rad ));	// km, parsecs
        public const double pc2km = 3.0856775807e13;	// km, (4),
        public const double km2pc = 1.0 / pc2km;		// pc,

        /*
         * Masses of planets and star in the local system.
         */

        // the following are in kg in terms of Earth's mass due to higher resolution
        // of my available data.  "Universe -- Origins and Evolution" by Snow/Brownsberger

        public const double Mass_e = 5.97370e24;		// kg, (76), Earth's mass
        public const double Mass_s = 1.98892e30;		// kg, (25), Sun's mass
        public const double Sun_m = Mass_s;

        public const double Mercury_m = 0.0558 * Mass_e;	// kg, 
        public const double Venus_m = 0.815 * Mass_e;		// kg, 
        public const double Earth_m = 1.000 * Mass_e;		// kg, 
        public const double Moon_m = 0.0123 * Mass_e;		// kg,
        public const double Mars_m = 0.107 * Mass_e;		// kg, 
        public const double Jupiter_m = 317.89 * Mass_e;	// kg, 
        public const double Saturn_m = 95.184 * Mass_e;		// kg, 
        public const double Uranus_m = 14.536 * Mass_e;		// kg, 
        public const double Neptune_m = 17.148 * Mass_e;	// kg, 
        public const double Pluto_m = 0.0022 * Mass_e;		// kg, 

        /*
         * Semi-major Axis' of Orbit around Sun
         */

        public const double Mercury_a = 0.387 * au2km;	// km, 
        public const double Venus_a = 0.723 * au2km;	// km, 
        public const double Earth_a = 1.000 * au2km;	// km, 
        public const double Moon_a = 3.84401e5;			// km,
        public const double Mars_a = 1.524 * au2km;		// km, 
        public const double Jupiter_a = 5.203 * au2km;	// km, 
        public const double Saturn_a = 9.555 * au2km;	// km, 
        public const double Uranus_a = 19.22 * au2km;	// km, 
        public const double Neptune_a = 30.11 * au2km;	// km, 
        public const double Pluto_a = 39.44 * au2km;	// km, 

        /*
         * Period of Orbit about the Sun
         */

        public const double Mercury_p = 0.241 * NuGenConv.yr2sec;	// s,
        public const double Venus_p = 0.615 * NuGenConv.yr2sec;	// s,
        public const double Earth_p = 1.0 * NuGenConv.yr2sec;		// s,
        public const double Moon_p = 29.0 * NuGenConv.day2sec + 12.0 * NuGenConv.hr2sec + 44.0 * NuGenConv.min2sec + 3.0;	// s,
        public const double Mars_p = 1.881 * NuGenConv.yr2sec;	// s,
        public const double Jupiter_p = 11.86 * NuGenConv.yr2sec;	// s,
        public const double Saturn_p = 29.46 * NuGenConv.yr2sec;	// s,
        public const double Uranus_p = 84.01 * NuGenConv.yr2sec;	// s,
        public const double Neptune_p = 164.79 * NuGenConv.yr2sec;	// s,
        public const double Pluto_p = 248.5 * NuGenConv.yr2sec;	// s,
    }

}
