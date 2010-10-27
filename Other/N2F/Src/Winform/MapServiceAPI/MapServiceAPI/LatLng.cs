using System;
using System.Collections.Generic;
using System.Text;

namespace MapServiceAPI
{
    public struct LatLng
    {
        public Double Latitude;
        public Double Longitude;

        public LatLng(double latitude, double longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }
        public override string ToString()
        {
            return Latitude + ", " + Longitude;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="course">In Nautical Mile</param>
        /// <param name="distance">Degrees 0 north?</param>
        /// <returns></returns>
        public LatLng FromCourse(double course, double distance)
        {
            var EPS = 0.00000000005;

            double lat = Latitude * (Math.PI / 180);
            double lon = Longitude * (Math.PI / 180);
            double crs = (-course) * (Math.PI/180);
            double dis = distance / (180 * 60 / Math.PI);

            LatLng results = new LatLng();

            if ((Math.Abs(Math.Cos(lat)) < EPS) && !(Math.Abs(Math.Sin(crs)) < EPS))
            {
                throw new Exception("Starting from pole requires N-S course");
            }

            results.Latitude = Math.Asin(Math.Sin(lat) * Math.Cos(dis) + Math.Cos(lat) * Math.Sin(dis) * Math.Cos(crs));

            if (Math.Abs(Math.Cos(results.Latitude)) < EPS)
            {
                results.Longitude = 0;
            }
            else
            {
                double dlon = Math.Atan2(Math.Sin(crs) * Math.Sin(dis) * Math.Cos(lat), Math.Cos(dis) - Math.Sin(lat) * Math.Sin(results.Latitude));
                results.Longitude = mod(lon - dlon + Math.PI, 2 * Math.PI) - Math.PI;
            }
            results.Latitude *= (180 / Math.PI);
            results.Longitude *= (180 / Math.PI);
            return results;
        }
        private static double mod(double x, double y)
        {
            return x - y * Math.Floor(x / y);
        }
        
        /*
        public static void CourseDistance(double lat1, double lon1, double lat2, double lon2, out double distance, out double course)
        {
            const double EarthRadiusKms = 6376.5;
            
            distance = double.MinValue;
            course = double.MinValue;

            double lat1InRad = lat1 * (Math.PI / 180);
            double lon1InRad = lon1 * (Math.PI / 180);
            double lat2InRad = lat2 * (Math.PI / 180);
            double lon2InRad = lon2 * (Math.PI / 180);

            double longitude = lon2InRad - lon1InRad;
            double latitude = lat2InRad - lat1InRad;

            double a = Math.Pow(Math.Sin(latitude / 2), 2) + Math.Cos(lat1InRad) * Math.Cos(lat2InRad) * Math.Pow(Math.Sin(longitude / 2), 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            distance = EarthRadiusKms * c;

            double distanceNorth = latitude;
            double distanceEast = longitude * Math.Cos(lat1InRad);

            course = Math.Atan2(distanceEast, distanceNorth) % (2 * Math.PI);
            course = course * (180 / Math.PI);
            if (course < 0) course += 360;
        }*/
    }
}
