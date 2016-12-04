using System.Collections.Generic;
using Xamarin.Forms;

namespace skolerute.data
{
	public static class Constants
	{
        // Selecte Schools
        public const int MaximumSelectedSchools = 3;

        // CSV Parser 
		public const string URL = "https://github.com/danielbarati/test/raw/master/skolerute-2016-17.csv";
		public const string positionURL = "https://github.com/danielbarati/test/raw/master/skoler.csv";
		public static string skoleruterCSV;

        // Calendar
        public const int ShownCalendarDaysCount = 42;

        // Colors
		static readonly Color red = Color.FromHsla(0, 1, 0.5);
		static readonly Color blue = Color.FromHsla(0.667, 0.5, 0.4);
		static readonly Color green = Color.FromHsla(0.319, 0.75, 0.50);
        public static readonly List<Color> colors = new List<Color>() { red, blue, green };
    }
}
