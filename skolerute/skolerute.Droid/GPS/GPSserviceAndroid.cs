using System;
using skolerute.GPS;
using skolerute.db;
using skolerute.data;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Locations;
using Xamarin.Forms;
using System.Diagnostics;
using System.Threading.Tasks;
using skolerute.Views;

[assembly: Xamarin.Forms.Dependency(typeof(skolerute.Droid.GPS.GPSserviceAndroid))]
namespace skolerute.Droid.GPS
{
    class GPSserviceAndroid : Java.Lang.Object, IGPSservice
    {

        LocationManager mgr = (LocationManager)Forms.Context.GetSystemService(Context.LocationService);

        LocationListener locationListener = new LocationListener();    

        public void ConnectGps()
        {
            string bestProvider = mgr.GetBestProvider(new Criteria(), true);

            mgr.RequestSingleUpdate(bestProvider, locationListener, null);
        }

        public Coordinate GetGpsCoordinates()
        {
            string bestProvider = mgr.GetBestProvider(new Criteria(), true);

            var position = mgr.GetLastKnownLocation(bestProvider);

            return new Coordinate(position.Latitude,position.Longitude);
        }
    }

    class LocationListener : Java.Lang.Object, ILocationListener
    {
        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public void OnLocationChanged(Location location)
        {
            MessagingCenter.Send(new Coordinate(location.Latitude,location.Longitude), "locationUpdate");
        }

        public void OnProviderDisabled(string provider)
        {
            //throw new NotImplementedException();
        }

        public void OnProviderEnabled(string provider)
        {
            //throw new NotImplementedException();
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {

        }
    }
}