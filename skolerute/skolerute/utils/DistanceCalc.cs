using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace skolerute.utils
{
    static class DistanceCalc
    {
       
        public static double ToRadian(this double value)
        {
            return (Math.PI / 180) * value;
        }
        public static double HaversineDistance(double lat1, double long1, double lat2, double long2)
        {
            double R = 6371;
            var lat = (lat2 - lat1).ToRadian();
            var lng = (long2 - long1).ToRadian();

            var h1 = Math.Sin(lat / 2) * Math.Sin(lat / 2) +
                     Math.Cos(lat1.ToRadian()) * Math.Cos(lat2.ToRadian()) *
                     Math.Sin(lng / 2) * Math.Sin(lng / 2);

            var h2 = 2 * Math.Asin(Math.Min(1, Math.Sqrt(h1)));

            return R * h2;
        }
    }
}
