using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using skolerute.data;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using skolerute.ExportCalendar;
using skolerute.utils;

namespace skolerute.Views
{
    public partial class CalendarPage : ContentPage
    {
        static DateTime current;
        static List<School> favoriteSchools;
        private static List<School> selectedSchools;
        private static List<Picker> pickers;
        static Grid cal;
        static StackLayout MS;
        

        public bool isLoading;

        public CalendarPage()
        {
            InitializeComponent();

            this.BindingContext = isLoading;

            // Setting some static variables
            pickers = new List<Picker>();
            pickers.Add(picker1);
            pickers.Add(picker2);
            pickers.Add(picker3);
            MS = MonthSelect;
            current = DateTime.Now;
            cal = calendar;

            DisplayCalendar(cal, MS);

            // Placeholder list over favorite schools
            ObservableCollection<string> favoriteSchoolNames = new ObservableCollection<string>();
            favoriteSchools = new List<School>();
            selectedSchools = new List<School>();
            MessagingCenter.Subscribe<StartUpPage>(this, "newSchoolSelected", (sender) =>
            {
                //TODO: Make this binding for better reusability
                isLoading = true;
                LoadingIndicator.IsRunning = isLoading;
                LoadingIndicator.IsVisible = isLoading;
                MonthSelect.FindByName<Image>("NextImg").IsEnabled = !isLoading;
                MonthSelect.FindByName<Image>("PrevImg").IsEnabled = !isLoading;
            });
            
            MessagingCenter.Subscribe<StartUpPage, School>(this, "choosenSch", (sender, args) =>
			{

                if (!favoriteSchoolNames.Contains(args.name))
                {
                    favoriteSchools.Add(args);
                    favoriteSchoolNames.Add(args.name);

                    ResetAllIndicators();
                    DisplayCalendar(cal, MS);

                    //TODO: Make this binding for better reusability
                    isLoading = false;
                    LoadingIndicator.IsRunning = isLoading;
                    LoadingIndicator.IsVisible = isLoading;
                    MonthSelect.FindByName<Image>("NextImg").IsEnabled = !isLoading;
                    MonthSelect.FindByName<Image>("PrevImg").IsEnabled = !isLoading;
                    SetPickers();
                }
			});


            MessagingCenter.Subscribe<StartUpPage, string>(this, "deleteSch", async(sender, args) =>
            {
                favoriteSchoolNames.Remove(args);
                await SettingsManager.SavePreferenceAsync("" + (favoriteSchools.Find(x => x.name.Contains(args)).ID), false);
                favoriteSchools.Remove(favoriteSchools.Find(x => x.name.Contains(args)));
                ResetAllIndicators();
                SetPickers();
            });
            SetLegends();
        }



        protected override void OnAppearing()
        {
            base.OnAppearing();

            DisplayCalendar(cal, MonthSelect);
            
        }

        private static void DisplayCalendar(Grid cal, StackLayout MonthSelect)
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
            MonthSelect.FindByName<Label>("monthName").Text = Calendar.MonthToString(current.Month);
            MonthSelect.FindByName<Label>("year").Text = current.Year.ToString();

            var calChildren = cal.Children;

            List<int> consecutiveDays = data.Calendar.GetCal(current);
            IEnumerator enumerator = calChildren.GetEnumerator();
            int i = 0;

            List<School> favoriteSchoolsTrimmed = selectedSchools;
            List<List<CalendarDay>> selectedSchoolsCalendars = new List<List<CalendarDay>>();
            if (favoriteSchoolsTrimmed != null && favoriteSchoolsTrimmed.Count > Constants.MaximumSelectedSchools)
                favoriteSchoolsTrimmed = favoriteSchools.GetRange(0, Constants.MaximumSelectedSchools);

            // Hardcoded the check on each index due to user being able to somethingsomething dark side
            if (favoriteSchoolsTrimmed != null && favoriteSchoolsTrimmed.Count > 0) {
                foreach (var favschool in favoriteSchoolsTrimmed)
                {
                    if (favschool != null) selectedSchoolsCalendars.Add(data.Calendar.GetRelevantFreeDays(favschool.calendar, current));
                }
            }

