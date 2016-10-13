using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using skolerute.data;
using skolerute.db;

using Xamarin.Forms;

namespace skolerute.Views
{
    public partial class CalendarPage : ContentPage
    {
        static DatabaseManagerAsync db;
        static List<School> schools;
        static DateTime current;
        
        public CalendarPage()
        {
            InitializeComponent();
			// Placeholder liste over favoritt-skoler
			List<int> favorites = new List<int>();
			MessagingCenter.Subscribe<StartUpPage, int>(this, "choosenSch", (sender, args) =>
			{
				favorites.Add(args);
				//SchoolPicker.Items.Add(args.ToString());

			});

        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            db = new DatabaseManagerAsync();
            schools = await db.GetSchools();
            var cal = calendar;
            Label mName = monthName;
            Label yName = year;
            // current = DateTime.Now;
            current = new DateTime(2017,1,1);
            DisplayCalendar(mName, yName, cal);
            
            Prev.Tapped += (s, e) =>
            {
                current = current.AddMonths(-1);
                DisplayCalendar(mName, yName, cal);
            };

            Next.Tapped += (s, e) =>
            {
                current = current.AddMonths(1);
                DisplayCalendar(mName, yName, cal);
            };

        }

        private static void DisplayCalendar(Label monthName, Label year, Grid cal)
        {
            monthName.Text = MonthToString(current.Month);
            year.Text = current.Year.ToString();

            var calChildren = cal.Children;
            School school = schools[0];

            List<int> consecutiveDays = Calendar.GetCal(current.Year, current.Month);
            IEnumerator enumerator = calChildren.GetEnumerator();
            int i = 0;

            List<CalendarDay> freeDays = Calendar.GetRelevantFreeDays(school.calendar, current.Year, current.Month);
            while (enumerator.MoveNext())
            {
                try
                {
                    StackLayout sl = enumerator.Current as StackLayout;
                    Label label = sl.Children.First() as Label;
                    BoxView box = sl.Children.ElementAt(1) as BoxView;
                    label.Text = consecutiveDays.ElementAt(i).ToString();

                    box.IsVisible = freeDays.ElementAt(i).isFreeDay;

                    i++;
                }
                catch (Exception e)
                {
                    monthName.Text = string.Empty; //Replace with e.stacktrace
                }
            };
        }

        public static string MonthToString(int i)
        {
            switch (i)
            {
                case 1:
                    return "Januar";
                case 2:
                    return "Februar";
                case 3:
                    return "Mars";
                case 4:
                    return "April";
                case 5:
                    return "Mai";
                case 6:
                    return "Juni";
                case 7:
                    return "Juli";
                case 8:
                    return "August";
                case 9:
                    return "September";
                case 10:
                    return "Oktober";
                case 11:
                    return "November";
                case 12:
                    return "Desember";
            }
            return "";
        }



    }
}

