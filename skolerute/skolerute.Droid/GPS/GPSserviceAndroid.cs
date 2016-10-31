using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using skolerute.GPS;
using skolerute.db;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Locations;
using Xamarin.Forms;


[assembly: Xamarin.Forms.Dependency(typeof(skolerute.Droid.GPS.GPSserviceAndroid))]
namespace skolerute.Droid.GPS
{
    class GPSserviceAndroid : Java.Lang.Object, IGPSservice
    {
        public List<double> GetGpsCoordinates()
        {
            LocationManager mgr = (LocationManager)Forms.Context.GetSystemService(Context.LocationService);
            var LC = new Criteria();
            LC.Accuracy = Accuracy.Coarse;
            LC.PowerRequirement = Power.Medium;
            LocationListener LL = new LocationListener();
            string LP = mgr.GetBestProvider(LC, true);
            mgr.RequestLocationUpdates(LP, 500, 100, LL);
            System.Threading.Thread.Sleep(550);
            try { 
                Location location = mgr.GetLastKnownLocation(LP);
                double lat = location.Latitude;
                double lon = location.Longitude;
                return new List<double> { lat, lon };
            } catch (Exception e) { }

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