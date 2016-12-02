using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using skolerute.data;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Globalization;
using skolerute.ExportCalendar;
using skolerute.utils;
using Calendar = skolerute.data.Calendar;

namespace skolerute.Views
{
    public partial class CalendarPage : ContentPage
    {
        static DateTime current;
        static List<School> favoriteSchools;
        private static List<School> selectedSchools;
        private static List<Picker> pickers;

        public bool isLoading;

        public CalendarPage()
        {
            InitializeComponent();

            // Setting some static variables
            pickers = new List<Picker>();
            pickers.Add(picker1);
            pickers.Add(picker2);
            pickers.Add(picker3);

            current = DateTime.Now;

            DisplayCalendar();

            ObservableCollection<string> favoriteSchoolNames = new ObservableCollection<string>();
            favoriteSchools = new List<School>();
            selectedSchools = new List<School>();
            MessagingCenter.Subscribe<StartUpPage>(this, "newSchoolSelected", (sender) =>
            {
                //TODO: Make this binding for better reusability
                isLoading = true;
                LoadingIndicator.IsRunning = isLoading;
                LoadingIndicator.IsVisible = isLoading;
                NextImg.IsEnabled = !isLoading;
                PrevImg.IsEnabled = !isLoading;
            });
            
            MessagingCenter.Subscribe<StartUpPage, School>(this, "choosenSch", (sender, args) =>
			{

                if (!favoriteSchoolNames.Contains(args.name))
                {
                    favoriteSchools.Add(args);
                    favoriteSchoolNames.Add(args.name);

                    DisplayCalendar();
                    SetLegends();

                    //TODO: Make this binding for better reusability
                    isLoading = false;
                    LoadingIndicator.IsRunning = isLoading;
                    LoadingIndicator.IsVisible = isLoading;
                    NextImg.IsEnabled = !isLoading;
                    PrevImg.IsEnabled = !isLoading;
                }
			});


            MessagingCenter.Subscribe<StartUpPage, string>(this, "deleteSch", async(sender, args) =>
            {
                favoriteSchoolNames.Remove(args);
                await SettingsManager.SavePreferenceAsync("" + (favoriteSchools.Find(x => x.name.Contains(args)).ID), false);
                favoriteSchools.Remove(favoriteSchools.Find(x => x.name.Contains(args)));
                ResetAllIndicators();
                SetLegends();
            });

            SetLegends();
        }

        private void SetUpCalendar()
        {
            DateTime currentDateTime = Calendar.GetFirstRelevantDateTime(current);

            foreach (var calendarElement in CalendarGrid.Children)
            {
                StackLayout contentContainer = (StackLayout) calendarElement;

                if (contentContainer.ClassId == "WeekDay" || contentContainer.ClassId == "WeekendDay")
                {
                    Label dayNumber = contentContainer.Children.First() as Label;
                    dayNumber.Text = currentDateTime.Day.ToString();
                    currentDateTime = currentDateTime.AddDays(1);
                }
                else if (contentContainer.ClassId == "WeekNumber")
                {
                    Label weekNumber = contentContainer.Children.First() as Label;
                    weekNumber.Text = Calendar.GetWeekNumber(currentDateTime).ToString();
                }
            }
        }

        private void DisplayCalendar()
        {
            SetUpCalendar();
            UpdateCalendarControls();
            if (selectedSchools != null && selectedSchools.Count != 0)
            {
                ResetAllIndicators();
                UpdateIndicators();
            }        
        }

        private void UpdateIndicators()
        {
            List<List<CalendarDay>> freeDays = Calendar.GetAllRelevantCalendarDays(selectedSchools, current);

            int currentCalendarDayIndex = 0;
            foreach (var view in CalendarGrid.Children)
            {
                var calendarElement = (StackLayout) view;
                if (calendarElement.ClassId == "WeekDay")
                {
                    StackLayout indicatorContainer = calendarElement.Children.Last() as StackLayout;

                    int schoolIndex = 0;
                    if (indicatorContainer != null)
                        foreach (var view1 in indicatorContainer.Children)
                        {
                            if(schoolIndex >= freeDays.Count) break;
                            var indicator = (BoxView) view1;
                            if (freeDays[schoolIndex][currentCalendarDayIndex].IsFreeDay)
                            {
                                indicator.Opacity = 1;
                                indicator.Color = GetCorrectColor(schoolIndex);

                            }
                            schoolIndex++;
                        }
                    currentCalendarDayIndex++;
                }
                else if (calendarElement.ClassId == "WeekendDay")
                {
                    currentCalendarDayIndex++;
                }
            }
        }

        private Color GetCorrectColor(int i)
        {
            var children = Legend.Children;
            if (i < 0 || i >= selectedSchools.Count) return Color.Fuchsia;
            foreach (StackLayout child in children)
            {
                Picker picker = (Picker)child.Children.Last();
                if (picker.SelectedIndex < 0 || picker.SelectedIndex >= picker.Items.Count) continue;
                if (selectedSchools[i].name == picker.Items[picker.SelectedIndex])
                    return child.Children.First().BackgroundColor;
            }
            return Color.Fuchsia;
        }



