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
    class GPSserviceAndroid : IGPSservice
    {
        Context context;
        public List<double> GetGpsCoordinates()
        {
            context = Forms.Context;
            LocationManager mgr = (LocationManager)context.GetSystemService("");
            var LC = new Criteria();
            LC.Accuracy = Accuracy.Coarse;
            LC.PowerRequirement = Power.Medium;

            string LP = mgr.GetBestProvider(LC, true);
            LocationListener LL = new LocationListener();
            mgr.RequestLocationUpdates(LP, 1000, 100, LL);
            Location location = LL.getLocation();
            double lat = location.Latitude;
            double lon = location.Longitude;

            return new List<Double>() { lat, lon };
        }
    }

    class LocationListener : ILocationListener
    {
        Location currentlocation;
        public IntPtr Handle
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void OnLocationChanged(Location location)
        {
            currentlocation = location;
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
            throw new NotImplementedException();
        }

        public Location getLocation()
        {
            return currentlocation;
        }
    }
}