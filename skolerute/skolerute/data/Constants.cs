using System.Collections.Generic;
using Xamarin.Forms;

namespace skolerute.data
{
	public static class Constants
	{
        // Selecte Schools
        public const int MaximumSelectedSchools = 3;

        // CSV Parser 
		public const string URL = "http://open.stavanger.kommune.no/dataset/86d3fe44-111e-4d82-be5a-67a9dbfbfcbb/resource/7472239b-e18e-47c4-9810-780240122552/download/skolerute.csv";
		public const string positionURL = "http://open.stavanger.kommune.no/dataset/8f8ac030-0d03-46e2-8eb7-844ee11a6203/resource/356f485c-e200-45b5-9929-5035d165c394/download/skoler.csv";

        // Calendar
        public const int ShownCalendarDaysCount = 42;

        // Colors
        static readonly Color yellow = Color.Yellow;
        static readonly Color blue = Color.FromHex("5555ff");
        static readonly Color purple = Color.FromHex("aa55ff");
        static readonly Color teal = Color.Teal;
        static readonly Color maroon = Color.Maroon;
        static readonly Color lime = Color.Lime;
        public static readonly List<Color> colors = new List<Color>() { yellow, blue, purple, teal, maroon, lime };

        // Settings
        public const string OfflineMode = "offline";
		public const string Notify = "Notify";
    }
}
