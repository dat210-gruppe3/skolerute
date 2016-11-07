using System;
using skolerute.GPS;
using skolerute.data;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Locations;
using Xamarin.Forms;


[assembly: Xamarin.Forms.Dependency(typeof(skolerute.Droid.GPS.GPSserviceAndroid))]
namespace skolerute.Droid.GPS
{
    class GPSserviceAndroid : Java.Lang.Object, IGPSservice
    {
        public Coordinate GetGpsCoordinates()
        {
            LocationManager mgr = (LocationManager)Forms.Context.GetSystemService(Context.LocationService);

            var criteria = new Criteria();
            criteria.Accuracy = Accuracy.Coarse;
            criteria.PowerRequirement = Power.Medium;

            LocationListener locationListener = new LocationListener();
            string bestProvider = mgr.GetBestProvider(criteria, true);

            mgr.RequestLocationUpdates(bestProvider, 500, 100, locationListener);

            System.Threading.Thread.Sleep(550);

            try
            { 
                Location location = mgr.GetLastKnownLocation(bestProvider);
                return new Coordinate(location.Latitude, location.Longitude);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                // TODO: Error handling here
            }

            return null; 
        }
    }

    class LocationListener : Java.Lang.Object, ILocationListener
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void OnLocationChanged(Location location)
        {

        }

        public void OnProviderDisabled(string provider)
        {
            throw new NotImplementedException();
        }

        public void OnProviderEnabled(string provider)
        {
            throw new NotImplementedException();
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            
        }
    }
}