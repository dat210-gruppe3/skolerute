using System;
using Xamarin.Forms;
using System.Collections.Generic;
namespace skolerute
{
	public static class Constants
	{
        // CSV Parser
		public const string URL = "http://open.stavanger.kommune.no/dataset/86d3fe44-111e-4d82-be5a-67a9dbfbfcbb/resource/32d52130-ce7c-4282-9d37-3c68c7cdba92/download/skolerute-2016-17.csv";
		public const string PositionURL = "http://open.stavanger.kommune.no/dataset/8f8ac030-0d03-46e2-8eb7-844ee11a6203/resource/0371a1db-7074-4568-a0cc-499a5dccfe98/download/skoler.csv";

        // Calendar
        public const int shownCalendarDaysCount = 42;

        // Colors
        static Color yellow = Color.Yellow;
        static Color blue = Color.FromHex("5555ff");
        static Color purple = Color.FromHex("aa55ff");
        static Color teal = Color.Teal;
        static Color maroon = Color.Maroon;
        static Color lime = Color.Lime;
        public static List<Color> colors = new List<Color>() { yellow, blue, purple, teal, maroon, lime };

        // Settings
        public const string offlineMode = "offline"; 
    }
}
