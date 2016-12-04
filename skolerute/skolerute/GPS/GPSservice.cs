using System;
using System.Collections.Generic;
using System.Linq;
using skolerute.data;
using Xamarin.Forms;

namespace skolerute.GPS
{
    public static class GPSservice
    {

        public static List<WrappedListItems<School>> GetNearbySchools(List<WrappedListItems<School>> WrappedItems)
        {
            Coordinate gpsCoordinates = DependencyService.Get<GPS.IGPSservice>().GetGpsCoordinates();
            if(gpsCoordinates == null) { return null; }

            foreach (WrappedListItems<School> item in WrappedItems)
            {
                School x = item.Item;
                item.DistanceVisible = true;
                item.Distance = gpsCoordinates.HaversineDistance(x.latitude, x.longitude);
                item.Distance = Math.Round(item.Distance, 1);
            }

            List<WrappedListItems<School>> newWrappedItems = WrappedItems.OrderBy(o => o.Distance).ToList();

            return newWrappedItems;
        }
    }


}
