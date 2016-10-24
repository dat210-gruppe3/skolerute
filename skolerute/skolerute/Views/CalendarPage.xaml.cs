using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using skolerute.data;
using skolerute.db;

using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace skolerute.Views
{
    public partial class CalendarPage : ContentPage
    {
        static DatabaseManagerAsync db;
        static List<School> schools;
        static DateTime current;
        static List<int> favorites;
        static List<School> favoriteSchools;

        public CalendarPage()
        {
            InitializeComponent();

            // Placeholder liste over favoritt-skoler
            ObservableCollection<string> favoriteSchoolNames = new ObservableCollection<string>();
            favoriteSchools = new List<School>();
			MessagingCenter.Subscribe<StartUpPage, School>(this, "choosenSch", (sender, args) =>
			{
                favoriteSchools.Add(args);
				favoriteSchoolNames.Add(args.name);
                SchoolPicker.ItemsSource = favoriteSchoolNames;
			});

            MessagingCenter.Subscribe<StartUpPage, School>(this, "deleteSch", (sender, args) =>
            {
                favoriteSchoolNames.Remove(args.name);
                SchoolPicker.ItemsSource = favoriteSchoolNames;
            });

        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            db = new DatabaseManagerAsync();
            schools = await db.GetSchools();
            var cal = calendar;
            current = DateTime.Now;
            DisplayCalendar(cal, MonthSelect);
            
            Prev.Tapped += (s, e) =>
            {
                if (current.Month != 8) { 
                    current = current.AddMonths(-1);
                    DisplayCalendar(cal, MonthSelect);
                }
            };

            Next.Tapped += (s, e) =>
            {
                if (current.Month != 6)
                {
                    current = current.AddMonths(1);
                    DisplayCalendar(cal, MonthSelect);
                }
            };

        }

        private static void DisplayCalendar(Grid cal, StackLayout MonthSelect)
        {
            if (current.Month != 8) {
                MonthSelect.FindByName<Image>("PrevImg").Opacity = 1;
            } else {
                MonthSelect.FindByName<Image>("PrevImg").Opacity = 0.3;
            }
            if (current.Month != 6) {
                MonthSelect.FindByName<Image>("NextImg").Opacity = 1;
            } else {
                MonthSelect.FindByName<Image>("NextImg").Opacity = 0.3;
            }
            MonthSelect.FindByName<Label>("monthName").Text = MonthToString(current.Month);
            MonthSelect.FindByName<Label>("year").Text = current.Year.ToString();

            var calChildren = cal.Children;
            School school = favoriteSchools[0]; ///FEILER PÅ iOS

            List<int> consecutiveDays = Calendar.GetCal(current);
            IEnumerator enumerator = calChildren.GetEnumerator();
            int i = 0;

            List<CalendarDay> freeDays = Calendar.GetRelevantFreeDays(school.calendar, current);
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
                    MonthSelect.FindByName<Label>("monthName").Text = string.Empty;
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

