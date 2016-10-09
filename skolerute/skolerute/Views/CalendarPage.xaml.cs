using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

using Xamarin.Forms;

namespace skolerute.Views
{
    public partial class CalendarPage : ContentPage
    {
        public CalendarPage()
        {
            InitializeComponent();

            // Placeholder liste over favoritt-skoler
            List<string> favorites = new List<string> { "skole1", "skole2", "skole3" };
            SchoolPicker.ItemsSource = favorites;

            var cal = calendar;
			var calChildren = cal.Children;
			List<int> consecutiveDays = data.Calendar.displayCal(2016, 03);
			IEnumerator enumerator = calChildren.GetEnumerator();
			int i = 0;
			while (enumerator.MoveNext())
			{
				try
				{
					StackLayout sl = enumerator.Current as StackLayout;
					Label label = sl.Children.First() as Label;
					label.Text = consecutiveDays.ElementAt(i).ToString();
					i++;
				}
				catch(Exception e)
				{
					monthName.Text = e.StackTrace;
				}
			}
        }
    }
}