        private void SetLegends()
        {
            var i = 0;
            foreach (StackLayout x in Legend.Children)
            {
                x.Children.First().BackgroundColor = Constants.colors[i];
                i++;
            }
            SetPickers();
        }

        private void SetPickers()
        {
            //Clear pickers and add fresh list of schools
            foreach (var picker in pickers)
            {
                picker.Items.Clear();
            }
            foreach (var school in  favoriteSchools)
            {
                foreach (var picker in pickers) picker.Items.Add(school.name);
            }

            // Populate pickers with previously selected schools, if any 
            //**(Does not currently work when starting the app, only works when adding or removing schools from favorites list)
            int i = 0;
            foreach (var picker in pickers)
            {
                string pickername = "picker" + i;
                var preferred = SettingsManager.GetPreference(pickername);
                if (preferred != null && (string)preferred != "ignore")
                {
                    picker.SelectedIndex = picker.Items.IndexOf(preferred);

                }
                i++;
            }
        }

        private async void OnSelect(object o, EventArgs e)
        {
            if (0 > ((Picker) o).SelectedIndex) return;
            selectedSchools = new List<School>();
            foreach (var school in favoriteSchools)
            {
                int i = 0;
                foreach (var picker in pickers)
                {
                    if (picker.SelectedIndex >= 0)
                    {
                        if (picker.SelectedIndex < picker.Items.Count &&
                            school.name == picker.Items[picker.SelectedIndex])
                        {
                            await SettingsManager.SavePreferenceAsync("picker" + i, school.name);
                            selectedSchools.Add(school);
                        }
                    }
                    i++;
                }
            }
            DisplayCalendar();
        }

        private void UpdateCalendarControls()
        {
            PrevImg.Opacity = current.Month != 8 ? 1 : 0.3;
            NextImg.Opacity = current.Month != 6 ? 1 : 0.3;

            MonthLabel.Text = Calendar.MonthToString(current.Month);
            YearLabel.Text = current.Year.ToString();
        }

        void ResetAllIndicators()
        {
            foreach (var calendarElement in CalendarGrid.Children)
            {
                if (calendarElement.ClassId == "WeekDay")
                {
                    StackLayout indicatorContainer = ((StackLayout) calendarElement).Children.Last() as StackLayout;
                    foreach (BoxView indicator in indicatorContainer.Children)
                    {
                        indicator.Opacity = 0;
                    }
                }
            }
        }
        void NextMonth(object o, EventArgs e)
        {
            if (current.Month != 6)
            {
                current = current.AddMonths(1);
                DisplayCalendar();
            }
        }

        void PrevMonth(object o, EventArgs e)
        {
            if (current.Month != 8)
            {
                current = current.AddMonths(-1);
                DisplayCalendar();
            }
        }     

        
        /*
         * The reason why this method contains device specific code is because of the permission system for ios when asking for permission.
         * It would not wait for the user to give or deny permission, and therefore return. It then calls an anonymous method when an answer is
         * recieved. Unfortunately, at this point the ExportToCalendar method has returned long ago, so it is no use. That is the reason
         * why we had to handle everything in the callback code for iOS. Since we couldn't make it how we wanted, we tried doing the same
         * in the android code, but this was not an easy feat. You would have to make a custom layout and inflate it into an AlertDialog.
         * Xamarin comes to the rescue, making it easy to display an action sheet for both platforms, although we only use it for android
         * in this case. This is the explanation as to why this handler is a little messy.
         * */
        private async void ExportCalendarButtonClicked(object sender, EventArgs e)
        {

            if (Device.OS == TargetPlatform.iOS)
            {
                DependencyService.Get<IExportCalendar>()
                    .ExportToCalendar(data.Calendar.AddSchoolToList(favoriteSchools), null);
            }
            else
            {
                List<MyCalendar> myCalendars = DependencyService.Get<IExportCalendar>().GetCalendarInfo();
                if (myCalendars == null || myCalendars.Count == 0)
                {
                    await
                        DisplayAlert("Eksporter kalender", "Du har ingen tilgjengelige kalendere i kalender appen din.",
                            "Avbryt");
                }
                else if (myCalendars.Count == 1) // Do this to not bother the user with popup messages when it is unnecessary
                {
                    DependencyService.Get<IExportCalendar>()
                        .ExportToCalendar(data.Calendar.AddSchoolToList(favoriteSchools), myCalendars.FirstOrDefault());
                }
                else
                {
                    List<string> calendarLabels = new List<string>(myCalendars.Count);
                    calendarLabels.AddRange(myCalendars.Select(myCalendar => $"{myCalendar.Name} - {myCalendar.Accout}"));

                    string choice =
                        await DisplayActionSheet("Eksporter kalender", "Avbryt", null, calendarLabels.ToArray());
                    string choiceName = choice.Split(' ').First();

                    MyCalendar chosenCalendar = myCalendars.FirstOrDefault(myCalendar => choiceName == myCalendar.Name);

                    if (chosenCalendar != null)
                    {
                        DependencyService.Get<IExportCalendar>()
                            .ExportToCalendar(data.Calendar.AddSchoolToList(favoriteSchools), chosenCalendar);
                    }
                }
            }
        }
    }
}

