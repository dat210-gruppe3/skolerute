using skolerute.GPS;
using CoreLocation;
using UIKit;
using skolerute.data;
using Xamarin.Forms;

[assembly: Dependency(typeof(skolerute.iOS.gps.LocationManager))]
namespace skolerute.iOS.gps
{
    public class LocationManager : IGPSservice
    {
        protected CLLocationManager locationManager;

        public CLLocationManager LocManager
        {
            get { return this.locationManager; }
        }

        /// <summary>
        /// This method returns the latitude and longitude values of the device.
        /// </summary>
        /// <returns>List<double> { longitude, latitude } or null if location services are not allowed</returns>
        public Coordinate GetGpsCoordinates()
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

            if (CLLocationManager.LocationServicesEnabled)
            {
                LocManager.DesiredAccuracy = 100; // In meters

                

				var mgr = new CLLocationManager();
				mgr.Delegate = new MyLocationDelegate();
				mgr.StartUpdatingLocation();

				//LocManager.RequestLocation();

				//return new Coordinate(0, 0);
				return new Coordinate(LocManager.Location.Coordinate.Latitude, LocManager.Location.Coordinate.Longitude);
            }

            return null;
        }
    }

	public class MyLocationDelegate : CLLocationManagerDelegate
	{
		public override void LocationsUpdated(CLLocationManager manager, CLLocation[] locations)
		{
			foreach (var loc in locations)
			{
				//Console.WriteLine(loc);
			}
		}
	}
}
