using System.Collections.Generic;
using Xamarin.Forms;

namespace skolerute.data
{
	public static class Constants
	{
        // Selecte Schools
        public const int MaximumSelectedSchools = 3;

        // CSV Parser 
		public const string URL = "http://open.stavanger.kommune.no/dataset/86d3fe44-111e-4d82-be5a-67a9dbfbfcbb/resource/3a27a8e9-5485-4c16-834c-108b86418818/download/skolerute-2016-17.csv";
		public const string positionURL = "http://open.stavanger.kommune.no/dataset/8f8ac030-0d03-46e2-8eb7-844ee11a6203/resource/12f0d499-6474-4fe4-b457-db976e52cb37/download/skoler.csv";

        // Calendar
        public const int ShownCalendarDaysCount = 42;

        // Colors
		static readonly Color red = Color.FromHsla(0, 1, 0.5);
		static readonly Color blue = Color.FromHsla(0.667, 0.5, 0.4);
		static readonly Color green = Color.FromHsla(0.319, 0.75, 0.50);
		static readonly Color purple = Color.FromHsla(0.836, 0.60, 0.60);
		static readonly Color teal = Color.FromHsla(0.480, 0.90, 0.45);
		static readonly Color blood = Color.FromHsla(0, 0.60, 0.50);
        public static readonly List<Color> colors = new List<Color>() { red, blue, green, purple, teal, blood };

        // Settings
        public const string OfflineMode = "offline";
		public const string Notify = "Notify";
    }
}
