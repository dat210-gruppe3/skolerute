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
using skolerute.utils;

namespace skolerute.Views
{
    public partial class CalendarPage : ContentPage
    {
        static DateTime current;
        static List<School> favoriteSchools;
        static Grid cal;
        static StackLayout MS;
        public CalendarPage()
        {
            InitializeComponent();

            MS = MonthSelect;
            current = DateTime.Now;
            cal = calendar;
            DisplayCalendar(cal, MS);
            // Placeholder liste over favoritt-skoler
            ObservableCollection<string> favoriteSchoolNames = new ObservableCollection<string>();
            favoriteSchools = new List<School>();

            
            
            MessagingCenter.Subscribe<StartUpPage, School>(this, "choosenSch", (sender, args) =>
			{

                if (!favoriteSchoolNames.Contains(args.name))
                {
                    favoriteSchools.Add(args);
                    favoriteSchoolNames.Add(args.name);
                    SchoolPicker.ItemsSource = favoriteSchoolNames;
                }

                
			});


            MessagingCenter.Subscribe<StartUpPage, string>(this, "deleteSch", async(sender, args) =>
            {
                favoriteSchoolNames.Remove(args);
                await SettingsManager.SavePreferenceAsync("" + (favoriteSchools.Find(x => x.name.Contains(args)).ID), false);
                favoriteSchools.Remove(favoriteSchools.Find(x => x.name.Contains(args)));
                SchoolPicker.ItemsSource = favoriteSchoolNames;
                
            });


        }



        protected override void OnAppearing()
        {
            base.OnAppearing();

            ResetAllIndicators();

            var cal = calendar;
            current = DateTime.Now;
            DisplayCalendar(cal, MonthSelect);
        }

        public static void DisplayCalendar(Grid cal, StackLayout MonthSelect)
        {
            // Makes the buttons transparent if user is at the end of intended month intverval
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

            List<int> consecutiveDays = Calendar.GetCal(current);
            IEnumerator enumerator = calChildren.GetEnumerator();
            int i = 0;

            List<School> favoriteSchoolsTrimmed = favoriteSchools;
            List<List<CalendarDay>> selectedSchoolsCalendars = new List<List<CalendarDay>>();
            if (favoriteSchoolsTrimmed != null && favoriteSchoolsTrimmed.Count > Constants.MaximumSelectedSchools)
                favoriteSchoolsTrimmed = favoriteSchools.GetRange(0, Constants.MaximumSelectedSchools);

            // TODO: Change from favorite schools to selected schools to enable the user to choose schools to be displayed
            if (favoriteSchoolsTrimmed != null && favoriteSchoolsTrimmed.Count > 0) { 
                foreach (School selected in favoriteSchoolsTrimmed)
                {
                    selectedSchoolsCalendars.Add(Calendar.GetRelevantFreeDays(selected.calendar, current));
                }
            }

            while (enumerator.MoveNext())
            {
                try
                {
                    StackLayout sl = enumerator.Current as StackLayout;
                    Label label = sl.Children.First() as Label;
                    StackLayout boxes = sl.Children.Last() as StackLayout;
                    
                    label.Text = consecutiveDays.ElementAt(i).ToString();

                    if (selectedSchoolsCalendars != null && selectedSchoolsCalendars.Count > 0)
                    {
                        for (int j = 0; j < selectedSchoolsCalendars.Count && j < favoriteSchoolsTrimmed.Count; j++)
                        {
							boxes.Children.ElementAt(j).IsVisible = true;
                            boxes.Children.ElementAt(j).BackgroundColor = Constants.colors.ElementAt(j);
							if (selectedSchoolsCalendars.ElementAt(j).ElementAt(i).isFreeDay && ((int)selectedSchoolsCalendars.ElementAt(j).ElementAt(i).date.DayOfWeek) % 6 != 0 && ((int)selectedSchoolsCalendars.ElementAt(j).ElementAt(i).date.DayOfWeek) % 7 != 0) { 
                                boxes.Children.ElementAt(j).Opacity = 1.0;
                            } else {
                                boxes.Children.ElementAt(j).Opacity = 0.0;
                            }

                        }
                    }
                    i++;
                }
                catch (Exception e)
                {
                    MonthSelect.FindByName<Label>("monthName").Text = string.Empty;
                }
            };
        }

        void NextMonth(object o, EventArgs e)
        {
            if (current.Month != 6)
            {
                current = current.AddMonths(1);
                DisplayCalendar(cal, MS);
            }
        }

        void PrevMonth(object o, EventArgs e)
        {
            if (current.Month != 8)
            {
                current = current.AddMonths(-1);
                DisplayCalendar(cal, MS);
            }
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

        void ResetAllIndicators()
        {
            foreach (StackLayout day in cal.Children)
            {
                StackLayout boxContainer = day.Children.Last() as StackLayout;
                foreach (BoxView box in boxContainer.Children)
                {
                    box.IsVisible = false;
                }
            }
        }

    }
}

