using System;
using Xamarin.Forms;
using System.Collections.Generic;
namespace skolerute
{
	public static class Constants
	{
        // CSV Parser
		public const string URL = "http://open.stavanger.kommune.no/dataset/86d3fe44-111e-4d82-be5a-67a9dbfbfcbb/resource/32d52130-ce7c-4282-9d37-3c68c7cdba92/download/skolerute-2016-17.csv";

        // Calendar
        public const int shownCalendarDaysCount = 42;

        // Colors
        static Color yellow = Color.Yellow;
        static Color blue = Color.Navy;
        static Color purple = Color.Purple;
        static Color teal = Color.Teal;
        static Color maroon = Color.Maroon;
        public static List<Color> colors = new List<Color>() { yellow, blue, purple, teal, maroon };

        // Settings
        public const string offlineMode = "offline"; 
    }
}
