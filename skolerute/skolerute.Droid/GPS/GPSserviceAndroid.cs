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

            string bestProvider = "";

            bestProvider = mgr.IsProviderEnabled("gps") == false ? mgr.GetBestProvider(criteria, true) : "gps";

            mgr.RequestSingleUpdate(bestProvider,locationListener,null);

            try
            {   
                Location location = mgr.GetLastKnownLocation(bestProvider);

                if (bestProvider == "gps")
                {          
                    Stopwatch stopWatch = new Stopwatch();
                    stopWatch.Start();
                    Location location2 = mgr.GetLastKnownLocation(bestProvider);

                    if (location == null)
                    {
                        Toast.MakeText(Forms.Context, "Fikk ingen lokasjon, prøv igjen", ToastLength.Short).Show();
                        return null;
                    }

                    while (location2 == null || location.Time == location2.Time)
                    {
                        location2 = mgr.GetLastKnownLocation(bestProvider);
                        if (stopWatch.ElapsedMilliseconds <= 10000) continue;
                        bestProvider = mgr.GetBestProvider(criteria, true);
                        mgr.RequestSingleUpdate(bestProvider,locationListener,null);
                        location = mgr.GetLastKnownLocation(bestProvider);
                        break;
                    }
                    if (bestProvider == "gps") location = location2;
                    stopWatch.Stop();
                }

                if (location == null)
                {
                    Toast.MakeText(Forms.Context, "Fikk ingen lokasjon, prøv igjen", ToastLength.Short).Show();
                    return null;
                }

                if (location.Provider == "network") Toast.MakeText(Forms.Context, "Ingen GPS tilgjengelig, oppgitt avstand tilnærmet", ToastLength.Short).Show();

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
            //throw new NotImplementedException();
        }

        public void OnLocationChanged(Location location)
        {

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