            while (enumerator.MoveNext())
            {
                try
                {
                    StackLayout sl = enumerator.Current as StackLayout;
                    Label label = sl.Children.First() as Label;
                    StackLayout boxes = sl.Children.Last() as StackLayout;

                    // HACK: Separate all days in the calendar from other elements like week days 
                    // or week numbers based on the elements they contain
                    if (sl.Children.Last().GetType() == typeof(StackLayout))
                    {
                        if (label != null) label.Text = consecutiveDays.ElementAt(i).ToString();

                        if (selectedSchoolsCalendars.Count > 0 && favoriteSchoolsTrimmed != null)
                        {
                            for (int j = 0; j < selectedSchoolsCalendars.Count && j < favoriteSchoolsTrimmed.Count; j++)
                            {
                                if (boxes != null)
                                {
                                    boxes.Children.ElementAt(j).IsVisible = true;
                                    boxes.Children.ElementAt(j).BackgroundColor = Constants.colors.ElementAt(j);
                                    if (selectedSchoolsCalendars.ElementAt(j).ElementAt(i).IsFreeDay &&
                                        ((int) selectedSchoolsCalendars.ElementAt(j).ElementAt(i).Date.DayOfWeek)%6 != 0 &&
                                        ((int) selectedSchoolsCalendars.ElementAt(j).ElementAt(i).Date.DayOfWeek)%7 != 0 &&
                                        favoriteSchoolsTrimmed[j] != null)
                                    {

                                        boxes.Children.ElementAt(j).Opacity = 1.0;
                                    }
                                    else
                                    {
                                        boxes.Children.ElementAt(j).Opacity = 0.0;
                                    }
                                }
                            }
                        }
                        i++;
                    }
                    else
                    {
                        if (label.Text.Length <= 2 && selectedSchoolsCalendars != null && selectedSchoolsCalendars.Count != 0)
                        {
                            label.Text = Calendar.GetWeekNumber(selectedSchoolsCalendars.ElementAt(0).ElementAt(i).Date).ToString();
                        }
                    }
                }
                catch (Exception e)
                {
                    MonthSelect.FindByName<Label>("monthName").Text = string.Empty;
                }
            }
        }

        private void SetPickers()
        {
            // Save previously selected schools if possible
            var selected = new List<string>();
            foreach (var picker in pickers)
            {
                var foundschool = false;
                foreach (var school in favoriteSchools)
                {
                    if (picker.SelectedIndex >= 0 && school.name == picker.Items[picker.SelectedIndex])
                    {
                        selected.Add(school.name);
                        foundschool = true;
                    }
                }
                if (!foundschool)
                {
                    selected.Add("ignore");
                }
            }

            //Clear pickers and add fresh list of schools
            foreach (var picker in pickers)
            {
                picker.Items.Clear();
                picker.Items.Add("Velg skole");
            }
            foreach (var school in  favoriteSchools)
            {
                foreach (var picker in pickers) picker.Items.Add(school.name);
            }

            // Populate pickers with previously selected schools, if any
            int i = 0;
            foreach (var picker in pickers)
            {
                if (selected[i] != "ignore")
                {
                    picker.SelectedIndex = picker.Items.IndexOf(selected[i]);

                }
                else
                {
                    picker.SelectedIndex = 0;
                }
                i++;
            }
        }

        private void OnSelect(object o, EventArgs e)
        {
            var i = 0;
            selectedSchools = new List<School>();
            foreach (var school in favoriteSchools)
            {
                foreach (var picker in pickers)
                {
                    if (picker.SelectedIndex >= 0)
                    {
                        if (school.name == picker.Items[picker.SelectedIndex] && i < picker.Items.Count)
                        {
                            selectedSchools.Add(school);
                        }
                    }
                    i++;
                }
            }
            SetLegends();
            DisplayCalendar(cal, MS);
        }

        private void SetLegends()
        {
            var children = Legend.Children;
            var i = 0;
            foreach (StackLayout x in children)
            {
                x.Children.First().BackgroundColor = Constants.colors[i];
                i++;
            }
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

        

        void ResetAllIndicators()
        {
            foreach (var child in cal.Children)
            {
                var day = (StackLayout) child;
                StackLayout boxContainer = day.Children.Last() as StackLayout;
                if (boxContainer != null)
                    foreach (var view in boxContainer.Children)
                    {
                        var box = (BoxView) view;
                        box.IsVisible = false;
                    }
            }
        }

        private async void ExportCalendarButtonClicked(object sender, EventArgs e)
        {
            List<MyCalendar> myCalendars = DependencyService.Get<IExportCalendar>().GetCalendarInfo();
            if (myCalendars == null || myCalendars.Count == 0)
            {
                await DisplayAlert("Eksporter kalender", "Du har ingen tilgjengelige kalendere i kalender appen din.", "Avbryt");
            }
            else if (myCalendars.Count == 1) // Do this to not bother the user with popup messages when it is unnecessary
            {
                await DependencyService.Get<IExportCalendar>()
                    .ExportToCalendar(Calendar.AddSchoolToList(favoriteSchools), myCalendars.FirstOrDefault());
            }
            else
            {
                List<string> calendarLabels = new List<string>(myCalendars.Count);
                calendarLabels.AddRange(myCalendars.Select(myCalendar => $"{myCalendar.Name} - {myCalendar.Accout}"));

                string choice = await DisplayActionSheet("Eksporter kalender", "Avbryt", null, calendarLabels.ToArray());
                string choiceName = choice.Split(' ').First();

                MyCalendar chosenCalendar = myCalendars.FirstOrDefault(myCalendar => choiceName == myCalendar.Name);

                if (chosenCalendar != null)
                {
                    await DependencyService.Get<IExportCalendar>()
                        .ExportToCalendar(Calendar.AddSchoolToList(favoriteSchools), chosenCalendar);
                }
            }
        }
    }
}

