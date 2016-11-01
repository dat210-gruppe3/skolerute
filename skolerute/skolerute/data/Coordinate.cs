using System;

namespace skolerute.data
{
    public class Coordinate
    {
        private double latitude;
        private double longitude;

        public Coordinate(double latitude, double longitude)
        {
            this.latitude = latitude;
            this.longitude = longitude;
        }

        private static double ToRadians(double value)
        {
            return (Math.PI / 180) * value;
        }

        public double HaversineDistance(double otherLatitude, double otherLongitude)
        {
            double R = 6371;
            double latitudeRadians = ToRadians(otherLatitude - latitude);
            double longitudeRadians = ToRadians(otherLongitude - longitude); 

            double haversineFunction = Math.Sin(latitudeRadians / 2) * Math.Sin(latitudeRadians / 2) +
                     Math.Cos(ToRadians(latitude)) * Math.Cos(ToRadians(otherLatitude)) *
                     Math.Sin(longitudeRadians / 2) * Math.Sin(longitudeRadians / 2);

            double haversineValue = 2 * Math.Asin(Math.Min(1, Math.Sqrt(haversineFunction)));

            return R * haversineValue;
        }
    }
}
