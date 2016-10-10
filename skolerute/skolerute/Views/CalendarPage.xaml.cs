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
            List<string> favorites = new List<string> { "skole1", "skole2", "skole3" };
            SchoolPicker.ItemsSource = favorites;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            School school;
            DatabaseManagerAsync db = new DatabaseManagerAsync();
            school = await db.GetSchool(1);

            var currentMonth = 3;

            bool inPreviousMonth = true;
            bool inCurrentMonth = false;
            bool inNextMonth = false;

            var cal = calendar;
            var calChildren = cal.Children;
            List<int> consecutiveDays = data.Calendar.displayCal(2016, currentMonth);
            IEnumerator enumerator = calChildren.GetEnumerator();
            int i = 0;
            while (enumerator.MoveNext())
            {
                try
                {
                    StackLayout sl = enumerator.Current as StackLayout;
                    Label label = sl.Children.First() as Label;
                    BoxView box = sl.Children.ElementAt(1) as BoxView;
                    label.Text = consecutiveDays.ElementAt(i).ToString();
                    if (inPreviousMonth)
                    {
                        if (consecutiveDays.ElementAt(i) == 1)
                        {
                            inPreviousMonth = false;
                            inCurrentMonth = true;
                        }
                        else
                        {
                            box.IsVisible = Calendar.DayIsFree(school, currentMonth - 1, consecutiveDays.ElementAt(i));
                        }
                    }
                    else if (inCurrentMonth)
                    {
                        if (DateTime.DaysInMonth(2016, currentMonth) == consecutiveDays.ElementAt(i))
                        {
                            inCurrentMonth = false;
                            inNextMonth = true;
                        }
                        else
                        {
                            box.IsVisible = Calendar.DayIsFree(school, currentMonth, consecutiveDays.ElementAt(i));
                        }
                    }
                    else if (inNextMonth)
                    {
                        inNextMonth = false;

                        box.IsVisible = Calendar.DayIsFree(school, currentMonth + 1, consecutiveDays.ElementAt(i));
                    }

                    i++;
                }
                catch (Exception e)
                {
                    monthName.Text = e.StackTrace;
                }
            }
        }
    }
}
