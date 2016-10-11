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
            
            var cal = calendar;
            Label mName = monthName;
            Label yName = year;
            DateTime current = DateTime.Now;
            DisplayCalendar(mName, yName, cal, current);
            
            Prev.Tapped += (s, e) =>
            {
                current = current.AddMonths(-1);
                DisplayCalendar(mName, yName, cal, current);
            };

            Next.Tapped += (s, e) =>
            {
                current = current.AddMonths(1);
                DisplayCalendar(mName, yName, cal, current);
            };

        }

        private async static void DisplayCalendar(Label monthName, Label year, Grid cal, DateTime current)
        {
            monthName.Text = MonthToString(current.Month);
            year.Text = current.Year.ToString();
            
            var calChildren = cal.Children;
            DatabaseManagerAsync db = new DatabaseManagerAsync();
            List<School> schools = await db.GetSchools();
            School school = schools[0];
            bool inPreviousMonth = true;
            bool inCurrentMonth = false;
            bool inNextMonth = false;
            List<int> consecutiveDays = data.Calendar.GetCal(current.Year, current.Month);
            IEnumerator enumerator = calChildren.GetEnumerator();
            int i = 0;
            int j = 0;
            //bool[] freeDays = Calendar.DayIsFree(school, current.Month, current.Year);
            while (enumerator.MoveNext())
            {
                try
                {
                    StackLayout sl = enumerator.Current as StackLayout;
                    Label label = sl.Children.First() as Label;
                    label.Text = consecutiveDays.ElementAt(i).ToString();
                    if (consecutiveDays[i] == 1 && inPreviousMonth)
                    {
                        if (i > 9)
                        {
                            inCurrentMonth = false;
                            inNextMonth = true;
                        }
                        else
                        {
                            inCurrentMonth = true;
                            inPreviousMonth = false;
                        }

                    }

                    if (consecutiveDays[i] >= 1 && inCurrentMonth)
                    {

                        //box.IsVisible = freeDays[j + 1];

                        j++;
                    }


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

