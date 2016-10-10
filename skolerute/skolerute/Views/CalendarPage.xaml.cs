using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

using Xamarin.Forms;
using System.Globalization;

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
            DateTime current = DateTime.Now;
            monthName.Text = MonthToString(current.Month);
            year.Text = current.Year.ToString();
            List<int> consecutiveDays = data.Calendar.displayCal(current.Year, current.Month);
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
                catch (Exception e)
                {
                    monthName.Text = e.StackTrace;
                }
            }

            Prev.Tapped += (s, a) =>
            {
                current = current.AddMonths(-1);
                monthName.Text = MonthToString(current.Month);
                year.Text = current.Year.ToString();
                consecutiveDays = data.Calendar.displayCal(current.Year, current.Month);
                enumerator = calChildren.GetEnumerator();
                i = 0;
                while (enumerator.MoveNext())
                {
                    try
                    {
                        StackLayout sl = enumerator.Current as StackLayout;
                        Label label = sl.Children.First() as Label;
                        label.Text = consecutiveDays.ElementAt(i).ToString();
                        i++;
                    }
                    catch (Exception e)
                    {
                        monthName.Text = e.StackTrace;
                    }
                }
            };
            
            Next.Tapped += (s, a) =>
            {
                current = current.AddMonths(1);
                monthName.Text = MonthToString(current.Month);
                year.Text = current.Year.ToString();
                consecutiveDays = data.Calendar.displayCal(current.Year, current.Month);
                enumerator = calChildren.GetEnumerator();
                i = 0;
                while (enumerator.MoveNext())
                {
                    try
                    {
                        StackLayout sl = enumerator.Current as StackLayout;
                        Label label = sl.Children.First() as Label;
                        label.Text = consecutiveDays.ElementAt(i).ToString();
                        i++;
                    }
                    catch (Exception e)
                    {
                        monthName.Text = e.StackTrace;
                    }
                }

            };

            

            
        }
       
        public string MonthToString(int i)
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
