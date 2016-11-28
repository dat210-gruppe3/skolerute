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

        public bool isLoading;

        public CalendarPage()
        {
            InitializeComponent();

            current = DateTime.Now;

            DisplayCalendar();

            ObservableCollection<string> favoriteSchoolNames = new ObservableCollection<string>();
            favoriteSchools = new List<School>();

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
            });


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
            if (favoriteSchools != null && favoriteSchools.Count != 0)
            {
                ResetAllIndicators();
                UpdateIndicators();
            }        
        }

        private void UpdateIndicators()
        {
            List<List<CalendarDay>> freeDays = Calendar.GetAllRelevantCalendarDays(favoriteSchools, current);

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
                                indicator.Color = Constants.colors[schoolIndex];
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

        private void UpdateCalendarControls()
        {
            PrevImg.Opacity = current.Month != 8 ? 1 : 0.3;
            NextImg.Opacity = current.Month != 6 ? 1 : 0.3;

            MonthLabel.Text = Calendar.MonthToString(current.Month);
            YearLabel.Text = current.Year.ToString();
        }

        private void SetLegends()
        {
            var children = Legend.Children;
            var i = 0;
            foreach (var view in children)
            {
                var x = (StackLayout) view;
                var box = x.Children.First() as BoxView;
                var label = x.Children.Last() as Label;
                if (i >= favoriteSchools.Count)
                {
                    x.Opacity = 0;
                }
                else
                {
                    var currentschool = favoriteSchools[i];
                    label.Text = currentschool.name;
                    box.Color = Constants.colors[i];
                    x.Opacity = 1;
                }

                i++;
            }
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

