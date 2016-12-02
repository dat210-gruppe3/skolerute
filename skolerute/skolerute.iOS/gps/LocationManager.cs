using System;
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
		CLAuthorizationStatus locAuthorized;

		public void ConnectGps()
		{
		}

		public Coordinate GetGpsCoordinates()
		{
			var locationManager = new CLLocationManager();
			locationManager.PausesLocationUpdatesAutomatically = true;

			// iOS 8 has additional permissions requirements
			if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
			{
				locationManager.RequestWhenInUseAuthorization();

				locationManager.AuthorizationChanged += (sender, args) =>
				{
					locAuthorized = args.Status;
				};

				if (locAuthorized == CLAuthorizationStatus.AuthorizedWhenInUse) {

					if (CLLocationManager.LocationServicesEnabled)
					{
						locationManager.DesiredAccuracy = 100; // In meters
						locationManager.StartUpdatingLocation();
						//Wait for location-update
						System.Threading.Thread.Sleep(10);

						try
						{
							locationManager.StopUpdatingLocation();
							return new Coordinate(locationManager.Location.Coordinate.Latitude, locationManager.Location.Coordinate.Longitude);
						}
						catch (NullReferenceException e)
						{
							Console.WriteLine(e);
							new UIAlertView("Prøv på nytt",
							"Det skjedde en feil ved innhenting av din posisjon.",
							null, "Ok").Show();
						}
					}
				}
			}
			return null;
		}

	}
}
