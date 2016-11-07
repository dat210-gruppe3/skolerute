using System.Collections.Generic;
using Xamarin.Forms;

namespace skolerute.data
{
	public static class Constants
	{
        // Selecte Schools
        public const int MaximumSelectedSchools = 6;

        // CSV Parser
		public const string URL = "http://open.stavanger.kommune.no/dataset/86d3fe44-111e-4d82-be5a-67a9dbfbfcbb/resource/32d52130-ce7c-4282-9d37-3c68c7cdba92/download/skolerute-2016-17.csv";
		public const string positionURL = "http://open.stavanger.kommune.no/dataset/8f8ac030-0d03-46e2-8eb7-844ee11a6203/resource/0371a1db-7074-4568-a0cc-499a5dccfe98/download/skoler.csv";

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
    }
}
