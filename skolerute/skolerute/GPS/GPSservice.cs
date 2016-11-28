using System;
using System.Collections.Generic;
using System.Linq;
using skolerute.data;
using System.Text;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace skolerute.GPS
{
    public static class GPSservice
    {

        public static List<WrappedListItems<School>> GetNearbySchools(List<WrappedListItems<School>> WrappedItems)
        {
            Coordinate gpsCoordinates = DependencyService.Get<GPS.IGPSservice>().GetGpsCoordinates();

            foreach (WrappedListItems<School> item in WrappedItems)
            {
                School x = item.Item;
                item.DistanceVisible = true;
                item.Distance = gpsCoordinates.HaversineDistance(x.latitude, x.longitude);
            }

            List<WrappedListItems<School>> newWrappedItems = WrappedItems.OrderBy(o => o.Distance).ToList();

            return newWrappedItems;
        }
    }


}
