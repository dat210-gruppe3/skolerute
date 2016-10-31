using System;
using System.Collections.Generic;
using System.Text;
using skolerute.GPS;
using CoreLocation;
using UIKit;

namespace skolerute.iOS.gps
{
    public class LocationManager : IGPSservice
    {
        protected CLLocationManager locationManager;

        public LocationManager()
        {
            this.locationManager = new CLLocationManager();
            this.locationManager.PausesLocationUpdatesAutomatically = true;

            // iOS 8 has additional permissions requirements
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                locationManager.RequestWhenInUseAuthorization(); // Only in foreground
                // locationManager.RequestAlwaysAuthorization would work in the background
            }

            if (UIDevice.CurrentDevice.CheckSystemVersion(9, 0))
            {
                locationManager.AllowsBackgroundLocationUpdates = true;
            }
        }

        public CLLocationManager LocManager
        {
            get { return this.locationManager; }
        }

        public List<double> GetGpsCoordinates()
        {
            LocManager.DesiredAccuracy = 100; // In meters
            LocManager.RequestLocation();
            double latitude = LocManager.Location.Coordinate.Latitude;
            double longitude = LocManager.Location.Coordinate.Longitude;
            return new List<double>(2) { latitude, longitude };
        }
    }
}
